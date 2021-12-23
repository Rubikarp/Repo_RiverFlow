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

    public Canal(Vector2Int _startNode, Vector2Int _endNode)
    {
        startNode = _startNode;
        endNode = _endNode;
        canalTiles = new List<Vector2Int>();
    }
    public Canal(Canal canal)
    {
        startNode = canal.startNode;
        endNode = canal.endNode;
        canalTiles = new List<Vector2Int>(canal.canalTiles);
    }
    public Canal(Vector2Int _startNode, Vector2Int _endNode, List<Vector2Int> _tiles)
    {
        startNode = _startNode;
        endNode = _endNode;
        canalTiles = new List<Vector2Int>(_tiles);
    }

}
