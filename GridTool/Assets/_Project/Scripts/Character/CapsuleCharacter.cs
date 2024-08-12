using System.Collections.Generic;
using UnityEngine;

public class CapsuleCharacter : MonoBehaviour, IGridMover
{
    [SerializeField] private float moveSpeed = 5f;
    
    private List<Vector2Int> path;
    public void Move(List<Vector2Int> path)
    {
        this.path = path;
    }

    private void Update()
    {
        if (path == null || path.Count == 0) return;
        
        Vector2Int targetPosition = path[0];
        Vector3 targetWorldPosition = new Vector3(targetPosition.x, 0, targetPosition.y);
        Vector3 direction = targetWorldPosition - transform.position;
        transform.position += direction.normalized * moveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, targetWorldPosition) < 0.1f)
        {
            path.RemoveAt(0);
            
            if (path.Count == 0)
            {
                Debug.Log("Arrived at destination");
            }
        }
    }
}
