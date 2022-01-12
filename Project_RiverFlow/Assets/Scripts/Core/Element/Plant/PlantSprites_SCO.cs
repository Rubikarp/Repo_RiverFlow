using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "PlantSprites_new", menuName = "GraphData/PlantSprites")]
public class PlantSprites_SCO : ScriptableObject
{
    [Header("Grass")]
    public PlantGrowth grass_plantGrowth;
    [Header("Clay")]
    public PlantGrowth clay_plantGrowth;
    [Header("Sand")]
    public PlantGrowth sand_plantGrowth;

    public Sprite GetSprite(PlantState state, TileType type)
    {
        switch (type)
        {
            case TileType.other:
                return null;

            case TileType.grass:
                return grass_plantGrowth.StateSprite(state);

            case TileType.clay:
                return clay_plantGrowth.StateSprite(state);

            case TileType.sand:
                return sand_plantGrowth.StateSprite(state);

            default:
                return null;
        }
    }
}
