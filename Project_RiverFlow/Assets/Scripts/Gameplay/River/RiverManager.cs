using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obselite
{
    public class RiverManager : MonoBehaviour
    {
        public InputHandler input;
        public DigingHandler digging;
        public GameGrid grid;

        public List<Canal> canals = new List<Canal>();
        
        void Start()
        {
            digging.onLink.AddListener(OnLink);
            digging.onBreak.AddListener(OnBreak);
        }

        void Update()
        {
            //WaterStep();
        }

        
        public void OnLink(GameTile startTile, GameTile endTile)
        {
            Canal linkCanal = new Canal(startTile.gridPos, endTile.gridPos, new List<Vector2Int>(), startTile.riverStrenght);
            //Check if isloate (no tile have more than 2 link)
            if (startTile.linkedTile.Count < 2 && endTile.linkedTile.Count < 2)
            {
                canals.Add(linkCanal);
                startTile.canalsIn.Add(linkCanal);
                endTile.canalsIn.Add(linkCanal);
            }
            else
            {
                //Check fusion of two canal (start and End have than 2 link)
                if (startTile.linkedTile.Count == 2 && endTile.linkedTile.Count == 2)
                {
                    ///TODO : merge cCanal
                    if (startTile.gridPos == startTile.canalsIn[0].startNode)
                    {
                        if (endTile.gridPos == endTile.canalsIn[0].startNode)
                        {
                            Canal reverseStartCanal = Canal.Inverse(startTile.canalsIn[0]);
                            Canal mergeCanal = Canal.Merge(reverseStartCanal, endTile.canalsIn[0]);

                            //startTile.canalsIn[0].startNode.canalsIn.Remove(startTile.canalsIn[0]);
                            grid.GetTile(endTile.canalsIn[0].endNode).canalsIn.Remove(endTile.canalsIn[0]);
                            canals.RemoveAt(canals.IndexOf(startTile.canalsIn[0]));
                            canals.RemoveAt(canals.IndexOf(endTile.canalsIn[0]));

                            grid.GetTile(mergeCanal.startNode).canalsIn.Remove(startTile.canalsIn[0]);

                            grid.GetTile(mergeCanal.startNode).canalsIn.Add(mergeCanal);
                            grid.GetTile(mergeCanal.endNode).canalsIn.Add(mergeCanal);
                            for (int i = 0; i < mergeCanal.canalTiles.Count; i++)
                            {
                                grid.GetTile(mergeCanal.canalTiles[i]).canalsIn = new List<Canal>();
                                grid.GetTile(mergeCanal.canalTiles[i]).canalsIn.Add(mergeCanal);
                            }

                            canals.Add(mergeCanal);
                        }
                        else
                        if (endTile == grid.GetTile(endTile.canalsIn[0].endNode))
                        {
                            Canal reverseStartCanal = Canal.Inverse(startTile.canalsIn[0]);
                            Canal reverseEndCanal = Canal.Inverse(endTile.canalsIn[0]);

                            Canal mergeCanal = Canal.Merge(reverseStartCanal, endTile.canalsIn[0]);

                            //startTile.canalsIn[0].startNode.canalsIn.Remove(startTile.canalsIn[0]);
                            //endTile.canalsIn[0].endNode.canalsIn.Remove(endTile.canalsIn[0]);
                            canals.RemoveAt(canals.IndexOf(startTile.canalsIn[0]));
                            canals.RemoveAt(canals.IndexOf(endTile.canalsIn[0]));

                            grid.GetTile(mergeCanal.startNode).canalsIn.Remove(startTile.canalsIn[0]);
                            grid.GetTile(mergeCanal.endNode).canalsIn.Remove(endTile.canalsIn[0]);

                            grid.GetTile(mergeCanal.startNode).canalsIn.Add(mergeCanal);
                            grid.GetTile(mergeCanal.endNode).canalsIn.Add(mergeCanal);
                            for (int i = 0; i < mergeCanal.canalTiles.Count; i++)
                            {
                                grid.GetTile(mergeCanal.canalTiles[i]).canalsIn = new List<Canal>();
                                grid.GetTile(mergeCanal.canalTiles[i]).canalsIn.Add(mergeCanal);
                            }

                            canals.Add(mergeCanal);
                        }
                    }
                    else
                    if (startTile == grid.GetTile(startTile.canalsIn[0].endNode))
                    {
                        if (endTile == grid.GetTile(endTile.canalsIn[0].startNode))
                        {
                            //merge Canal = start
                            Canal mergeCanal = Canal.Merge(startTile.canalsIn[0], endTile.canalsIn[0]);

                            grid.GetTile(startTile.canalsIn[0].startNode).canalsIn.Remove(startTile.canalsIn[0]);
                            grid.GetTile(endTile.canalsIn[0].endNode).canalsIn.Remove(endTile.canalsIn[0]);
                            canals.RemoveAt(canals.IndexOf(startTile.canalsIn[0]));
                            canals.RemoveAt(canals.IndexOf(endTile.canalsIn[0]));

                            grid.GetTile(mergeCanal.startNode).canalsIn.Add(mergeCanal);
                            grid.GetTile(mergeCanal.endNode).canalsIn.Add(mergeCanal);
                            for (int i = 0; i < mergeCanal.canalTiles.Count; i++)
                            {
                                grid.GetTile(mergeCanal.canalTiles[i]).canalsIn = new List<Canal>();
                                grid.GetTile(mergeCanal.canalTiles[i]).canalsIn.Add(mergeCanal);
                            }

                            canals.Add(mergeCanal);
                        }
                        else
                        if (endTile == grid.GetTile(endTile.canalsIn[0].endNode))
                        {
                            Canal reverseEndCanal = Canal.Inverse(endTile.canalsIn[0]);
                            Canal mergeCanal = Canal.Merge(startTile.canalsIn[0], reverseEndCanal);

                            grid.GetTile(startTile.canalsIn[0].startNode).canalsIn.Remove(startTile.canalsIn[0]);
                            //endTile.canalsIn[0].endNode.canalsIn.RemoveAt(indexEndCanal);
                            canals.RemoveAt(canals.IndexOf(startTile.canalsIn[0]));
                            canals.RemoveAt(canals.IndexOf(endTile.canalsIn[0]));

                            grid.GetTile(mergeCanal.endNode).canalsIn.Remove(endTile.canalsIn[0]);

                            grid.GetTile(mergeCanal.startNode).canalsIn.Add(mergeCanal);
                            grid.GetTile(mergeCanal.endNode).canalsIn.Add(mergeCanal);
                            for (int i = 0; i < mergeCanal.canalTiles.Count; i++)
                            {
                                grid.GetTile(mergeCanal.canalTiles[i]).canalsIn = new List<Canal>();
                                grid.GetTile(mergeCanal.canalTiles[i]).canalsIn.Add(mergeCanal);
                            }

                            canals.Add(mergeCanal);
                        }
                    }
                }
                else
                {
                    //Check Prolonging an existing linkCanal (start or End have than 2 link)
                    if (startTile.linkedTile.Count == 2)
                    {
                        ExtendCanal(startTile.canalsIn[0], startTile, endTile);
                    }
                    if (endTile.linkedTile.Count == 2)
                    {
                        ExtendCanal(endTile.canalsIn[0], endTile, startTile);
                    }

                    bool haveAddCanal = false;
                    //Check Split on Tile (end/start have than more than 2 link)
                    if (startTile.linkedTile.Count > 2)
                    {
                        if (!haveAddCanal)
                        {
                            //Split
                            ///TODO

                            //The new Part
                            canals.Add(linkCanal);
                            startTile.canalsIn.Add(linkCanal);
                            endTile.canalsIn.Add(linkCanal);

                            haveAddCanal = true;
                        }
                    }
                    if (endTile.linkedTile.Count > 2)
                    {
                        if (!haveAddCanal)
                        {
                            //Split
                            ///TODO


                            //The new Part
                            //Extend Something ?
                            if (grid.GetTile(linkCanal.startNode).linkedTile.Count >= 2)
                            {
                                ExtendCanal(startTile.canalsIn[0], startTile, endTile);
                            }
                            else
                            {
                                canals.Add(linkCanal);
                                startTile.canalsIn.Add(linkCanal);
                                endTile.canalsIn.Add(linkCanal);
                            }

                            haveAddCanal = true;
                        }
                    }
                }
            }
        }
        public void OnBreak(GameTile erasedTile)
        {
            List<Canal> canals = new List<Canal>();
            canals = erasedTile.canalsIn;

            for (int i = 0; i < erasedTile.canalsIn.Count; i++)
            {
                ShortenCanal(erasedTile.canalsIn[i], erasedTile);
            }

        }
        
        public void ExtendCanal(Canal canal, GameTile extremumTile, GameTile addedTile)
        {
            if (canal.startNode == extremumTile.gridPos)
            {
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
                canal.canalTiles.Add(canal.endNode);

                canal.endNode = addedTile.gridPos;
                addedTile.canalsIn.Add(canal);
            }
        }
        public void ShortenCanal(Canal canal, GameTile erasedTile)
        {
            if (grid.GetTile(canal.startNode) == erasedTile)
            {
                //est ce qu'il reste une tile entre deux ?
                if (canal.canalTiles.Count > 0)
                {
                    grid.GetTile(canal.startNode).RemoveAllLinkedTile();
                    canal.startNode = canal.canalTiles[0];
                    canal.canalTiles.RemoveAt(0);
                }
                else
                {
                    grid.GetTile(canal.startNode).RemoveAllLinkedTile();
                    grid.GetTile(canal.endNode).RemoveAllLinkedTile();
                    canals.Remove(canal);
                }
            }
            else
            if (grid.GetTile(canal.endNode) == erasedTile)
            {
                //est ce qu'il reste une tile entre deux ?
                if (canal.canalTiles.Count > 0)
                {
                    grid.GetTile(canal.endNode).RemoveAllLinkedTile();
                    canal.endNode = canal.canalTiles[canal.canalTiles.Count - 1];
                    canal.canalTiles.RemoveAt(canal.canalTiles.Count - 1);
                }
                else
                {
                    grid.GetTile(canal.startNode).RemoveAllLinkedTile();
                    grid.GetTile(canal.endNode).RemoveAllLinkedTile();
                    canals.Remove(canal);
                }

            }
            else //Splitting
            {
                if (canal.canalTiles.Count <= 1)
                {
                    //3 block avec mid suppr
                    grid.GetTile(canal.startNode).RemoveAllLinkedTile();
                    for (int i = 0; i < canal.canalTiles.Count; i++)
                    {
                        grid.GetTile(canal.canalTiles[i]).RemoveAllLinkedTile();
                    }
                    grid.GetTile(canal.endNode).RemoveAllLinkedTile();
                    canals.Remove(canal);
                }
                else
                {
                    int index = canal.canalTiles.IndexOf(erasedTile.gridPos);
                    #region SecondPart
                    //si plus de 2 tile après rupture alors je crer un nouveau canal
                    // (tile + start + end) - ((tileIndex+1 car commence à 0)+start)
                    int _righttile = (canal.canalTiles.Count + 2) - ((index + 1) + 1);
                    if (_righttile >= 2)
                    {
                        List<Vector2Int> _templist = new List<Vector2Int>();
                        Canal secondaryCanal = new Canal(canal.canalTiles[canal.canalTiles.Count - 1], canal.endNode, _templist);
                        if (index + 2 < canal.canalTiles.Count)
                        {
                            for (int i = index + 2; i < canal.canalTiles.Count; i++)
                            {
                                _templist.Add(canal.canalTiles[i]);
                            }

                            secondaryCanal = new Canal(canal.canalTiles[index + 1], canal.endNode, _templist);
                        }


                        canals.Add(secondaryCanal);

                        //Updates Canals In
                        grid.GetTile(secondaryCanal.startNode).canalsIn.Remove(canal);
                        grid.GetTile(secondaryCanal.startNode).canalsIn.Add(secondaryCanal);
                        for (int i = 0; i < secondaryCanal.canalTiles.Count; i++)
                        {
                            grid.GetTile(secondaryCanal.canalTiles[i]).canalsIn.Remove(canal);
                            grid.GetTile(secondaryCanal.canalTiles[i]).canalsIn.Add(secondaryCanal);
                        }
                        grid.GetTile(secondaryCanal.endNode).canalsIn.Remove(canal);
                        grid.GetTile(secondaryCanal.endNode).canalsIn.Add(secondaryCanal);

                    }
                    else
                    {
                        //toute les canalTiles après on les suppr
                        for (int i = index + 1; i < canal.canalTiles.Count; i++)
                        {
                            grid.GetTile(canal.canalTiles[i]).RemoveAllLinkedTile();
                        }
                        grid.GetTile(canal.endNode).RemoveAllLinkedTile();
                    }
                    #endregion
                    #region FirstPart
                    //si plus de 2 tile avant rupture alors je raccorcie le canal
                    // (start + tileIndex (pas besoin de -1 car index commence à 0))
                    int _leftTile = (1 + index);
                    if (_leftTile >= 2)
                    {
                        canal.endNode = canal.canalTiles[index - 1];
                        for (int i = canal.canalTiles.Count - 1; i > (index - 2); i--)
                        {
                            canal.canalTiles.RemoveAt(i);
                        }
                    }
                    else
                    {
                        grid.GetTile(canal.startNode).RemoveAllLinkedTile();
                        //toute les canalTiles avant on les suppr
                        for (int i = 0; i < index; i++)
                        {
                            grid.GetTile(canal.canalTiles[i]).RemoveAllLinkedTile();
                        }
                        for (int i = index; i < canal.canalTiles.Count; i++)
                        {
                            if (i > 0)
                            {
                                grid.GetTile(canal.canalTiles[i]).linkedTile.Remove(new Direction((DirectionEnum)grid.GetTile(canal.canalTiles[i]).NeighborIndex(grid.GetTile(canal.canalTiles[i - 1]))));
                            }
                            else
                            {
                                grid.GetTile(canal.canalTiles[i]).linkedTile.Remove(new Direction((DirectionEnum)grid.GetTile(canal.canalTiles[i]).NeighborIndex(grid.GetTile(canal.startNode))));
                            }
                            grid.GetTile(canal.canalTiles[i]).canalsIn.Remove(canal);
                        }

                        if (canal.canalTiles.Count > 0)
                        {
                            grid.GetTile(canal.endNode).linkedTile.Remove(new Direction((DirectionEnum)grid.GetTile(canal.endNode).NeighborIndex(grid.GetTile(canal.canalTiles[canal.canalTiles.Count - 1]))));
                        }
                        else
                        {
                            grid.GetTile(canal.endNode).linkedTile.Remove(new Direction((DirectionEnum)grid.GetTile(canal.endNode).NeighborIndex(grid.GetTile(canal.startNode))));
                        }

                        grid.GetTile(canal.endNode).canalsIn.Remove(canal);
                    }
                    #endregion

                }
            }
        }
        
        public bool InCanals(Vector2Int tilePos)
        {
            bool result = false;

            for (int i = 0; i < canals.Count; i++)
            {
                if (tilePos == canals[i].startNode)
                { }
                else
                if (tilePos == canals[i].endNode)
                { }
                else
                {
                    for (int j = 0; j < canals[i].canalTiles.Count; j++)
                    {
                        if (tilePos == canals[i].canalTiles[j])
                        {
                            if (result)
                            {
                                Debug.LogError(tilePos + "is Contains in many canals", this);
                            }
                            result = true;
                        }
                    }
                }
            }
            return result;
        }
        public int CanalIn(Vector2Int tilePos)
        {
            for (int i = 0; i < canals.Count; i++)
            {
                for (int j = 0; j < canals[i].canalTiles.Count; j++)
                {
                    if (tilePos == canals[i].canalTiles[j])
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        public void RemoveCanalRef(GameTile startNode, List<GameTile> canalTiles, GameTile endNode, Canal canal)
        {
            startNode.canalsIn.Remove(canal);
            for (int i = 0; i < canalTiles.Count; i++)
            {
                canalTiles[i].canalsIn.Remove(canal);
            }
            endNode.canalsIn.Remove(canal);
        }
        public void AddCanalRef(GameTile startNode, List<GameTile> canalTiles, GameTile endNode, Canal canal)
        {
            startNode.canalsIn.Add(canal);
            for (int i = 0; i < canalTiles.Count; i++)
            {
                canalTiles[i].canalsIn.Add(canal);
            }
            endNode.canalsIn.Add(canal);
        }

        private void OnDrawGizmos()
        {
            for (int i = 0; i < canals.Count; i++)
            {
                if (canals[i].canalTiles.Count > 0)
                {
                    Debug.DrawLine(grid.TileToPos(canals[i].startNode), grid.TileToPos(canals[i].canalTiles[0]), Color.blue);
                    for (int j = 0; j < canals[i].canalTiles.Count - 1; j++)
                    {
                        Debug.DrawLine(grid.TileToPos(canals[i].canalTiles[j]), grid.TileToPos(canals[i].canalTiles[j + 1]), Color.blue);
                    }
                    Debug.DrawLine(grid.TileToPos(canals[i].canalTiles[canals[i].canalTiles.Count-1]), grid.TileToPos(canals[i].endNode), Color.blue);

                }
                else
                {
                    Debug.DrawLine(grid.TileToPos(canals[i].startNode), grid.TileToPos(canals[i].endNode), Color.blue);
                }
            }
        }
    }
}
