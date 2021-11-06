using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile : MonoBehaviour
{
    [Header("Essential Data")]
    public TileData data;
    public Vector2Int gridPos
    {
        get
        {
            return data.position;
        }
    }
    public Vector3 worldPos
    {
        get
        {
            return transform.position;
        }
    }
    public Vector2 worldPos2D
    {
        get
        {
            return transform.position;
        }
    }
    public Element element
    {
        get
        {
            return data.element;
        }
        set
        {
            data.element = value;
        }
    }
    public TileType type
    {
        get
        {
            return data.type;
        }
        set
        {
            data.type = value;
        }
    }
    public RiverStrenght riverStrenght
    {
        get
        {
            return data.riverStrenght;
        }
        set
        {
            data.riverStrenght = value;
        }
    }
    [Space(10)]
    public GameTile[] neighbors;
    [Space(5)]
    public List<Canal> canalsIn = new List<Canal>();

    public bool isElement
    {
        get
        {
            return data.isElement;
        }
    }
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
    public bool isDuged
    {
        get
        {
            if (linkedTile.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
    public bool isRiver
    {
        get
        {
            if (data.riverStrenght > 0 && isDuged)
            {
                return true;
            }
            return false;
        }
    }

    [Header("Input/Output")]
    public List<GameTile> linkedTile = new List<GameTile>();
    public bool isNode
    { 
        get 
        {
            return linkedTile.Count == 2 ? false : true;
        }
    }


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
