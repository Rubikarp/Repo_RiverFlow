using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LD_new", menuName = "LevelDesign/NewLevel")]

public class LD_Data : ScriptableObject
{
    public Vector2Int gridSize = new Vector2Int(8,8);
    public TileType[] gridFieldType = new TileType[8*8];

    //public TileData[] gridTiles;
    //public List<SpawnArea> spawnArea = new List<SpawnArea>();

#if UNITY_EDITOR
    public Color[] tileColor = new Color[5 /*System.Enum.GetValues(typeof(TileType)).Length*/]
    {Color.magenta, Color.green, Color.red, Color.yellow, Color.grey };

    public TileType selectedType;
#endif
}

[System.Serializable]
public struct TileData
{
    public Vector2Int gridPos;
    //
    [Header("Data")]
    public TileType type;
    public int spawnAreaIn;
}

[System.Serializable]
public struct SpawnArea
{
    public int areaNumber;
    //
    [Header("Data")]
    public Vector2Int unlockDelay; // Vector2Int(minute,seconde)
    public int UnlockDelay
    {
        get { return (unlockDelay.x * 60) + unlockDelay.y; }
    }
    public Color color;
}