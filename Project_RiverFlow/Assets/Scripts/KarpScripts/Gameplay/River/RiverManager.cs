using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverManager : Singleton<RiverManager>
{
    public InputHandler input;
    public DigingHandler digging;
    public GameGrid grid;
    public GameTime gameTime;

    public List<Canal> canals = new List<Canal>();

    void Start()
    {
        digging.onLink.AddListener(OnLinkV2);
        digging.onBreak.AddListener(OnBreak);

        gameTime = GameTime.Instance;
        gameTime.onWaterSimulationStep.AddListener(FlowStep);

    }
    private void OnDestroy()
    {
        digging.onLink.RemoveListener(OnLink);
        digging.onBreak.RemoveListener(OnBreak);

        gameTime.onWaterSimulationStep.RemoveListener(FlowStep);
    }

    private void OnLink(GameTile startTile, GameTile endTile)
    {
        //Check if isloate (no tile have link)
        if (startTile.linkedTile.Count < 1 && endTile.linkedTile.Count < 1)
        {
            GenerateRiverCanal(startTile.gridPos, endTile.gridPos);
        }
        //Check Prolonging an existing linkCanal (start or End have 1 link)
        else if (startTile.linkedTile.Count == 1)
        {
            if (endTile.linkedTile.Count < 1)
            {
                for (int i = 0; i < canals.Count; i++)
                {
                    if (startTile.canalsIn[0] == canals[i])
                    {
                        ExtendCanal(canals[i], startTile, endTile);
                    }
                }
            }
            else
            if (endTile.linkedTile.Count == 1)
            {
                if (startTile.riverStrenght == endTile.riverStrenght)
                {
                    for (int i = 0; i < canals.Count; i++)
                    {
                        if (startTile.canalsIn[0] == canals[i])
                        {
                            ExtendCanal(canals[i], startTile, endTile);
                        }
                    }
                }
                else
                {
                    if (startTile.gridPos == startTile.canalsIn[0].endNode)
                    {
                        if (endTile.gridPos == endTile.canalsIn[0].startNode)
                        {
                            Merge(startTile.canalsIn[0], endTile.canalsIn[0]);

                            for (int i = 0; i < canals.Count; i++)
                            {
                                if (endTile.canalsIn[0] == canals[i])
                                {
                                    AddCanalRef(canals[i], startTile.canalsIn[0]);
                                    RemoveCanalRef(canals[i], canals[i]);

                                    canals.Remove(canals[i]);
                                }
                            }
                        }
                        else
                        if (endTile.gridPos == endTile.canalsIn[0].endNode)
                        {
                            Inverse(endTile.canalsIn[0]);
                            Merge(startTile.canalsIn[0], endTile.canalsIn[0]);

                            for (int i = 0; i < canals.Count; i++)
                            {
                                if (endTile.canalsIn[0] == canals[i])
                                {
                                    AddCanalRef(canals[i], startTile.canalsIn[0]);
                                    RemoveCanalRef(canals[i], canals[i]);

                                    canals.Remove(canals[i]);
                                }
                            }
                        }
                    }
                    else
                    if (startTile.gridPos == startTile.canalsIn[0].startNode)
                    {
                        if (endTile.gridPos == endTile.canalsIn[0].startNode)
                        {
                            Inverse(startTile.canalsIn[0]);
                            Merge(startTile.canalsIn[0], endTile.canalsIn[0]);

                            for (int i = 0; i < canals.Count; i++)
                            {
                                if (endTile.canalsIn[0] == canals[i])
                                {
                                    AddCanalRef(canals[i], startTile.canalsIn[0]);
                                    RemoveCanalRef(canals[i], canals[i]);

                                    canals.Remove(canals[i]);
                                }
                            }
                        }
                        else
                        if (endTile == grid.GetTile(endTile.canalsIn[0].endNode))
                        {
                            Inverse(startTile.canalsIn[0]);
                            Inverse(endTile.canalsIn[0]);

                            Merge(startTile.canalsIn[0], endTile.canalsIn[0]);

                            for (int i = 0; i < canals.Count; i++)
                            {
                                if (endTile.canalsIn[0] == canals[i])
                                {
                                    AddCanalRef(canals[i], startTile.canalsIn[0]);
                                    RemoveCanalRef(canals[i], canals[i]);

                                    canals.Remove(canals[i]);
                                }
                            }
                        }
                    }
                }
            }
        }
        //Check link between to extremum (merge canal)
        else if (endTile.linkedTile.Count == 1)
        {
            for (int i = 0; i < canals.Count; i++)
            {
                if (endTile.canalsIn[0] == canals[i])
                {
                    ExtendCanal(canals[i], endTile, startTile);
                }
            }
        }
        else
        {
            //Check Split on Tile (end/start have than more than 2 link)
            if (startTile.linkedTile.Count >= 2)   //create divergence
            {
                if (!(startTile.canalsIn.Count >= 2))
                {
                    //Split
                    SplitCanal(startTile.canalsIn[0], startTile);
                }
                //The new Part
                GenerateRiverCanal(startTile.gridPos, endTile.gridPos);
            }
            else
            if (endTile.linkedTile.Count >= 2)     //create confluence
            {
                if (!(endTile.canalsIn.Count >= 2))
                {
                    //Split
                    SplitCanal(endTile.canalsIn[0], endTile);
                }

                //The new Part
                //Extend Something ?
                if (grid.GetTile(startTile.gridPos).linkedTile.Count < 1)
                {
                    GenerateRiverCanal(startTile.gridPos, endTile.gridPos);
                }
                else
                {
                    for (int i = 0; i < canals.Count; i++)
                    {
                        if (startTile.canalsIn[0] == canals[i])
                        {
                            ExtendCanal(canals[i], startTile, endTile);
                        }
                    }
                }
            }
        }

        FlowStep();
    }
    private void OnLinkV2(GameTile startTile, GameTile endTile)
    {
        #region works
        switch (startTile.linkedTile.Count)
        {
            case 0:
                switch (endTile.linkedTile.Count)
                {
                    case 0: //in a void
                        if (endTile.element is WaterSource)
                        {
                            GenerateRiverCanal(endTile.gridPos, startTile.gridPos);
                        }
                        else 
                        { 
                            GenerateRiverCanal(startTile.gridPos, endTile.gridPos); 
                        }
                        break;
                    case 1: //extending the end canal
                        for (int i = 0; i < canals.Count; i++)
                        {
                            if (endTile.canalsIn[0] == canals[i])
                            {
                                ExtendCanal(canals[i], endTile, startTile);
                                Inverse(canals[i]);
                            }
                        }
                        break;
                    default: //x >= 2
                        if (endTile.canalsIn.Count == 1)
                        {
                            //Split
                            SplitCanal(endTile.canalsIn[0], endTile);
                        }

                        //The new Part
                        if (endTile.riverStrenght > 0)
                        {
                            GenerateRiverCanal(endTile.gridPos, startTile.gridPos);
                        }
                        else
                        {
                            GenerateRiverCanal(startTile.gridPos, endTile.gridPos);
                        }
                        break;
                }
                break;
            case 1:
                switch (endTile.linkedTile.Count)
                {
                    case 0: //extend in void
                        for (int i = 0; i < canals.Count; i++)
                        {
                            if (startTile.canalsIn[0] == canals[i])
                            {
                                ExtendCanal(canals[i], startTile, endTile);
                                if (endTile.element is WaterSource)
                                {
                                    Inverse(canals[i]);
                                }
                            }
                        }
                        break;
                    case 1: //to extremum
                        if (startTile.riverStrenght == endTile.riverStrenght)
                        {
                            for (int i = 0; i < canals.Count; i++)
                            {
                                if (startTile.canalsIn[0] == canals[i])
                                {
                                    ExtendCanal(canals[i], startTile, endTile);
                                }
                            }
                        }
                        else
                        if (startTile.riverStrenght > endTile.riverStrenght)
                        {
                            if (startTile.gridPos == startTile.canalsIn[0].endNode)
                            {
                                if (endTile.gridPos == endTile.canalsIn[0].startNode)
                                {
                                    Merge(startTile.canalsIn[0], endTile.canalsIn[0]);

                                    ReplaceCanal(endTile.canalsIn[0], startTile.canalsIn[0]);
                                }
                                else
                                if (endTile.gridPos == endTile.canalsIn[0].endNode)
                                {
                                    Inverse(endTile.canalsIn[0]);
                                    Merge(startTile.canalsIn[0], endTile.canalsIn[0]);

                                    ReplaceCanal(endTile.canalsIn[0], startTile.canalsIn[0]);
                                }
                            }
                            else if (startTile.gridPos == startTile.canalsIn[0].startNode)
                            {
                                if (endTile.gridPos == endTile.canalsIn[0].startNode)
                                {
                                    Inverse(startTile.canalsIn[0]);
                                    Merge(startTile.canalsIn[0], endTile.canalsIn[0]);

                                    ReplaceCanal(endTile.canalsIn[0], startTile.canalsIn[0]);
                                }
                                else
                                if (endTile == grid.GetTile(endTile.canalsIn[0].endNode))
                                {
                                    Inverse(startTile.canalsIn[0]);
                                    Inverse(endTile.canalsIn[0]);

                                    Merge(startTile.canalsIn[0], endTile.canalsIn[0]);

                                    ReplaceCanal(endTile.canalsIn[0], startTile.canalsIn[0]);
                                }
                            }

                        }
                        else //startTile.riverStrenght < endTile.riverStrenght
                        {
                            if (startTile.gridPos == startTile.canalsIn[0].endNode)
                            {
                                if (endTile.gridPos == endTile.canalsIn[0].startNode)
                                {
                                    Inverse(endTile.canalsIn[0]);
                                    Inverse(startTile.canalsIn[0]);

                                    Merge(endTile.canalsIn[0], startTile.canalsIn[0]);

                                    ReplaceCanal(startTile.canalsIn[0], endTile.canalsIn[0]);
                                }
                                else
                                if (endTile.gridPos == endTile.canalsIn[0].endNode)
                                {
                                    Inverse(startTile.canalsIn[0]);
                                    Merge(endTile.canalsIn[0], startTile.canalsIn[0]);

                                    ReplaceCanal(startTile.canalsIn[0], endTile.canalsIn[0]);
                                }
                            }
                            else
                            if (startTile.gridPos == startTile.canalsIn[0].startNode)
                            {
                                if (endTile.gridPos == endTile.canalsIn[0].startNode)
                                {
                                    Inverse(endTile.canalsIn[0]);
                                    Merge(endTile.canalsIn[0], startTile.canalsIn[0]);

                                    ReplaceCanal(startTile.canalsIn[0], endTile.canalsIn[0]);
                                }
                                else
                                if (endTile == grid.GetTile(endTile.canalsIn[0].endNode))
                                {
                                    Merge(endTile.canalsIn[0], startTile.canalsIn[0]);

                                    ReplaceCanal(startTile.canalsIn[0], endTile.canalsIn[0]);
                                }
                            }
                        }
                        break;
                    default: //x >= 2
                        if (endTile.canalsIn.Count == 1)
                        {
                            //Split
                            SplitCanal(endTile.canalsIn[0], endTile);
                        }
                        //Calcul Highest flow
                        int maxFlow = 0;
                        for (int i = 0; i < endTile.flowIn.Count; i++)
                        {
                            maxFlow = Mathf.Max(maxFlow, (int)endTile.GetNeighbor(endTile.flowIn[i]).riverStrenght);
                        }
                        //Case
                            ExtendCanal(startTile.canalsIn[0], startTile, endTile);
                        if (startTile.riverStrenght > (FlowStrenght)maxFlow) //j'override les deux autre flow
                        {
                            for (int i = 0; i < endTile.canalsIn.Count; i++)
                            {
                                if (endTile.canalsIn[i].endNode == endTile.gridPos)//ceux à contre sens
                                {
                                    if (endTile.canalsIn[i].canalTiles.Count < 1)//link simple
                                    {
                                        if (grid.GetTile(endTile.canalsIn[i].startNode).riverStrenght < (FlowStrenght)maxFlow)
                                        {
                                            if (endTile.canalsIn[i].canalTiles.Count < 1)//link simple
                                            {
                                                ErasedCanal(endTile.canalsIn[i]);
                                            }
                                            else
                                            {
                                                SplitCanal(endTile.canalsIn[i], grid.GetTile(endTile.canalsIn[i].canalTiles[0]));
                                                Inverse(endTile.canalsIn[i]);
                                            }
                                        }
                                        else
                                        if (grid.GetTile(endTile.canalsIn[i].startNode).riverStrenght > (FlowStrenght)(2 * maxFlow))
                                        {
                                            SplitCanal(endTile.canalsIn[i], grid.GetTile(endTile.canalsIn[i].canalTiles[0]));
                                            Inverse(startTile.canalsIn[0]);
                                        }
                                        else //==
                                        {
                                            //is okay
                                        }

                                    }
                                }
                            }
                        }
                        else if (startTile.riverStrenght < (FlowStrenght)(2 * maxFlow)) //je me fais peut etre override
                        {
                            Inverse(startTile.canalsIn[0]);
                        }
                        else
                        {
                            //je m'imisse sans soucis
                        }
                        break;
                }
                break;
            default: // 2 ou +
                switch (endTile.linkedTile.Count)
                {
                    case 0: //new divergence
                        if(startTile.canalsIn.Count == 1)
                        {
                            //Split
                            SplitCanal(startTile.canalsIn[0], startTile);
                        }
                        GenerateRiverCanal(startTile.gridPos, endTile.gridPos);
                        break;
            #endregion
                    case 1: //extend to convergence
                        if (startTile.canalsIn.Count == 1)
                        {
                            //Split
                            SplitCanal(startTile.canalsIn[0], startTile);
                        }
                        //Calcul Highest flow
                        int maxFlow = 0;
                        for (int i = 0; i < startTile.flowIn.Count; i++)
                        {
                            maxFlow = Mathf.Max(maxFlow, (int)startTile.GetNeighbor(startTile.flowIn[i]).riverStrenght);
                        }
                        ExtendCanal(endTile.canalsIn[0], endTile, startTile);
                        //Case
                        if (endTile.riverStrenght > (FlowStrenght)maxFlow) //je me fais override
                        {
                            for (int i = 0; i < startTile.canalsIn.Count; i++)
                            {
                                if (startTile.canalsIn[i].endNode == startTile.gridPos)//ceux à contre sens
                                {
                                    if (grid.GetTile(startTile.canalsIn[i].startNode).riverStrenght < (FlowStrenght)maxFlow)
                                    {
                                        if (startTile.canalsIn[i].canalTiles.Count < 1)//link simple
                                        {
                                            ErasedCanal(startTile.canalsIn[i]);
                                        }
                                        else
                                        {
                                            Inverse(startTile.canalsIn[i]);
                                        }
                                    }
                                    else
                                    if (grid.GetTile(startTile.canalsIn[i].startNode).riverStrenght > (FlowStrenght)(2 * maxFlow))
                                    {
                                        Inverse(endTile.canalsIn[0]);
                                    }
                                    else //==
                                    {
                                        //is okay
                                    }
                                }
                            }
                        }
                        else
                        if (endTile.riverStrenght < (FlowStrenght)(2 * maxFlow)) //je peut override 
                        {
                            Inverse(endTile.canalsIn[0]);
                        }
                        else
                        {
                            //il s'imisse sans soucis
                        }
                        break;
                    default: //x >= 2
                        /*
                        if (startTile.canalsIn.Count == 1)
                        {
                            //Split
                            SplitCanal(startTile.canalsIn[0], startTile);
                        }
                        if (endTile.canalsIn.Count == 1)
                        {
                            //Split
                            SplitCanal(endTile.canalsIn[0], startTile);
                        }

                        //Calcul Highest flow
                        int maxFlowStart = 0;
                        for (int i = 0; i < startTile.flowIn.Count; i++)
                        {
                            maxFlowStart = Mathf.Max(maxFlowStart, (int)startTile.GetNeighbor(startTile.flowIn[i]).riverStrenght);
                        }
                        int maxFlowEnd = 0;
                        for (int i = 0; i < startTile.flowOut.Count; i++)
                        {
                            maxFlowEnd = Mathf.Max(maxFlowEnd, (int)startTile.GetNeighbor(startTile.flowIn[i]).riverStrenght);
                        }
                        
                        //Case
                        if (endTile.riverStrenght > (FlowStrenght)maxFlowStart) //je me fais override
                        {
                            for (int i = 0; i < startTile.canalsIn.Count; i++)
                            {
                                if (startTile.canalsIn[i].endNode == startTile.gridPos)//ceux à contre sens
                                {
                                    if (grid.GetTile(startTile.canalsIn[i].startNode).riverStrenght < (FlowStrenght)maxFlowStart)
                                    {
                                        if (startTile.canalsIn[i].canalTiles.Count < 1)//link simple
                                        {
                                            ErasedCanal(startTile.canalsIn[i]);
                                        }
                                        else
                                        {
                                            Inverse(startTile.canalsIn[i]);
                                        }
                                    }
                                    else
                                    if (grid.GetTile(startTile.canalsIn[i].startNode).riverStrenght > (FlowStrenght)(2 * maxFlowStart))
                                    {
                                        Inverse(endTile.canalsIn[0]);
                                    }
                                    else //==
                                    {
                                        //is okay
                                    }
                                }
                            }
                        }
                        else
                        if (endTile.riverStrenght < (FlowStrenght)(2 * maxFlowStart)) //je peut override 
                        {
                            Inverse(endTile.canalsIn[0]);
                        }
                        else
                        {
                            //il s'imisse sans soucis
                        }

                        //Case
                        if (startTile.riverStrenght > (FlowStrenght)maxFlowEnd) //j'override les deux autre flow
                        {
                            for (int i = 0; i < endTile.canalsIn.Count; i++)
                            {
                                if (endTile.canalsIn[i].endNode == endTile.gridPos)//ceux à contre sens
                                {
                                    if (endTile.canalsIn[i].canalTiles.Count < 1)//link simple
                                    {
                                        if (grid.GetTile(endTile.canalsIn[i].startNode).riverStrenght < (FlowStrenght)maxFlowEnd)
                                        {
                                            if (endTile.canalsIn[i].canalTiles.Count < 1)//link simple
                                            {
                                                ErasedCanal(endTile.canalsIn[i]);
                                            }
                                            else
                                            {
                                                Inverse(endTile.canalsIn[i]);
                                            }
                                        }
                                        else
                                        if (grid.GetTile(endTile.canalsIn[i].startNode).riverStrenght > (FlowStrenght)(2 * maxFlowEnd))
                                        {
                                            Inverse(startTile.canalsIn[0]);
                                        }
                                        else //==
                                        {
                                            //is okay
                                        }

                                    }
                                }
                            }
                        }
                        else
                        if (startTile.riverStrenght < (FlowStrenght)(2 * maxFlowEnd)) //je me fais peut etre override
                        {
                            Inverse(startTile.canalsIn[0]);
                        }
                        else
                        {
                            //je m'imisse sans soucis
                        }*/
                        Debug.LogError("TODO");
                        break;
                }
                break;
        }

        FlowStep();
    }

    private void OnBreak(GameTile erasedTile)
    {
        if (erasedTile.isElement && !(erasedTile.element is WaterSource) && !( erasedTile.element is Plant))
        {
            digging.RemoveElement(erasedTile.element);
        }
        else
        {
            List<Canal> canals = new List<Canal>();
            canals = erasedTile.canalsIn;
            int temp = canals.Count;

            for (int i = 0; i < temp; i++)
            {
                if (canals[canals.Count-1].Contains(erasedTile.gridPos))
                {
                    int indexInCanal = canals[canals.Count - 1].IndexOf(erasedTile.gridPos);
                    if (indexInCanal == 0 || indexInCanal == canals[canals.Count - 1].canalTiles.Count + 1)
                    {
                        ShortenCanal(canals[canals.Count - 1], erasedTile);
                    }
                    else
                    {
                        BreakCanalIn2(canals[canals.Count - 1], erasedTile);
                    }
                }
            }
            erasedTile.StopFlow();
        }
        FlowStep();
    }

    public void ReplaceCanal(Canal oldCanal, Canal newCanal)
    {
        bool doOnce = true;
        for (int i = 0; i < canals.Count; i++)
        {
            if (oldCanal == canals[i] && doOnce)
            {
                AddCanalRef(canals[i], newCanal);
                ErasedCanal(canals[i]);
                doOnce = false;
            }
        }
    }
    public void FlowStep()
    {
        for (int i = 0; i < canals.Count; i++)
        {
            watergoing(canals[i]);
        }
    }
    private void watergoing(Canal canal)
    {
        if (canal.canalTiles.Count > 0)
        {
            grid.GetTile(canal.startNode).FlowStep();
            for (int i = 0; i < canal.canalTiles.Count; i++)
            {
                grid.GetTile(canal.canalTiles[i]).FlowStep();
            }
            grid.GetTile(canal.endNode).FlowStep();
        }
        else
        {
            grid.GetTile(canal.startNode).FlowStep();
            grid.GetTile(canal.endNode).FlowStep();
        }
    }


    private void GenerateRiverCanal(Vector2Int _startNode, Vector2Int _endNode)
    {
        Canal linkCanal = new Canal(_startNode, _endNode);

        canals.Add(linkCanal);

        grid.GetTile(_startNode).canalsIn.Add(linkCanal);
        grid.GetTile(_endNode).canalsIn.Add(linkCanal);

        GameTile.Link(grid.GetTile(_startNode), grid.GetTile(_endNode));
    }
    private void ErasedCanal(Canal canal)
    {
        canals.Remove(canal);
        RemoveCanalRef(canal,canal);
    }

    public void Merge(Canal canalA, Canal canalB)
    {
        //add endNode to the end linkCanal
        canalA.canalTiles.Add(canalA.endNode);
        GameTile.Link(grid.GetTile(canalA.endNode), grid.GetTile(canalB.startNode));
        //add start linkCanal to the end linkCanal
        canalA.canalTiles.Add(canalB.startNode);
        canalA.canalTiles.AddRange(canalB.canalTiles);

        //add endNode to the end  
        canalA.endNode = canalB.endNode;
    }
    public void Inverse(Canal canal)
    {
        if(canal.canalTiles.Count > 0)
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
                GameTile.InverseLink(grid.GetTile(canal.canalTiles[i]), grid.GetTile(canal.canalTiles[i+1]));
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
    private void ExtendCanal(Canal canal, GameTile extremumTile, GameTile addedTile)
    {
        if (canal.startNode == extremumTile.gridPos)
        {
            GameTile.Link(extremumTile, addedTile);

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
            Debug.LogError("added tile isnot at  any extremum", this);
        }
    }
    private void ShortenCanal(Canal canal, GameTile erasedTile)
    {
        if (grid.GetTile(canal.startNode) == erasedTile)
        {
            //est ce qu'il reste une tile entre deux ?
            if (canal.canalTiles.Count > 0)
            {
                grid.GetTile(canal.startNode).canalsIn.Remove(canal);
                GameTile.UnLink(grid.GetTile(canal.startNode), grid.GetTile(canal.canalTiles[0]));

                canal.startNode = canal.canalTiles[0];
                canal.canalTiles.RemoveAt(0);
            }
            else
            {
                GameTile.UnLink(grid.GetTile(canal.startNode), grid.GetTile(canal.endNode));
                ErasedCanal(canal);
            }
        }
        else
        if (grid.GetTile(canal.endNode) == erasedTile)
        {
            //est ce qu'il reste une tile entre deux ?
            if (canal.canalTiles.Count > 0)
            {
                grid.GetTile(canal.endNode).canalsIn.Remove(canal);
                GameTile.UnLink(grid.GetTile(canal.canalTiles[canal.canalTiles.Count-1]), grid.GetTile(canal.endNode));

                canal.endNode = canal.canalTiles[canal.canalTiles.Count - 1];
                canal.canalTiles.RemoveAt(canal.canalTiles.Count - 1);
            }
            else
            {
                GameTile.UnLink(grid.GetTile(canal.startNode), grid.GetTile(canal.endNode));
                ErasedCanal(canal);
            }

        }
        else
        {
            Debug.LogError("added tile isnot at  any extremum", this);
        }
    }
    private void SplitCanal(Canal canal, GameTile splitTile)
    {
        //ErrorCheck
        if (canal.canalTiles.Count < 1)
        {
            Debug.LogError("Can't Split an empty canal");
        }

        //StartSpliting
        int index = canal.canalTiles.IndexOf(splitTile.gridPos);

        #region SecondPart
        List<Vector2Int> _newCanalTiles = new List<Vector2Int>();
        // index + 1 car index start node donc index + 1 première tile
        for (int i = index + 1; i < canal.canalTiles.Count; i++)
        {
            _newCanalTiles.Add(canal.canalTiles[i]);
        }

        Canal secondaryCanal = new Canal(canal.canalTiles[index], canal.endNode, _newCanalTiles);
        canals.Add(secondaryCanal);

        //Updates Canals In
        grid.GetTile(secondaryCanal.startNode).canalsIn.Add(secondaryCanal);
        for (int i = 0; i < secondaryCanal.canalTiles.Count; i++)
        {
            grid.GetTile(secondaryCanal.canalTiles[i]).canalsIn.Add(secondaryCanal);
            //Not link to canal anymore
            grid.GetTile(secondaryCanal.canalTiles[i]).canalsIn.Remove(canal);
        }
        grid.GetTile(secondaryCanal.endNode).canalsIn.Add(secondaryCanal);
        //Not link to canal anymore
        grid.GetTile(secondaryCanal.endNode).canalsIn.Remove(canal);

        #endregion
        #region FirstPart
        CutCanalTo(canal, grid.GetTile(canal.canalTiles[index]));
        #endregion

    }
    private void CutCanalTo(Canal canal, GameTile to)
    {
        if (!canal.Contains(to.gridPos))
        {
            Debug.LogError(canal + "don't contain" + to, this);
            return;
        }

        for (int i = canal.IndexOf(to.gridPos); i < canal.canalTiles.Count; i++)
        {
            grid.GetTile(canal.canalTiles[i]).canalsIn.Remove(canal);
        }
        grid.GetTile(canal.endNode).canalsIn.Remove(canal);

        canal.canalTiles = canal.canalTiles.GetRange(0, canal.IndexOf(to.gridPos) - 1);
        canal.endNode = to.gridPos;
    }
    private void BreakCanalIn2(Canal canal, GameTile breakTile)
    {
        for (int i = 0; i < breakTile.canalsIn.Count; i++)
        {
            if (grid.GetTile(canal.startNode) == breakTile || grid.GetTile(canal.endNode) == breakTile)
            {
                ShortenCanal(canal, breakTile);
            }
            else
            {
                if (canal.canalTiles.Count <= 1)
                {
                    //3 block avec mid suppr
                    ErasedCanal(canal);
                }
                else
                {
                    int index = canal.canalTiles.IndexOf(breakTile.gridPos);
                    //
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
                            for (int j = index + 2; j < canal.canalTiles.Count; j++)
                            {
                                _templist.Add(canal.canalTiles[j]);
                            }

                            secondaryCanal = new Canal(canal.canalTiles[index + 1], canal.endNode, _templist);
                        }

                        canals.Add(secondaryCanal);

                        //Updates Canals In
                        RemoveCanalRef(secondaryCanal, canal);
                        AddCanalRef(secondaryCanal, secondaryCanal);

                    }
                    else
                    {
                        //toute les canalTiles après on les suppr
                        for (int j = index + 1; j < canal.canalTiles.Count; j++)
                        {
                            grid.GetTile(canal.canalTiles[j]).RemoveAllLinkedTile();
                        }
                        grid.GetTile(canal.endNode).RemoveAllLinkedTile();
                    }
                    #endregion
                    //
                    #region FirstPart
                    //si plus de 2 tile avant rupture alors je raccorcie le canal
                    // (start + tileIndex (pas besoin de -1 car index commence à 0))
                    int _leftTile = (1 + index);
                    if (_leftTile >= 2)
                    {
                        canal.endNode = canal.canalTiles[index - 1];
                        for (int j = canal.canalTiles.Count - 1; j > (index - 2); j--)
                        {
                            canal.canalTiles.RemoveAt(j);
                        }
                    }
                    else
                    {
                        GameTile.UnLink(grid.GetTile(canal.startNode), grid.GetTile(canal.canalTiles[0]));
                        //toute les canalTiles avant on les suppr
                        for (int j = 0; j < index; j++)
                        {
                            GameTile.UnLink(grid.GetTile(canal.canalTiles[j]), grid.GetTile(canal.canalTiles[j+1]));
                        }

                        ErasedCanal(canal);
                    }
                    #endregion
                }
            }
        }
        GameTile.UnLinkAll(breakTile);
    }

    private bool InCanals(Vector2Int tilePos)
    {
        bool result = false;

        for (int i = 0; i < canals.Count; i++)
        {
            if (tilePos == canals[i].startNode)
            {
                if(canals[i].startNode == tilePos)
                {
                    result = true;
                }
            }
            else
            if (tilePos == canals[i].endNode)
            {
                if (canals[i].endNode == tilePos)
                {
                    result = true;
                }
            }
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
    private int CanalIn(Vector2Int tilePos)
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

    private void RemoveCanalRef(Canal canalTile, Canal canalRef)
    {
        grid.GetTile(canalTile.startNode).canalsIn.Remove(canalRef);
        for (int i = 0; i < canalTile.canalTiles.Count; i++)
        {
            grid.GetTile(canalTile.canalTiles[i]).canalsIn.Remove(canalRef);
        }
        grid.GetTile(canalTile.endNode).canalsIn.Remove(canalRef);
    }
    private void RemoveCanalRef(List<Vector2Int> canalTile, Canal canalRef)
    {
        for (int i = 0; i < canalTile.Count; i++)
        {
            grid.GetTile(canalTile[i]).canalsIn.Remove(canalRef);
        }
    }
    private void AddCanalRef(Canal canalTile, Canal canalRef)
    {
        if (!grid.GetTile(canalTile.startNode).canalsIn.Contains(canalRef))
        {
            grid.GetTile(canalTile.startNode).canalsIn.Add(canalRef);
        }

        for (int i = 0; i < canalTile.canalTiles.Count; i++)
        {
            if (!grid.GetTile(canalTile.canalTiles[i]).canalsIn.Contains(canalRef))
            {
                grid.GetTile(canalTile.canalTiles[i]).canalsIn.Add(canalRef);
            }
        }

        if (!grid.GetTile(canalTile.endNode).canalsIn.Contains(canalRef))
        {
            grid.GetTile(canalTile.endNode).canalsIn.Add(canalRef);
        }
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < canals.Count; i++)
        {
            if (canals[i].canalTiles.Count > 0)
            {
                Debug.DrawLine(grid.TileToPos(canals[i].startNode), grid.TileToPos(canals[i].canalTiles[0]), Color.green);
                for (int j = 0; j < canals[i].canalTiles.Count - 1; j++)
                {
                    Debug.DrawLine(grid.TileToPos(canals[i].canalTiles[j]), grid.TileToPos(canals[i].canalTiles[j + 1]), Color.red);
                }
                Debug.DrawLine(grid.TileToPos(canals[i].canalTiles[canals[i].canalTiles.Count - 1]), grid.TileToPos(canals[i].endNode), Color.green);

            }
            else
            {
                Debug.DrawLine(grid.TileToPos(canals[i].startNode), grid.TileToPos(canals[i].endNode), Color.green);
            }
        }
    }
}
