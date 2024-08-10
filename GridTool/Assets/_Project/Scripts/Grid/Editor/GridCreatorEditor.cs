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
        previousWidth = gridCreator.Width;
        previousHeight = gridCreator.Height;
        previousTileSize = gridCreator.TileSize;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if (previousWidth != gridCreator.Width
            || previousHeight != gridCreator.Height)
        {
            gridCreator.DestroyAllTiles();
            gridCreator.CreateGrid();
            
            previousHeight = gridCreator.Height;
            previousWidth = gridCreator.Width;
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

        if (gridCreator.Width > 0 && gridCreator.Height > 0)
        {
            // Calculate width and height for dynamic grid layout
            float inspectorWidth = EditorGUIUtility.currentViewWidth * .8f;
            float buttonSize = inspectorWidth / gridCreator.Width;
            
            for (int y = gridCreator.Height - 1; y >= 0; y--)
            {
                GUILayout.BeginHorizontal(); // Start a new row
                
                for (int x = 0; x < gridCreator.Width; x++)
                {
                    // int index = gridCreator.TileObjectIndex[x, y];
                    //if (index >= 0 && index < gridCreator.TileObjects.Length)
                    //{
                        Texture2D previewTexture = AssetPreview.GetAssetPreview(gridCreator.GetTileObject(0));
                        
                        // Display as a button
                        if (GUILayout.Button(previewTexture, GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
                        {
                            // Action when button is clicked
                            Debug.Log($"Button at [{x}, {y}] clicked!");
                            // You can call a method or set a property here
                        }
                    //}
                }
                
                GUILayout.EndHorizontal(); // End the current row
            }
        }
    }
}
