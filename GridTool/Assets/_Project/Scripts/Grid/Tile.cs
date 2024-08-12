using UnityEngine;

public class Tile : MonoBehaviour
{
    private Vector2Int tilePosition;
    
    private ITileObject tileObject;
    
    public void SetTilePosition(Vector2Int tilePosition)
    {
        this.tilePosition = tilePosition;
    }
    
    public Vector2Int GetTilePosition()
    {
        return tilePosition;
    }
    
    public void SetTileObject(ITileObject tileObject)
    {
        this.tileObject = tileObject;
    }
    
    public ITileObject GetTileObject()
    {
        return tileObject;
    }
    
    public bool IsTileEmpty()
    {
        return tileObject == null;
    }
}
