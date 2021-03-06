using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CanalExtension
{
    /// <summary>
    /// Return in index of the pos from 
    /// <para> [0 => startNode] [canalTile +1 => endNode] </para>
    /// <para> return -1 in case of OOB </para>
    /// </summary>
    public static int IndexOf(this Canal canal,Vector2Int checkPos)
    {
        if (canal.startNode == checkPos)
        {
            return 0;
        }
        if (canal.canalTiles.Contains(checkPos))
        {
            return 1 + canal.canalTiles.IndexOf(checkPos);
        }
        if (canal.endNode == (checkPos))
        {
            return canal.canalTiles.Count + 1  /* +2 - 1*/;
        }
        return -1;
    }
    public static bool Contains(this Canal canal, Vector2Int checkPos)
    {
        if (canal.startNode == checkPos) { return true; }
        if (canal.endNode == (checkPos)) { return true; }
        if (canal.canalTiles.Contains(checkPos)) { return true; }
        return false;
    }


    public static void Inverse(this Canal canal, GameGrid grid)
    {
        if (canal.canalTiles.Count > 0)
        {
            List<Vector2Int> invTile = new List<Vector2Int>(canal.canalTiles);
            invTile.Reverse();
            canal.canalTiles = invTile;

            Vector2Int start = canal.startNode;
            canal.startNode = canal.endNode;
            canal.endNode = start;

            //inverse sens
            GameTile.InverseLink(grid.GetTile(canal.startNode), grid.GetTile(canal.canalTiles[0]));
            for (int i = 0; i < canal.canalTiles.Count - 1; i++)
            {
                GameTile.InverseLink(grid.GetTile(canal.canalTiles[i]), grid.GetTile(canal.canalTiles[i + 1]));
            }
            GameTile.InverseLink(grid.GetTile(canal.canalTiles[canal.canalTiles.Count - 1]), grid.GetTile(canal.endNode));
        }
        else
        {
            Vector2Int start = canal.startNode;
            canal.startNode = canal.endNode;
            canal.endNode = start;

            //inverse sens
            GameTile.InverseLink(grid.GetTile(canal.startNode), grid.GetTile(canal.endNode));
        }
    }
    public static void Extend(this Canal canal, GameTile extremumTile, GameTile addedTile)
    {
        if (extremumTile.neighbors.Contain<GameTile>(addedTile))
        {
            if (canal.startNode == extremumTile.gridPos)
            {
                GameTile.Link(addedTile, extremumTile);

                List<Vector2Int> list = new List<Vector2Int>();
                list.Add(canal.startNode);
                list.AddRange(canal.canalTiles);
                canal.canalTiles = list;

                canal.startNode = addedTile.gridPos;
                addedTile.canalsIn.Add(canal);
            }
            else
            if (canal.endNode == extremumTile.gridPos)
            {
                GameTile.Link(extremumTile, addedTile);

                canal.canalTiles.Add(canal.endNode);
                canal.endNode = addedTile.gridPos;
                addedTile.canalsIn.Add(canal);
            }
            else
            {
                Debug.LogWarning("added tile is not at  any extremum");
            }
        }
        else
        {
            Debug.LogWarningFormat("{0} is not a neighbor of {1}",addedTile, extremumTile);
        }
    }

}
