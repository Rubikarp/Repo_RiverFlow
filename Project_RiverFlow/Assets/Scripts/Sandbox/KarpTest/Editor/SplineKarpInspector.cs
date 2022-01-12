using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEditor;

//[CanEditMultipleObjects]
[CustomEditor(typeof(SplineKarp))]
public class SplineKarpInspector : Editor
{

    SerializedProperty splinePointsProp;
    ReorderableList splinePointsList;

    private Transform self;
    private SplineKarp objectLinked;

    // Start like
    private void OnEnable()
    {
        splinePointsProp = serializedObject.FindProperty(nameof(SplineKarp.points));
        objectLinked = target as SplineKarp;
        self = objectLinked.transform;

        #region reordable list
        //Initialise la liste
        splinePointsList = new ReorderableList(serializedObject, splinePointsProp, true, true, true, true);
        //linker à une serielizedProperty (une collection, array, liste,etc)
        //Preparer les callbacks
        splinePointsList.drawElementCallback += ElementDrawer;
        splinePointsList.drawHeaderCallback += HeaderDrawer;
        ///
        splinePointsList.onAddCallback += AddCallBack;
        splinePointsList.onRemoveCallback += RemoveCallBack;
        ///
        //splinePointsList.elementHeightCallback += ElementHeightCallBack;
    }


    void HeaderDrawer(Rect rect)
    {
        EditorGUI.LabelField(rect, "Spline Points");
    }
    void ElementDrawer(Rect rect, int index, bool isActive, bool isFocused)
    {
        EditorGUI.PropertyField(rect, splinePointsProp.GetArrayElementAtIndex(index));
    }
    void AddCallBack(ReorderableList rList)
    {
        splinePointsProp.arraySize++;
        var prop = splinePointsProp.GetArrayElementAtIndex(splinePointsProp.arraySize-1);
        if(splinePointsProp.arraySize - 2 <= 0)
        {
            prop.FindPropertyRelative(nameof(SplinePoint.pos)).vector3Value = Vector3.zero;
            prop.FindPropertyRelative(nameof(SplinePoint.tangDir)).vector3Value = Vector3.right;
            prop.FindPropertyRelative(nameof(SplinePoint.tangStrenght)).floatValue = 0.5f;
        }
        else
        {
            var previousPoint = splinePointsProp.GetArrayElementAtIndex(splinePointsProp.arraySize - 2);
            prop.FindPropertyRelative(nameof(SplinePoint.pos)).vector3Value = Vector3.up + previousPoint.FindPropertyRelative(nameof(SplinePoint.pos)).vector3Value;
            prop.FindPropertyRelative(nameof(SplinePoint.tangDir)).vector3Value = -1f * previousPoint.FindPropertyRelative(nameof(SplinePoint.tangDir)).vector3Value.normalized;
            prop.FindPropertyRelative(nameof(SplinePoint.tangStrenght)).floatValue = previousPoint.FindPropertyRelative(nameof(SplinePoint.tangStrenght)).floatValue;
        }
    }
    void RemoveCallBack(ReorderableList rList)
    {
        splinePointsProp.DeleteArrayElementAtIndex(rList.index);
    }
    float ElementHeightCallBack(int index)
    {
        float numberOfLine = 4;
        return (EditorGUIUtility.singleLineHeight * numberOfLine) + 1;
    }
    #endregion

    // Update like
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.FlexibleSpace();

        serializedObject.Update();

        splinePointsList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }

    private void OnSceneGUI()
    {
        //serializedObject copy ma target
        serializedObject.Update();

        float zoomScaler = HandleUtility.GetHandleSize(self.position);

        Matrix4x4 matrix = Matrix4x4.TRS(self.position, self.rotation, self.lossyScale);
        using (new Handles.DrawingScope(Color.green))
        {
            for (int i = 0; i < splinePointsProp.arraySize; i++)
            {
                SerializedProperty currentPoint = splinePointsProp.GetArrayElementAtIndex(i);
                SerializedProperty currentPointPos = currentPoint.FindPropertyRelative(nameof(SplinePoint.pos));
                SerializedProperty currentPointTangDir = currentPoint.FindPropertyRelative(nameof(SplinePoint.tangDir));
                SerializedProperty currentPointTangStrength = currentPoint.FindPropertyRelative(nameof(SplinePoint.tangStrenght));

                Vector3 position = currentPointPos.vector3Value;
                Vector3 tangDir = currentPointTangDir.vector3Value;
                Vector3 inverseTangDir = -1f * currentPointTangDir.vector3Value;
                float strength = currentPointTangStrength.floatValue;

                #region Tangente
                Handles.color = Color.red;

                tangDir = currentPointTangDir.vector3Value;
                tangDir = Handles.FreeMoveHandle(position + (tangDir * strength), Quaternion.identity, 0.15f * zoomScaler, Vector3.one, Handles.SphereHandleCap) - position;
                //Update the Dir
                tangDir = Vector3.Scale(tangDir, new Vector3(1, 1, 0));
                currentPointTangDir.vector3Value = tangDir.normalized;
                //Update the strength
                strength = Mathf.Abs(tangDir.magnitude);
                currentPointTangStrength.floatValue = strength;
                //draw the line
                Handles.DrawLine(position, position + tangDir, 0.5f * zoomScaler);
                #endregion

                #region InverseTangente
                Handles.color = Color.blue;

                inverseTangDir = -1f * currentPointTangDir.vector3Value;
                inverseTangDir = Handles.FreeMoveHandle(position + (inverseTangDir * strength), Quaternion.identity, 0.15f * zoomScaler, Vector3.one, Handles.SphereHandleCap) - position;
                //Update the Dir
                inverseTangDir = Vector3.Scale(inverseTangDir, new Vector3(1, 1, 0));
                currentPointTangDir.vector3Value = -inverseTangDir.normalized;
                //Update the strength
                strength = Mathf.Abs(inverseTangDir.magnitude);
                currentPointTangStrength.floatValue = strength;
                //draw the line
                Handles.DrawLine(position, position + inverseTangDir, 0.5f * zoomScaler);
                #endregion

                #region Position
                Handles.color = Color.green;

                position = currentPointPos.vector3Value;
                position = Handles.FreeMoveHandle(position, Quaternion.identity, 0.3f * zoomScaler, Vector3.one, Handles.SphereHandleCap);
                position = Vector3.Scale(position, new Vector3(1, 1, 0));
                currentPointPos.vector3Value = position;
                #endregion
            }
        }
        //target recois serializedObject values (comprend le set dirty et le 
        serializedObject.ApplyModifiedProperties();
    }
}