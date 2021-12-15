using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "new_proto_GridData", menuName = "Gameplay/GridData")]
public class GridData_SCO : ScriptableObject
{
    [Header("Parameter")]
    public Vector2Int size = new Vector2Int(10, 10);

    [Header("Data")]

    public TileSaveData[/*size.x * size.y*/] tilesData;

    public TileSaveData GetTile(int x, int y)
    {
        return tilesData[x + (y * (size.x))];
    }
    public TileSaveData GetTile(Vector2Int pos)
    {
        return tilesData[pos.x + (pos.y * (size.x))];
    }

    public void SetTile(int x, int y, TileSaveData value)
    {
        tilesData[x + (y * (size.x))] = value;
    }
    public void SetTile(Vector2Int pos, TileSaveData value)
    {
        tilesData[pos.x + (pos.y * (size.x))] = value;
    }


    [Button]
    private void PopulateGrid()
    {
        tilesData = new TileSaveData[size.x * size.y];
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                SetTile(x,y,new TileSaveData(x,y));
            }
        }
    }

}
