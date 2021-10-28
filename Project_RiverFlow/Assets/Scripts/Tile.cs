using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    #region Variable 
    [Header("Data")]
    public Vector2Int position = new Vector2Int(0, 0);
    [Space(5)]
    public Tile[] neighbors = new Tile[8];
    #endregion

    #region Constructor 
    public Tile( int pos_x = 0, int pos_y = 0)
    {
        this.position = new Vector2Int(pos_x, pos_y);
    }
    public Tile(Vector2Int pos)
    {
        this.position = pos;
    }
    #endregion


}
