using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile_Drawer : MonoBehaviour
{
    public GameTile tile;
    public SpriteRenderer rend;

    [Header("Ground")]
    [SerializeField, ColorUsage(true, false)] Color fullGround;
    [SerializeField, ColorUsage(true, false)] Color holedGround;
    [Space(5)]
    [SerializeField, ColorUsage(true, false)] Color wat25;
    [SerializeField, ColorUsage(true, false)] Color wat50;
    [SerializeField, ColorUsage(true, false)] Color wat75;
    [SerializeField, ColorUsage(true, false)] Color wat100;
    [Space(10)]
    [SerializeField, ColorUsage(true, false)] Color errorMat;


    void Update()
    {
        UpdateTileColor();
    }

    public void UpdateTileColor()
    {
        if (tile.isDuged)
        {
            if (tile.isRiver)
            {
                switch (tile.flowStrenght)
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
    
    /*
    private void OnDrawGizmos()
    {
        UpdateTileColor();
    }
    */
}
