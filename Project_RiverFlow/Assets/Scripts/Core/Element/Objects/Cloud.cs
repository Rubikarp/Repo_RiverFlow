using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : Element
{
    private TimeManager gameTime;

    public GameTile tileOn;
    public override GameTile TileOn
    {
        get { return tileOn; }
        set { tileOn = value; }
    }
    public override bool isLinkable { get { return true; } }
}
