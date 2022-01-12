using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerVisual : MonoBehaviour
{
    public PlantSprites_SCO visuals;
    public SpriteRenderer renderer;
    public TileType type = TileType.grass;

    void Start()
    {
        GenerateVisual();
    }

    public void GenerateVisual()
    {
        switch (type)
        {
            case TileType.grass:
                renderer.sprite = visuals.grass_plantGrowth.flowers.Random();
                break;
            case TileType.clay:
                renderer.sprite = visuals.clay_plantGrowth.flowers.Random();
                break;
            case TileType.sand:
                renderer.sprite = visuals.sand_plantGrowth.flowers.Random();
                break;
            default:
                break;
        }
    }
}
