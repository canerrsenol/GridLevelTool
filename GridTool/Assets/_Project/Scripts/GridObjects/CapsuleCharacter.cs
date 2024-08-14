using System;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleCharacter : MonoBehaviour, IGridMover, ITileObject
{
    [SerializeField] private float moveSpeed = 5f;
    
    private List<TilePosition> path;
    
    private GridBase gridBase;
    
    private Vector3 targetWorldPosition;

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
        targetWorldPosition = gridBase.GetWorldPosition(path[0]);
        
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

        float distanceToTarget = Vector3.Distance(transform.position, targetWorldPosition);

        if (distanceToTarget < 0.01f)
        {
            if (path.Count != 1)
            {
                Tile currentTile = gridBase.GetTile(path[0]);
                currentTile.SetTileObject(null);
            }
            
            path.RemoveAt(0);

            if (path.Count > 0)
            {
                targetWorldPosition = gridBase.GetWorldPosition(path[0]);
                Tile nextTile = gridBase.GetTile(path[0]);
                nextTile.SetTileObject(this);
            }
        }
        else
        {
            Vector3 direction = targetWorldPosition - transform.position;
            transform.position += direction.normalized * (moveSpeed * Time.deltaTime);
        }
    }
}
