using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lake : Element
{
    [Header("Element Child")]
    public GameTile tileOn;
    public GameTile[] allTilesOn;
    public override GameTile TileOn
    {
        get 
        {
            if (tileOn != null)
            {
                return tileOn;
            }
            else
            {
                return TilesOn[0];
            }
        }
        set { tileOn = value; }
    }
    public override GameTile[] TilesOn
    {
        get { return allTilesOn; }
        set { allTilesOn = value; }
    }
    public override bool isLinkable { get { return true; } }

    [Header("Dependancy")]
    private TimeManager gameTime;

    [Header("State")]
    public bool isVertical; //s'il n'est pas vertical, il est horizontal. 
    public bool hasFish = false;
    private int neededTrees = 0;
    [Space(5)]
    public GameTile entryTile;
    public FlowStrenght entryFlow = FlowStrenght._25_;

    [Header("Visual")]
    public GameObject lakeMainBodySprite;
    public SpriteRenderer lakeRender;
    [Space(5)]
    [SerializeField] Sprite lake25;
    [SerializeField] Sprite lake50;
    [SerializeField] Sprite lake75;
    [SerializeField] Sprite lake100;
    private bool isTurned = false;

    void Start()
    {
        if (isTurned == false && isVertical == true)
        {
            lakeMainBodySprite.transform.localRotation = Quaternion.Euler(0, 0, 90);
            isTurned = true;
        }
    }

    private void Update()
    {
        entryFlow = entryTile.ReceivedFlow();
        switch (entryFlow)
        {
            case FlowStrenght._25_:
                lakeRender.sprite = lake25;
                break;
            case FlowStrenght._50_:
                lakeRender.sprite = lake50;
                break;
            case FlowStrenght._75_:
                lakeRender.sprite = lake75;
                break;
            case FlowStrenght._100_:
                lakeRender.sprite = lake100;
                break;
            default:
                break;
        }

        VerifyFish();
    }

    private void VerifyFish()
    {
        neededTrees = 0;

        if (isVertical == false)
        {
            for (int a = 0; a < allTilesOn.Length; a++)
            {
                if (allTilesOn[a].neighbors[1].neighbors[1].element is Plant && allTilesOn[a].neighbors[1].neighbors[1].IsIrrigate == true)
                {
                    neededTrees++;
                }

                if (allTilesOn[a].neighbors[5].neighbors[5].element is Plant && allTilesOn[a].neighbors[5].neighbors[5].IsIrrigate == true)
                {
                    neededTrees++;
                }
            }
        }
        else if (isVertical == true)
        {
            for (int b = 0; b < allTilesOn.Length; b++)
            {
                if (allTilesOn[b].neighbors[3].neighbors[3].element is Plant && allTilesOn[b].neighbors[3].neighbors[3].IsIrrigate == true)
                {
                    neededTrees++;
                }

                if (allTilesOn[b].neighbors[7].neighbors[7].element is Plant && allTilesOn[b].neighbors[7].neighbors[7].IsIrrigate == true)
                {
                    neededTrees++;
                }
            }
        }

        if (neededTrees == 6)
        {
            hasFish = true;
        }
        else
        {
            hasFish = false;
        }
    }

}
