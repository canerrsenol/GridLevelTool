using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    private bool canMoveDiagonal = false;
    
    private GridBase gridSystem;
    
    public Pathfinder(GridBase gridSystem)
    {
        this.gridSystem = gridSystem;
    }
    
    public List<Vector2Int> FindPath(Vector2Int startTilePosition, Vector2Int endTilePosition)
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
                Vector2Int gridPosition = new Vector2Int(x, z);
                Tile tile = gridSystem.GetTile(gridPosition);
                
                // tile.SetGCost(int.MaxValue);
                // tile.SetHCost(0);
                // tile.CalculateFCost();
                // tile.ResetCameFromPathNode();
            }
        }
        
        // Calculating start node costs
        // startNode.SetGCost(0);
        // startNode.SetHCost(CalculateDistance(startTilePosition, endTilePosition));
        // startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            Tile currentNode = GetLowestFCostPathTile(openList);
            
            if(currentNode == endNode)
            {
                return CalculatePath(endNode);
            }
            
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (Tile neighbourNode in GetNeighbourList(currentNode))
            {
                // If we searched this node already then continue
                if(closedList.Contains(neighbourNode)) continue;
                
                // If the node is not walkable then continue
                if (!neighbourNode.IsTileEmpty())
                {
                    closedList.Add(neighbourNode);
                    continue;
                }
                
                // int tentativeGCost = currentNode.GetGCost() + CalculateDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition());
                //
                // // Check we have a better path
                // if (tentativeGCost < neighbourNode.GetGCost())
                // {
                //     neighbourNode.SetCameFromPathNode(currentNode);
                //     neighbourNode.SetGCost(tentativeGCost);
                //     neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), endTilePosition));
                //     neighbourNode.CalculateFCost();
                //
                //     if (!openList.Contains(neighbourNode))
                //     {
                //         openList.Add(neighbourNode);
                //     }
                // }
            }
        }
        
        // No path found
        return null;
    }

    private List<Vector2Int> CalculatePath(Tile endTile)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        path.Add(endTile.GetTilePosition());
        Tile currentTile = endTile;

        // while (currentTile.GetCameFromPathNode() != null)
        // {
        //     currentTile = currentTile.GetCameFromPathNode();
        //     path.Add(currentTile.GetGridPosition());
        // }

        path.Reverse();
        return path;
    }
    
    private Tile GetNode(int x, int z)
    {
        return gridSystem.GetTile(new Vector2Int(x, z));
    }

    private List<Tile> GetNeighbourList(Tile currentTile)
    {
        List<Tile> neighbourList = new List<Tile>();
        Vector2Int gridPosition = currentTile.GetTilePosition();

        List<Vector2Int> neighbourOffsets = new List<Vector2Int>
        {
            new Vector2Int(-1, 0),  // Left
            new Vector2Int(1, 0),   // Right
            new Vector2Int(0, -1),  // Down
            new Vector2Int(0, 1),   // Up
        };

        if (canMoveDiagonal)
        {
            neighbourOffsets.Add(new Vector2Int(-1, -1)); // Left Down
            neighbourOffsets.Add(new Vector2Int(-1, 1));  // Left Up
            neighbourOffsets.Add(new Vector2Int(1, -1));  // Right Down
            neighbourOffsets.Add(new Vector2Int(1, 1));   // Right Up
        }

        foreach (Vector2Int offset in neighbourOffsets)
        {
            Vector2Int neighbourPosition = gridPosition + offset;
            if (neighbourPosition.x >= 0 && neighbourPosition.x < gridSystem.GetWidth() &&
                neighbourPosition.y >= 0 && neighbourPosition.y < gridSystem.GetHeight())
            {
                neighbourList.Add(GetNode(neighbourPosition.x, neighbourPosition.y));
            }
        }

        return neighbourList;
    }
    
    public int CalculateDistance(Vector2Int a, Vector2Int b)
    {
        Vector2Int distance = a - b;
        int xDistance = Mathf.Abs(distance.x);
        int zDistance = Mathf.Abs(distance.y);
        int remaining = Mathf.Abs(xDistance - zDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private Tile GetLowestFCostPathTile(List<Tile> pathNodeList)
    {
        Tile lowestFCost = pathNodeList[0];

        // for (int i = 0; i < pathNodeList.Count; i++)
        // {
        //     if(pathNodeList[i].GetFCost() < lowestFCost.GetFCost())
        //     {
        //         lowestFCost = pathNodeList[i];
        //     }
        // }
        
        return lowestFCost;
    }
}
