using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RiverFlow.V1
{

    public class IteractionGrid : MonoBehaviour
    {
        [Header("Parameter")]
        public float cellSize = 1;
        public Vector2Int gridSize = new Vector2Int(3, 3);
        public Vector2 gridOffSet;
        [Space(10)]
        [Header("Parameter")]
        public OldTile[,] tiles;

        public GameObject tileTemplate;

        [Header("Debug")]
        public bool showDebug;
        public bool showCenter;

        void Start()
        {
            tiles = new OldTile[gridSize.x, gridSize.y];
            PopulateGrid();
        }

        void Update()
        {

        }

        private void PopulateGrid()
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    GameObject go = Instantiate(tileTemplate, TileToPos(new Vector2Int(x, y)), Quaternion.identity, transform);
                    go.name = "Tile_(" + x + "/" + y + ")";

                    OldTile tile = go.GetComponent<OldTile>();
                    if (tile == null)
                    {
                        Debug.LogError("can't Find Tile on the object");
                    }
                    tile.SetValue(new Vector2Int(x, y), TileType.soil, TileState.Full);
                    tiles[x, y] = tile;
                }
            }
        }


        /// <summary>
        /// Convert a Grid Pos to a worldPos
        /// WARNING ! The result can be extrapolate farther than the GridSize
        /// </summary>
        /// <param name="posInGrid"> Position on the Grid (can be negative)</param>
        /// <returns></returns>
        public Vector3 TileToPos(Vector2Int posInGrid)
        {
            //Get bottom left corner |_
            Vector3 bottomLeft = new Vector3(gridOffSet.x, gridOffSet.y, 0);
            bottomLeft -= new Vector3(gridSize.x, gridSize.y, 0) * 0.5f * cellSize;
            //Centre de la case
            bottomLeft += new Vector3(cellSize * 0.5f, cellSize * 0.5f, 0);

            Vector3 result = bottomLeft + (new Vector3(posInGrid.x, posInGrid.y, 0) * cellSize);

            return result;
        }

        /// <summary>
        /// Convert a Point on the GamePlane to the Grid Pos related
        /// WARNING ! it can result a pos outside of the actual grid
        /// </summary>
        /// <param name="planePos"> point on the plane where you look for the tile</param>
        /// <returns></returns>
        public Vector2Int PosToTile(Vector3 planePos)
        {
            //Get bottom left corner |_
            Vector3 bottomLeft = new Vector3(gridOffSet.x, gridOffSet.y, 0);
            bottomLeft -= new Vector3(gridSize.x, gridSize.y, 0) * 0.5f * cellSize;
            //Pas de centrage sur la case car floor, si round activer la ligne
            //bottomLeft += new Vector3(cellSize * 0.5f, cellSize * 0.5f, 0);

            Vector3 posRelaToGrid = planePos - bottomLeft;
            float returnToCellsize = 1 / cellSize;
            Vector2Int result = new Vector2Int(Mathf.FloorToInt(posRelaToGrid.x * returnToCellsize), Mathf.FloorToInt(posRelaToGrid.y * returnToCellsize));

            return result;
        }
        public OldTile GetTile(Vector2Int posInGrid)
        {
            return tiles[posInGrid.x, posInGrid.y]; ;
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
}
