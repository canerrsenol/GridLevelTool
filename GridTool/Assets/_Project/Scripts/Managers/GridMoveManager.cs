using UnityEngine;

public class GridMoveManager : MonoBehaviour
{
    private IGridMover currentMover;
    
    private Pathfinder pathfinder;
    
    private void Start()
    {
        pathfinder = new Pathfinder(GridBase.Instance);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
        }
    }
}
