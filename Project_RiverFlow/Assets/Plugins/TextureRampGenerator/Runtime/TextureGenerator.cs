using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

namespace Karprod
{
    public enum TextureType { PNG, JPG};
    public static class TextureGenerator
    {
        private const int defaultSize = 256;
        private const string defaultName = "new_Texture";

        //Generate Texture Methodes
        public static Texture2D Generate(int size = defaultSize, bool alpha = true)
        {
            //Création de la Texture
            Texture2D texture = TextureGenerator.Generate(new Vector2Int(size, size), alpha);

            return texture;
        }
        public static Texture2D Generate(int width, int height, bool alpha = true)
        {
            //Création de la Texture
            Texture2D texture = TextureGenerator.Generate(new Vector2Int(width, height), alpha);

            return texture;
        }
        public static Texture2D Generate(Vector2Int size, bool alpha = true)
        {
            //Création de la Texture
            Texture2D texture;
            if (alpha)
            {
                texture = new Texture2D(size.x, size.y, TextureFormat.ARGB32, false);
            }
            else
            {
                texture = new Texture2D(size.x, size.y, TextureFormat.RGB24, false);
            }

            //define texture color (default color is white)
            Color[] cols = new Color[size.x * size.y];
            for (int i = 0; i < cols.Length; i++)
            {
                cols[i] = Color.white;
            }

            //Set the texture color
            texture.SetPixels(0, 0, size.x, size.y, cols);

            //Save la modification
            texture.Apply();

            return texture;
        }

#if UNITY_EDITOR

        public static void Create(string name, int size = 256, TextureType fileType = TextureType.PNG)
        {
            //Creating The texture
            Texture2D texture = TextureGenerator.Generate(size, fileType == TextureType.PNG);

            Create(name, texture, fileType);
        }
        public static void Create(string name, Texture2D texture, TextureType fileType = TextureType.PNG)
        {
            //generate the path
            string path = TextureGenerator.PathAsking(name, fileType);
            if (path == null || path == string.Empty)
            {
                Debug.Log("Texture creation have been cancel");
                return;
            }

            //Check for copy
            TextureGenerator.DeleteCopy(path);

            //create the asset
            if (fileType == TextureType.PNG)
            {
                TextureGenerator.TextureToAssetPNG(path, texture);
            }
            else
            {
                TextureGenerator.TextureToAssetJPG(path, texture);
            }

            //Debug.log the test       
            if (TextureGenerator.TestTexture2DAtPath(path))
            {
                Debug.Log("The texture have been successfully Created");
            }
            else
            {
                Debug.LogError("Error during the Texture creation");
            }
        }
        public static void Create(Texture2D texture, string path, TextureType fileType = TextureType.PNG)
        {
            //Check for copy
            TextureGenerator.DeleteCopy(path);

            //create the asset
            if (fileType == TextureType.PNG)
            {
                TextureGenerator.TextureToAssetPNG(path, texture);
            }
            else
            {
                TextureGenerator.TextureToAssetJPG(path, texture);
            }

            //Debug.log the test       
            if (TextureGenerator.TestTexture2DAtPath(path))
            {
                Debug.Log("The texture have been successfully Created");
            }
            else
            {
                Debug.LogError("Error during the Texture creation");
            }

            //https://forum.unity.com/threads/how-to-change-png-import-settings-via-script.734834/
            //.meta 
        }

        private static TextureImporter GetPixelImport(string path)
        {
            TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(path);

            importer.isReadable = true;
            importer.textureType = TextureImporterType.Sprite;

            TextureImporterSettings importerSettings = new TextureImporterSettings();
            importer.ReadTextureSettings(importerSettings);
            importerSettings.spriteExtrude = 0;
            importerSettings.spriteGenerateFallbackPhysicsShape = false;
            importerSettings.spriteMeshType = SpriteMeshType.FullRect;
            importerSettings.spriteMode = (int)SpriteImportMode.Multiple;
            importer.SetTextureSettings(importerSettings);

            importer.spriteImportMode = SpriteImportMode.Multiple;
            importer.maxTextureSize = 1024; // or whatever
            importer.alphaIsTransparency = true;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.alphaSource = TextureImporterAlphaSource.FromInput;

            EditorUtility.SetDirty(importer);
            importer.SaveAndReimport();

            /*if multiple*/
            /*
            Rect[] spriteRects = texture.PackTextures( SPRITE_TEXTURES , PADDING , textureSize, false);
            SpriteMetaData[] isheet = new SpriteMetaData[spriteRects.Length];
            for (t = 0; t < isheet.Length; t++)
            {
            SpriteMetaData md = new SpriteMetaData();
            md.rect = new Rect(
            spriteRects[t].position.x * (float)texture.width,
            spriteRects[t].position.y * (float)texture.height,
            spriteRects[t].size.x * (float)texture.width,
            spriteRects[t].size.y * (float)texture.height
            );
            // md.alignment = .. i.e. (int)SpriteAlignment.Custom;
            // md.pivot = .. i.e. Vector3.zero
            // md.name = ..
            isheet[t] = md;
            }
            importer.spritesheet = isheet;
            */

return importer;
}

#region Texture Encoding
public static void TextureToAssetPNG(string filePath, Texture2D texture)
{
FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
BinaryWriter writer = new BinaryWriter(stream);

byte[] bytes = texture.EncodeToPNG();
for (int i = 0; i < bytes.Length; i++)
{
    writer.Write(bytes[i]);
}

writer.Close();
stream.Close();

AssetDatabase.ImportAsset(filePath, ImportAssetOptions.ForceUpdate);

//Update Folder
AssetDatabase.Refresh();
}
public static void TextureToAssetJPG(string filePath, Texture2D texture)
{
FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
BinaryWriter writer = new BinaryWriter(stream);

byte[] bytes = texture.EncodeToJPG();
for (int i = 0; i < bytes.Length; i++)
{
    writer.Write(bytes[i]);
}

writer.Close();
stream.Close();

AssetDatabase.ImportAsset(filePath, ImportAssetOptions.ForceUpdate);

//Update Folder
AssetDatabase.Refresh();
}
#endregion

#region Texture Asset managing Methodes
public static string PathAsking(string name, TextureType fileType = TextureType.PNG)
{
string path;
switch (fileType)
{
    case TextureType.PNG:
        path = EditorUtility.SaveFilePanel("Save Texture Asset", "Assets/", name, "png");
        break;

    case TextureType.JPG:
        path = EditorUtility.SaveFilePanel("Save Texture Asset", "Assets/", name, "jpg");
        break;

    default:
        path = EditorUtility.SaveFilePanel("Save Texture Asset", "Assets/", name, "png");
        break;
}
path = FileUtil.GetProjectRelativePath(path);
return path;
}
public static void DeleteCopy(string filePath)
{
if ((Texture2D)AssetDatabase.LoadAssetAtPath(filePath, typeof(Texture2D)) != null)
{
    Debug.Log("A file have already this name, this file have been replace");

    FileUtil.DeleteFileOrDirectory(filePath);

    //Update Folder
    AssetDatabase.Refresh();
}

}
public static bool TestTexture2DAtPath(string filePath)
{
if ((Texture2D)AssetDatabase.LoadAssetAtPath(filePath, typeof(Texture2D)) == null)
{
    return false;
}
else
{
    return true;
}
}
#endregion

#endif
}
}
