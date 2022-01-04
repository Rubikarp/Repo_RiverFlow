using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class GameTile : MonoBehaviour
{
    public const int maxConnection = 3;

    [Header("Position")]
    public Vector2Int gridPos;
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

    [Header("State")]
    public TileType type;
    public FlowStrenght riverStrenght;

    [Header("GameObject Data")]
    public Element element;
    [HorizontalLine(height:5,color:EColor.Red)]
    public GameTile[] neighbors = new GameTile[8];
    public GameTile[] neighborsDist2 = new GameTile[16];
    [HorizontalLine(height:5, color: EColor.Green)]
    public List<Direction> flowIn = new List<Direction>();
    public List<Direction> flowOut = new List<Direction>();
    [HorizontalLine(height:5, color: EColor.Yellow)]
    public List<Canal> canalsIn = new List<Canal>();
    #region Getter / Setter
    public bool haveElement
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
            if (linkAmount >= maxConnection) 
            {
                return false;
            }
            if(haveElement)
            {
                if (!element.isLinkable)
                {
                    return false;
                }
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
            if (riverStrenght > 0 && isDuged)
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
                    switch (type)
                    {
                        case TileType.grass:
                            if (neighbor.riverStrenght >= FlowStrenght._25_)
                            {
                                return true;
                            }
                            break;
                        case TileType.clay:
                            if (neighbor.riverStrenght >= FlowStrenght._50_)
                            {
                                return true;
                            }
                            break;
                        case TileType.sand:
                            if (neighbor.riverStrenght >= FlowStrenght._75_)
                            {
                                return true;
                            }
                            break;
                        default:
                            Debug.Log("oupsie");
                            break;
                    }

                }
                foreach (GameTile neighborOfNeighbor in neighbor.neighbors)
                {
                    if (neighborOfNeighbor.haveElement)
                    {
                        if (neighborOfNeighbor.element is Lake)
                        {
                            if (neighborOfNeighbor.isRiver)
                            {
                                switch (type)
                                {
                                    case TileType.grass:
                                        if (neighborOfNeighbor.riverStrenght >=FlowStrenght._25_)
                                        {
                                            return true;
                                        }
                                        break;
                                    case TileType.clay:
                                        if (neighborOfNeighbor.riverStrenght >= FlowStrenght._50_)
                                        {
                                            return true;
                                        }
                                        break;
                                    case TileType.sand:
                                        if (neighborOfNeighbor.riverStrenght >= FlowStrenght._75_)
                                        {
                                            return true;
                                        }
                                        break;
                                    default:
                                        break;
                                }

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
    private DistanceField distanceField;
    public TileSpawnScore spawnScore;
    public int spawnArea = 0;

    void Start()
    {
        spawnScore = GetComponent<TileSpawnScore>();
        distanceField = GameObject.Find("PlantSpawner").GetComponent<DistanceField>();
    }

    //Flow
    public void UpdateReceivedFlow()
    {
        bool irrigate = riverStrenght > 0;
        //Update RiverStrenght
        riverStrenght = ReceivedFlow();

        /*if(irrigate != riverStrenght > 0)
        {
            if(distanceField != null)
            {
                distanceField.SetZero(gridPos.x, gridPos.y);
            }
        }*/
    }
    public FlowStrenght ReceivedFlow()
    {
        if (element is WaterSource)
        {
            return FlowStrenght._100_;
        }
        int received = (int)FlowStrenght._00_;
        if (flowIn.Count > 0)
        {
            if (element is Cloud)
            {
                received += (int)FlowStrenght._25_;
            }
            for (int i = 0; i < flowIn.Count; i++)
            {
                received += (int)GetNeighbor(flowIn[i]).AskForWater(this);
            }
            return (FlowStrenght)Mathf.Clamp(received, 0, (int)FlowStrenght._100_);
        }
        return (FlowStrenght)received;
    }
    public FlowStrenght AskForWater(GameTile asker)
    {
        Vector2Int tileToMe = asker.gridPos - gridPos;
        Direction dir = new Direction(tileToMe);

        if (flowOut.Contains(dir))
        {
            // Envoie l'eau Thierry !
            float riverPower = (int)riverStrenght;
            float riverSplits = (float)flowOut.Count;

            if(flowOut.Count == 1)
            {
                return (FlowStrenght)riverPower;
            }
            else 
            if (flowOut.Count > 1)
            {
                if ((int)riverStrenght % 2 == 0)
                {
                    return (FlowStrenght)Mathf.FloorToInt((int)riverStrenght / (float)flowOut.Count);
                }
                else
                {
                    if(flowOut.IndexOf(dir) == 0)
                    {
                        return (FlowStrenght)Mathf.CeilToInt((int)riverStrenght / (float)flowOut.Count);
                    }
                    else
                    {
                        return (FlowStrenght)Mathf.FloorToInt((int)riverStrenght / (float)flowOut.Count);
                    }
                }
            }
            return FlowStrenght._00_;
        }
        else
        {
            return FlowStrenght._00_;
        }
    }

    //LINK
    public static void Link(GameTile tileA, GameTile tileB)
    {
        Vector2Int tileToMe = tileB.gridPos - tileA.gridPos;
        Direction dir = new Direction(tileToMe);

        tileA.AddLinkedTile(dir, FlowType.flowOut);
        tileB.AddLinkedTile(Direction.Inverse(dir), FlowType.flowIn);
    }
    public static void UnLink(GameTile tileA, GameTile tileB)
    {
        Vector2Int tileToMe = tileB.gridPos - tileA.gridPos;
        Direction dir = new Direction(tileToMe);

        tileA.RemoveLinkedTile(dir, FlowType.flowOut);
        tileB.RemoveLinkedTile(Direction.Inverse(dir), FlowType.flowIn);
    }
    public static void UnLinkAll(GameTile tileA)
    {
        //Flow In
        for (int i = 0; i < tileA.flowIn.Count; i++)
        {
            GameTile neighB = tileA.GetNeighbor(tileA.flowIn[i]);
            UnLink(neighB, tileA);
        }
        //Flow Out
        for (int i = 0; i < tileA.flowOut.Count; i++)
        {
            GameTile neighB = tileA.GetNeighbor(tileA.flowOut[i]);
            UnLink(tileA, neighB);
        }
    }
    public static void InverseLink(GameTile tileA, GameTile tileB)
    {
        Vector2Int tileToMe = tileB.gridPos - tileA.gridPos;
        Direction dir = new Direction(tileToMe);

        tileA.RemoveLinkedTile(dir, FlowType.flowIn);
        tileB.RemoveLinkedTile(Direction.Inverse(dir), FlowType.flowOut);

        tileA.AddLinkedTile(dir, FlowType.flowOut);
        tileB.AddLinkedTile(Direction.Inverse(dir), FlowType.flowIn);

    }

    public void AddLinkedTile(Direction addedDir, FlowType flow)
    {
        if (flow == FlowType.flowIn)
        {
            if (!flowIn.Contains(addedDir))
            {
                flowIn.Add(addedDir);
            }
        }
        else
        {
            if (!flowOut.Contains(addedDir))
            {
                flowOut.Add(addedDir);
            }
        }
    }
    public void RemoveLinkedTile(Direction removeDir, FlowType flow)
    {
        if (flow == FlowType.flowIn)
        {
            if (flowIn.Contains(removeDir))
            {
                flowIn.Remove(removeDir);
            }
        }
        else
        {
            if (flowOut.Contains(removeDir))
            {
                flowOut.Remove(removeDir);
            }
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
    public void FillNeighbor(GameGrid grid)
    {        
        //Clear
        neighbors = new GameTile[8];

        Vector2Int tempPos; 
        Direction dir;
        for (int i = 0; i < 8; i++)
        {
            dir = new Direction((DirectionEnum)i);
            tempPos = gridPos + dir.dirValue;
            neighbors[i] = grid.PosInGrid(tempPos) ? grid.GetTile(tempPos) : null;
        }

        FillNeighborDist2(grid);
    }
    public void FillNeighborDist2(GameGrid grid)
    {
        //Clear
        neighborsDist2 = new GameTile[16];

        /* 
        Tracing Order
        00/01/02/03/04
        15          05
        14          06
        13          07
        12/11/10/09/08
        */
        Vector2Int testPos = gridPos + new Vector2Int(-2,2);
        int dist2Index = 0;
        //Upleft->UpRight Line
        for (int i = 0; i < 4; i++)
        {
            neighborsDist2[dist2Index] = grid.PosInGrid(testPos)?grid.GetTile(testPos) : null;
            testPos += Vector2Int.right;
            dist2Index++;
        }
        //UpRight->DownRight Line
        for (int i = 0; i < 4; i++)
        {
            neighborsDist2[dist2Index] = grid.PosInGrid(testPos) ? grid.GetTile(testPos) : null;
            testPos += Vector2Int.down;
            dist2Index++;
        }
        //DownRight->DownLeft Line
        for (int i = 0; i < 4; i++)
        {
            neighborsDist2[dist2Index] = grid.PosInGrid(testPos) ? grid.GetTile(testPos) : null;
            testPos += Vector2Int.left;
            dist2Index++;
        }
        //DownLeft->Upleft Line
        for (int i = 0; i < 4; i++)
        {
            neighborsDist2[dist2Index] = grid.PosInGrid(testPos) ? grid.GetTile(testPos) : null;
            testPos += Vector2Int.up;
            dist2Index++;
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
        return -1;
    }
    public GameTile GetNeighbor(Direction dir)
    {
        return neighbors[(int)dir.dirEnum];
    }
    //
    public bool IsLinkTo(GameTile tile)
    {
        return IsLinkInDir(new Direction(NeighborIndex(tile)));
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
    public bool IsLinkInDir(Direction dir)
    {
        if (flowIn.Contains(dir))
        {
            return true;
        }
        if (flowOut.Contains(dir))
        {
            return true;
        }
        return false;
    }
    //
    public List<GameTile> GetLinkedTile()
    {
        List<GameTile> result = new List<GameTile>();
        for (int i = 0; i < flowIn.Count; i++)
        {
            result.Add(GetNeighbor(flowIn[i]));
        }
        for (int i = 0; i < flowOut.Count; i++)
        {
            result.Add(GetNeighbor(flowOut[i]));
        }
        return result;
    }

    //GameTile to Data
    public void SaveInTileData(TileSaveData data)
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

    private void OnDrawGizmos()
    {
        Vector3 vec;
        for (int i = 0; i < flowOut.Count; i++)
        {
            vec = new Vector3(flowOut[i].dirValue.x, flowOut[i].dirValue.y);

            Debug.DrawRay(worldPos - Vector3.Cross(vec, Vector3.forward).normalized * 0.1f, vec, Color.cyan);
        }
        for (int i = 0; i < flowIn.Count; i++)
        {
            vec = new Vector3(flowIn[i].dirValue.x, flowIn[i].dirValue.y);
            Debug.DrawRay(worldPos + Vector3.Cross(vec, Vector3.back).normalized * 0.1f, vec, Color.magenta);
        }

    }
}
