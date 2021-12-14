using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile_Drawer : MonoBehaviour
{
    public GameTile tile;
    public SpriteRenderer rend;
    public GameGrid grid;

    [Header("Ground")]
    [SerializeField] TilePalette_SCO palette;

    [ContextMenu("Start")]
    private void Start()
    {
        grid = GameGrid.instance;
        tile = gameObject.GetComponent<GameTile>();
        rend = gameObject.GetComponent<SpriteRenderer>();

        UpdateTileColor();
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
    }

    private void OnDrawGizmos()
    {
        //UpdateTileColor();
    }
}
