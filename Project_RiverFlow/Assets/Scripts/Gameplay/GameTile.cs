using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile : MonoBehaviour
{
    [Header("Essential Data")]
    public TileData data;
    public GameTile[] neighbors;
    [Space(10)]
    public List<Canal> canalsIn = new List<Canal>();
    public bool isLinkable 
    { 
        get 
        {
            if (!data.element.isLinkable)
            {
                return false;
            }
            else
            if (linkedTile.Count >= 3)
            {
                return false;
            }
            return true; 
        } 
    }

    [Header("Input/Output")]
    public List<GameTile> linkedTile = new List<GameTile>();
    public bool isNode
    { get {return linkedTile.Count == 2 ? false : true;}}

    #region Constructor 
    public GameTile()
    {
        linkedTile = new List<GameTile>();
    }
    public GameTile(int pos_x = 0, int pos_y = 0)
    {
        this.position = new Vector2Int(pos_x, pos_y);
        linkedTile = new List<GameTile>();
    }
    public GameTile(Vector2Int pos)
    {
        this.position = pos;
        linkedTile = new List<GameTile>();
    }
    public GameTile(GameTile tile)
    {
        this.position = tile.position;
        this.neighbors = tile.neighbors;
        this.element = tile.element;
        this.canalsIn = tile.canalsIn;
        this.type = tile.type;
        this.isDuged = tile.isDuged;
        this.isRiver = tile.isRiver;
        this.riverStrenght = tile.riverStrenght;
        this.linkedTile = tile.linkedTile;
    }

    #endregion

    void Start()
    {
        
    }
    void Update()
    {
        
    }

    //LINK
    public void AddLinkedTile(GameTile addedTile)
    {
        //Check if tile is already in list
        for (int i = 0; i < linkedTile.Count; i++)
        {
            if (linkedTile[i] == addedTile)
            {
                //Tile Already here! 
                return;
            }
        }
        
        linkedTile.Add(addedTile);
    }
    public void RemoveLinkedTile(GameTile removeTile)
    {
        for (int i = 0; i < linkedTile.Count; i++)
        {
            if(linkedTile[i] == removeTile)
            {
                linkedTile.RemoveAt(i);
            }
        }
        if (linkedTile.Count <1)
        {
            isDuged = false;
            linkedTile = new List<GameTile>();
        }
    }
    public void RemoveAllLinkedTile()
    {
        GameTile removeTile;
        for (int i = 0; i < linkedTile.Count; i++)
        {
            removeTile = linkedTile[i];
            removeTile.RemoveLinkedTile(this);
        }

        isDuged = false;
        isRiver = false;
        riverStrenght = RiverStrenght._00_;
        
        canalsIn = new List<Canal>();
        linkedTile = new List<GameTile>();
    }
    
    //GameTile to Data
    public void LoadLinkedTile(TileData data)
    {
        for (int i = 0; i < 8; i++)
        {
            if (data.linkedToNeighbors[i])
            {
                linkedTile.Add(neighbors[i]);
            }
        }
    }
    public int LinkToNeighborPos(GameTile tile)
    {
        for (int i = 0; i < 8; i++)
        {
            if(neighbors[i] == tile)
            {
                return i;
            }
        }
        Debug.LogError("Linked tile "+ tile.data.position + " n'est pas un voisin de "+ this.data.position +".", this);
        return 8;
    }
}
