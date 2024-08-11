using UnityEngine;

public class TileObstacle : MonoBehaviour, ITileObject
{
    public Vector2Int TilePosition { get; set; }

    public void SetTilePosition(Vector2Int position)
    {
        TilePosition = position;
    }

    public Vector2Int GetTilePosition()
    {
        return TilePosition;
    }
}
