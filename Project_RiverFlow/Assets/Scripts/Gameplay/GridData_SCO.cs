using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new_proto_GridData", menuName = "GameplaySCO/GridData")]
public class GridData_SCO : ScriptableObject
{
    [Header("Parameter")]
    public Vector2Int size = new Vector2Int(10, 10);

    [Header("Data")]

    public TileData[/*size.x * size.y*/] tilesData;

    public TileData GetTile(int x, int y)
    {
        return tilesData[x + (y * (size.x))];
    }
    public TileData GetTile(Vector2Int pos)
    {
        return tilesData[pos.x + (pos.y * (size.x))];
    }

    public void SetTile(int x, int y, TileData value)
    {
        tilesData[x + (y * (size.x))] = value;
    }
    public void SetTile(Vector2Int pos, TileData value)
    {
        tilesData[pos.x + (pos.y * (size.x))] = value;
    }
}
