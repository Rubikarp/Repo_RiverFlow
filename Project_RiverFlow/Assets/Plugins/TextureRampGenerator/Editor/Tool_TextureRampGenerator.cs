using UnityEditor;
using UnityEngine;

namespace Karprod
{
    public class Tool_TextureRampGenerator : EditorWindow
    {
        #region Constante
        private const int defaultSize = 256;
        private const string defaultName = "new_TextureRamp";
        private static readonly GradientColorKey[] defaultGradient = new GradientColorKey[5]
        {
                new GradientColorKey(Color.red, 0),
                new GradientColorKey(Color.yellow, 0.25f),
                new GradientColorKey(Color.green, 0.5f),
                new GradientColorKey(Color.cyan, 0.75f),
                new GradientColorKey(Color.blue, 1)
        };
        //GUI
        private const int MinHeight = 50;
        private const int LeftBordWidth = 150;
        private const int Width = 250;
        private const int MaxWidth = Width*2;
        #endregion

        #region Variable
        //Texture Name
        private string textureName = defaultName;
        
        //Texture Gradient
        private Gradient gradient = new Gradient() { colorKeys = defaultGradient };

        //Texture Format
        private TextureType actualTexType = TextureType.PNG;
        
        //Texture Size
        private bool isSquare = false;
        private int squareSize = defaultSize;
        private Vector2Int size = new Vector2Int(defaultSize, defaultSize);
        #endregion

        [MenuItem("Tools/TextureRampGenerator")]
        public static void ShowWindow()
        {
            EditorWindow myWindow = GetWindow(typeof(Tool_TextureRampGenerator), false ,"TextureRampGenerator");

            myWindow.name = "TextureRampGenerator";
            myWindow.minSize = new Vector2(420, 220);
        }

        private void OnGUI()
        {
            GUILayout.Space(5);
            //Main Property
            using (new GUILayout.VerticalScope(GUILayout.MaxWidth(MaxWidth)))
            {
                GUILayout.Label("Texture Ramp", EditorStyles.boldLabel);

                textureName = EditorGUILayout.TextField("Name", textureName);
                gradient = EditorGUILayout.GradientField("Gradient", gradient);
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
    }
}