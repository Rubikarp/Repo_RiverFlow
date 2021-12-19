using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : Element
{
    private GameTime gameTime;

    public GameTile tileOn;
    public override GameTile TileOn
    {
        get { return tileOn; }
        set { tileOn = value; }
    }


    public override bool isLinkable { get { return true; } }

    // Start is called before the first frame update
    void Awake()
    {
        gameTime = GameTime.Instance;
        gameTime.onWaterSimulationStep.AddListener(FlowStep);
    }

    // Update is called once per frame
    void FlowStep()
    {
        if (tileOn.linkAmount >= 2)
        {
            tileOn.receivedFlow += 1;
        }
    }
}
