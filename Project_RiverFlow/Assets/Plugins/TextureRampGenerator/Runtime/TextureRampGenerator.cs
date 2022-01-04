using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Karprod
{
    public static class TextureRampGenerator
    {
        private const int defaultSize = 256;
        private const string defaultName = "new_TextureRamp";

        public static Texture2D Generate(Gradient grd, int size = defaultSize, bool alpha = true)
        {
            //Création de la Texture Ramp
            Texture2D textureRamp = TextureRampGenerator.Generate(grd, new Vector2Int(size, size), alpha);

            return textureRamp;
        }
        public static Texture2D Generate(Gradient grd, int width, int height, bool alpha = true)
        {
            //Création de la Texture Ramp
            Texture2D textureRamp = TextureRampGenerator.Generate(grd, new Vector2Int(width,height), alpha);

            return textureRamp;
        }
        public static Texture2D Generate(Gradient grd, Vector2Int size, bool alpha = true)
        {
            //Var pour répartir le gradient sur la texture
            float hToGrd = (float)1f / size.x;

            //Création de la Texture
            Texture2D textureRamp = TextureGenerator.Generate(size.x, size.y, alpha);

            //define the texture ramp on horizontal axis
            Color[] cols = new Color[size.x];
            for (int x = 0; x < size.x; x++)
            {
                cols[x] = grd.Evaluate(x * hToGrd);
            }

            //extend it on the texture height
            for (int y = 0; y < size.y; y++)
            {
                textureRamp.SetPixels(0, y, size.x, 1, cols);
            }

            textureRamp.Apply();

            return textureRamp;
        }

        #if UNITY_EDITOR

        public static void Create(Gradient grd, int size = defaultSize, string name = defaultName, TextureType fileType = TextureType.PNG)
        {
            //Création de la Texture Ramp
            TextureRampGenerator.Create(grd, new Vector2Int(size, size), name, fileType);
        }
        public static void Create(Gradient grd, int width, int height, string name = defaultName, TextureType fileType = TextureType.PNG)
        {
            //Création de la Texture Ramp
           TextureRampGenerator.Create(grd, new Vector2Int(width, height), name, fileType);
        }
        public static void Create(Gradient grd, Vector2Int size, string name, TextureType fileType = TextureType.PNG)
        {
            //generate the path
            string path = TextureRampGenerator.PathAsking(name, fileType);
            if (path == null || path == string.Empty)
            {
                Debug.Log("TextureRamp creation have been cancel");
                return;
            }
            
            //Creating The texture
            Texture2D texture = TextureRampGenerator.Generate(grd, size.x, size.y, fileType == TextureType.PNG);

            //Check for copy
            TextureGenerator.DeleteCopy(path);

            //create the asset
            if (fileType == TextureType.JPG)
            {
                TextureGenerator.TextureToAssetJPG(path, texture);
            }
            else
            {
                TextureGenerator.TextureToAssetPNG(path, texture);
            }

            //Debug.log the test       
            if (TextureGenerator.TestTexture2DAtPath(path))
            {
                Debug.Log("The TextureRamp have been successfully Created");
            }
            else
            {
                Debug.LogError("Error during the TextureRamp creation");
            }
        }

        #region Texture Asset managing Methodes
        public static string PathAsking(string name, TextureType fileType = TextureType.PNG)
        {
            string path;
            switch (fileType)
            {
                case TextureType.PNG:
                    path = EditorUtility.SaveFilePanel("Save TextureRamp Asset", "Assets/", name, "png");
                    break;

                case TextureType.JPG:
                    path = EditorUtility.SaveFilePanel("Save TextureRamp Asset", "Assets/", name, "jpg");
                    break;

                default:
                    path = EditorUtility.SaveFilePanel("Save TextureRamp Asset", "Assets/", name, "png");
                    break;
            }
            path = FileUtil.GetProjectRelativePath(path);
            return path;
        }
        #endregion

        #endif
    }
}
