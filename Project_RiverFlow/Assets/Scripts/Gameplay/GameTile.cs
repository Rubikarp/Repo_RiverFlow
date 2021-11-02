using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile : MonoBehaviour
{
    [Header("Essential Data")]
    public Vector2Int position = new Vector2Int(0, 0);
    public GameTile[] neighbors = new GameTile[8];

    public Element element;
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

    public List<Canal> canalsIn = new List<Canal>();
    public bool isLinkable 
    { 
        get 
        {
            if (element != null)
            {
                return element.isLinkable;
            }
            return true; 
        } 
    }

    [Header("State")]
    public TileType type = TileType.soil;
    public bool isDuged = false;
    public bool isRiver = false;
    public RiverStrenght riverStrenght = RiverStrenght._00_;

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
    #endregion

    void Start()
    {
        
    }
    void Update()
    {
        
    }

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
        linkedTile = new List<GameTile>();
    }

}
