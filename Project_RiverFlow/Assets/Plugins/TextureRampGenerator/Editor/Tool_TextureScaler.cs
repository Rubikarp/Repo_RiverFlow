#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Karprod
{
    public class Tool_TextureScaler : EditorWindow
    {
        #region Constante
        //GUI
        private const int MinHeight = 50;
        private const int LeftBordWidth = 150;
        private const int Width = 250;
        private const int MaxWidth = Width*2;
        #endregion

        #region Variable
        //Texture Name
        private Texture2D originalTexture;
        private string textureName = string.Empty;
        private int scaleFactor = 2;
        #endregion

        [MenuItem("Tools/TextureScaler")]
        public static void ShowWindow()
        {
            EditorWindow myWindow = GetWindow(typeof(Tool_TextureScaler), false , "TextureScaler");

            myWindow.name = "TextureScaler";
            myWindow.minSize = new Vector2(420, 220);
        }
        
        private void OnGUI()
        {
            //Main Property
            using (new GUILayout.VerticalScope(GUILayout.MaxWidth(MaxWidth)))
            {
                GUILayout.Label("Texture", EditorStyles.boldLabel);
                originalTexture = (Texture2D)EditorGUILayout.ObjectField(originalTexture, typeof(Texture2D));
                GUILayout.Space(10);
                //textureName = EditorGUILayout.TextField("Name", textureName);
                scaleFactor = EditorGUILayout.IntField("Scale", scaleFactor);
            }
            GUILayout.Space(15);
            //Creation Button
            using (new GUILayout.HorizontalScope(GUILayout.MaxWidth(MaxWidth)))
            {
                GUILayout.Label("", EditorStyles.boldLabel, GUILayout.Width(LeftBordWidth));
                using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    if (GUILayout.Button("Generate Texture Ramp", GUILayout.MinHeight(MinHeight)))
                    {
                        ButtonScaleTexture();
                    }
                }
            }
        }
        GUIStyle GetButtonStyle()
        {
            var s = new GUIStyle(GUI.skin.button);
            s.margin.left = 0;
            s.margin.top = 0;
            s.margin.right = 0;
            s.margin.bottom = 0;
            s.border.left = 0;
            s.border.top = 0;
            s.border.right = 0;
            s.border.bottom = 0;
            return s;
        }
        private void ButtonScaleTexture()
        {
            string path = AssetDatabase.GetAssetPath(originalTexture);
            string[] pathBlock = path.Split('.');

            pathBlock[0] = pathBlock[0] + "x" + scaleFactor.ToString() +'.';
            path = pathBlock[0] + pathBlock[1];

            Texture2D temp = originalTexture;
            Texture2D newTexture = TextureScaler.Scale(temp, scaleFactor);

            TextureGenerator.Create(newTexture, path);
        }
    }
}
#endif
