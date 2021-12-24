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
        gameTime = GameTime.Instance;
        gameTime.onWaterSimulationStep.AddListener(FlowStep);
        digging.onLink.AddListener(OnLink);
        digging.onBreak.AddListener(OnBreakV2);
    }
    private void OnDestroy()
    {
        gameTime.onWaterSimulationStep.RemoveListener(FlowStep);
        digging.onLink.RemoveListener(OnLink);
        digging.onBreak.RemoveListener(OnBreakV2);
    }

    #region Make Link
    private void OnLink(GameTile startTile, GameTile endTile)
    {
        switch (startTile.linkAmount)
        {
            case 0:
                switch (endTile.linkAmount)
                {
                    case 0: //in a void
                        Link0To0(startTile, endTile);
                        break;
                    case 1: //extending the end canal
                        Link1To0(endTile, startTile);
                        break;
                    default: //x >= 2
                        Link2To0(endTile, startTile);
                        break;
                }
                break;
            case 1:
                switch (endTile.linkAmount)
                {
                    case 0: //in a void
                        Link1To0(startTile, endTile);
                        break;
                    case 1: //extending the end canal
                        Link1To1(startTile, endTile);
                        break;
                    default: //x >= 2
                        Link2To1(endTile, startTile);
                        break;
                }
                break;
            default: // 2 ou +
                switch (endTile.linkAmount)
                {
                    case 0: //in a void
                        Link2To0(startTile, endTile);
                        break;
                    case 1: //extending the end canal
                        Link2To1(startTile, endTile);
                        break;
                    default: //x >= 2
                        Link2To2(startTile, endTile);
                        break;
                }
                break;
        }

        FlowStep();
    }
    //
    private void Link0To0(GameTile tileA, GameTile tileB)
    {
        if (tileA.ReceivedFlow() >= tileB.ReceivedFlow())
        {
            GenerateRiverCanal(tileA, tileB);
        }
        else //if (tileA.ReceivedFlow() < tileB.ReceivedFlow())
        {
            GenerateRiverCanal(tileB, tileA);
        }
    }
    private void Link1To0(GameTile tileA, GameTile tileB)
    {
        if (InCanalList(tileA.canalsIn[0]))
        {
            Canal listCanal = CanalList(tileA.canalsIn[0]);

            if (listCanal.endNode == tileA.gridPos)
            {
                listCanal.Extend(tileA, tileB);
            }
            else
            {
                listCanal.Extend(tileB, tileA);
            }
            
            if (tileA.ReceivedFlow() < tileB.ReceivedFlow()) 
            {
                listCanal.Inverse(grid);
            }
        }
        else
        {
            Debug.LogError(tileA.canalsIn[0] + "not in list");
        }
    }
    private void Link1To1(GameTile tileA, GameTile tileB)
    {
        if (InCanalList(tileA.canalsIn[0]))
        {
            Canal listCanalA = CanalList(tileA.canalsIn[0]);
            Canal listCanalB = CanalList(tileB.canalsIn[0]);

            if(listCanalA == listCanalB)
            {
                Debug.LogError("Move Interdit");
                return;
            }
            //Done
            if (tileA.gridPos == tileA.canalsIn[0].endNode && tileB.gridPos == tileB.canalsIn[0].endNode)
            {//==>to<==
                if (tileA.ReceivedFlow() == tileB.ReceivedFlow())
                {
                    if (tileA.ReceivedFlow() == FlowStrenght._00_)
                    {//==>==>
                        listCanalB.Extend(tileB, tileA);
                        listCanalB.Inverse(grid);
                        ReplaceCanal(listCanalB, listCanalA);
                        Merge(listCanalA, listCanalB);
                    }
                    else
                    {//==>o<==
                        listCanalA.Extend(tileA, tileB);
                    }
                }
                else if (tileA.ReceivedFlow() > tileB.ReceivedFlow())
                {//==>==>
                    listCanalB.Extend(tileB, tileA);
                    listCanalB.Inverse(grid);
                    ReplaceCanal(listCanalB, listCanalA);
                    Merge(listCanalA, listCanalB);
                }
                else if (tileA.ReceivedFlow() < tileB.ReceivedFlow())
                {//<==<==
                    listCanalA.Extend(tileA, tileB);
                    listCanalA.Inverse(grid);
                    ReplaceCanal(listCanalA, listCanalB);
                    Merge(listCanalB, listCanalA);
                }
            }
            else if (tileA.gridPos == tileA.canalsIn[0].endNode && tileB.gridPos == tileB.canalsIn[0].startNode)
            {//==>to==>
                if (tileA.ReceivedFlow() >= tileB.ReceivedFlow())
                {//==>==>
                    listCanalA.Extend(tileA, tileB);
                    ReplaceCanal(listCanalB, listCanalA);
                    Merge(listCanalA, listCanalB);
                }
                else if (tileA.ReceivedFlow() < tileB.ReceivedFlow())
                {//<==o==> //La tileB est forcément un point d'émission (nuage ?)
                    listCanalA.Extend(tileA, tileB);
                    listCanalA.Inverse(grid);
                }
            }
            else if (tileA.gridPos == tileA.canalsIn[0].startNode && tileB.gridPos == tileB.canalsIn[0].endNode)
            {//<==to<==
                if (tileA.ReceivedFlow() > tileB.ReceivedFlow())
                {//<==o==> //La tileA est forcément un point d'émission (nuage ?)
                    listCanalB.Extend(tileB, tileA);
                    listCanalB.Inverse(grid);
                }
                else if (tileA.ReceivedFlow() <= tileB.ReceivedFlow())
                {//<==<==
                    listCanalB.Extend(tileB, tileA);
                    ReplaceCanal(listCanalA, listCanalB);
                    Merge(listCanalB, listCanalA);
                }
            }
            else if (tileA.gridPos == tileA.canalsIn[0].startNode && tileB.gridPos == tileB.canalsIn[0].startNode)
            {//<==to==>
                if (tileA.ReceivedFlow() == tileB.ReceivedFlow())
                {
                    if (tileA.ReceivedFlow() == FlowStrenght._00_)
                    {//==>==>
                        listCanalA.Extend(tileA, tileB);
                        listCanalA.Inverse(grid);
                        Merge(listCanalA, listCanalB);
                        ReplaceCanal(listCanalB, listCanalA);
                    }
                    else
                    {//<==oo==> ??? temp behavior //==>==>
                        listCanalA.Extend(tileA, tileB);
                        listCanalA.Inverse(grid);
                        ReplaceCanal(listCanalB, listCanalA);
                        Merge(listCanalA, listCanalB);
                        //Debug.LogError("Demande aux GD");
                    }
                }
                else if (tileA.ReceivedFlow() > tileB.ReceivedFlow())
                {//<==o==> //La tileA est forcément un point d'émission (nuage ?)
                    listCanalB.Extend(tileB, tileA);
                    listCanalB.Inverse(grid);
                }
                else if (tileA.ReceivedFlow() < tileB.ReceivedFlow())
                {//<==o==> //La tileB est forcément un point d'émission (nuage ?)
                    listCanalA.Extend(tileA, tileB);
                    listCanalA.Inverse(grid);
                }
            }
        }
        else
        {
            Debug.LogError(tileA.canalsIn[0] + "not in list");
        }

    }
    private void Link2To0(GameTile tileA, GameTile tileB)
    {
        if (InCanalList(tileA.canalsIn[0]))
        {
            if (tileA.canalsIn.Count == 1)
            {
                Canal listCanal = CanalList(tileA.canalsIn[0]);
                SplitCanal(listCanal, tileA);
            }

            if ((int)tileA.ReceivedFlow() >= (2 * (int)tileB.ReceivedFlow())) 
            {
                GenerateRiverCanal(tileA, tileB);
            }
            else if ((int) tileA.ReceivedFlow() < (2 * (int)tileB.ReceivedFlow())) 
            {
                GenerateRiverCanal(tileB, tileA);
            }
        }
        else
        {
            Debug.LogError(tileA.canalsIn[0] + "not in list");
        }
    }
    private void Link2To1(GameTile tileA, GameTile tileB) 
    {
        //Divergence
        if (InCanalList(tileA.canalsIn[0]))
        {
            Canal listCanalA = CanalList(tileA.canalsIn[0]);
            Canal listCanalB = CanalList(tileB.canalsIn[0]);

            if (tileA.canalsIn.Count == 1)
            {
                if (listCanalA == listCanalB)
                {
                    Debug.LogError("Move Interdit");
                    return;
                }
                //Split
                SplitCanal(listCanalA, tileA);
            }
            if (listCanalA.endNode != tileA.gridPos)
            {
                listCanalA = CanalList(tileA.canalsIn[1]);
            }

            int FlowOf2 = (int)tileA.ReceivedFlow();
            int FlowOf1 = (int)tileB.ReceivedFlow();

            if (FlowOf2 > (2 * FlowOf1))
            {
                listCanalB.Extend(tileB, tileA);
                listCanalB.Inverse(grid);
            }
            else if (FlowOf2 <= (2 * FlowOf1))
            {
                listCanalB.Extend(tileB, tileA);
            }
        }
        else
        {
            Debug.LogError(tileA.canalsIn[0] + "not in list");
        }
    }  
    private void Link2To2(GameTile tileA, GameTile tileB)
    {
        if (InCanalList(tileA.canalsIn[0]))
        {
            Canal listCanalA = CanalList(tileA.canalsIn[0]);
            if (tileA.canalsIn.Count == 1) 
            {
                SplitCanal(listCanalA, tileA);
            }
            if (listCanalA.endNode != tileA.gridPos) 
            { 
                listCanalA = CanalList(tileA.canalsIn[1]); 
            }

            if (InCanalList(tileB.canalsIn[0]))
            {
                Canal listCanalB = CanalList(tileB.canalsIn[0]);
                if (tileB.canalsIn.Count == 1) 
                {
                    SplitCanal(listCanalB, tileB);
                }
                if (listCanalB.endNode != tileB.gridPos)
                {
                    listCanalB = CanalList(tileB.canalsIn[1]);
                }

                int FlowOfA = (int)tileA.ReceivedFlow();
                int FlowOfB = (int)tileB.ReceivedFlow();

                if (FlowOfA >= (2 * FlowOfB))
                {
                    GenerateRiverCanal(tileA, tileB);
                }
                else if (FlowOfA < (2 * FlowOfB))
                {
                    GenerateRiverCanal(tileB, tileA);
                }
            }
            else
            {
                Debug.LogError(tileA.canalsIn[0] + "not in list");
            }
        }
        else
        {
            Debug.LogError(tileA.canalsIn[0] + "not in list");
        }
    }
    #endregion
    #region Break Link
    private void OnBreak(GameTile erasedTile)
    {
        if (erasedTile.haveElement && !(erasedTile.element is WaterSource) && !( erasedTile.element is Plant))
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
                else
                {
                    canals.Remove(canals[canals.Count - 1]);
                }
            }
            erasedTile.riverStrenght = 0;
        }

        FlowStep();
    }
    private void OnBreakV2(GameTile erasedTile)
    {
        if (erasedTile.haveElement && !(erasedTile.element is WaterSource) && !(erasedTile.element is Plant))
        {
            ErasedElement(erasedTile);
        }
        else
        {
            ErasedRiverInTile(erasedTile);
        }
        FlowStep();
    }
    //
    private void ErasedElement(GameTile erasedTile)
    {
        Element element = erasedTile.element;

        digging.RemoveElement(erasedTile.element);
        erasedTile.riverStrenght = 0;

        element.UnLinkElementToGrid(grid);
    }
    private void ErasedRiverInTile(GameTile erasedTile)
    {
        List<Canal> canals = new List<Canal>();
        canals = erasedTile.canalsIn;
        int temp = canals.Count;

        for (int i = 0; i < temp; i++)
        {
            if (canals[canals.Count - 1].Contains(erasedTile.gridPos))
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
            else
            {
                canals.Remove(canals[canals.Count - 1]);
            }
        }
        erasedTile.riverStrenght = 0;
    }
    #endregion

    private bool InCanalList(Canal canal)
    {
        for (int i = 0; i < canals.Count; i++)
        {
            if (canal == canals[i])
            {
                return true;
            }
        }
        return false;
    }
    private Canal CanalList(Canal canal)
    {
        for (int i = 0; i < canals.Count; i++)
        {
            if (canal == canals[i])
            {
                return canals[i];
            }
        }
        return null;
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


    private void GenerateRiverCanal(GameTile _startNode, GameTile _endNode)
    {
        Canal linkCanal = new Canal(_startNode.gridPos, _endNode.gridPos);
        canals.Add(linkCanal);

        _startNode.canalsIn.Add(linkCanal);
        _endNode.canalsIn.Add(linkCanal);
        GameTile.Link(_startNode, _endNode);
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
    private void RemoveCanalRef(Canal targetCanal, Canal canalRef)
    {
        List<Vector2Int> tiles = new List<Vector2Int>() { targetCanal.startNode };
        tiles.AddRange(targetCanal.canalTiles);
        tiles.Add(targetCanal.endNode);

        RemoveCanalRef(tiles, canalRef);
    }
    private void RemoveCanalRef(List<Vector2Int> canalTiles, Canal canalRef)
    {
        for (int i = 0; i < canalTiles.Count; i++)
        {
            grid.GetTile(canalTiles[i]).canalsIn.Remove(canalRef);
        }
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
    private void ErasedCanal(Canal canal)
    {
        canals.Remove(canal);
        RemoveCanalRef(canal,canal);
    }

    /// <summary>
    /// =CanalA={=CanalB=}=> Or =CanalB={=CanalA=}=>
    /// </summary>
    public void Merge(Canal canalA, Canal canalB)
    {
        if(canalA.endNode == canalB.startNode)
        {// =CanalA={=CanalB=}=>

            //add endNode to the end linkCanal
            canalA.canalTiles.Add(canalA.endNode); // ou canalA.canalTiles.Add(canalB.startNode);
            canalA.canalTiles.AddRange(canalB.canalTiles);
            //add endNode to the end  
            canalA.endNode = canalB.endNode;

            ErasedCanal(canalB);
        }
        else 
        if (canalB.endNode == canalA.startNode)
        {// =CanalB={=CanalA=}=>

            //add endNode to the end linkCanal
            canalB.canalTiles.Add(canalB.endNode); // ou canalB.canalTiles.Add(canalA.startNode);
            canalB.canalTiles.AddRange(canalA.canalTiles);
            //add endNode to the end  
            canalB.endNode = canalA.endNode;

            ErasedCanal(canalA);
        }
        else
        {
            Debug.LogErrorFormat("can't merge {0} and {1}, they don't have any extremum in common", canalA, canalB);
        }

    }
    [System.Obsolete] public void OldMerge(Canal canalA, Canal canalB) 
    {
        //add endNode to the end linkCanal
        canalA.canalTiles.Add(canalA.endNode);
        //if not already link
        GameTile.Link(grid.GetTile(canalA.endNode), grid.GetTile(canalB.startNode));
        //add start linkCanal to the end linkCanal
        canalA.canalTiles.Add(canalB.startNode);
        canalA.canalTiles.AddRange(canalB.canalTiles);
        //add endNode to the end  
        canalA.endNode = canalB.endNode;

        ErasedCanal(canalB);
    }
    //
    private void ShortenCanal(Canal canal, GameTile erasedTile)
    {
        if (canal.startNode == erasedTile.gridPos)
        {
            //est ce qu'il reste une tile entre deux ?
            if (canal.canalTiles.Count > 0)
            {
                Canal listCanal = CanalList(canal);
                erasedTile.canalsIn.Remove(listCanal);
                GameTile.UnLink(erasedTile, grid.GetTile(listCanal.canalTiles[0]));

                listCanal.startNode = listCanal.canalTiles[0];
                listCanal.canalTiles.RemoveAt(0);
            }
            else
            {
                GameTile.UnLink(erasedTile, grid.GetTile(canal.endNode));
                ErasedCanal(canal);
            }
        }
        else
        if (canal.endNode == erasedTile.gridPos)
        {
            //est ce qu'il reste une tile entre deux ?
            if (canal.canalTiles.Count > 0)
            {
                Canal listCanal = CanalList(canal);
                erasedTile.canalsIn.Remove(listCanal);
                GameTile.UnLink(grid.GetTile(listCanal.canalTiles[listCanal.canalTiles.Count-1]), erasedTile);

                listCanal.endNode = listCanal.canalTiles[listCanal.canalTiles.Count - 1];
                listCanal.canalTiles.RemoveAt(listCanal.canalTiles.Count - 1);
            }
            else
            {
                GameTile.UnLink(grid.GetTile(canal.startNode), erasedTile);
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
        if (InCanalList(canal))
        {
            Canal listCanal = CanalList(canal);
            //ErrorCheck
            if (listCanal.canalTiles.Count < 1)
            {
                Debug.LogError("Can't Split an empty canal");
            }

            //StartSpliting
            int index = listCanal.canalTiles.IndexOf(splitTile.gridPos);

            #region SecondPart
            List<Vector2Int> _newCanalTiles = new List<Vector2Int>();
            // index + 1 car index start node donc index + 1 première tile
            for (int i = index + 1; i < listCanal.canalTiles.Count; i++)
            {
                _newCanalTiles.Add(listCanal.canalTiles[i]);
            }

            Canal secondaryCanal = new Canal(listCanal.canalTiles[index], listCanal.endNode, _newCanalTiles);
            //Updates Canals In
            grid.GetTile(secondaryCanal.startNode).canalsIn.Add(secondaryCanal);
            for (int i = 0; i < secondaryCanal.canalTiles.Count; i++)
            {
                grid.GetTile(secondaryCanal.canalTiles[i]).canalsIn.Add(secondaryCanal);
                //Not link to canal anymore
                grid.GetTile(secondaryCanal.canalTiles[i]).canalsIn.Remove(listCanal);
            }
            grid.GetTile(secondaryCanal.endNode).canalsIn.Add(secondaryCanal);
            //Not link to canal anymore
            grid.GetTile(secondaryCanal.endNode).canalsIn.Remove(listCanal);
            //Add to the list
            canals.Add(secondaryCanal);
            #endregion

            #region FirstPart
            CutCanalTo(listCanal, grid.GetTile(listCanal.canalTiles[index]));
            #endregion
        }
        else
        {
            Debug.LogError(canal + "not in list");
        }
    }
    private void CutCanalTo(Canal canal, GameTile to)
    {
        if (InCanalList(canal))
        {
            Canal listCanal = CanalList(canal);

            if (!listCanal.Contains(to.gridPos))
            {
                Debug.LogError(listCanal + "don't contain" + to, this);
                return;
            }

            if(!(listCanal.canalTiles.Last() == to.gridPos))
            {
                int indexOfTo = listCanal.IndexOf(to.gridPos);
                RemoveCanalRef(listCanal.canalTiles.GetRange(indexOfTo, listCanal.canalTiles.Count - indexOfTo), listCanal);
            }
            grid.GetTile(listCanal.endNode).canalsIn.Remove(listCanal);

            listCanal.canalTiles = listCanal.canalTiles.GetRange(0, listCanal.IndexOf(to.gridPos) - 1);
            listCanal.endNode = to.gridPos;
        }
        else
        {
            Debug.LogError(canal + "not in list");
        }
    }
    //Check if it work
    private void BreakCanalIn2(Canal canal, GameTile breakTile)
    {
        if (InCanalList(canal))
        {
            Canal listCanal = CanalList(canal);

            if (listCanal.startNode == breakTile.gridPos || listCanal.endNode == breakTile.gridPos)
            {
                ShortenCanal(listCanal, breakTile);
            }
            else
            {
                if (listCanal.canalTiles.Count == 1)
                {
                    GameTile.UnLink(grid.GetTile(listCanal.startNode), breakTile);
                    GameTile.UnLink(breakTile, grid.GetTile(listCanal.endNode));
                    //3 block avec mid suppr
                    ErasedCanal(listCanal);
                }
                else
                {
                    int index = listCanal.IndexOf(breakTile.gridPos);

                    #region SecondPart
                    //si plus de 2 tile après rupture alors je crer un nouveau canal
                    // (tile + start + end) - (tileIndex)
                    int endPartTile = (listCanal.canalTiles.Count + 2) - (index+1);
                    if (endPartTile >= 2)
                    {
                        if (endPartTile == 2)
                        {
                            GenerateRiverCanal(grid.GetTile(listCanal.canalTiles.Last()), grid.GetTile(listCanal.endNode));
                        }
                        else
                        {

                            Canal secondaryCanal = new Canal(listCanal.canalTiles[index], listCanal.endNode, new List<Vector2Int>());
                            //Si la position de l'hypotétique canalTile[0] ne dépasse pas la longueur de la liste
                            if (index < listCanal.canalTiles.Count)
                            {
                                for (int j = index + 1; j < listCanal.canalTiles.Count; j++)
                                {
                                    secondaryCanal.canalTiles.Add(listCanal.canalTiles[j]);
                                }
                            }
                            canals.Add(secondaryCanal);

                            //Updates Canals In
                            RemoveCanalRef(secondaryCanal, listCanal);
                            AddCanalRef(secondaryCanal, secondaryCanal);
                        }
                    }
                    else
                    {
                        //toute les canalTiles jusqu'a l'avant dernière
                        for (int j = index; j < listCanal.canalTiles.Count-1; j++)
                        {
                            GameTile.UnLink(grid.GetTile(listCanal.canalTiles[j]), grid.GetTile(listCanal.canalTiles[j+1]));
                            grid.GetTile(listCanal.canalTiles[j]).canalsIn.Remove(listCanal);
                        }
                        //la dernière canalTile et endNode
                        GameTile.UnLink(grid.GetTile(listCanal.canalTiles.Last()), grid.GetTile(listCanal.endNode));
                        grid.GetTile(listCanal.canalTiles.Last()).canalsIn.Remove(listCanal);
                    }
                    #endregion
                    //
                    #region FirstPart
                    //si plus de 2 tile avant rupture alors je raccorcie le canal
                    // (tileIndex-1 (tile avant rupture) + 1(start))
                    int beginPartTile = index-2;
                    if (beginPartTile >= 2)
                    {
                        CutCanalTo(listCanal, grid.GetTile(listCanal.canalTiles[beginPartTile]));
                    }
                    else
                    {
                        GameTile.UnLink(grid.GetTile(listCanal.startNode), grid.GetTile(listCanal.canalTiles[0]));
                        //toute les canalTiles avant on les suppr
                        for (int j = 0; j < index; j++)
                        {
                            GameTile.UnLink(grid.GetTile(listCanal.canalTiles[j]), grid.GetTile(listCanal.canalTiles[j + 1]));
                        }

                        ErasedCanal(listCanal);
                    }
                    #endregion
                }
                GameTile.UnLinkAll(breakTile);
            }
        }
        else
        {
            Debug.LogError(canal + "not in list");
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
