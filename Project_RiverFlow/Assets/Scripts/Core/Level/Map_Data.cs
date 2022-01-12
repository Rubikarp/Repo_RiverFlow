using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Map_new", menuName = "LevelDesign/New_Map")]

public class Map_Data : ScriptableObject
{
    public Vector2Int gridSize = new Vector2Int(8,8);
    public TileType[] gridFieldType = new TileType[8*8];

#if UNITY_EDITOR
    public Color[] tileColor = new Color[5 /*System.Enum.GetValues(typeof(TileType)).Length*/]
    {Color.magenta, Color.green, Color.red, Color.yellow, Color.grey };

    public TileType selectedType;
#endif

    public TileType GetTileType(int x, int y)
    {
        //i reverted le y  order
        return gridFieldType[x + (((gridSize.y-1)-y) * (gridSize.x))];
    }
    public TileType GetTileType(Vector2Int pos)
    {
        return GetTileType(pos.x, pos.y);
    }
}