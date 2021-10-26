using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer : MonoBehaviour
{
    [Header("reférence")]
    public IteractionGrid grid;

    [Header("parameter")]
    public Vector2Int startPos = new Vector2Int(0, 0);
    public Vector2Int size = new Vector2Int(0, 0);
    public Tile[,] tiles;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
