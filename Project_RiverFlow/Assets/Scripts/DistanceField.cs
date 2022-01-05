using System.Collections.Generic;
using UnityEngine;
using Karprod;
using System.Text;

public class DistanceField : MonoBehaviour
{

    private int[,] array;
    private int sizeX;
    private int sizeY;
    public int defaultValue;

    public RiverManager riverManager;
    public GameGrid gameGrid;

    public void Start()
    {
        sizeX = gameGrid.size.x;
        sizeY = gameGrid.size.y;
        array = new int[sizeX,sizeY];
        ResetArray();
    }

    private void CompleteArrayWithCanals() // C'est le nouveau set zero
    {
        
        foreach (Canal canal in riverManager.canals)
        {
            array[canal.endNode.x, canal.endNode.y] = 0;
            array[canal.startNode.x, canal.startNode.y] = 0;
            foreach (Vector2Int tile in canal.canalTiles)
            {
                array[tile.x, tile.y] = 0;
            }
        }
    }

    private void ResetArray()
    {
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                array[i, j] = defaultValue;
            }
        }
    }

    private void UpdateDistance(int currentValue)
    {
        for(int x = 0; x < sizeX; x++)
        {
            for(int y = 0; y < sizeY; y++)
            {
                if(array[x,y] == currentValue)
                {
                    for(int i = -1; i<=1; i++)
                    {
                        for(int j = -1; j<=1; j++)
                        {
                            try
                            {
                                if (array[x + i, y + j] > currentValue + 1)
                                {
                                    array[x + i, y + j] = currentValue + 1;
                                }
                            }
                            catch
                            {
                                // C'est pour les OutOfBound Exception
                            }
                        }
                    }
                }
            }
        }
    }

    public void GenerateArray()
    {
        ResetArray();

        CompleteArrayWithCanals();

        for(int i = 0; i<defaultValue; i++)
        {
            UpdateDistance(i);
        }

        Debug.Log($"\n{Print()}");
    }

    public string Print()
    {

        StringBuilder strBuilder = new StringBuilder();
        for (int i = sizeY - 1; i >= 0; i--)
        {
            for (int j = 0; j < sizeX; j++)
            {
                strBuilder.Append($"[{array[j, i].ToString()}]");
            }
            strBuilder.Append("\n");
        }
        return strBuilder.ToString();
    }
}
