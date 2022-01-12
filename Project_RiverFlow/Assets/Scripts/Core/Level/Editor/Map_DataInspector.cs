using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(Map_Data))]
public class Map_DataInspector : Editor
{
    public TilePalette_SCO palette;
    SerializedProperty paletteProp;

    SerializedProperty gridSize;
    SerializedProperty tileColor;

    // Start like
    private void OnEnable()
    {
        //Link variables to property
        gridSize = serializedObject.FindProperty(nameof(Map_Data.gridSize));
        tileColor = serializedObject.FindProperty(nameof(Map_Data.tileColor));

    }

    // Update like
    public override void OnInspectorGUI()
    {
        //serializedObject copy ma target
        serializedObject.Update();

        using (var check = new EditorGUI.ChangeCheckScope())
        {
            EditorGUILayout.PropertyField(gridSize);
            EditorGUILayout.PropertyField(tileColor);
            if (check.changed)
            {
                //GridSize Min Size (1,1)
                if(gridSize.vector2IntValue.x < 1)
                {
                    gridSize.vector2IntValue = new Vector2Int(1, gridSize.vector2IntValue.y);
                }
                if (gridSize.vector2IntValue.y < 1)
                {
                    gridSize.vector2IntValue = new Vector2Int(gridSize.vector2IntValue.x ,1);
                }

                //target recois serializedObject values (comprend le set dirty et le 
                serializedObject.ApplyModifiedProperties();
            }
        }
        using (new GUILayout.HorizontalScope())
        {
            palette = (TilePalette_SCO)EditorGUILayout.ObjectField(palette, typeof(TilePalette_SCO), true);

            if (GUILayout.Button("UpdateColor", EditorStyles.miniButton))
            {
                UpdateColor();
                serializedObject.ApplyModifiedProperties();
            }
        }

        if (GUILayout.Button("Open Editor Window", EditorStyles.miniButton, GUILayout.Height(3 * EditorGUIUtility.singleLineHeight)))
        {
            OpenWindow();
        }
    }

    private void OpenWindow()
    {
        //Dock to Inspector
        Map_DataEditorWindow myWindow;

        if (!EditorWindow.HasOpenInstances<Map_DataEditorWindow>())
        {
            myWindow = EditorWindow.CreateWindow<Map_DataEditorWindow>("Level Editor Window", new Type[] {typeof(SceneView)});
        }
        else
        {
            myWindow = EditorWindow.GetWindow(typeof(Map_DataEditorWindow)) as Map_DataEditorWindow;
        }

        myWindow.InitWindow(target as Map_Data);
        myWindow.Show();
    }

    public void UpdateColor()
    {
        tileColor.GetArrayElementAtIndex(0).colorValue = palette.errorMat;
        tileColor.GetArrayElementAtIndex(1).colorValue = palette.groundGrass;
        tileColor.GetArrayElementAtIndex(2).colorValue = palette.groundClay;
        tileColor.GetArrayElementAtIndex(3).colorValue = palette.groundAride;
        tileColor.GetArrayElementAtIndex(4).colorValue = palette.mountain;
    }

}
