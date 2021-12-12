using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile : MonoBehaviour
{
    public const int maxConnection = 3;
    public static float simulStepDur = 0.3f;

    [Header("Essential Data")]
    public TileData data;
    #region Getter / Setter
    public Vector2Int gridPos
    {
        get
        {
            return data.gridPos;
        }
        set
        {
            data.gridPos = value;
        }
    }
    public Vector3 worldPos
    {
        get
        {
            return transform.position;
        }
    }
    public Vector2 worldPos2D
    {
        get
        {
            return transform.position;
        }
    }
    public TileType type
    {
        get
        {
            return data.type;
        }
        set
        {
            data.type = value;
        }
    }
    public FlowStrenght riverStrenght
    {
        get
        {
            return data.riverStrenght;
        }
        set
        {
            data.riverStrenght = value;
        }
    }
    #endregion

    [Header("GameObject Data")]
    public Element element;
    public GameTile[] neighbors = new GameTile[8];
    [Space(8)]
    public FlowStrenght receivedFlow = FlowStrenght._00_;
    [Space(8)]
    public List<Direction> flowIn = new List<Direction>();
    public List<Direction> flowOut = new List<Direction>();
    [Space(8)]
    public List<GameTile> linkedTile = new List<GameTile>();
    public List<Canal> canalsIn = new List<Canal>();
    #region Getter / Setter
    public bool isElement
    {
        get
        {
            return element != null;
        }
    }
    public bool isLinkable
    {
        get
        {
            if(element!= null)
            {
                if (!element.isLinkable) //S'il n'y a pas d'element
                {
                    return false;
                }
            }
            else
            if (linkAmount >= maxConnection) //Si la tile n'est pas saturé
            {
                return false;
            }
            return true;
        }
    }
    public int linkAmount
    { 
        get 
        {
            return flowIn.Count + flowOut.Count;
        }
    }
    public bool isDuged
    {
        get
        {
            if (linkAmount > 0)
            {
                return true;
            }
            return false;
        }
    }
    public bool isRiver
    {
        get
        {
            if (data.riverStrenght > 0 && isDuged)
            {
                return true;
            }
            return false;
        }
    }
    public bool IsIrrigate
    {
        get
        {
            foreach(GameTile neighbor in neighbors)
            {
                if(neighbor.riverStrenght > 0)
                {
                    return true;
                }
                foreach (GameTile neighborOfNeighbor in neighbor.neighbors)
                {
                    if (neighborOfNeighbor.isElement)
                    {
                        if (neighborOfNeighbor.element is Lake)
                        {
                            if (neighborOfNeighbor.isRiver)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
    #endregion

    [Space(8)]
    public GameTime gameTime;
    public TileSpawnScore spawnScore;

    void Start()
    {
        gameTime = GameTime.Instance;
        //gameTime.onWaterSimulationStep.AddListener(FlowStep);
        spawnScore = GetComponent<TileSpawnScore>();
    }

    void Update()
    {

    }
    private void OnDestroy()
    {
        //gameTime.onWaterSimulationStep.RemoveListener(FlowStep);
    }

    //Flow
    public void FlowStep()
    {
        //Check For Water
        if (!ReceiveWater())
        {
            StopFlow();
            return;
        }
        //Set Flow
        if (element != null)
        {
            if (element is WaterSource)
            {
                receivedFlow = FlowStrenght._100_;
            }
        }
        //Clamp FlowStrenght
        receivedFlow = (FlowStrenght)Mathf.Clamp((int)receivedFlow, 0, (int)FlowStrenght._100_);
        SetFlow();
        //Check for FlowOut
        if (linkAmount == 2)
        {
            //Link
            if (flowOut.Count > flowIn.Count)
            {
                //flowOut.Count = 2
                GameTile neighborA = GetNeighbor(flowOut[0]);
                GameTile neighborB = GetNeighbor(flowOut[1]);

                if (neighborA.riverStrenght > neighborB.riverStrenght)
                {
                    InverseLink(this,neighborA);
                }
                else
                if (neighborA.riverStrenght < neighborB.riverStrenght)
                {
                    InverseLink(this,neighborB);
                }
                else
                {
                    //neighborA.riverStrenght == neighborB.riverStrenght
                    //noeud
                }
            }
            else
            if (flowOut.Count < flowIn.Count)
            {
                //flowIn.Count = 2
                GameTile neighborA = GetNeighbor(flowIn[0]);
                GameTile neighborB = GetNeighbor(flowIn[1]);

                if (neighborA.riverStrenght > neighborB.riverStrenght)
                {
                    InverseLink(this,neighborB);
                }
                else
                if (neighborA.riverStrenght < neighborB.riverStrenght)
                {
                    InverseLink(this,neighborA);
                }
                else
                {
                    //neighborA.riverStrenght == neighborB.riverStrenght
                    //noeud
                }
            }
            else
            {
                //flowOut.Count == flowIn.Count
                //Du coup it's okay
            }
        }
        else 
        if (linkAmount > 2)
        {
            //check si une entrée vide
            for (int i = 0; i < flowIn.Count; i++)
            {
                if (GetNeighbor(flowIn[i]).riverStrenght == FlowStrenght._00_)
                {
                    //Became Flow Out
                    GameTile.InverseLink(this,GetNeighbor(flowIn[i]));
                }
            }
        }
        //Send Water to neighbor
        if (SendWater())
        {
            // Envoie l'eau Thierry !
            float riverPower = (int)riverStrenght;
            float riverSplits = (float)flowOut.Count;
            switch (flowOut.Count)
            {
                case 1:
                    GetNeighbor(flowOut[0]).receivedFlow += Mathf.FloorToInt(riverPower / riverSplits);
                    break;
                case 2:
                    if ((int)riverStrenght % 2 == 0)
                    {
                        GetNeighbor(flowOut[0]).receivedFlow += Mathf.FloorToInt(riverPower / riverSplits);
                        GetNeighbor(flowOut[1]).receivedFlow += Mathf.FloorToInt(riverPower / riverSplits);
                    }
                    else
                    {
                        GetNeighbor(flowOut[0]).receivedFlow += Mathf.CeilToInt(riverPower / riverSplits);
                        GetNeighbor(flowOut[1]).receivedFlow += Mathf.FloorToInt(riverPower / riverSplits);
                    }
                    break;
                case 3:
                    if ((int)riverStrenght % 3 == 0)
                    {
                        GetNeighbor(flowOut[0]).receivedFlow += Mathf.FloorToInt(riverPower / riverSplits);
                        GetNeighbor(flowOut[1]).receivedFlow += Mathf.FloorToInt(riverPower / riverSplits);
                        GetNeighbor(flowOut[2]).receivedFlow += Mathf.FloorToInt(riverPower / riverSplits);
                    }
                    else
                    {
                        GetNeighbor(flowOut[0]).receivedFlow += Mathf.CeilToInt(riverPower / riverSplits);
                        GetNeighbor(flowOut[1]).receivedFlow += Mathf.FloorToInt(riverPower / riverSplits);
                        GetNeighbor(flowOut[2]).receivedFlow += Mathf.FloorToInt(riverPower / riverSplits);
                    }
                    break;
                default:
                    for (int i = 0; i < flowOut.Count; i++)
                    {
                        GetNeighbor(flowOut[1]).receivedFlow += Mathf.FloorToInt(riverPower / riverSplits);
                    }
                    break;
            }
        }
        //Reset receivedFlow
        receivedFlow = FlowStrenght._00_;
    }
    public bool ReceiveWater()
    {
        return flowIn.Count > 0 || element is WaterSource;
    }
    public bool SendWater()
    {
        return flowOut.Count > 0;
    }
    public void SetFlow()
    {
        //riverStrenght++;
        //riverStrenght = (FlowStrenght)Mathf.Clamp((int)riverStrenght, 0, (int)receivedFlow);
        riverStrenght = receivedFlow;
    }
    public void StopFlow()
    {
        /*
        riverStrenght--;
        riverStrenght = (FlowStrenght)Mathf.Max((int)riverStrenght, 0);
        receivedFlow = FlowStrenght._00_;
        */

        receivedFlow = FlowStrenght._00_;
        riverStrenght = FlowStrenght._00_;
    }

    //LINK
    public static void Link(GameTile tileA, GameTile tileB)
    {
        Vector2Int tileToMe = tileB.gridPos - tileA.gridPos;
        Direction dir = new Direction(tileToMe);

        tileA.AddLinkedTile(dir, FlowType.flowOut);
        tileB.AddLinkedTile(Direction.Inverse(dir), FlowType.flowIn);

        tileA.linkedTile.Add(tileB);
        tileB.linkedTile.Add(tileA);
    }
    public static void UnLink(GameTile tileA, GameTile tileB)
    {
        Vector2Int tileToMe = tileB.gridPos - tileA.gridPos;
        Direction dir = new Direction(tileToMe);

        tileA.RemoveLinkedTile(dir, FlowType.flowOut);
        tileB.RemoveLinkedTile(Direction.Inverse(dir), FlowType.flowIn);

        tileA.linkedTile.Remove(tileB);
        tileB.linkedTile.Remove(tileA);
    }
    public static void InverseLink(GameTile tileA, GameTile tileB)
    {
        UnLink(tileA, tileB);
        Link(tileB, tileA);
    }

    public void AddLinkedTile(Direction addedDir, FlowType flow)
    {
        if (flow == FlowType.flowIn)
        {
            flowIn.Add(addedDir);
        }
        else
        {
            flowOut.Add(addedDir);
        }
    }
    public void RemoveLinkedTile(Direction removeDir, FlowType flow)
    {
        if (flow == FlowType.flowIn)
        {
            flowIn.Remove(removeDir);
        }
        else
        {
            flowOut.Remove(removeDir);
        }
    }
    public void RemoveAllLinkedTile()
    {
        //Flow In
        for (int i = 0; i < flowIn.Count; i++)
        {
            UnLink(this, GetNeighbor(flowIn[i]));
        }
        //Flow Out
        for (int i = 0; i < flowOut.Count; i++)
        {
            UnLink(this, GetNeighbor(flowOut[i]));
        }
    }

    //Helper
    public void FillNeighbor()
    {
        Vector2Int temp = gridPos;
        Direction dir = new Direction(0);

        neighbors = new GameTile[8];
        for (int i = 0; i < 8; i++)
        {
            dir = new Direction((DirectionEnum)i);
            temp = gridPos + dir.dirValue;

            if (temp.x < 0 || temp.y < 0)
            {
                neighbors[i] = null;
            }
            else
            if (temp.x > GameGrid.instance.size.x - 1 || temp.y > GameGrid.instance.size.y - 1)
            {
                neighbors[i] = null;
            }
            else
            {
                neighbors[i] = GameGrid.instance.GetTile(temp);
            }
        }

    }
    public int NeighborIndex(GameTile tile)
    {
        for (int i = 0; i < 8; i++)
        {
            if (neighbors[i] == tile)
            {
                return i;
            }
        }
        Debug.LogError("Linked tile " + tile.gridPos + " n'est pas un voisin de " + this.gridPos + ".", this);
        return 8;
    }
    public GameTile GetNeighbor(Direction dir)
    {
        return neighbors[(int)dir.dirEnum];
    }
    public bool IsLinkInDir(Direction dir, FlowType flow)
    {
        if (flow == FlowType.flowIn)
        {
            return flowIn.Contains(dir);
        }
        else
        {
            return flowOut.Contains(dir);
        }
    }

    //GameTile to Data
    public void SaveInTileData(TileData data)
    {
        for (int i = 0; i < data.flowIn_Neighbors.Length; i++)
        {
            if (data.flowIn_Neighbors[i])
            {
                flowIn.Add(Direction.DirFromInt(i));
            }
        }
        for (int i = 0; i < data.flowOut_Neighbors.Length; i++)
        {
            if (data.flowOut_Neighbors[i])
            {
                flowOut.Add(Direction.DirFromInt(i));
            }
        }
    }
}
