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
            /*
            input.onLeftClickUp.AddListener();
            input.onLeftClicking.AddListener();
            input.onLeftClickDown.AddListener();
            input.onRightClickUp.AddListener();
            input.onRightClicking.AddListener();
            input.onRightClickDown.AddListener();
            */
            //digging.onLink.AddListener(OnLink);
            //digging.onBreak.AddListener(OnBreak);
        }
        /*
            void Update()
            {
                WaterStep();
            }

            public void OnLink(GameTile startTile, GameTile endTile)
            {
                Canal linkCanal = new Canal(startTile, endTile, new List<GameTile>(), startTile.riverStrenght);
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
                        if (startTile == startTile.canalsIn[0].startNode)
                        {
                            if (endTile == endTile.canalsIn[0].startNode)
                            {
                                Canal reverseStartCanal = Canal.Inverse(startTile.canalsIn[0], grid);
                                Canal mergeCanal = Canal.Merge(reverseStartCanal, endTile.canalsIn[0]);

                                //startTile.canalsIn[0].startNode.canalsIn.Remove(startTile.canalsIn[0]);
                                endTile.canalsIn[0].endNode.canalsIn.Remove(endTile.canalsIn[0]);
                                canals.RemoveAt(canals.IndexOf(startTile.canalsIn[0]));
                                canals.RemoveAt(canals.IndexOf(endTile.canalsIn[0]));

                                mergeCanal.startNode.canalsIn.Remove(startTile.canalsIn[0]);

                                mergeCanal.startNode.canalsIn.Add(mergeCanal);
                                mergeCanal.endNode.canalsIn.Add(mergeCanal);
                                for (int i = 0; i < mergeCanal.tiles.Count; i++)
                                {
                                    mergeCanal.tiles[i].canalsIn = new List<Canal>();
                                    mergeCanal.tiles[i].canalsIn.Add(mergeCanal);
                                }

                                canals.Add(mergeCanal);
                            }
                            else
                            if (endTile == endTile.canalsIn[0].endNode)
                            {
                                Canal reverseStartCanal = Canal.Inverse(startTile.canalsIn[0], grid);
                                Canal reverseEndCanal = Canal.Inverse(endTile.canalsIn[0], grid);

                                Canal mergeCanal = Canal.Merge(reverseStartCanal, endTile.canalsIn[0]);

                                //startTile.canalsIn[0].startNode.canalsIn.Remove(startTile.canalsIn[0]);
                                //endTile.canalsIn[0].endNode.canalsIn.Remove(endTile.canalsIn[0]);
                                canals.RemoveAt(canals.IndexOf(startTile.canalsIn[0]));
                                canals.RemoveAt(canals.IndexOf(endTile.canalsIn[0]));

                                mergeCanal.startNode.canalsIn.Remove(startTile.canalsIn[0]);
                                mergeCanal.endNode.canalsIn.Remove(endTile.canalsIn[0]);

                                mergeCanal.startNode.canalsIn.Add(mergeCanal);
                                mergeCanal.endNode.canalsIn.Add(mergeCanal);
                                for (int i = 0; i < mergeCanal.tiles.Count; i++)
                                {
                                    mergeCanal.tiles[i].canalsIn = new List<Canal>();
                                    mergeCanal.tiles[i].canalsIn.Add(mergeCanal);
                                }

                                canals.Add(mergeCanal);
                            }
                        }
                        else
                        if (startTile == startTile.canalsIn[0].endNode)
                        {
                            if (endTile == endTile.canalsIn[0].startNode)
                            {
                                //merge Canal = start
                                Canal mergeCanal = Canal.Merge(startTile.canalsIn[0], endTile.canalsIn[0]);


                                startTile.canalsIn[0].startNode.canalsIn.Remove(startTile.canalsIn[0]);
                                endTile.canalsIn[0].endNode.canalsIn.Remove(endTile.canalsIn[0]);
                                canals.RemoveAt(canals.IndexOf(startTile.canalsIn[0]));
                                canals.RemoveAt(canals.IndexOf(endTile.canalsIn[0]));

                                mergeCanal.startNode.canalsIn.Add(mergeCanal);
                                mergeCanal.endNode.canalsIn.Add(mergeCanal);
                                for (int i = 0; i < mergeCanal.tiles.Count; i++)
                                {
                                    mergeCanal.tiles[i].canalsIn = new List<Canal>() ;
                                    mergeCanal.tiles[i].canalsIn.Add(mergeCanal);
                                }

                                canals.Add(mergeCanal);
                            }
                            else
                            if (endTile == endTile.canalsIn[0].endNode)
                            {
                                Canal reverseEndCanal = Canal.Inverse(endTile.canalsIn[0],grid);
                                Canal mergeCanal = Canal.Merge(startTile.canalsIn[0], reverseEndCanal);

                                startTile.canalsIn[0].startNode.canalsIn.Remove(startTile.canalsIn[0]);
                                //endTile.canalsIn[0].endNode.canalsIn.RemoveAt(indexEndCanal);
                                canals.RemoveAt(canals.IndexOf(startTile.canalsIn[0]));
                                canals.RemoveAt(canals.IndexOf(endTile.canalsIn[0]));

                                mergeCanal.endNode.canalsIn.Remove(endTile.canalsIn[0]);

                                mergeCanal.startNode.canalsIn.Add(mergeCanal);
                                mergeCanal.endNode.canalsIn.Add(mergeCanal);
                                for (int i = 0; i < mergeCanal.tiles.Count; i++)
                                {
                                    mergeCanal.tiles[i].canalsIn = new List<Canal>();
                                    mergeCanal.tiles[i].canalsIn.Add(mergeCanal);
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
                                if(linkCanal.startNode.linkedTile.Count >= 2)
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
                if (canal.startNode == extremumTile)
                {
                    List<GameTile> list = new List<GameTile>();
                    list.Add(canal.startNode);
                    list.AddRange(canal.tiles);
                    canal.tiles = list;

                    canal.startNode = addedTile;
                    addedTile.canalsIn.Add(canal);
                }
                else
                if (canal.endNode == extremumTile)
                {
                    canal.tiles.Add(canal.endNode);

                    canal.endNode = addedTile;
                    addedTile.canalsIn.Add(canal);
                }
            }
            public void ShortenCanal(Canal canal, GameTile erasedTile)
            {
                if (canal.startNode == erasedTile)
                {
                    //est ce qu'il reste une tile entre deux ?
                    if (canal.tiles.Count > 0)
                    {
                        canal.startNode.RemoveAllLinkedTile();
                        canal.startNode = canal.tiles[0];
                        canal.tiles.RemoveAt(0);
                    }
                    else
                    {
                        canal.startNode.RemoveAllLinkedTile();
                        canal.endNode.RemoveAllLinkedTile();
                        canals.Remove(canal);
                    }
                }
                else
                if (canal.endNode == erasedTile)
                {
                    //est ce qu'il reste une tile entre deux ?
                    if (canal.tiles.Count > 0)
                    {
                        canal.endNode.RemoveAllLinkedTile();
                        canal.endNode = canal.tiles[canal.tiles.Count - 1];
                        canal.tiles.RemoveAt(canal.tiles.Count - 1);
                    }
                    else
                    {
                        canal.startNode.RemoveAllLinkedTile();
                        canal.endNode.RemoveAllLinkedTile();
                        canals.Remove(canal);
                    }

                }
                else //Splitting
                {
                    if (canal.tiles.Count <= 1)
                    {
                        //3 block avec mid suppr
                        canal.startNode.RemoveAllLinkedTile();
                        for (int i = 0; i < canal.tiles.Count; i++)
                        {
                            canal.tiles[i].RemoveAllLinkedTile();
                        }
                        canal.endNode.RemoveAllLinkedTile();
                        canals.Remove(canal);
                    }
                    else
                    {
                        int index = canal.tiles.IndexOf(erasedTile);
                        #region SecondPart
                        //si plus de 2 tile après rupture alors je crer un nouveau canal
                        // (tile + start + end) - ((tileIndex+1 car commence à 0)+start)
                        int _righttile = (canal.tiles.Count + 2) - ((index + 1) + 1);
                        if (_righttile >= 2)
                        {
                            List<GameTile> _templist = new List<GameTile>();
                            Canal secondaryCanal = new Canal(canal.tiles[canal.tiles.Count - 1], canal.endNode, _templist);
                            if (index + 2 < canal.tiles.Count)
                            {
                                for (int i = index + 2; i < canal.tiles.Count; i++)
                                {
                                    _templist.Add(canal.tiles[i]);
                                }

                                secondaryCanal = new Canal(canal.tiles[index + 1], canal.endNode, _templist);
                            }


                            canals.Add(secondaryCanal);

                            //Updates Canals In
                            secondaryCanal.startNode.canalsIn.Remove(canal);
                            secondaryCanal.startNode.canalsIn.Add(secondaryCanal);
                            for (int i = 0; i < secondaryCanal.tiles.Count; i++)
                            {
                                secondaryCanal.tiles[i].canalsIn.Remove(canal);
                                secondaryCanal.tiles[i].canalsIn.Add(secondaryCanal);
                            }
                            secondaryCanal.endNode.canalsIn.Remove(canal);
                            secondaryCanal.endNode.canalsIn.Add(secondaryCanal);

                        }
                        else
                        {
                            //toute les tiles après on les suppr
                            for (int i = index + 1; i < canal.tiles.Count; i++)
                            {
                                canal.tiles[i].RemoveAllLinkedTile();
                            }
                            canal.endNode.RemoveAllLinkedTile();
                        }
                        #endregion
                        #region FirstPart
                        //si plus de 2 tile avant rupture alors je raccorcie le canal
                        // (start + tileIndex (pas besoin de -1 car index commence à 0))
                        int _leftTile = (1 + index );
                        if (_leftTile >= 2)
                        {
                            canal.endNode = canal.tiles[index - 1];
                            for (int i = canal.tiles.Count - 1; i > (index - 2); i--)
                            {
                                canal.tiles.RemoveAt(i);
                            }
                        }
                        else
                        {
                            canal.startNode.RemoveAllLinkedTile();
                            //toute les tiles avant on les suppr
                            for (int i = 0; i < index; i++)
                            {
                                canal.tiles[i].RemoveAllLinkedTile();
                            }
                            for (int i = index; i < canal.tiles.Count; i++)
                            {
                                canal.tiles[i].linkedTile.Remove(i > 0 ? canal.tiles[i-1] : canal.startNode);
                                canal.tiles[i].canalsIn.Remove(canal);
                            }
                            canal.endNode.linkedTile.Remove(canal.tiles.Count > 0? canal.tiles[canal.tiles.Count-1]: canal.startNode);
                            canal.endNode.canalsIn.Remove(canal);
                        }
                        #endregion

                    }
                }
            }

            public void RemoveCanalRef(GameTile startNode, List<GameTile> tiles, GameTile endNode, Canal canal)
            {
                startNode.canalsIn.Remove(canal);
                for (int i = 0; i < tiles.Count; i++)
                {
                    tiles[i].canalsIn.Remove(canal);
                }
                endNode.canalsIn.Remove(canal);
            }
            public void AddCanalRef(GameTile startNode, List<GameTile> tiles, GameTile endNode, Canal canal)
            {
                startNode.canalsIn.Add(canal);
                for (int i = 0; i < tiles.Count; i++)
                {
                    tiles[i].canalsIn.Add(canal);
                }
                endNode.canalsIn.Add(canal);
            }

        */
        private void OnDrawGizmos()
        {
            /*
            for (int i = 0; i < canals.Count; i++)
            {
                if (canals[i].tiles.Count > 0)
                {
                    Debug.DrawLine(grid.TileToPos(canals[i].startNode.gridPos), grid.TileToPos(canals[i].tiles[0].gridPos), Color.blue);
                    for (int j = 0; j < canals[i].tiles.Count - 1; j++)
                    {
                        Debug.DrawLine(grid.TileToPos(canals[i].tiles[j].gridPos), grid.TileToPos(canals[i].tiles[j + 1].gridPos), Color.blue);
                    }
                    Debug.DrawLine(grid.TileToPos(canals[i].tiles[canals[i].tiles.Count-1].gridPos), grid.TileToPos(canals[i].endNode.gridPos), Color.blue);

                }
                else
                {
                    Debug.DrawLine(grid.TileToPos(canals[i].startNode.gridPos), grid.TileToPos(canals[i].endNode.gridPos), Color.blue);
                }
            }
            */
        }
    }
}
