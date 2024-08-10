using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class GridCreator : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    
    [Min(0)] [SerializeField] private int width;
    [Min(0)] [SerializeField] private int height;
    [SerializeField] private float tileSize = 1.1f;
    public int Width => width;
    public int Height => height;
    public float TileSize => tileSize;
    
    [SerializeField] public GameObject[] TileObjects;
    
    private int[,] tileObjectIndexArray;

    public GameObject GridParent;
    
    public GameObject TileObjectsParent;
    
    public void CreateGrid()
    {
        tileObjectIndexArray = new int[width, height];
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject tile = PrefabUtility.InstantiatePrefab(tilePrefab) as GameObject;
                SetTilePosition(tile.transform, x, y);
                tile.transform.parent = transform;
            }
        }
            
        EditorUtility.SetDirty(gameObject);
    }
    
    public GameObject GetTileObject(int index)
    {
        return TileObjects[index];
    }

    public void UpdateAllTilesPosition()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                SetTilePosition(transform.GetChild(x * height + y), x, y);
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
        
        width = 0;
        height = 0;
        tileSize = 1;
    }

    public void DestroyAllTiles()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
}
