using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour, Element
{
    [Header("Variable")]
    public GameTile tileOn;
    public GameTile[] TilesOn
    {
        get
        {
            if (tileOn == null)
            {
                Debug.LogError("Can't find the tile where is this WaterSource", this);
                return new GameTile[1] { null };
            }
            return new GameTile[1] { tileOn };
        }
        set { tileOn = value[0]; }
    }
    ///Not Likable
    public bool isLinkable { get { return false; } }


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
