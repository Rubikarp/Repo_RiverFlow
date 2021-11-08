using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile_Drawer : MonoBehaviour
{
    public GameTile tile;
    public SpriteRenderer rend;
    public GameGrid grid;
    [Space(8)]
    public GameObject lineRendererTemplate;
    public List<LineRenderer> lines = new List<LineRenderer>();

    private Vector3[] linePos = new Vector3[0];

    [Header("Ground")]
    [SerializeField] TilePalette_SCO palette;

    private void Start()
    {
        grid = GameGrid.instance;
    }

    void Update()
    {
        UpdateTileColor();
        UpdateRiver();
    }

    public void UpdateTileColor()
    {
        //Tile Coloring
        switch (tile.type)
        {
            case TileType.other:
                rend.color = palette.errorMat;
                break;

            case TileType.grass:
                rend.color = palette.groundGrass;
                break;

            case TileType.clay:
                rend.color = palette.groundClay;
                break;

            case TileType.sand:
                rend.color = palette.groundAride;
                break;

            default:
                rend.color = palette.errorMat;
                break;
        }

        for (int i = 0; i < lines.Count; i++)
        {
            //RiverColor
            if (tile.isDuged)
            {
                switch (tile.riverStrenght)
                {
                    case RiverStrenght._00_:
                        lines[i].startColor = palette.holedGround;
                        lines[i].endColor = palette.holedGround;
                        break;
                    case RiverStrenght._25_:
                        lines[i].startColor = palette.wat25;
                        lines[i].endColor = palette.wat25;
                        break;
                    case RiverStrenght._50_:
                        lines[i].startColor = palette.wat50;
                        lines[i].endColor = palette.wat50;
                        break;
                    case RiverStrenght._75_:
                        lines[i].startColor = palette.wat75;
                        lines[i].endColor = palette.wat75;
                        break;
                    case RiverStrenght._100_:
                        lines[i].startColor = palette.wat100;
                        lines[i].endColor = palette.wat100;
                        break;
                    default:
                        lines[i].startColor = palette.errorMat;
                        lines[i].endColor = palette.errorMat;
                        break;
                }
            }
            else
            {
                lines[i].startColor = palette.holedGround;
                lines[i].endColor = palette.holedGround;
            }

        }
    }
    public void UpdateRiver()
    {
        if (tile.linkAmount > lines.Count)
        {
            lines.Add(Instantiate(lineRendererTemplate, transform.position, Quaternion.identity, transform).GetComponent<LineRenderer>());
        }
        else
        if (tile.linkAmount-1 < lines.Count)
        {
            for (int i = tile.linkAmount; i < lines.Count; i++)
            {
                lines[i].positionCount = 0;
            }
        }

        int lineIndex = 0;
        for (int i = 0; i < tile.flowOut.Count; i++)
        {
            lines[lineIndex].positionCount = 2;
            lines[lineIndex].SetPosition(0, tile.worldPos);
            lines[lineIndex].SetPosition(1, tile.worldPos + HalfDir(tile.flowOut[i]));
            lineIndex++;
        }
        for (int i = 0; i < tile.flowIn.Count; i++)
        {
            lines[lineIndex].positionCount = 2;
            lines[lineIndex].SetPosition(0, tile.worldPos);
            lines[lineIndex].SetPosition(1, tile.worldPos + HalfDir(tile.flowIn[i]));
            lineIndex++;
        }
    }

    public Vector3 HalfDir(Direction dir)
    {
       return new Vector3(dir.dirValue.x, dir.dirValue.y, 0) * grid.cellSize * 0.5f;
    }

    private void OnDrawGizmos()
    {
        Vector3 vec;
        for (int i = 0; i < tile.flowOut.Count; i++)
        {
            vec = new Vector3(tile.flowOut[i].dirValue.x, tile.flowOut[i].dirValue.y);
            Debug.DrawRay(tile.worldPos, vec,Color.cyan);
        }
        UpdateTileColor();
    }
}
