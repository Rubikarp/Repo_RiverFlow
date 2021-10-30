using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Canal
{
    public GameTile startNode;
    public GameTile endNode;
    [Space(10)]
    public RiverStrenght riverStrenght;
    public List<GameTile> tiles;

    public void FlowStrenghtChange(RiverStrenght newRiverStrenght)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].riverStrenght = newRiverStrenght;
        }
    }
}
