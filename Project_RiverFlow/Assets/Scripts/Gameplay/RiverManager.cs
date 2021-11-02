using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverManager : MonoBehaviour
{
    public InputHandler input;
    public DigingHandler digging;

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
        digging.onLink.AddListener(OnLink);
        digging.onBreak.AddListener(OnBreak);
    }

    void Update()
    {
        WaterStep();

    }

    public void OnLink(GameTile startTile, GameTile endTile)
    {
        Canal canal = new Canal(startTile, endTile, new List<GameTile>(), startTile.riverStrenght);

        //Check if isloate (no tile have more than 2 link)
        if (startTile.linkedTile.Count < 2 && endTile.linkedTile.Count < 2)
        {
            canals.Add(canal);
            startTile.canalsIn.Add(canal);
            endTile.canalsIn.Add(canal);
        }
        else
        {
            //Check fusion of two canal (start and End have than 2 link)
            if (startTile.linkedTile.Count == 2 && endTile.linkedTile.Count == 2)
            {
                ///TODO : merge canal
                //startTile.canalsIn[0];
                //endTile.canalsIn[0];

            }
            else
            {
                //Check Prolonging an existing canal (start or End have than 2 link)
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
                        ///TODO confluence
                        canals.Add(canal);
                        startTile.canalsIn.Add(canal);
                        endTile.canalsIn.Add(canal);

                        haveAddCanal = true;
                    }
                }
                if (endTile.linkedTile.Count > 2)
                {
                    if (!haveAddCanal)
                    {
                        ///TODO confluence
                        canals.Add(canal);
                        startTile.canalsIn.Add(canal);
                        endTile.canalsIn.Add(canal);

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
                canal.endNode = canal.tiles[canal.tiles.Count-1];
                canal.tiles.RemoveAt(canal.tiles.Count-1);
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
            int index = canal.tiles.IndexOf(erasedTile);

            #region SecondPart
            //si plus de 2 tile après rupture alors je crer un nouveau canal
            // (tile + start + end) - ((tileIndex+1 car commence à 0)+start)
            if ((canal.tiles.Count+2) - ((index+1)+1) >= 2)
            {
                List<GameTile> temp = new List<GameTile>();
                for (int i = index + 2; i < canal.tiles.Count; i++)
                {
                    temp.Add(canal.tiles[i]);
                }
                Canal secondaryCanal = new Canal(canal.tiles[index + 1], canal.endNode, temp);

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
            // (start + tileIndex)
            if ((1 + (index - 1)) >= 2)
            {
                canal.endNode = canal.tiles[index - 1];
                for (int i = canal.tiles.Count-1; i > (index - 2); i--)
                {
                    canal.tiles.RemoveAt(i);
                }
            }
            else
            {
                canal.startNode.RemoveAllLinkedTile();
                //toute les tiles avant on les suppr
                for (int i = 0; i < canal.tiles.Count; i++)
                {
                    canal.tiles[i].RemoveAllLinkedTile();
                }
                canal.endNode.RemoveAllLinkedTile();
            }
            #endregion
        }
    }

    public void WaterStep()
    {
        for (int i = 0; i < canals.Count; i++)
        {
            if (canals[i].startNode.isRiver)
            {
                if (canals[i].tiles.Count > 0)
                {
                    canals[i].tiles[0].isRiver = canals[i].startNode.isRiver;
                    canals[i].tiles[0].riverStrenght = canals[i].startNode.riverStrenght;
                    for (int j = 1; j < canals[i].tiles.Count; j++)
                    {
                        canals[i].tiles[j].isRiver = canals[i].tiles[j - 1].isRiver;
                        canals[i].tiles[j].riverStrenght = canals[i].tiles[j - 1].riverStrenght;
                    }
                    canals[i].endNode.isRiver = canals[i].tiles[canals[i].tiles.Count - 1].isRiver;
                    canals[i].endNode.riverStrenght = canals[i].tiles[canals[i].tiles.Count - 1].riverStrenght;
                }
                else
                {
                    canals[i].endNode.isRiver = canals[i].startNode.isRiver;
                    canals[i].endNode.riverStrenght = canals[i].startNode.riverStrenght;
                }
            }
        }
    }

}
