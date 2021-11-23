using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lake : Element
{
    public GameTile tileOn;
    public GameTile[] allTilesOn;
    public override GameTile TileOn
    {
        get { return tileOn; }
        set { tileOn = value; }
    }
    public override GameTile[] TilesOn
    {
        get { return allTilesOn; }
        set { allTilesOn = value; }
    }

    public bool isVertical; //s'il n'est pas vertical, il est horizontal. 
    private GameTime gameTime;
    public override bool isLinkable { get { return true; } }

    // Start is called before the first frame update
    void Awake()
    {
        allTilesOn = new GameTile[3];

        if (isVertical == false)
        {
            allTilesOn[0] = tileOn.neighbors[3];
            allTilesOn[1] = tileOn;
            allTilesOn[2] = tileOn.neighbors[7];
        }
        else if (isVertical == true)
        {
            allTilesOn[0] = tileOn.neighbors[1];
            allTilesOn[1] = tileOn;
            allTilesOn[2] = tileOn.neighbors[5];
        }

        gameTime = GameTime.Instance;
        gameTime.onWaterSimulationStep.AddListener(FlowStep);
    }

    // Update is called once per frame
    void FlowStep()
    {
        if (isVertical == false)
        {
            for (int a = 0; a < allTilesOn.Length; a++)
            {
                if (allTilesOn[a].neighbors[1].linkedTile.Count == 0)
                {
                    allTilesOn[a].neighbors[1].receivedFlow += 1;
                }

                if (allTilesOn[a].neighbors[5].linkedTile.Count == 0)
                {
                    allTilesOn[a].neighbors[5].receivedFlow += 1;
                }
            }
        }

        else if (isVertical == true)
        {
            for (int b = 0; b < allTilesOn.Length; b++)
            {
                if (allTilesOn[b].neighbors[3].linkedTile.Count == 0)
                {
                    allTilesOn[b].neighbors[3].receivedFlow += 1;
                }

                if (allTilesOn[b].neighbors[7].linkedTile.Count == 0)
                {
                    allTilesOn[b].neighbors[7].receivedFlow += 1;
                }
            }
        }
    }
}
