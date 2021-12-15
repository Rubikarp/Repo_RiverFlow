using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LD_DataEditorWindow : EditorWindow
{
    LD_Data currentLD;
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

    public void InitWindow(LD_Data _currentLD)
    {
        currentLD = _currentLD;
        serializedObject = new SerializedObject(currentLD);

        //Link variables to property
        gridFieldTypeProp = serializedObject.FindProperty(nameof(LD_Data.gridFieldType));

        sizeProp = serializedObject.FindProperty(nameof(LD_Data.gridSize));
        width = sizeProp.vector2IntValue.x;
        height = sizeProp.vector2IntValue.y;

        tileColorProp = serializedObject.FindProperty(nameof(LD_Data.tileColor));
        selectedTileType = serializedObject.FindProperty(nameof(LD_Data.selectedType));

        marginRatio = 0.1f;
    }

    void OnGUI()
    {
        ProcessEvent();
        serializedObject.Update();
        width = sizeProp.vector2IntValue.x;
        height = sizeProp.vector2IntValue.y;

        using (new EditorGUI.DisabledGroupScope(true))
        {
            EditorGUILayout.Space();
            EditorGUILayout.ObjectField("Current LD", currentLD, typeof(LD_Data));
            EditorGUILayout.Vector2Field("Debug Mouse Pos", Event.current.mousePosition);
        }
        using (new GUILayout.HorizontalScope())
        {
            EditorGUILayout.PropertyField(selectedTileType);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(tileColorProp);
        }
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

        EditorGUILayout.Space();

        float totalWidth = EditorGUIUtility.currentViewWidth;
        float gridWidth = totalWidth * (1f - 2f * marginRatio);

        //GUI is call 2 time per frame and is null on the second frame
        Rect nextRect = EditorGUILayout.GetControlRect();
        //if (nextRect.y == 0)
        ///{ return; }

        Rect area = new Rect(nextRect.x + totalWidth * marginRatio, nextRect.y, gridWidth, gridWidth);
        EditorGUI.DrawRect(area, new Color(0.5f, 0.5f, 0.5f, 0.2f));

        if (currentLD.gridSize.x < 0) return;
        if (currentLD.gridSize.y < 0) return;
        if (gridFieldTypeProp.arraySize != width * height)
        {
            gridFieldTypeProp.arraySize = width * height;
        }

        using (new GUILayout.VerticalScope(GUILayout.Height(area.height), GUILayout.Width(area.height)))
        {

            #region Draw Square Grid
            float cellToSpaceRatio = 4f;
            float totalCellWidth = gridWidth * (cellToSpaceRatio) / (cellToSpaceRatio + 1f);

            float cellWidth = totalCellWidth / (float)height;
            float totalSpaceWitdh = gridWidth - totalCellWidth;

            float spaceWidth = totalSpaceWitdh / ((float)width + 1);

            float curY = area.y;
            for (int y = 0; y < currentLD.gridSize.x; y++)
            {
                curY += spaceWidth;

                float curX = area.x;
                for (int x = 0; x < currentLD.gridSize.y; x++)
                {
                    curX += spaceWidth;

                    Rect rect = new Rect(curX, curY, cellWidth, cellWidth);
                    curX += cellWidth;

                    int tileIndex = y * currentLD.gridSize.y + x;

                    //Utilisateur peint
                    bool isPaintingOverThis = false;
                    if (nextRect.y != 0)
                    {
                        if (rect.Contains(mousePos))
                        {
                            if (isClicking)
                            {
                                isPaintingOverThis = true;
                            }
                        }
                    }
                    if (isPaintingOverThis)
                    {
                        gridFieldTypeProp.GetArrayElementAtIndex(tileIndex).enumValueIndex = selectedTileType.enumValueIndex;

                        EditorGUI.DrawRect(rect, Color.red);
                        EditorGUI.DrawRect(new Rect(mousePos.x, mousePos.y, 10, 10), Color.yellow);

                        /*Debug.Log("The tile : " + x + " " + y + " " +
                            "in rect " + rect + "have value changed" +
                            " at pose " + mousePos);*/
                    }

                    //Draw tile
                    int enumIndexPalette = gridFieldTypeProp.GetArrayElementAtIndex(tileIndex).enumValueIndex;
                    Color rendColor = tileColorProp.GetArrayElementAtIndex(enumIndexPalette).colorValue;
                    EditorGUI.DrawRect(rect, rendColor);
                    EditorGUI.LabelField(rect, "Pos" + rect.x + "/" + rect.y + "\n Size" + rect.width + "/" + rect.height);

                }
                curY += cellWidth;
                GUILayout.Space(5);
            }
            #endregion
        }

        if (GUILayout.Button("Update Array Size Manually", EditorStyles.miniButton))
        {
            gridFieldTypeProp.arraySize = width * height;
        }

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
