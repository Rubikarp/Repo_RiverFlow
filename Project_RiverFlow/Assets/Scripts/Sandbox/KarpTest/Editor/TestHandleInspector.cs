using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CustomEditor(typeof(RuntimePath))]
public class TestHandleInspector : Editor
{
    SerializedProperty waypoints;
    SerializedProperty radius;

    private RuntimePath script;

    // Start like
    private void OnEnable()
    {
        //Recup linkedScript
        script = target as RuntimePath;

        //Link variables to property
        waypoints = serializedObject.FindProperty(nameof(RuntimePath.waypoints));
        radius = serializedObject.FindProperty(nameof(RuntimePath.radius));
    }

    // Update like
    public override void OnInspectorGUI()
    {
        //serializedObject copy ma target
        serializedObject.Update();

        EditorGUILayout.PropertyField(waypoints);
        EditorGUILayout.PropertyField(radius);

        //target recois serializedObject values (comprend le set dirty et le 
        serializedObject.ApplyModifiedProperties();
    }

    private void OnSceneGUI()
    {
        //Permet d'avoir une screen size constante
        ///à calculer dans la matrice par défaut
        float distRatio = HandleUtility.GetHandleSize(script.transform.position);

        //Pour les Handle
        /// Handle.Draw___ = juste repère visuel
        /// Handle.___Handle = repère intéractif
        /// Handle.___HandleCap = argument visuel du HandleInteractif

        Matrix4x4 matrix = Matrix4x4.TRS(script.transform.position, script.transform.rotation, script.transform.lossyScale);
        using (new Handles.DrawingScope(Color.red, matrix))
        {
            Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;

            #region DrawLine
            //Handles.DrawLine(Vector3.zero, Vector3.up * 2 * distRatio, 10f);
            #endregion
            #region Radius
            /*
            radius.floatValue = Handles.RadiusHandle(Quaternion.identity, Vector3.zero, radius.floatValue);
            GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
            style.fontSize = 20 * (int)distRatio;
            style.normal.textColor = Color.red;
            Handles.Label(Vector3.up * 1.1f * radius.floatValue, radius.floatValue.ToString(), style);
            */
            #endregion

            #region template
            #endregion
        }
        using (new Handles.DrawingScope())
        {
            using (new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Dingue de l'UI dans la scene"))
                {
                    Debug.Log("Dingue de l'UI dans la scene");
                }
            }
            #region test cap and other Handle
            Vector3 offSet = Vector3.one * 5;
            /*
            //Free Handle (de la merde)
            script.transform.position = Handles.FreeMoveHandle(script.transform.position, script.transform.rotation, 0.3f * distRatio,
            Vector3.zero, Handles.ConeHandleCap);*/
            //script.transform.rotation = Handles.FreeRotateHandle(script.transform.rotation, script.transform.position, 0.3f * distRatio);

            //Position
            //script.transform.position = -offSet + Handles.PositionHandle( script.transform.position + offSet, script.transform.rotation);
            //Handles.DoPositionHandle(script.transform.position, script.transform.rotation);

            //Scale
            //script.transform.localScale = Handles.ScaleHandle(script.transform.localScale,script.transform.position + Vector3.one, script.transform.rotation , 2*distRatio);

            //Transform
            /*
            Vector3 pos = script.transform.position + offSet;
            Quaternion rot = script.transform.rotation;
            Vector3 scale = script.transform.localScale;
            Handles.TransformHandle(ref pos, ref rot, ref scale);
            script.transform.position = pos - offSet;
            script.transform.rotation = rot;
            script.transform.localScale = scale;
            */

            //Scale Value = radius mais pour une flèche
            //radius.floatValue = Handles.ScaleValueHandle(radius.floatValue, script.transform.position, script.transform.rotation, radius.floatValue * distRatio, Handles.ConeHandleCap, 1f);

            //Button
            /*
            if(Handles.Button(script.transform.position + Vector3.up * 3, script.transform.rotation, 2, 3, Handles.CubeHandleCap))
            {
                Debug.Log("bim");
            }
            */
            #endregion
        }


        #region default Situation
        /*
        //if not using scope 
        Matrix4x4 defaultmatrix = Handles.matrix;
        Color colorTemp = Handles.color;
        Handles.BeginGUI();

        //The using parameter  do 
        //Matrix4x4 matrix = Matrix4x4.TRS(script.transform.position, script.transform.rotation, script.transform.lossyScale);
        //Handles.matrix = matrix;
        //Handles.color = Color.red;

        Handles.EndGUI();
        //if not using scope 
        Handles.matrix = defaultmatrix;
        Handles.color = colorTemp;
        */
        #endregion

        //L'équivalent d'un raycast
        //HandleUtility.RaySnap(Ray ray);
        //GameObject = HandleUtility.PickGameObject(Vector2 position, bool selectPrefabRoot);

        //target recois serializedObject values (comprend le set dirty et le 
        serializedObject.ApplyModifiedProperties();
    }
}
