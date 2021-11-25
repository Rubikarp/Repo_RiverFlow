using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CanEditMultipleObjects]
[CustomEditor(typeof(RiverSpline))]
public class RiverSplineInspector : Editor
{
    SerializedProperty splinePointsProp;

    private Transform self;
    private RiverSpline objectLinked;

    // Start like
    private void OnEnable()
    {
        splinePointsProp = serializedObject.FindProperty(nameof(RiverSpline.points));
        objectLinked = target as RiverSpline;
        self = objectLinked.transform;
    }

    // Update like
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.FlexibleSpace();

        serializedObject.Update();

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
                SerializedProperty currentPointPos = currentPoint.FindPropertyRelative(nameof(RiverPoint.pos));
                SerializedProperty currentPointTangDir = currentPoint.FindPropertyRelative(nameof(RiverPoint.tangDir));
                SerializedProperty currentPointTangStrength = currentPoint.FindPropertyRelative(nameof(RiverPoint.tangStrenght));

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
