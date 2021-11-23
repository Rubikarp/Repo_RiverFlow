using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Canal
{
    [Header("Data")]
    public Vector2Int startNode;
    public List<Vector2Int> canalTiles;
    public Vector2Int endNode;
    [Space(10)]
    public FlowStrenght riverStrenght;

    public Canal()
    {
        startNode = Vector2Int.zero;
        endNode = Vector2Int.zero;
        canalTiles = new List<Vector2Int>();
        riverStrenght = FlowStrenght._00_;

    }
    public Canal(Canal canal)
    {
        startNode = canal.startNode;
        endNode = canal.endNode;
        canalTiles = new List<Vector2Int>(canal.canalTiles);
        riverStrenght = canal.riverStrenght;
    }
    public Canal(Vector2Int _startNode, Vector2Int _endNode, List<Vector2Int> _tiles, FlowStrenght _riverStrengfht = FlowStrenght._00_)
    {
        startNode = _startNode;
        endNode = _endNode;
        canalTiles = new List<Vector2Int>(_tiles);
        riverStrenght = _riverStrengfht;
    }

    public static Canal Inverse(Canal canal)
    {
        List<Vector2Int> temp = new List<Vector2Int>(canal.canalTiles);
        temp.Reverse();
        return new Canal(canal.endNode, canal.startNode, canal.canalTiles, canal.riverStrenght);
    }
    public static Canal Merge(Canal startCanalEnd1, Canal startCanalEnd2)
    {
        Canal mergeCanal = new Canal();
        mergeCanal = new Canal(startCanalEnd1.startNode, startCanalEnd1.endNode, new List<Vector2Int>(startCanalEnd1.canalTiles), startCanalEnd1.riverStrenght);

        //add endNode to the end linkCanal
        mergeCanal.canalTiles.Add(startCanalEnd1.endNode);

        //add start linkCanal to the end linkCanal
        mergeCanal.canalTiles.Add(startCanalEnd2.startNode);
        mergeCanal.canalTiles.AddRange(startCanalEnd2.canalTiles);
        mergeCanal.endNode = startCanalEnd2.endNode;

        return mergeCanal;
    }

    public void ChangeFlowStrenght(FlowStrenght newRiverStrenght, GameGrid grid)
    {
        for (int i = 0; i < canalTiles.Count; i++)
        {
            grid.GetTile(canalTiles[i]).riverStrenght = newRiverStrenght;
        }
    }

}
