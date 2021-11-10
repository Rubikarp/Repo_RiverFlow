using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Canal
{
    [Header("Data")]
    public GameTile startNode;
    public List<GameTile> tiles;
    public GameTile endNode;
    [Space(10)]
    public RiverStrenght riverStrenght;

    public Canal(Canal canal)
    {
        startNode = canal.startNode;
        endNode = canal.endNode;
        tiles = new List<GameTile>(canal.tiles);
        riverStrenght = canal.riverStrenght;
    }
    public Canal(GameTile _startNode, GameTile _endNode, List<GameTile> _tiles, RiverStrenght _riverStrengfht = RiverStrenght._00_)
    {
        startNode = _startNode;
        endNode = _endNode;
        tiles = new List<GameTile>(_tiles);
        riverStrenght = _riverStrengfht;
    }

    public static Canal Inverse(Canal canal, GameGrid grid)
    {
        List<GameTile> temp = new List<GameTile>(canal.tiles);
        temp.Reverse();
        return new Canal(grid.GetTile(canal.endNode.position), grid.GetTile(canal.startNode.position), canal.tiles, canal.riverStrenght);
    }
    public static Canal Merge(Canal startCanalEnd1, Canal startCanalEnd2)
    {
        Canal mergeCanal = new Canal();
        mergeCanal = new Canal(startCanalEnd1.startNode, startCanalEnd1.endNode, new List<GameTile>(startCanalEnd1.tiles), startCanalEnd1.riverStrenght);

        //add endNode to the end linkCanal
        mergeCanal.tiles.Add(startCanalEnd1.endNode);

        //add start linkCanal to the end linkCanal
        mergeCanal.tiles.Add(startCanalEnd2.startNode);
        mergeCanal.tiles.AddRange(startCanalEnd2.tiles);
        mergeCanal.endNode =  startCanalEnd2.endNode;

        return mergeCanal;
    }

    public void ChangeFlowStrenght(RiverStrenght newRiverStrenght)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].riverStrenght = newRiverStrenght;
        }
    }

}
