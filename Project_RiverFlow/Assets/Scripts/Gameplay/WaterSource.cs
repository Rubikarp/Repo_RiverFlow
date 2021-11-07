using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSource : Element
{
    #region Element
    [Header("Element Data")]
    public GameTile tileOn;
    public override GameTile TileOn
    {
        get
        {
            if (tileOn == null)
            {
                Debug.LogError("Can't find the tile where is this WaterSource", this);
                return null;
            }
            return tileOn;
        }
        set
        { tileOn = value; }
    }
    public override GameTile[] TilesOn 
    {
        get 
        { 
            if(tileOn == null)
            {
                Debug.LogError("Can't find the tile where is this WaterSource", this);
                return new GameTile[1] { null };
            }
            return new GameTile[1] { tileOn }; 
        } 
        set { tileOn = value[0]; } 
    }
    public override bool isLinkable { get { return true; } }
    public override bool IsLinkable()
    {
        return true;
    }
    #endregion

    public override void AddLink(Direction dir) 
    {
        ///TODO : 
        Debug.Log("ToDo", this);
    }

    public Direction flowOut;

    void Start()
    {
        if (!tileOn.isElement)
        {
            tileOn.element = this;
        }
    }

    void Update()
    {
        
    }
}
