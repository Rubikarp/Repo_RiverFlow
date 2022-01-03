using System.Collections.Generic;

public class DistanceField
{
    private int[,] array;
    private int sizeX;
    private int sizeY;
    private int defaultValue;
    private List<int[]> updateList;

    public DistanceField(int sizeX, int sizeY, int defaultValue) 
    { 
        this.sizeX = sizeX;
        this.sizeY = sizeY;
        this.defaultValue = defaultValue;
        this.array = new int[sizeX,sizeY];
        for (int i = 0; i < sizeX; i++) 
        {
            for (int j = 0; j < sizeY; j++) 
            {
                this.array[i, j] = defaultValue;
            }
        }
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
    }

    private void AppendToUpdateList(int x, int y) {
        bool isXTooLow = (x < 0);
        bool isXTooHigh = (x >= this.sizeX);
        bool isYTooLow = (y < 0);
        bool isYTooHigh = (y >= this.sizeY);
        if(!isXTooLow && !isXTooHigh && !isYTooLow && !isYTooHigh) 
        {
            this.updateList.Add(new int[]{x, y});
        }
    }

    public void UpdateList() 
    { 
        for(int i = 0 ; i < updateList.Count ; i++) 
        {
            SetValue(updateList[i][0], updateList[i][1]);
        }
    }
}
