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

    private void Update()
    {
        if (TileOn != null)
        {
            if (TileOn.ReceivedFlow() == FlowStrenght._00_)
            {
                UnLinkElementToGrid(GameGrid.Instance);
                Destroy(gameObject);
            }
        }
    }
    public override void UnLinkElementToGrid(GameGrid grid)
    {
        //UnLink Element and Tile
        TileOn.element = null;
        TileOn = null;
        InventoryManager.Instance.cloudsAmmount++;
        Destroy(gameObject);
    }
}
