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
    private List<int[]> updateList;
    private List<int[]> zerosList;

    public RiverManager riverManager;
    public GameGrid gameGrid;

    public Texture2D debugTexture;
    int debugScale = 10;
    public void Start()
    {
        sizeX = gameGrid.size.x;
        sizeY = gameGrid.size.y;
        this.array = new int[sizeX,sizeY];
        ResetArray();
        updateList = new List<int[]>();
        zerosList = new List<int[]>();

        debugTexture = Karprod.TextureGenerator.Generate(sizeX * debugScale, sizeY * debugScale, false);
    }

    public void SetZero(int x, int y) 
    {
        this.array[x, y] = 0;
        for(int i = -1; i <= 1; i++) 
        {
            for(int j = -1; j <= 1; j++) 
            {
                if(i != 0 && j != 0) 
                {
                    AppendToUpdateList(x+i,y+j);
                    debugTexture.SetPixels((x + 1)* debugScale, (y + 1) * debugScale, debugScale, debugScale, GetBlockColor(Color.blue, debugScale));
                    debugTexture.Apply();
                }
            }
        }
        this.UpdateList();
    }

    private Color[] GetBlockColor(Color col, int size)
    {
        Color[] result = new Color[size * size];
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                result[x + y * size] = col;
            }
        }
        return result;
    }

    public void SetValue(int x, int y) {
        int minimalValue = this.defaultValue;
        for(int i = -1; i <= 1; i++) 
        {
            for(int j = -1; j <= 1; j++) 
            {
                try 
                {
                    if(this.array[x+i,y+j] < minimalValue) 
                    {
                        minimalValue = this.array[x+i,y+j];
                    }
                }
                catch 
                {
                    continue;
                }
                  
                if(i != 0 && j != 0) {
                    AppendToUpdateList(x+i,y+j);
                }
            }
        }
        this.array[x,y] = minimalValue + 1;
        debugTexture.SetPixels((x + 1) * debugScale, (y + 1) * debugScale, debugScale, debugScale, GetBlockColor(new Color(0,0, (float)(minimalValue + 1) / (float)defaultValue,0), debugScale));
        debugTexture.Apply();
    }

    private void AppendToUpdateList(int x, int y) {
        bool isXTooLow = (x < 0);
        bool isXTooHigh = (x >= this.sizeX);
        bool isYTooLow = (y < 0);
        bool isYTooHigh = (y >= this.sizeY);
        if(!isXTooLow && !isXTooHigh && !isYTooLow && !isYTooHigh) 
        {
            this.updateList.Add(new int[2]{x, y});
        }
    }

    public void UpdateList() 
    { 
        for(int i = 0 ; i < this.updateList.Count ; i++) 
        {

                SetValue(this.updateList[i][0], this.updateList[i][1]);
            
        }

        Debug.Log("\n" + Print());
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
                this.array[i, j] = defaultValue;
            }
        }
    }
    public void GenerateArray()
    {
        ResetArray();

        CompleteArrayWithCanals();
        
        //Debug.Log($"\n{Print()}");
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
