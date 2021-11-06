using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TileData 
{
    [Header("Essential Data")]
    public Vector2Int position;
    public Element element;

    [Header("State")]
    public TileType type;
    public RiverStrenght riverStrenght;

    [Header("Stored Value")]
    public bool[] linkedToNeighbors;

    //Getter & Setter
    public bool isElement
    {
        get
        {
            if (element != null)
            {
                return true;
            }
            return false;
        }
    }

    #region Constructor 
    public TileData (Vector2Int _pos, Element _element = null, TileType _type = TileType.soil, bool _isDuged = false, RiverStrenght _riverStrenght = RiverStrenght._00_)
    {
        ///Essential Data
        position = _pos;
        linkedToNeighbors = new bool[8] { false, false , false , false , false , false , false , false };
        element = _element;

        ///State
        type = _type;
        riverStrenght = _riverStrenght;
    }
    public TileData (int x , int y, Element _element = null, TileType _type = TileType.soil, bool _isDuged = false, RiverStrenght _riverStrenght = RiverStrenght._00_)
    {
        ///Essential Data
        position = new Vector2Int(x, y);
        linkedToNeighbors = new bool[8] { false, false, false, false, false, false, false, false };
        element = _element;

        ///State
        type = _type;
        riverStrenght = _riverStrenght;
    }
    public TileData(TileData tile)
    {
        this.position = tile.position;
        this.linkedToNeighbors = tile.linkedToNeighbors;

        this.element = tile.element;

        this.type = tile.type;
        this.riverStrenght = tile.riverStrenght;
    }
    #endregion

    public void Save(GameTile tile)
    {
        this = tile.data;

        for (int i = 0; i < tile.linkedTile.Count; i++)
        {
            linkedToNeighbors[tile.LinkToNeighborPos(tile.linkedTile[i])] = true;
        }
    }

}
