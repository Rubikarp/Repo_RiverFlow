using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(LD_Data))]
public class LD_DataInspector : Editor
{
    SerializedProperty gridSize;
    SerializedProperty tileColor;

    // Start like
    private void OnEnable()
    {
        //Link variables to property
        gridSize = serializedObject.FindProperty(nameof(LD_Data.gridSize));
        tileColor = serializedObject.FindProperty(nameof(LD_Data.tileColor));
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

        if (GUILayout.Button("Open Editor Window", EditorStyles.miniButton))
        {
            OpenWindow();
        }

    }

    private void OpenWindow()
    {
        //Dock to Inspector
        LD_DataEditorWindow myWindow;

        if (!EditorWindow.HasOpenInstances<LD_DataEditorWindow>())
        {
            myWindow = EditorWindow.CreateWindow<LD_DataEditorWindow>("Level Editor Window", new Type[] {typeof(SceneView)});
        }
        else
        {
            myWindow = EditorWindow.GetWindow(typeof(LD_DataEditorWindow)) as LD_DataEditorWindow;
        }

        myWindow.InitWindow(target as LD_Data);
        myWindow.Show();
    }

}
