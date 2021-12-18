using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NaughtyAttributes;

public class GameGrid : Singleton<GameGrid>
{
    [Header("References")]
    [Required] public Transform gridContainer;
    [Required] public GameObject tileTemplate;

    [Header("Parameter")]
    public float cellSize = 1;
    public Vector2Int size = new Vector2Int(16, 16);
    public Vector2 offSet = new Vector2Int(0, 0);

    [Header("Data")]
    public Map_Data mapData;
    public GameTile[] tiles;
    #region Grid-Tile Methodes
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

    [BoxGroup("Debug")] public bool showDebug;
    [BoxGroup("Debug")] [ShowIf("showDebug")] public Color debugLineColor = Color.red;
    [BoxGroup("Debug")] [ShowIf("showDebug")] public bool showCenter;
    [BoxGroup("Debug")] [ShowIf("showCenter")] public Color debugCenterColor = Color.black;

    void Start()
    {
        //LoadMap();
    }

    #region Map Manage
    [Button]
    private void LoadMap()
    {
        if(mapData == null)
        {
            Debug.LogError("No map Data Available");
            return;
        }
        size = mapData.gridSize;
        //offSet = -size/2;

        //Clear the map
        for (int i = 0; i < gridContainer.childCount; i++)
        {
#if UNITY_EDITOR
            DestroyImmediate(gridContainer.GetChild(0).gameObject);
#else
            Destroy(gridContainer.GetChild(0).gameObject);
#endif
        }
        ClearGrid();

        PopulateGrid(size);
        ReferenceTheGrid();
        SetNeighbor();
        UpdateGraph(size);
    }
    private void ClearGrid()
    {
        if (tiles != null)
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i] != null)
                {
                    GameObject go = tiles[i].gameObject;
#if UNITY_EDITOR
                    DestroyImmediate(go);
#else
                    Destroy(go);
#endif
                }
            }
            tiles = null;
        }
    }
    private void PopulateGrid(Vector2Int size)
    {
        if(tiles == null || tiles.Length ==0)
        {
            tiles = new GameTile[size.x * size.y];
        }
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                if (GetTile(x, y) == null)
                {
#if UNITY_EDITOR
                    GameObject go = PrefabUtility.InstantiatePrefab(tileTemplate, gridContainer) as GameObject;
#else
                    GameObject go = Instantiate(tileTemplate, gridContainer);
#endif 
                    go.name = "Tile_(" + x + "/" + y + ")";
                    go.transform.position = TileToPos(new Vector2Int(x, y));

                    GameTile tile = go.GetComponent<GameTile>();
                    if (tile == null)
                    {
                        Debug.LogError("can't Find Tile on the object");
                    }

                    tile.gridPos = new Vector2Int(x, y);
                    tile.type = mapData.GetTileType(x, y);
                    SetTile(x, y, tile);
                }
            }
        }
    }
    private void ReferenceTheGrid()
    {
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                GetTile(x,y).type = mapData.GetTileType(x, y);
            }
        }
    }
    private void SetNeighbor()
    {
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                GetTile(x, y).FillNeighbor(this);
            }
        }
    }
    private void UpdateGraph(Vector2Int size)
    {
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                if (GetTile(x, y) != null)
                {
                    GameObject go = GetTile(x, y).gameObject;
                    GameTile_Drawer tile = go.GetComponent<GameTile_Drawer>();
                    if (tile == null)
                    {
                        Debug.LogError("can't Find Tile on the object");
                    }
                    tile.UpdateTileColor();
                }
            }
        }
    }

    [Button]
    private void LoadSavedMap()
    {

    }
    #endregion

    #region Grid<->World Convertion
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
    #endregion

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
                Color temp = Gizmos.color;
                Gizmos.color = debugCenterColor;
                for (int x = 0; x < size.x; x++)
                {
                    for (int y = 0; y < size.y; y++)
                    {
                        Gizmos.DrawWireSphere(startPos + new Vector3(x * cellSize, y * cellSize, 0) + new Vector3(cellSize * 0.5f, cellSize * 0.5f, 0), cellSize * 0.5f * 0.8f);
                    }
                }
                Gizmos.color = temp;
                #endregion
            }

            //GameGrid decals
            for (int x = 0; x <= size.x; x++)
            {
                Debug.DrawRay(startPos + new Vector3(cellSize * x, 0, 0), Vector3.up * size.y * cellSize, debugLineColor);
            }
            for (int y = 0; y <= size.y; y++)
            {
                Debug.DrawRay(startPos + new Vector3(0, cellSize * y, 0), Vector3.right * size.x * cellSize, debugLineColor);
            }
        }
    }
}
