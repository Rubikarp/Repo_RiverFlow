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
    public RiverStrenght riverStrenght
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

    [Header("Essential Data")]
    public Element element;
    public GameTile[] neighbors = new GameTile[8];
    [Space(8)]
    public RiverStrenght receivedFlow = RiverStrenght._00_;
    [Space(8)]
    public List<Direction> flowIn = new List<Direction>();
    public List<Direction> flowOut = new List<Direction>();
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
    #endregion

    [SerializeField] float timer;
    void Start()
    {
        //InvokeRepeating("FlowStep", 0, 1f);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > simulStepDur)
        {
            FlowStep();
            timer = 0f;
        }
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
                receivedFlow = RiverStrenght._100_;
            }
        }
        //Clamp RiverStrenght
        receivedFlow = (RiverStrenght)Mathf.Clamp((int)receivedFlow, 0, (int)RiverStrenght._100_);
        SetFlow();
        //Check for FlowOut
        if(linkAmount > 1)
        {
            if (linkAmount == 2 && flowOut.Count != flowIn.Count)
            {
                if (flowOut.Count > flowIn.Count)
                {
                    //flowOut.Count = 2
                    GameTile neighborA = Neighbor(flowOut[0]);
                    GameTile neighborB = Neighbor(flowOut[1]);

                    if (neighborA.riverStrenght > neighborB.riverStrenght)
                    {
                        UnLinkFrom(neighborA);
                        neighborA.LinkTo(this);
                    }
                    else
                    {
                        UnLinkFrom(neighborB);
                        neighborB.LinkTo(this);
                    }
                }
                else
                if (flowOut.Count < flowIn.Count)
                {
                    //flowIn.Count = 2
                    GameTile neighborA = Neighbor(flowIn[0]);
                    GameTile neighborB = Neighbor(flowIn[1]);

                    if (neighborA.riverStrenght > neighborB.riverStrenght)
                    {
                        neighborB.UnLinkFrom(this);
                        LinkTo(neighborB);
                    }
                    else
                    {
                        neighborA.UnLinkFrom(this);
                        LinkTo(neighborA);
                    }
                }
            }
            /*
            else
            if (linkAmount > 2 && flowOut.Count < 1 && flowIn.Count < 1)
            {
                //oh fuck comment  je  fais quand il y en a 3?
                // pot'être en check qu'il y a  au moins une entrée et une  sortie
                if (flowOut.Count < 1)
                {
                    GameTile weakRiver = Neighbor(flowIn[0]);
                    for (int i = 1; i < flowIn.Count; i++)
                    {
                        if(Neighbor(flowIn[i]).riverStrenght < weakRiver.riverStrenght)
                        {
                            weakRiver = Neighbor(flowIn[i]);
                        }
                    }

                    weakRiver.UnLinkFrom(this);
                    LinkTo(weakRiver);
                }
                else
                if (flowIn.Count < 1)
                {
                    GameTile weakRiver = Neighbor(flowOut[0]);
                    for (int i = 1; i < flowOut.Count; i++)
                    {
                        if (Neighbor(flowOut[i]).riverStrenght < weakRiver.riverStrenght)
                        {
                            weakRiver = Neighbor(flowOut[i]);
                        }
                    }

                    weakRiver.UnLinkFrom(this);
                    LinkTo(weakRiver);
                }

            }
            */
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
                    Neighbor(flowOut[0]).receivedFlow += Mathf.FloorToInt(riverPower / riverSplits);
                    break;
                case 2:
                    if ((int)riverStrenght % 2 == 0)
                    {
                        Neighbor(flowOut[0]).receivedFlow += Mathf.FloorToInt(riverPower / riverSplits);
                        Neighbor(flowOut[1]).receivedFlow += Mathf.FloorToInt(riverPower / riverSplits);
                    }
                    else
                    {
                        Neighbor(flowOut[0]).receivedFlow += Mathf.FloorToInt(riverPower / riverSplits);
                        Neighbor(flowOut[1]).receivedFlow += Mathf.CeilToInt(riverPower / riverSplits);
                    }
                    break;
                case 3:
                    if ((int)riverStrenght % 3 == 0)
                    {
                        Neighbor(flowOut[0]).receivedFlow += Mathf.FloorToInt(riverPower / riverSplits);
                        Neighbor(flowOut[1]).receivedFlow += Mathf.FloorToInt(riverPower / riverSplits);
                        Neighbor(flowOut[2]).receivedFlow += Mathf.FloorToInt(riverPower / riverSplits);
                    }
                    else
                    {
                        Neighbor(flowOut[0]).receivedFlow += Mathf.FloorToInt(riverPower / riverSplits);
                        Neighbor(flowOut[1]).receivedFlow += Mathf.FloorToInt(riverPower / riverSplits);
                        Neighbor(flowOut[2]).receivedFlow += Mathf.CeilToInt(riverPower / riverSplits);
                    }
                    break;
                default:
                    for (int i = 0; i < flowOut.Count; i++)
                    {
                        Neighbor(flowOut[1]).receivedFlow += Mathf.FloorToInt(riverPower / riverSplits);
                    }
                    break;
            }
        }
        //Reset receivedFlow
        receivedFlow = RiverStrenght._00_;
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
        riverStrenght++;
        riverStrenght = (RiverStrenght)Mathf.Clamp((int)riverStrenght, 0, (int)receivedFlow);
    }
    public void StopFlow()
    {
        riverStrenght--;
        riverStrenght = (RiverStrenght)Mathf.Max((int)riverStrenght, 0);
        receivedFlow = RiverStrenght._00_;
    }

    //LINK
    public void LinkTo(GameTile tile)
    {
        Vector2Int tileToMe = tile.gridPos - gridPos;
        Direction dir = new Direction(tileToMe);

        AddLinkedTile(dir, FlowType.flowOut);
        Neighbor(dir).AddLinkedTile(Direction.Inverse(dir), FlowType.flowIn);
    }
    public void UnLinkFrom(GameTile tile)
    {
        Vector2Int tileToMe = tile.gridPos - gridPos;
        Direction dir = new Direction(tileToMe);

        if (flowIn.Contains(dir))
        {
            tile.RemoveLinkedTile(Direction.Inverse(dir), FlowType.flowOut);
            flowIn.Remove(dir);
        }
        else 
        if (flowOut.Contains(dir))
        {
            tile.RemoveLinkedTile(Direction.Inverse(dir), FlowType.flowIn);
            flowOut.Remove(dir);
        }
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
            UnLinkFrom(Neighbor(flowIn[i]));
        }
        //Flow Out
        for (int i = 0; i < flowOut.Count; i++)
        {
            UnLinkFrom(Neighbor(flowOut[i]));
        }

        riverStrenght = RiverStrenght._00_;
        receivedFlow = RiverStrenght._00_;
    }

    //Help
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
    public GameTile Neighbor(Direction dir)
    {
        return neighbors[(int)dir.dirEnum];
    }
    public bool IsLinkInDir(Direction dir, bool _flowIn)
    {
        if (_flowIn)
        {
            return flowIn.Contains(dir);
        }
        else
        {
            return flowOut.Contains(dir);
        }
    }

    //GameTile to Data
    public void LoadLinkedTile(TileData data)
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
