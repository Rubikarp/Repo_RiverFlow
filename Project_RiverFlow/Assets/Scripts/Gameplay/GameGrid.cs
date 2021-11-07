using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    public static GameGrid instance;

    [Header("Parameter")]
    public float cellSize = 1;
    public Vector2Int size = new Vector2Int(10, 10);
    public Vector2 offSet = new Vector2Int(-5, -5);
    [Space(10)]
    [Header("Data")]
    public Transform gridContainer;
    public GameObject tileTemplate;
    #region Grid-GameTile
    public GameTile[] tiles;
    public GameTile GetTile(int x, int y)
    {
        return tiles[x + (y * (size.x))];
    }
    public GameTile GetTile(Vector2Int pos)
    {
        return tiles[pos.x + (pos.y * (size.x))];
    }
    public void SetTile(int x, int y, GameTile value)
    {
        tiles[x + (y * (size.x))] = value;
    }
    public void SetTile(Vector2Int pos, GameTile value)
    {
        tiles[pos.x + (pos.y * (size.x))] = value;
    }
    #endregion
    public GridData_SCO gridData;
    
    [Header("Debug")]
    public bool showDebug;
    public bool showCenter;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        PopulateGrid();
        SetNeighbor();
    }

    void Update()
    {
        
    }

    [ContextMenu("Populate The GameGrid")]
    private void PopulateGrid()
    {
        if(tiles == null)
        {
            tiles = new GameTile[size.x * size.y];
        }
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                if (GetTile(x, y) == null)
                {
                    GameObject go = Instantiate(tileTemplate, TileToPos(new Vector2Int(x, y)), Quaternion.identity, gridContainer);
                    go.name = "Tile_(" + x + "/" + y + ")";

                    GameTile tile = go.GetComponent<GameTile>();
                    if (tile == null)
                    {
                        Debug.LogError("can't Find Tile on the object");
                    }
                    tile.data = gridData.GetTile(x, y);
                    SetTile(x, y, tile);
                }
            }
        }
    }
    
    [ContextMenu("Reference The GameGrid")]
    private void ReferenceTheGrid()
    {
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                GetTile(x,y).data = gridData.GetTile(x, y);
            }
        }
    }

    [ContextMenu("Set Neighbor")]
    private void SetNeighbor()
    {
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                FillNeighbor(GetTile(x, y).gridPos);
            }
        }
    }
    
    [ContextMenu("Clear The GameGrid")]
    private void ClearGrid()
    {
        if (tiles != null)
        {
            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    if (GetTile(x, y) != null)
                    {
                        GameObject go = GetTile(x, y).gameObject;

                        if (Application.isEditor)
                        {
                            DestroyImmediate(go);
                        }
                        else
                        {
                            Destroy(go);
                        }
                        SetTile(x,y,(GameTile)null);
                    }
                }
            }
        }
    }
    private void FillNeighbor(Vector2Int pos)
    {
        Vector2Int temp = pos;
        Direction dir = new Direction(0);

        GetTile(pos).neighbors = new GameTile[8];
        for (int i = 0; i < 8; i++)
        {
            dir = new Direction((DirectionEnum)i);
            temp = pos + dir.dirValue;

            if(temp.x < 0 || temp.y < 0)
            {
                GetTile(pos).neighbors[i] = null;
            }
            else
            if (temp.x > size.x - 1 || temp.y > size.y - 1)
            {
                GetTile(pos).neighbors[i] =  null;
            }
            else
            {
                GetTile(pos).neighbors[i] = GetTile(temp);
            }
        }

    }

    /// <summary>
    /// Convert a GameGrid Pos to a worldPos
    /// WARNING ! The result can be extrapolate farther than the GridSize
    /// </summary>
    /// <param name="posInGrid"> Position on the GameGrid (can be negative)</param>
    /// <returns></returns>
    public Vector3 TileToPos(Vector2Int posInGrid)
    {
        //Get bottom left corner |_
        Vector3 bottomLeft = new Vector3(offSet.x, offSet.y, 0);
        bottomLeft -= new Vector3(size.x, size.y, 0) * 0.5f * cellSize;
        //Centre de la case
        bottomLeft += new Vector3(cellSize * 0.5f, cellSize * 0.5f, 0);

        Vector3 result = bottomLeft + (new Vector3(posInGrid.x, posInGrid.y, 0) * cellSize);

        return result;
    }

    /// <summary>
    /// Convert a Point on the GamePlane to the GameGrid Pos related
    /// WARNING ! it can result a pos outside of the actual grid
    /// </summary>
    /// <param name="planePos"> point on the plane where you look for the tile</param>
    /// <returns></returns>
    public Vector2Int PosToTile(Vector3 planePos)
    {
        //Get bottom left corner |_
        Vector3 bottomLeft = new Vector3(offSet.x, offSet.y, 0);
        bottomLeft -= new Vector3(size.x, size.y, 0) * 0.5f * cellSize;
        //Pas de centrage sur la case car floor, si round activer la ligne
        //bottomLeft += new Vector3(cellSize * 0.5f, cellSize * 0.5f, 0);

        Vector3 posRelaToGrid = planePos - bottomLeft;
        float returnToCellsize = 1 / cellSize;
        Vector2Int result = new Vector2Int(Mathf.FloorToInt(posRelaToGrid.x * returnToCellsize), Mathf.FloorToInt(posRelaToGrid.y * returnToCellsize));

        return result;
    }

    private void OnDrawGizmos()
    {
        if (showDebug)
        {
            Vector3 startPos = new Vector3(offSet.x, offSet.y, 0);
            startPos -= new Vector3(size.x, size.y, 0) * 0.5f * cellSize;

            float halfCell = cellSize * 0.5f;

            if (showCenter)
            {
                #region center point

                Gizmos.color = Color.green;
                for (int x = 0; x < size.x; x++)
                {
                    for (int y = 0; y < size.y; y++)
                    {
                        Gizmos.DrawWireSphere(startPos + new Vector3(x * cellSize, y * cellSize, 0) + new Vector3(cellSize * 0.5f, cellSize * 0.5f, 0), cellSize * 0.5f * 0.8f);
                    }
                }
                #endregion
            }

            //GameGrid decals
            Gizmos.color = Color.red;
            for (int x = 0; x <= size.x; x++)
            {
                Debug.DrawRay(startPos + new Vector3(cellSize * x, 0, 0), Vector3.up * size.y * cellSize, Color.red);
            }
            for (int y = 0; y <= size.y; y++)
            {
                Debug.DrawRay(startPos + new Vector3(0, cellSize * y, 0), Vector3.right * size.x * cellSize, Color.red);
            }
        }
    }
}
