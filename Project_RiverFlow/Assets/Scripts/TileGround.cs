using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    soil,
    clay,
    sand
}

public enum RiverStrenght
{ 
    _00_,
    _25_,
    _50_,
    _75_,
    _100_
}

public class TileGround : Tile
{
    [Header("Input/Output")]
    public List<Direction> flowIn = new List<Direction>();
    public List<Direction> flowOut = new List<Direction>();
    public bool isNode{
        get 
        {
            if(flowIn.Count==1 & flowOut.Count == 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    [Header("Data")]
    public TileType type = TileType.soil;


    [Header("State")]
    public bool isDuged = false;
    public bool isRiver = false;
    public RiverStrenght flowStrenght = RiverStrenght._00_;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
