using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    private bool canMoveDiagonal;
    
    private GridBase gridSystem;
    
    public Pathfinder(GridBase gridSystem, bool canMoveDiagonal = false)
    {
        this.gridSystem = gridSystem;
        this.canMoveDiagonal = canMoveDiagonal;
    }
    
    public List<TilePosition> FindPath(TilePosition startTilePosition, TilePosition endTilePosition)
    {
        List<Tile> openList = new List<Tile>();
        HashSet<Tile> closedList = new HashSet<Tile>();

        Tile startNode = gridSystem.GetTile(startTilePosition);
        Tile endNode = gridSystem.GetTile(endTilePosition);
        openList.Add(startNode);
        
        // All nodes are initialized for pathfinding
        for(int x = 0; x < gridSystem.GetWidth(); x++)
        {
            for(int z = 0; z < gridSystem.GetHeight(); z++)
            {
                TilePosition gridPosition = new TilePosition(x, z);
                Tile tile = gridSystem.GetTile(gridPosition);
                
                tile.GetNode().SetGCost(int.MaxValue);
                tile.GetNode().SetHCost(0);
                tile.GetNode().CalculateFCost();
                tile.GetNode().ResetCameFromTile();
            }
        }
        
        startNode.GetNode().SetGCost(0);
        startNode.GetNode().SetHCost(CalculateDistance(startTilePosition, endTilePosition));
        startNode.GetNode().CalculateFCost();

        while (openList.Count > 0)
        {
            Tile currenTile = GetLowestFCostPathTile(openList);
            
            if(currenTile == endNode)
            {
                return CalculatePath(endNode);
            }
            
            openList.Remove(currenTile);
            closedList.Add(currenTile);

            foreach (Tile neighbourTile in GetNeighbourList(currenTile))
            {
                // If we searched this node already then continue
                if(closedList.Contains(neighbourTile)) continue;
                
                // If the node is not walkable then continue
                if (!neighbourTile.IsTileEmpty())
                {
                    closedList.Add(neighbourTile);
                    continue;
                }
                
                int tentativeGCost = currenTile.GetNode().GetGCost() + CalculateDistance(currenTile.GetTilePosition(), neighbourTile.GetTilePosition());
                
                // Check we have a better path
                if (tentativeGCost < neighbourTile.GetNode().GetGCost())
                {
                    neighbourTile.GetNode().SetCameFromTile(currenTile);
                    neighbourTile.GetNode().SetGCost(tentativeGCost);
                    neighbourTile.GetNode().SetHCost(CalculateDistance(neighbourTile.GetTilePosition(), endTilePosition));
                    neighbourTile.GetNode().CalculateFCost();
                
                    if (!openList.Contains(neighbourTile))
                    {
                        openList.Add(neighbourTile);
                    }
                }
            }
        }
        
        // No path found
        return null;
    }

    private List<TilePosition> CalculatePath(Tile endTile)
    {
        List<TilePosition> path = new List<TilePosition>();
        path.Add(endTile.GetTilePosition());
        Tile currentTile = endTile;

        while (currentTile.GetNode().GetCameFromTile() != null)
        {
            currentTile = currentTile.GetNode().GetCameFromTile();
            path.Add(currentTile.GetTilePosition());
        }

        path.Reverse();
        return path;
    }
    
    private Tile GetNode(int x, int z)
    {
        return gridSystem.GetTile(new TilePosition(x, z));
    }

    private List<Tile> GetNeighbourList(Tile currentTile)
    {
        List<Tile> neighbourList = new List<Tile>();
        TilePosition gridPosition = currentTile.GetTilePosition();

        List<TilePosition> neighbourOffsets = new List<TilePosition>
        {
            new TilePosition(-1, 0),  // Left
            new TilePosition(1, 0),   // Right
            new TilePosition(0, -1),  // Down
            new TilePosition(0, 1),   // Up
        };

        if (canMoveDiagonal)
        {
            neighbourOffsets.Add(new TilePosition(-1, -1)); // Left Down
            neighbourOffsets.Add(new TilePosition(-1, 1));  // Left Up
            neighbourOffsets.Add(new TilePosition(1, -1));  // Right Down
            neighbourOffsets.Add(new TilePosition(1, 1));   // Right Up
        }

        foreach (TilePosition offset in neighbourOffsets)
        {
            TilePosition neighbourPosition = gridPosition + offset;
            if (neighbourPosition.x >= 0 && neighbourPosition.x < gridSystem.GetWidth() &&
                neighbourPosition.z >= 0 && neighbourPosition.z < gridSystem.GetHeight())
            {
                neighbourList.Add(GetNode(neighbourPosition.x, neighbourPosition.z));
            }
        }

        return neighbourList;
    }
    
    public int CalculateDistance(TilePosition a, TilePosition b)
    {
        TilePosition distance = a - b;
        int xDistance = Mathf.Abs(distance.x);
        int zDistance = Mathf.Abs(distance.z);
        int remaining = Mathf.Abs(xDistance - zDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private Tile GetLowestFCostPathTile(List<Tile> pathNodeList)
    {
        Tile lowestFCost = pathNodeList[0];

        for (int i = 0; i < pathNodeList.Count; i++)
        {
            if(pathNodeList[i].GetNode().GetFCost() < lowestFCost.GetNode().GetFCost())
            {
                lowestFCost = pathNodeList[i];
            }
        }
        
        return lowestFCost;
    }
}
