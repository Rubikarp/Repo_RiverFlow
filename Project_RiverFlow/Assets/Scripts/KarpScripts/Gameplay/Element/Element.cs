using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    public Vector2Int gridPos;
    public virtual GameTile TileOn 
    {
        get
        {
            return null;
        }
        set
        {
            gridPos = value.gridPos;
        }
    }
    public virtual GameTile[] TilesOn
    {
        get
        {
            if (TileOn == null)
            {
                Debug.LogError("Can't find the tile where is the element", this);
                return new GameTile[1] { null };
            }
            return new GameTile[1] { TileOn };
        }
        set { TileOn = value[0]; }
    }
    public virtual bool isLinkable 
    { 
        get { return false; } 
    }
    public virtual bool IsLinkable()
    {
        return false;
    }

    public virtual void AddLink(Direction dir) { }
    public virtual void LinkElementToGrid(GameGrid grid)
    {
        //Link Element and Tile
        TileOn = grid.GetTile(gridPos);
        grid.GetTile(gridPos).element = this;
    }
    public virtual void UnLinkElementToGrid(GameGrid grid)
    {
        //UnLink Element and Tile
        grid.GetTile(gridPos).element = null;
        TileOn = null;
    }

}
