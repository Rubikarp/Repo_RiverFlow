using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Canal
{
    public string name;
    [Header("Data")]
    public GameTile startNode;
    public List<GameTile> tiles;
    public GameTile endNode;
    [Space(10)]
    public RiverStrenght riverStrenght;

    public Canal(GameTile _startNode, GameTile _endNode, List<GameTile> _tiles, RiverStrenght _riverStrengfht = RiverStrenght._00_)
    {
        this.startNode = _startNode;
        this.endNode = _endNode;
        this.tiles = _tiles;
        this.riverStrenght = _riverStrengfht;
    }

    public Canal Inverse(Canal canal)
    {
        canal.tiles.Reverse();
        return new Canal(canal.endNode, canal.startNode, canal.tiles, canal.riverStrenght);
    }

    public void ChangeFlowStrenght(RiverStrenght newRiverStrenght)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].riverStrenght = newRiverStrenght;
        }
    }

}
