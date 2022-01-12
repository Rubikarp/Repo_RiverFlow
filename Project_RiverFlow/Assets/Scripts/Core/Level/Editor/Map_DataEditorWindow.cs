using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Map_DataEditorWindow : EditorWindow
{
    Map_Data currentLD;
    SerializedObject serializedObject;
    //
    SerializedProperty gridFieldTypeProp;
    SerializedProperty sizeProp;
    int width, height;
    //
    SerializedProperty tileColorProp;
    SerializedProperty selectedTileType;

    float marginRatio;
    bool isClicking;
    Vector2 mousePos;
    Rect rectIn;
    Vector2 scrollPos;
    //TODO
    //int brushSize = 1;

    public void InitWindow(Map_Data _currentLD)
    {
        currentLD = _currentLD;
        serializedObject = new SerializedObject(currentLD);

        //Link variables to property
        gridFieldTypeProp = serializedObject.FindProperty(nameof(Map_Data.gridFieldType));

        sizeProp = serializedObject.FindProperty(nameof(Map_Data.gridSize));
        width = sizeProp.vector2IntValue.x;
        height = sizeProp.vector2IntValue.y;

        tileColorProp = serializedObject.FindProperty(nameof(Map_Data.tileColor));
        selectedTileType = serializedObject.FindProperty(nameof(Map_Data.selectedType));

        marginRatio = 0.1f;
    }

    //GUI is call 2 time per frame and is null on the second frame
    void OnGUI()
    {
        ProcessEvent();
        serializedObject.Update();
        width = sizeProp.vector2IntValue.x;
        height = sizeProp.vector2IntValue.y;

        //Info
        using (new EditorGUI.DisabledGroupScope(true))
        {
            EditorGUILayout.ObjectField("Edited Map : ", currentLD, typeof(Map_Data));
            //Debug
            //EditorGUILayout.Vector2Field("Debug Mouse Pos", Event.current.mousePosition);
        }
        EditorGUILayout.Space(1 * EditorGUIUtility.singleLineHeight);
        //Show actual Brush
        using (new GUILayout.HorizontalScope())
        {
            using (new EditorGUI.DisabledGroupScope(true))
            {
                EditorGUILayout.PropertyField(selectedTileType);
                using (new GUILayout.HorizontalScope())
                {
                    Rect zone = EditorGUILayout.GetControlRect();
                    EditorGUI.DrawRect(zone, tileColorProp.GetArrayElementAtIndex(selectedTileType.enumValueIndex).colorValue);
                }
            }
        }
        //Choose Brush
        using (new GUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Herbe", EditorStyles.miniButton))
            {
                selectedTileType.enumValueIndex = 1;
            }
            if (GUILayout.Button("Argileux", EditorStyles.miniButton))
            {
                selectedTileType.enumValueIndex = 2;
            }
            if (GUILayout.Button("Désert", EditorStyles.miniButton))
            {
                selectedTileType.enumValueIndex = 3;
            }
            if (GUILayout.Button("Mountain", EditorStyles.miniButton))
            {
                selectedTileType.enumValueIndex = 4;
            }
        }
        //Fill Option
        if (GUILayout.Button("Fill grid by selected Brush", EditorStyles.miniButton))
        {
            for (int y = 0; y < currentLD.gridSize.y; y++)
            {
                for (int x = 0; x < currentLD.gridSize.x; x++)
                {
                    gridFieldTypeProp.GetArrayElementAtIndex(y * currentLD.gridSize.x + x).enumValueIndex = selectedTileType.enumValueIndex;
                }
            }
        }
        //edit Brush
        using (new GUILayout.HorizontalScope())
        {
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                EditorGUILayout.PropertyField(sizeProp);
                if (check.changed)
                {
                    //GridSize Min Size (1,1)
                    if (sizeProp.vector2IntValue.x < 1)
                    {
                        sizeProp.vector2IntValue = new Vector2Int(1, sizeProp.vector2IntValue.y);
                    }
                    if (sizeProp.vector2IntValue.y < 1)
                    {
                        sizeProp.vector2IntValue = new Vector2Int(sizeProp.vector2IntValue.x, 1);
                    }

                    width = sizeProp.vector2IntValue.x;
                    height = sizeProp.vector2IntValue.y;

                    //target recois serializedObject values (comprend le set dirty et le 
                    serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(currentLD);
                }
            }
        }
        EditorGUILayout.Space(1 *EditorGUIUtility.singleLineHeight);

        #region Edit Area
        //Error Check
        if (currentLD.gridSize.x < 0) return;
        if (currentLD.gridSize.y < 0) return;
        //Grid Size check
        if (gridFieldTypeProp.arraySize != width * height)
        {
            gridFieldTypeProp.arraySize = width * height;
        }

        //Define Area
        Rect nextRect = EditorGUILayout.GetControlRect();
        float totalWidth = EditorGUIUtility.currentViewWidth;

        float border = totalWidth * marginRatio;
        float gridWidth = totalWidth - (2f * border);

        Rect gridArea = new Rect(nextRect.x + border, nextRect.y , gridWidth, gridWidth * ((float)height / (float)width));

        float maxHeightAvailable = position.height - nextRect.y - (2 * EditorGUIUtility.singleLineHeight);
        float visibleAreaHeight = Mathf.Min(gridArea.height, maxHeightAvailable);

        Rect visibleArea = new Rect(gridArea.x, nextRect.y, gridArea.width + 15, visibleAreaHeight);

        //Debug
        EditorGUI.DrawRect(visibleArea, new Color(1f, 1f, 0f, 0.1f));
        EditorGUI.DrawRect(gridArea, new Color(0.5f, 0.5f, 0.5f, 0.2f));

        scrollPos = GUI.BeginScrollView(visibleArea, scrollPos, gridArea);
        using (new GUILayout.VerticalScope())
        {
            #region Draw Grid
            int spaceBtwCell = 4;
            float totalCellWidth = gridWidth + (width * spaceBtwCell);

            float cellWidth = totalCellWidth / (float)width;
            float totalSpaceWitdh = gridWidth - totalCellWidth;

            float spaceWidth = totalSpaceWitdh / ((float)width + 1);

            float curY = gridArea.y;
            for (int y = 0; y < currentLD.gridSize.y; y++)
            {
                curY += spaceWidth;

                float curX = gridArea.x;
                for (int x = 0; x < currentLD.gridSize.x; x++)
                {
                    curX += spaceWidth;

                    Rect rect = new Rect(curX, curY, cellWidth, cellWidth);
                    curX += cellWidth;

                    int tileIndex = y * currentLD.gridSize.x + x;

                    //Utilisateur peint
                    if (nextRect.y != 0)
                    {
                        if (rect.Contains(mousePos + scrollPos))
                        {
                            if (isClicking)
                            {
                                gridFieldTypeProp.GetArrayElementAtIndex(tileIndex).enumValueIndex = selectedTileType.enumValueIndex;

                                int debugBorder = 0;
                                Rect debugRect = new Rect(rect.x - debugBorder, rect.y - debugBorder, rect.width + 2 * debugBorder, rect.height + 2 * debugBorder);

                                EditorGUI.DrawRect(debugRect, Color.magenta);

                                // Debug.Log("The tile : " + x + " " + y + " " + "in rect " + rect + "have value changed" + " at pose " + mousePos);
                            }
                        }
                    }

                    //Draw tile
                    int enumIndexPalette = gridFieldTypeProp.GetArrayElementAtIndex(tileIndex).enumValueIndex;
                    Color rendColor = tileColorProp.GetArrayElementAtIndex(enumIndexPalette).colorValue;
                    EditorGUI.DrawRect(rect, rendColor);
                    //EditorGUI.LabelField(rect, "Pos" + rect.x + "/" + rect.y + "\n Size" + rect.width + "/" + rect.height);

                }
                curY += cellWidth;
            }
            #endregion
        }
        // End the scroll view that we began above.
        GUI.EndScrollView();
        GUILayout.Space(visibleArea.height);
        #endregion

        Repaint();
        serializedObject.ApplyModifiedProperties();
    }

    void ProcessEvent()
    {
        mousePos = Event.current.mousePosition;

        if (Event.current.type == EventType.MouseDown)
        {
            isClicking = true;
        }
        if (Event.current.type == EventType.MouseUp)
        {
            isClicking = false;
        }
    }
}
