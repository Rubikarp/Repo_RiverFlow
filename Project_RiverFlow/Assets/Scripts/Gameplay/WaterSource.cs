using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSource : MonoBehaviour, Element
{
    [Header("Variable")]
    public GameTile tileOn;
    public GameTile[] TilesOn 
    {
        get 
        { 
            if(tileOn == null)
            {
                Debug.LogError("Can't find the tile where is this WaterSource", this);
                return new GameTile[1] { null };
            }
            return new GameTile[1] { tileOn }; 
        } 
        set { tileOn = value[0]; } 
    }
    public bool isLinkable { get { return true; } }

    public Direction flowOut;

    void Start()
    {
    }

    void Update()
    {
        
    }
}
