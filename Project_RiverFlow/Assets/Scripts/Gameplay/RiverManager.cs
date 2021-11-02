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
                        canals.Add(canal);
                        startTile.canalsIn.Add(canal);
                        endTile.canalsIn.Add(canal);

                        haveAddCanal = true;
                    }
                }
            }
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
}
