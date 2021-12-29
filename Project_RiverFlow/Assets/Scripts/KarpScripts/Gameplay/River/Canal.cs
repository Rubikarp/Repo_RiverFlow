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
    public List<Canal> parentCanals;//ID killer

    public Canal(Vector2Int _startNode, Vector2Int _endNode)
    {
        startNode = _startNode;
        canalTiles = new List<Vector2Int>();
        endNode = _endNode;

        parentCanals = new List<Canal>();
    }
    public Canal(Canal canal)
    {
        startNode = canal.startNode;
        canalTiles = new List<Vector2Int>(canal.canalTiles);
        endNode = canal.endNode;

        parentCanals = new List<Canal>();
    }
    public Canal(Vector2Int _startNode, Vector2Int _endNode, List<Vector2Int> _tiles)
    {
        startNode = _startNode;
        canalTiles = new List<Vector2Int>(_tiles);
        endNode = _endNode;

        parentCanals = new List<Canal>();
    }

}
