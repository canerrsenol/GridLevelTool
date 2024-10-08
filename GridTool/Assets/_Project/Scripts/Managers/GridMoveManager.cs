using UnityEngine;

public class GridMoveManager : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    
    [SerializeField] private LayerMask gridMoverLayer;
    
    private IGridMover currentMover;
    
    private InputManager inputManager;
    
    private GridBase gridBase;
    
    private Pathfinder pathfinder;

    private void Awake()
    {
        gridBase = GridBase.Instance;
        inputManager = InputManager.Instance;
    }
    
    private void OnEnable()
    {
        inputManager.OnClick += HandleClick;
    }
    
    private void OnDisable()
    {
        inputManager.OnClick -= HandleClick;
    }
    
    private void Start()
    {
        pathfinder = new Pathfinder(gridBase);
    }

    private void HandleClick(Vector2 screenPosition)
    { 
        if (currentMover != null)
        {
            if(currentMover.IsMoving()) return;
            
            Ray ray = GetRayFromScreenPosition(screenPosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, groundLayer))
            {
                Vector3 worldPosition = raycastHit.point;
                
                TilePosition moverTilePosition = gridBase.GetTilePosition(currentMover.GetPosition());
                TilePosition targetTilePosition = gridBase.GetTilePosition(worldPosition);
                
                if(!gridBase.IsValidGridPosition(targetTilePosition)) return;
                if(moverTilePosition == targetTilePosition) return;
                
                var path = pathfinder.FindPath(moverTilePosition, targetTilePosition);
                if (path == null || path.Count == 0) return;
                
                currentMover.Move(path);
                currentMover = null;
            }
        }
        else
        {
            Ray ray = GetRayFromScreenPosition(screenPosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, gridMoverLayer))
            {
                currentMover = raycastHit.collider.GetComponent<IGridMover>();
            }
        }
    }
    
    private Ray GetRayFromScreenPosition(Vector2 screenPosition)
    {
        return Camera.main.ScreenPointToRay(screenPosition);
    }
}
