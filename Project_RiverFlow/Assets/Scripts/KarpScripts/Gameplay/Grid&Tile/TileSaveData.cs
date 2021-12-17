using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct TileSaveData 
{
    public readonly string name
    {
        get { return "Tile : " + gridPos.ToString(); }
    }
    [Header("Essential Data")]
    public Vector2Int gridPos;

    [Header("State")]
    public TileType type;
    public FlowStrenght riverStrenght;

    [Header("Stored Value")]
    public bool[] flowIn_Neighbors;
    public bool[] flowOut_Neighbors;

    #region Constructor 
    public TileSaveData (Vector2Int _gridPos, TileType _type = TileType.grass, FlowStrenght _riverStrenght = FlowStrenght._00_)
    {
        ///Essential Data
        gridPos = _gridPos;
        flowIn_Neighbors = new bool[8] { false, false, false, false, false, false, false, false };
        flowOut_Neighbors = new bool[8] { false, false, false, false, false, false, false, false };

        ///State
        type = _type;
        riverStrenght = _riverStrenght;
    }
    public TileSaveData (int _gridPosX, int _gridPosY, TileType _type = TileType.grass, FlowStrenght _riverStrenght = FlowStrenght._00_)
    {
        ///Essential Data
        gridPos = new Vector2Int(_gridPosX, _gridPosY);
        flowIn_Neighbors = new bool[8] { false, false, false, false, false, false, false, false };
        flowOut_Neighbors = new bool[8] { false, false, false, false, false, false, false, false };

        ///State
        type = _type;
        riverStrenght = _riverStrenght;
    }
    public TileSaveData(TileSaveData tile)
    {
        ///Essential Data
        this.gridPos = tile.gridPos;
        this.flowIn_Neighbors = tile.flowIn_Neighbors;
        this.flowOut_Neighbors = tile.flowOut_Neighbors;

        ///State
        this.type = tile.type;
        this.riverStrenght = tile.riverStrenght;
    }
    #endregion

    public void LoadToGametile(GameTile tile)
    {
    }

}
