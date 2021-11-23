using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(RuntimePath))]
public class RunTimePtahInspector : Editor
{
    SerializedProperty waypoints;
    SerializedProperty radius;

    ReorderableList rList;

    private RuntimePath objectLinked;
    private Transform tr;

    // Start like
    private void OnEnable()
    {
        //Recup linkedScript
        objectLinked = target as RuntimePath;
        tr = objectLinked.transform;

        //Link variables to property
        waypoints = serializedObject.FindProperty(nameof(RuntimePath.waypoints));
        radius = serializedObject.FindProperty(nameof(RuntimePath.radius));

        #region reordable list
        //Initialise la liste
        rList = new ReorderableList(serializedObject, waypoints, true, true, true, true);
        //linker à une serielizedProperty (une collection, array, liste,etc)
        //Preparer les callbacks
        rList.drawElementCallback += ElementDrawer;
        rList.drawHeaderCallback += HeaderDrawer;

        rList.onAddCallback += AddCallBack;
        //rList.onAddDropdownCallback += AddDropDownCallBack;
        rList.onRemoveCallback += RemoveCallBack;

        //rList.onReorderCallback += ReorderCallback;
        rList.elementHeightCallback += ElementHeightCallBack;
    }

    void HeaderDrawer(Rect rect)
    {
        EditorGUI.LabelField(rect, "Titre de la liste");
    }
    void ElementDrawer(Rect rect, int index, bool isActive, bool isFocused)
    {
        EditorGUI.PropertyField(rect, waypoints.GetArrayElementAtIndex(index));
    }

    void AddCallBack(ReorderableList rList)
    {
        waypoints.arraySize++;
    }
    void AddDropDownCallBack(ReorderableList rList)
    {

    }

    void RemoveCallBack(ReorderableList rList)
    {
        waypoints.DeleteArrayElementAtIndex(rList.index);
    }
    void ReorderCallback(ReorderableList rList)
    {

    }

    float ElementHeightCallBack(int index)
    {
        float numberOfLine = EditorGUIUtility.currentViewWidth < 334 ? 2 : 1;
        return (EditorGUIUtility.singleLineHeight * numberOfLine) + 1 ;
    }
    #endregion

    // Update like
    public override void OnInspectorGUI()
    {
        //serializedObject copy ma target
        serializedObject.Update();

        rList.DoLayoutList();
        //Version GUI pur
        //rList.DoList();

        //EditorGUILayout.PropertyField(waypoints);
        EditorGUILayout.PropertyField(radius);

        //target recois serializedObject values (comprend le set dirty et le 
        serializedObject.ApplyModifiedProperties();
    }

    private void OnSceneGUI()
    {
        //serializedObject copy ma target
        serializedObject.Update();

        float distRatio = HandleUtility.GetHandleSize(tr.position);
        Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;


        Matrix4x4 matrix = Matrix4x4.TRS(tr.position, tr.rotation, tr.lossyScale);
        using (new Handles.DrawingScope(Color.red, matrix))
        {

            //Link wayPoints
            for (int i = 0; i < waypoints.arraySize; i++)
            {
                int next = (i + 1) % waypoints.arraySize;
                int next2 = (i + 2) % waypoints.arraySize;
                int previous = (i - 1)<0? (i - 1) + waypoints.arraySize : (i - 1);

                Vector3 startTangeante = waypoints.GetArrayElementAtIndex(next).vector3Value - waypoints.GetArrayElementAtIndex(previous).vector3Value;
                Vector3 endTangeante = waypoints.GetArrayElementAtIndex(i).vector3Value - waypoints.GetArrayElementAtIndex(next2).vector3Value;

                //Handles.DrawLine(waypoints.GetArrayElementAtIndex(i).vector3Value, waypoints.GetArrayElementAtIndex(next).vector3Value, 10f);
                Handles.DrawBezier(
                    waypoints.GetArrayElementAtIndex(i).vector3Value, 
                    waypoints.GetArrayElementAtIndex(next).vector3Value, 
                    waypoints.GetArrayElementAtIndex(i).vector3Value + startTangeante * radius.floatValue, 
                    waypoints.GetArrayElementAtIndex(next).vector3Value  + endTangeante * radius.floatValue, 
                    Color.red, new Texture2D(1,1),3f);
            }

            for (int i = 0; i < waypoints.arraySize; i++)
            {
                waypoints.GetArrayElementAtIndex(i).vector3Value = Handles.PositionHandle(waypoints.GetArrayElementAtIndex(i).vector3Value, Quaternion.identity);
                Vector3 pointPos = waypoints.GetArrayElementAtIndex(i).vector3Value;
                //Show number
                GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
                style.fontSize = 20 ;
                style.normal.textColor = Color.blue;
                Handles.Label(pointPos + Vector3.up * 1f, "Element "+(i).ToString(), style);
            }

            //Button
            if(Handles.Button(objectLinked.transform.position + Vector3.up * 3, objectLinked.transform.rotation, 2, 3, Handles.CubeHandleCap))
            {
                GenericMenu gm = new GenericMenu();

                gm.AddItem(new GUIContent("a", ""), false, HelloWolrd);
                gm.AddDisabledItem(new GUIContent("b", ""), false);
                gm.AddSeparator("");
                gm.AddItem(new GUIContent("c/d", ""), false, HelloWolrd);
                gm.AddSeparator("c/");
                gm.AddItem(new GUIContent("c/e", ""), false, Hello, "utilisateur");

                gm.ShowAsContext();
            }
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
        serializedObject.ApplyModifiedPropertiesWithoutUndo();
    }

    void HelloWolrd()
    {
        Debug.Log("HelloWolrd");
    }
    void Hello(object arg)
    {
        Debug.Log((string)arg);
    }

}

