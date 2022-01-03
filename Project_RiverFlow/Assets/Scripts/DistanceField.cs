using System.Collections.Generic;
using UnityEngine;

public class DistanceField : MonoBehaviour
{
    private int[,] array;
    private bool[,] isCalculated;
    public int sizeX;
    public int sizeY;
    public int defaultValue;
    private List<int[]> updateList;


    public void Start()
    { 
        this.array = new int[sizeX,sizeY];
        for (int i = 0; i < sizeX; i++) 
        {
            for (int j = 0; j < sizeY; j++) 
            {
                this.array[i, j] = defaultValue;
            }
        }
        this.isCalculated = new bool[sizeX, sizeY];
        updateList = new List<int[]>();
        this.ClearCalulatedArray();
    }

    public void SetZero(int x, int y) 
    {
        this.array[x, y] = 0;
        this.isCalculated[x, y] = true;
        for(int i = -1; i <= 1; i++) 
        {
            for(int j = -1; j <= 1; j++) 
            {
                if(i != 0 && j != 0) 
                {
                    AppendToUpdateList(x+i,y+j);
                }
            }
        }
        this.UpdateList();
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
        this.isCalculated[x, y] = true;
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

    private void ClearCalulatedArray()
    {
        for (int i = 0; i < this.sizeX; i++)
        {
            for (int j = 0; j < this.sizeY; j++)
            {
                this.isCalculated[i, j] = false;
            }
        }
    }

    public void UpdateList() 
    { 
        for(int i = 0 ; i < this.updateList.Count ; i++) 
        {
            if(!this.isCalculated[this.updateList[i][0], this.updateList[i][1]])
            {
                SetValue(this.updateList[i][0], this.updateList[i][1]);
            }
        }
        this.ClearCalulatedArray();
        Debug.Log("\n" + Print());
    }

    public string Print()
    {
        string output = "";
        for(int i = 0; i<sizeY ; i++)
        {
            for (int j = 0; j < sizeX; j++)
            {
                output += "" + array[i, j];
            }
            output += "\n";
        }
        return output;
    }
}
