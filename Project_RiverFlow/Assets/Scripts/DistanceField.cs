using System.Collections.Generic;
using UnityEngine;
using Karprod;
using System.Text;

public class DistanceField : MonoBehaviour
{

    public int[,] riverArray;
    public int[,] treeArray;
    private int sizeX;
    private int sizeY;
    public int defaultRiverValue;
    public int defaultTreeValue;

    public RiverManager riverManager;
    public ElementHandler elementHandler;
    public GameGrid gameGrid;

    public void Start()
    {
        sizeX = gameGrid.size.x;
        sizeY = gameGrid.size.y;
        riverArray = new int[sizeX,sizeY];
        treeArray = new int[sizeX, sizeY];
        ResetRiverArray();
        ResetTreeArray();
    }

    private void CompleteArrayWithCanals() // C'est le nouveau set zero
    {
        
        foreach (Canal canal in riverManager.canals)
        {
            riverArray[canal.endNode.x, canal.endNode.y] = 0;
            riverArray[canal.startNode.x, canal.startNode.y] = 0;
            foreach (Vector2Int tile in canal.canalTiles)
            {
                riverArray[tile.x, tile.y] = 0;
            }
        }
    }

    private void CompleteArrayWithTrees()
    {
        foreach(Plant plant in elementHandler.allPlants)
        {
            treeArray[plant.gridPos.x, plant.gridPos.y] = 0;
        }
    }

    private void ResetRiverArray()
    {
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                riverArray[i, j] = defaultRiverValue;
            }
        }
    }

    private void ResetTreeArray()
    {
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                treeArray[i, j] = defaultTreeValue;
            }
        }
    }

    private void UpdateDistance(int currentValue)
    {
        for(int x = 0; x < sizeX; x++)
        {
            for(int y = 0; y < sizeY; y++)
            {
                if(riverArray[x,y] == currentValue)
                {
                    for(int i = -1; i<=1; i++)
                    {
                        for(int j = -1; j<=1; j++)
                        {
                            try
                            {
                                if (riverArray[x + i, y + j] > currentValue + 1)
                                {
                                    riverArray[x + i, y + j] = currentValue + 1;
                                }
                            }
                            catch
                            {
                                // C'est pour les OutOfBound Exception
                            }
                        }
                    }
                }

                if (treeArray[x, y] == currentValue)
                {
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            try
                            {
                                if (treeArray[x + i, y + j] > currentValue + 1)
                                {
                                    treeArray[x + i, y + j] = currentValue + 1;
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
        ResetRiverArray();
        ResetTreeArray();

        CompleteArrayWithCanals();
        CompleteArrayWithTrees();

        for(int i = 0; i<defaultRiverValue; i++)
        {
            UpdateDistance(i);
        }

        //Debug.Log($"\n{Print(treeArray)}");
    }

    public string Print(int[,] array)
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
