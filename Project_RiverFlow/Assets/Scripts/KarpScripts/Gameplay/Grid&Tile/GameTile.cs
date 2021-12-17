using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameTile[] neighbors = new GameTile[8];
    public GameTile[] neighborsDist2 = new GameTile[16];
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
            if (linkAmount >= maxConnection) //Si la tile n'est pas saturé
            {
                return false;
            }
            if(type == TileType.mountain) //Si la tile est une montagne
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
                            break;
                    }

                }
                foreach (GameTile neighborOfNeighbor in neighbor.neighbors)
                {
                    if (neighborOfNeighbor.isElement)
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
    public TileSpawnScore spawnScore;
    public int spawnArea = 0;

    void Start()
    {
        spawnScore = GetComponent<TileSpawnScore>();
    }

    //Flow
    public void FlowStep()
    {
        //Reset receivedFlow
        receivedFlow = FlowStrenght._00_;

        //Check received Water
        if (flowIn.Count > 0 || element is WaterSource)
        {
            if (element is WaterSource)
            {
                receivedFlow = FlowStrenght._100_;
            }
            else
            {
                for (int i = 0; i < flowIn.Count; i++)
                {
                    receivedFlow += (int)GetNeighbor(flowIn[i]).AskForWater(this);
                }
                if (element is Cloud)
                {
                    receivedFlow += (int)FlowStrenght._25_;
                }
                receivedFlow = (FlowStrenght)Mathf.Clamp((int)receivedFlow, 0, (int)FlowStrenght._100_);
            }

            //SetFlow
            riverStrenght = receivedFlow;
        }
        else
        {
            StopFlow();
            return;
        }

        //Check for contradictory flow
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
            if(flowIn.Count >= 2)
            {
                int maxFlow = 0;

                for (int i = 0; i < flowIn.Count; i++)
                {
                    maxFlow = Mathf.Max(maxFlow, (int)GetNeighbor(flowIn[i]).riverStrenght);
                }

                //TODO
            }
        }
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
    public void StopFlow()
    {
        riverStrenght = FlowStrenght._00_;
        receivedFlow = FlowStrenght._00_;
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
            else
            {
                Debug.LogError("oupsie", this);
            }
        }
        else
        {
            if (!flowOut.Contains(addedDir))
            {
                flowOut.Add(addedDir);
            }
            else
            {
                Debug.LogError("oupsie", this);
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
            else 
            {
                Debug.LogError("oupsie", this);
            }
        }
        else
        {
            if (flowOut.Contains(removeDir))
            {
                flowOut.Remove(removeDir);
            }
            else
            {
                Debug.LogError("oupsie", this);
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
            if (temp.x > grid.size.x - 1 || temp.y > grid.size.y - 1)
            {
                neighbors[i] = null;
            }
            else
            {
                neighbors[i] = grid.GetTile(temp);
            }
        }

        //FillNeighborDist2();
    }
    public GameTile[] GetNeighbor()
    {
        GameTile[] result = new GameTile[8];

        Vector2Int temp = gridPos;
        Direction dir = new Direction(0);

        neighbors = new GameTile[8];
        for (int i = 0; i < 8; i++)
        {
            dir = new Direction((DirectionEnum)i);
            temp = gridPos + dir.dirValue;

            if (temp.x < 0 || temp.y < 0)
            {
                result[i] = null;
            }
            else
            if (temp.x > GameGrid.instance.size.x - 1 || temp.y > GameGrid.instance.size.y - 1)
            {
                result[i] = null;
            }
            else
            {
                result[i] = GameGrid.instance.GetTile(temp);
            }
        }

        return result;
    }

    public void FillNeighborDist2()
    {
        List<GameTile> pack = new List<GameTile>();
        pack.AddRange(neighbors[0].GetNeighbor());
        pack.AddRange(neighbors[2].GetNeighbor());
        pack.AddRange(neighbors[4].GetNeighbor());
        pack.AddRange(neighbors[6].GetNeighbor());

        List<GameTile> result = new List<GameTile>();

        neighbors = new GameTile[8];
        for (int i = 0; i < pack.Count; i++)
        {
            if(pack[i] != null && pack[i] != this)
            {
                if(NeighborIndex(pack[i]) == -1)
                {

                }
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
        return -1;
    }
    public GameTile GetNeighbor(Direction dir)
    {
        return neighbors[(int)dir.dirEnum];
    }
    public bool IsLinkTo(GameTile tile)
    {
        return linkedTile.Contains(tile);
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
