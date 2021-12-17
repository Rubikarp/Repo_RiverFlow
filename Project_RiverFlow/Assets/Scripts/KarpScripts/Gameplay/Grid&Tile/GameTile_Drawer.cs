using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class GameTile_Drawer : MonoBehaviour
{
    public GameTile tile;
    public SpriteRenderer rend;

    [Header("Ground")]
    [SerializeField] TilePalette_SCO palette;

    [ContextMenu("Start")]
    private void Start()
    {
        tile = gameObject.GetComponent<GameTile>();
        rend = gameObject.GetComponent<SpriteRenderer>();
        UpdateTileColor();
    }

    [Button("Reeboot")]
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

            case TileType.mountain:
                rend.color = palette.mountain;
                break;

            default:
                rend.color = palette.errorMat;
                break;
        }
    }
}
