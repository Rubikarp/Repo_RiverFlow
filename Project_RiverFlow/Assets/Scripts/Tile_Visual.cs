using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Visual : MonoBehaviour
{
    public Tile tile;
    public MeshRenderer rend;

    [Header("Ground")]
    public Material fullGround;
    public Material holeGround;
    public Material errorMat;
    [Header("Water")]
    public Material wat25;
    public Material wat50, wat75, wat100;

    void Update()
    {
        UpdateState();
    }
    public void UpdateState()
    {
        switch (tile.state)
        {
            case TileState.Full:
                if (rend.sharedMaterial != fullGround)
                    rend.sharedMaterial = fullGround;
                break;
            case TileState.Hole:
                if (rend.sharedMaterial != holeGround)
                    rend.sharedMaterial = holeGround;
                break;
            default:
                if (rend.sharedMaterial != errorMat)
                rend.sharedMaterial = errorMat;
                break;
        }
    }

}
