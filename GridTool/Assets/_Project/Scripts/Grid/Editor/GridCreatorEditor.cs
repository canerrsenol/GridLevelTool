using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridCreator))]
public class GridCreatorEditor : Editor
{
    private int previousWidth;
    private int previousHeight;
    private float previousTileSize;

    private GridCreator gridCreator;
    
    private void OnEnable()
    {
        gridCreator = (GridCreator)target;
        previousWidth = gridCreator.GridSize.x;
        previousHeight = gridCreator.GridSize.y;
        previousTileSize = gridCreator.TileSize;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if (previousWidth != gridCreator.GridSize.x
            || previousHeight != gridCreator.GridSize.y)
        {
            gridCreator.DestroyAllTiles();
            gridCreator.CreateGrid();
            
            previousHeight = gridCreator.GridSize.x;
            previousWidth = gridCreator.GridSize.y;
        }

        if (!Mathf.Approximately(previousTileSize, gridCreator.TileSize))
        {
            gridCreator.UpdateAllTilesPosition();
            previousTileSize = gridCreator.TileSize;
        }
        
        if (GUILayout.Button("Clear Grid"))
        {
            gridCreator.DestroyAllTiles();
            gridCreator.ResetGridSettings();
        }

        if (gridCreator.GridSize.x > 0 && gridCreator.GridSize.y > 0)
        {
            float inspectorWidth = EditorGUIUtility.currentViewWidth * .8f;
            float buttonSize = inspectorWidth / gridCreator.GridSize.x;
            
            for (int y = gridCreator.GridSize.y - 1; y >= 0; y--)
            {
                GUILayout.BeginHorizontal();
                
                for (int x = 0; x < gridCreator.GridSize.x; x++)
                {
                    int index = gridCreator.GetTileObjectIndex(x, y);
                    Texture2D previewTexture = AssetPreview.GetAssetPreview(gridCreator.GetTileObject(index));
                        
                    if (GUILayout.Button(previewTexture, GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
                    {
                        Debug.Log($"Button at [{x}, {y}] clicked!");
                        gridCreator.SetNextTileObject(new Vector2Int(x, y));
                    }
                }
                GUILayout.EndHorizontal();
            }
        }
    }
}
