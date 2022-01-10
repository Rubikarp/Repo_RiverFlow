﻿using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

namespace Karprod
{
    public enum TextureType { PNG, JPG};

    public static class TextureGenerator
    {
        //Generate Texture Methodes
        public static Texture2D Generate(int size = 256, bool alpha = true)
        {
            //Création de la Texture
            Texture2D texture = TextureGenerator.Generate(new Vector2Int(size,size), alpha);

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
            Texture2D texture = new Texture2D(size.x, size.y, TextureFormat.ARGB32, false);
            if (!alpha)
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
        //Create Texture Methodes
        public static void Create(string name, TextureType fileType = TextureType.PNG, int size = 256)
        {
            //generate the path
            string path = TextureGenerator.PathAsking(name, fileType);
            if (path == null || path == string.Empty)
            {
                Debug.Log("Texture creation have been cancel");
                return;
            }

            //Creating The texture
            Texture2D texture = TextureGenerator.Generate(size, fileType == TextureType.PNG);

            //Check for copy
            TextureGenerator.DeleteCopy(path);

            //create the asset
            if(fileType == TextureType.JPG)
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
                Debug.Log("The texture have been successfully Created");
            }
            else
            {
                Debug.LogError("Error during the Texture creation");
            }
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
