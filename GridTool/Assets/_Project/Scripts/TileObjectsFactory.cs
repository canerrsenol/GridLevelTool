using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "TileObjectsFactory", menuName = "TileObjectsFactory")]
public class TileObjectsFactory : ScriptableObject
{
    [SerializeField] private GameObject[] tileObjects;
    
    public int GetTileObjectsCount()
    {
        return tileObjects.Length;
    }
    
    public GameObject GetTileObjectPrefabAtIndex(int index)
    {
        return tileObjects[index];
    }

    public GameObject SpawnTileObject(int index)
    {
        return PrefabUtility.InstantiatePrefab(tileObjects[index]) as GameObject; 
    }
}
