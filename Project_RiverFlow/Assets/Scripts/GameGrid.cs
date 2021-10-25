using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{    
    [Header("Parameter")]
    public float cellSize = 1;
    public Vector2Int gridSize = new Vector2Int(3,3);
    public Vector2 gridOffSet;
    [Space(10)]
    [Header("Parameter")]
    public Tile[,] tiles;

    public GameObject tileTemplate;

    [Header("Debug")]
    public bool showDebug;
    public bool showCenter;

    void Start()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                GameObject go = Instantiate(tileTemplate, TileToPos(new Vector2Int(x, y)), Quaternion.identity, transform);
                go.name = "Tile_(" + x + "/" + y + ")";
            }
        }
    }

    void Update()
    {

    }

    public Vector3 TileToPos(Vector2Int posInGrid)
    {
        //Start at the bottom left border
        Vector3 bottomLeft = new Vector3(gridOffSet.x, gridOffSet.y, 0);
        bottomLeft += new Vector3(cellSize * 0.5f, cellSize * 0.5f, 0);
        bottomLeft -= new Vector3(gridSize.x, gridSize.y, 0) * 0.5f * cellSize;

        Vector3 result = bottomLeft + (new Vector3(posInGrid.x, posInGrid.y, 0) * cellSize);

        return result;
    }
    public Vector2Int PosToTile(Vector3 planePos)
    {
        //Start at the bottom left border
        Vector3 bottomLeft = new Vector3(gridOffSet.x, gridOffSet.y, 0);
        //bottomLeft += new Vector3(cellSize * 0.5f, cellSize * 0.5f, 0); //pas besoin car floor plus tard
        bottomLeft -= new Vector3(gridSize.x, gridSize.y, 0) * 0.5f * cellSize;

        Vector3 posRelaToGrid = planePos - bottomLeft;
        float returnToCellsize = 1 / cellSize;
        Vector2Int result = new Vector2Int(Mathf.FloorToInt(posRelaToGrid.x * returnToCellsize), Mathf.FloorToInt(posRelaToGrid.y * returnToCellsize));

        return result;
    }

    private void OnDrawGizmos()
    {
        if (showDebug)
        {
            Vector3 startPos = new Vector3(gridOffSet.x, gridOffSet.y, 0);
            startPos -= new Vector3(gridSize.x, gridSize.y, 0) * 0.5f * cellSize;

            float halfCell = cellSize * 0.5f;

            if (showCenter)
            {
            #region center point

                Gizmos.color = Color.green;
                for (int x = 0; x < gridSize.x; x++)
                {
                    for (int y = 0; y < gridSize.y; y++)
                    {
                        Gizmos.DrawWireSphere(startPos + new Vector3(x * cellSize, y * cellSize, 0) + new Vector3(cellSize * 0.5f, cellSize * 0.5f, 0), cellSize * 0.5f * 0.8f);
                    }
                }
            #endregion
            }

            //Grid decals
            Gizmos.color = Color.red;
            for (int x = 0; x <= gridSize.x; x++)
            {
                Debug.DrawRay(startPos + new Vector3(cellSize * x, 0, 0), Vector3.up * gridSize.y * cellSize, Color.red);
            }
            for (int y = 0; y <= gridSize.y; y++)
            {
                Debug.DrawRay(startPos + new Vector3(0, cellSize * y, 0), Vector3.right * gridSize.x * cellSize, Color.red);
            }
        }
    }

}
