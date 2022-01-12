#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Karprod
{
    public class Tool_TextureScaler : EditorWindow
    {
        #region Constante
        private const int defaultSize = 256;
        private const string defaultName = "new_TextureRamp";
        //GUI
        private const int MinHeight = 50;
        private const int LeftBordWidth = 150;
        private const int Width = 250;
        private const int MaxWidth = Width*2;
        #endregion

        #region Variable
        //Texture Name
        private Texture2D originalText;
        private string textureName = defaultName;
        private int scaleFactor = 2;
        #endregion

        [MenuItem("Tools/TextureScaler")]
        public static void ShowWindow()
        {
            EditorWindow myWindow = GetWindow(typeof(Tool_TextureScaler), false , "TextureScaler");

            myWindow.name = "TextureScaler";
            myWindow.minSize = new Vector2(420, 220);
        }
        /*
        private void OnGUI()
        {
            GUILayout.Space(5);
            //Main Property
            using (new GUILayout.VerticalScope(GUILayout.MaxWidth(MaxWidth)))
            {
                GUILayout.Label("Texture", EditorStyles.boldLabel);

                originalText = EditorGUILayout.ObjectField("Gradient", gradient);
                textureName = EditorGUILayout.TextField("Name", textureName);
            }
            GUILayout.Space(15);
            //Texture Size
            using (new GUILayout.HorizontalScope(GUILayout.MaxWidth(MaxWidth)))
            {
                using (new GUILayout.VerticalScope(GUILayout.MaxWidth(Width)))
                {
                    GUILayout.Label("Texture Size", EditorStyles.boldLabel);
                    isSquare = EditorGUILayout.Toggle("Texture Square ?", isSquare);
                }
                using (new GUILayout.VerticalScope(GUILayout.Width(Width)))
                {
                    GUILayout.Label("", EditorStyles.boldLabel);
                    if (isSquare)
                    {
                        squareSize = EditorGUILayout.IntField("", squareSize);
                    }
                    else
                    {
                        size = EditorGUILayout.Vector2IntField("", new Vector2Int(size.x, size.y));
                    }
                }
            }
            GUILayout.Space(5);
            //Texture Format
            using (new GUILayout.HorizontalScope(GUILayout.MaxWidth(MaxWidth)))
            {
                GUILayout.Label("Texture Format", EditorStyles.boldLabel, GUILayout.Width(LeftBordWidth));
                using (new GUILayout.HorizontalScope())
                {
                    TextureFormatButton("PNG", TextureType.PNG);
                    TextureFormatButton("JPG", TextureType.JPG);
                }
            }
            GUILayout.Space(10);
            //Creation Button
            using (new GUILayout.HorizontalScope(GUILayout.MaxWidth(MaxWidth)))
            {
                GUILayout.Label("", EditorStyles.boldLabel, GUILayout.Width(LeftBordWidth));
                using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    if (GUILayout.Button("Generate Texture Ramp", GUILayout.MinHeight(MinHeight)))
                    {
                        ButtonCreateGradient();
                    }
                }
            }
        }

        void TextureFormatButton(string name, TextureType texType)
        {
            GUIContent content = new GUIContent(name);

            var oldBackGroundColor = GUI.backgroundColor;
            var oldContentColor = GUI.contentColor;

            if (actualTexType == texType)
            {
                GUI.backgroundColor = new Color(0.3f, 0.3f, 0.3f);
                GUI.contentColor = Color.white;
            }

            if (GUILayout.Button(content, GetButtonStyle(), GUILayout.Height(25), GUILayout.MinWidth(40), GUILayout.MaxWidth((Width +50)/2)))
            {
                actualTexType = texType;
            }

            GUI.backgroundColor = oldBackGroundColor;
            GUI.contentColor = oldContentColor;
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


        private void ButtonCreateGradient()
        {
            if (isSquare)
            {
                TextureRampGenerator.Create(gradient, squareSize, textureName, actualTexType);
            }
            else
            {
                TextureRampGenerator.Create(gradient, size, textureName, actualTexType);
            }
        }
        */
    }
}
#endif
