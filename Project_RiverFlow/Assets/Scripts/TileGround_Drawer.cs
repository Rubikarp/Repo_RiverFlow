using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGround_Drawer : MonoBehaviour
{
    public TileGround ground;
    public SpriteRenderer rend;

    [Header("Ground")]
    public Color fullGround;
    public Color holedGround;
    [Space(5)]
    public Color wat25;
    public Color wat50;
    public Color wat75;
    public Color wat100;
    [Space(10)]
    public Color errorMat;


    void Update()
    {
        UpdateTileColor();
    }

    public void UpdateTileColor()
    {
        if (ground.isDuged)
        {
            if (ground.isRiver)
            {
                switch (ground.flowStrenght)
                {
                    case RiverStrenght._00_:
                        rend.color = holedGround;
                        break;
                    case RiverStrenght._25_:
                        rend.color = wat25;
                        break;
                    case RiverStrenght._50_:
                        rend.color = wat50;
                        break;
                    case RiverStrenght._75_:
                        rend.color = wat75;
                        break;
                    case RiverStrenght._100_:
                        rend.color = wat100;
                        break;
                    default:
                        rend.color = errorMat;
                        break;
                }
            }
            else
            {
                rend.color = holedGround;
            }
        }
        else
        {
            rend.color = fullGround;
        }
    }

}
