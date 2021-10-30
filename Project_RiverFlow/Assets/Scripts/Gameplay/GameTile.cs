using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile : MonoBehaviour
{
    [Header("Essential Data")]
    public Vector2Int position = new Vector2Int(0, 0);
    public GameTile[] neighbors = new GameTile[8];

    [Header("State")]
    public TileType type = TileType.soil;
    public bool isDuged = false;
    public bool isRiver = false;
    public RiverStrenght flowStrenght = RiverStrenght._00_;

    [Header("Input/Output")]
    public List<GameTile> linkedTile = new List<GameTile>();
    public List<Direction> flowIn = new List<Direction>();
    public List<Direction> flowOut = new List<Direction>();
    public bool isNode
    { get {return flowIn.Count == 1 & flowOut.Count == 1 ? false : true;}}
    
    #region Constructor 
    public GameTile(int pos_x = 0, int pos_y = 0)
    {
        this.position = new Vector2Int(pos_x, pos_y);
    }
    public GameTile(Vector2Int pos)
    {
        this.position = pos;
    }
    #endregion

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
