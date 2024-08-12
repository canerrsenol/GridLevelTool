using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityUtils;

public class GridBase : Singleton<GridBase>
{
    [SerializeField] private Vector2Int gridSize;
    private float tileSize;
    
    private Tile[,] tileObjectArray;

    protected override void Awake()
    {
        base.Awake();
        List <Tile> tiles = GetComponentsInChildren<Tile>().ToList();

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                tileObjectArray[x, y] = tiles[0];
                tiles.RemoveAt(0);
            }
        }
        
        GetTileObjects();
    }

    private void GetTileObjects()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector2Int tilePosition = new Vector2Int(x, y);
                Vector3 worldPosition = GetWorldPosition(tilePosition);
                float raycastOffsetDistance = 5f;
                 
                if (Physics.Raycast(worldPosition + Vector3.down * raycastOffsetDistance,
                        Vector3.up, out RaycastHit raycastHit, raycastOffsetDistance * 2))
                {
                    if(raycastHit.collider.TryGetComponent(out ITileObject tileObject))
                    {
                        tileObjectArray[x, y].SetTileObject(tileObject);
                    }
                }
            }
        }
    }

    public void SetGridSettings(Vector2Int gridSize, float tileSize)
    {
        this.gridSize = gridSize;
        this.tileSize = tileSize;
    }
    
    public int GetWidth() { return gridSize.x; }
    public int GetHeight() { return gridSize.y; }

    public Vector3 GetWorldPosition(Vector2Int tilePosition)
    {
        return new Vector3(tilePosition.x, 0f, tilePosition.y) * tileSize;
    }
    
    public Vector2Int GetTilePosition(Vector3 worldPosition)
    {
        return new Vector2Int(
            Mathf.RoundToInt(worldPosition.x / tileSize),
            Mathf.RoundToInt(worldPosition.z / tileSize)
        );
    }
    
    public Tile GetTile(Vector2Int tilePosition)
    {
        return tileObjectArray[tilePosition.x, tilePosition.y];
    }
    
    public bool IsValidGridPosition(Vector2Int tilePosition)
    {
        return tilePosition.x >= 0 &&
               tilePosition.x < gridSize.x && 
               tilePosition.y >= 0 && 
               tilePosition.y < gridSize.y;
    }
}
