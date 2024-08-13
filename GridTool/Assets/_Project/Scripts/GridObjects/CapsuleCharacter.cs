using System;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleCharacter : MonoBehaviour, IGridMover, ITileObject
{
    [SerializeField] private float moveSpeed = 5f;
    
    private List<TilePosition> path;
    
    private GridBase gridBase;

    private void Awake()
    {
        gridBase = GridBase.Instance;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void Move(List<TilePosition> path)
    {
        this.path = path;
        
        TilePosition currentTilePosition = gridBase.GetTilePosition(transform.position);
        Tile currentTile = gridBase.GetTile(currentTilePosition);
        currentTile.SetTileObject(null);
    }

    public bool IsMoving()
    {
        return path != null && path.Count > 0;
    }

    private void Update()
    {
        if (path == null || path.Count == 0) return;
        
        TilePosition targetPosition = path[0];
        Vector3 targetWorldPosition = new Vector3(targetPosition.x, 0, targetPosition.z) * gridBase.TileSize;
        Vector3 direction = targetWorldPosition - transform.position;
        transform.position += direction.normalized * (moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetWorldPosition) < 0.01f)
        {
            Tile tile = gridBase.GetTile(targetPosition);
            tile.SetTileObject(null);
            
            path.RemoveAt(0);
            
            if (path.Count == 0)
            {
                // Path is finished
            }
            else
            {
                Tile nextTile = gridBase.GetTile(path[0]);
                nextTile.SetTileObject(this);
            }
        }
    }
}
