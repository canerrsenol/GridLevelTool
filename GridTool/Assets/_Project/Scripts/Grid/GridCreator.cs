using UnityEditor;
using UnityEngine;

public class GridCreator : MonoBehaviour
{
    [SerializeField] private TileObjectsFactory tileObjectsFactory;
    
    [SerializeField] private GameObject tilePrefab;

    [SerializeField] private Vector2Int gridSize;
    
    public Vector2Int GridSize => gridSize;
    
    [SerializeField] private float tileSize = 1.1f;

    public float TileSize => tileSize;
    
    private int[,] tileObjectIndexArray;

    //[HideInInspector] 
    public GameObject GridParent;
    
    //[HideInInspector] 
    public GameObject TileObjectsParent;
    
    public GameObject GetTileObject(int index)
    {
        return tileObjectsFactory.GetTileObjectPrefabAtIndex(index);
    }

    public void CreateGrid()
    {
        if(gridSize.x <= 0 || gridSize.y <= 0) return;
        tileObjectIndexArray = new int[gridSize.x, gridSize.y];
        
        if (GridParent == null)
        {
            GameObject gridParent = new GameObject("GridBase");
            GridParent = gridParent;
        }
        
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                GameObject tile = PrefabUtility.InstantiatePrefab(tilePrefab) as GameObject;
                SetTilePosition(tile.transform, x, y);
                tile.transform.parent = GridParent.transform;
            }
        }
            
        EditorUtility.SetDirty(gameObject);
    }

    public void UpdateAllTilesPosition()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                SetTilePosition(GridParent.transform.GetChild(x * gridSize.y + y), x, y);
            }
        }
    }

    private void SetTilePosition(Transform tileTransform, int x, int y)
    {
        Vector3 tilePosition = new Vector3(x * tileSize, 0, y * tileSize);
        tileTransform.position = tilePosition;
        tileTransform.name = $"({x}, {y})";
    }
    
    public void ResetGridSettings()
    {
        DestroyImmediate(GridParent);
        DestroyImmediate(TileObjectsParent);
        
        gridSize.x = 0;
        gridSize.y = 0;
        tileSize = 1;
    }
    
    private void DestroyIfTileHasObject(Vector2Int tilePosition)
    {
        if (TileObjectsParent != null)
        {
            var tileObjects = TileObjectsParent.GetComponentsInChildren<ITileObject>();
            
            if(tileObjects != null && tileObjects.Length > 0)
            {
                foreach (ITileObject tileObject in tileObjects)
                {
                    if (tileObject.GetTilePosition() == tilePosition)
                    {
                        var tileObjectMonoBehaviour = tileObject as MonoBehaviour;
                        DestroyImmediate(tileObjectMonoBehaviour.gameObject);
                    }
                }
            }
        }
    }
    
    public void SetNextTileObject(Vector2Int tilePosition)
    {
        DestroyIfTileHasObject(tilePosition);
        if(tileObjectIndexArray == null) tileObjectIndexArray = new int[gridSize.x, gridSize.y];
        
        int newIndex = tileObjectIndexArray[tilePosition.x, tilePosition.y];
        newIndex = (newIndex + 1) % tileObjectsFactory.GetTileObjectsCount();
        tileObjectIndexArray[tilePosition.x, tilePosition.y] = newIndex;
        
        if(newIndex == 0) return;
        
        GameObject tileObject = tileObjectsFactory.SpawnTileObject(newIndex);
        tileObject.transform.position = new Vector3(tilePosition.x * tileSize, 0, tilePosition.y * tileSize);
        
        if(TileObjectsParent == null)
        {
            GameObject tileObjectsParent = new GameObject("TileObjects");
            TileObjectsParent = tileObjectsParent;
        }
        tileObject.transform.SetParent(TileObjectsParent.transform);
        
        ITileObject tileObjectComponent = tileObject.GetComponent<ITileObject>();
        if (tileObjectComponent != null) tileObjectComponent.SetTilePosition(tilePosition);
        
        EditorUtility.SetDirty(gameObject);
    }
    
    public int GetTileObjectIndex(int x, int y)
    {
        if (tileObjectIndexArray == null) return 0;
        return tileObjectIndexArray[x, y];
    }

    public void DestroyAllTiles()
    {
        DestroyImmediate(GridParent.gameObject);
    }
}
