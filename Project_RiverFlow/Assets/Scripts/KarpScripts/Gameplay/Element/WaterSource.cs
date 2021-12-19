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
        { 
            tileOn = value;
        }
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
    public override bool isLinkable { get { return tileOn.linkAmount < 1; } }
    #endregion

    public override void AddLink(Direction dir) 
    {
        ///TODO : 
        Debug.Log("ToDo", this);
    }

    public Direction flowOut;
    public Sprite[] spriteDirections;
    private SpriteRenderer chosenSprite;

    void Start()
    {
        if (!tileOn.haveElement)
        {
            tileOn.element = this;
        }

        chosenSprite = GetComponent <SpriteRenderer>();
    }

    void Update()
    {
        VerifySpriteDirection();
    }

    private void VerifySpriteDirection()
    {
        if (tileOn.linkAmount == 1)
        {
            for (int i = 0; i < tileOn.neighbors.Length; i++)
            {
                if (tileOn.GetLinkedTile()[0] == tileOn.neighbors[i])
                {
                    chosenSprite.sprite = spriteDirections[i];
                }
            }
        }
    }
}
