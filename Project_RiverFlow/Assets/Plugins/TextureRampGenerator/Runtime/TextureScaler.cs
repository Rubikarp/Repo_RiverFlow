using UnityEngine;

public static class TextureScaler
{
    public static Texture2D Scale(Texture2D baseTexture, int scaleFactor)
    {
        //Création de la Texture Upscale
        Texture2D newTexture = new Texture2D(baseTexture.width * scaleFactor, baseTexture.height * scaleFactor, baseTexture.format, false);

        //define texture color (default color is white)
        Color[] colorsBlock = new Color[scaleFactor * scaleFactor];
        Vector2Int pos = new Vector2Int(0, 0);

        for (int x = 0; x < baseTexture.width; x++)
        {
            for (int y = 0; y < baseTexture.height; y++)
            {
                pos = new Vector2Int(x, y);

                if (colorsBlock[0] != baseTexture.GetPixel(x, y))
                {
                    Color pixelCol = baseTexture.GetPixel(x, y);
                    for (int i = 0; i < colorsBlock.Length; i++)
                    {
                        colorsBlock[i] = pixelCol;
                    }
                }

                //Set the texture color
                newTexture.SetPixels(pos.x * scaleFactor, pos.y * scaleFactor, scaleFactor, scaleFactor, colorsBlock);
                //newTexture.SetPixel(pos.x , pos.y , colorsBlock[0]);
            }
        }

        //Save la modification
        newTexture.Apply();

        return newTexture;
    }

#if UNITY_EDITOR
    #region Texture Editing
    // Ne devrait pas être là
    // faut que j'en fasse un compute shader
    public static Texture2D Blur(Texture2D image, int blurSize)
    {
        Texture2D blurred = new Texture2D(image.width, image.height);

        // look at every pixel in the blur rectangle
        for (int xx = 0; xx < image.width; xx++)
        {
            for (int yy = 0; yy < image.height; yy++)
            {
                float avgR = 0, avgG = 0, avgB = 0, avgA = 0;
                int blurPixelCount = 0;

                // average the color of the red, green and blue for each pixel in the
                // blur size while making sure you don't go outside the image bounds
                for (int x = xx; (x < xx + blurSize && x < image.width); x++)
                {
                    for (int y = yy; (y < yy + blurSize && y < image.height); y++)
                    {
                        Color pixel = image.GetPixel(x, y);

                        avgR += pixel.r;
                        avgG += pixel.g;
                        avgB += pixel.b;
                        avgA += pixel.a;

                        blurPixelCount++;
                    }
                }

                avgR = avgR / blurPixelCount;
                avgG = avgG / blurPixelCount;
                avgB = avgB / blurPixelCount;
                avgA = avgA / blurPixelCount;

                // now that we know the average for the blur size, set each pixel to that color
                for (int x = xx; x < xx + blurSize && x < image.width; x++)
                {
                    for (int y = yy; y < yy + blurSize && y < image.height; y++)
                    {
                        blurred.SetPixel(x, y, new Color(avgR, avgG, avgB, avgA));

                    }
                }
            }
        }
        blurred.Apply();
        return blurred;
    }
    #endregion
#endif

}
