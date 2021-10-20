using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    public GameGrid grid;
    public Vector2Int gridPos;

    void Start()
    {
        gridPos = new Vector2Int(Mathf.FloorToInt(grid.gridSize.x * 0.5f), Mathf.FloorToInt(grid.gridSize.y / 2));
        transform.position = grid.TilePos(gridPos);
    }

    void Update()
    {
        //haut
        if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.UpArrow))
        {
            if (gridPos.y + 1 < grid.gridSize.y)
            {
                gridPos = gridPos + Vector2Int.up;
            }
        }
        //bas
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            if (0 <= gridPos.y - 1)
            {
                gridPos = gridPos + Vector2Int.down;
            }
        }
        //droite
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (gridPos.x + 1 < grid.gridSize.x)
            {
                gridPos = gridPos + Vector2Int.right;
            }
        }
        //gauche
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (0 <= gridPos.x - 1)
            {
                gridPos = gridPos + Vector2Int.left;
            }
        }

        //Clamp in zone
        gridPos = ClampPosInGrid(gridPos);

        transform.position = grid.TilePos(gridPos);
    }

    public Vector2Int ClampPosInGrid(Vector2Int pos)
    {
        return new Vector2Int(
            Mathf.Clamp(pos.x, 0, grid.gridSize.x - 1),
            Mathf.Clamp(pos.y, 0, grid.gridSize.y - 1)
            );
    }
}
