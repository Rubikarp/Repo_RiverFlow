using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawnScore : MonoBehaviour
{
    public int scoreValue = 0;
    public bool spawnable = true;
    
    private GameTile tile;
    private PlantSpawner plantSpawner;

    private void Start()
    {
        tile = GetComponent<GameTile>();
        plantSpawner = GetComponent<PlantSpawner>();
    }

    public void Evaluate()
    {
        spawnable = true;
        scoreValue = 0;

        scoreValue += EvalTerrainType() * CastThreatToInt(1,0);
        scoreValue += EvalSpawnArea() * CastThreatToInt(1, 0);
        scoreValue += EvalPlantsNearby() * CastThreatToInt(1, 0);
        scoreValue += EvalMountainsNearby() * CastThreatToInt(1, 0);
        scoreValue += EvalIrrigatedTile() * CastThreatToInt(1, 0);

        spawnable = EvalForbiddenCase();
    }

    private int CastThreatToInt()
    {
        int castInt = 1;
        switch(plantSpawner.threatState)
        {
            case ThreatState.PEACEFUL:
                castInt = 1;
                break;
            case ThreatState.CALM:
                castInt = 2;
                break;
            case ThreatState.NEUTRAL:
                castInt = 3;
                break;
            case ThreatState.THREATENING:
                castInt = 4;
                break;
            case ThreatState.CHAOTIC:
                castInt = 5;
                break;
            default:
                castInt = 3;
                break;
        }
        return castInt;
    }

    private int CastThreatToInt(int factor, int offset)
    {
        return (CastThreatToInt() * factor) + offset;
    }

    private int EvalTerrainType()
    {
        int ruleScore;
        switch(tile.type)
        {
            case TileType.grass:
                ruleScore = plantSpawner.scoreTerrainIsGrass;
                break;
            case TileType.clay:
                ruleScore = plantSpawner.scoreTerrainIsClay;
                break;
            case TileType.sand:
                ruleScore = plantSpawner.scoreTerrainIsSand;
                break;
            case TileType.other:
                ruleScore = plantSpawner.scoreTerrainIsOther;
                break;
            default:
                ruleScore = plantSpawner.scoreTerrainIsOther;
                break;
        }
        ruleScore *= plantSpawner.weightTerrainType;
        return ruleScore;
    }

    private int EvalSpawnArea()
    {
        int ruleScore = 0;
        /* tile.spawnArea is not defined yet
         * Uncomment this when implemented to use "area spawning" in generation scoring
        
        if(tile.spawnArea == plantSpawner.currentSpawnArea)
        {
            ruleScore = plantSpawner.scoreGoodSpawnArea;
        }
        else
        {
            ruleScore = plantSpawner.scoreBadSpawnArea;
        }
        ruleScore *= plantSpawner.weightSpawnArea;

        */
        return ruleScore;
    }

    // TODO
    private int EvalPlantsNearby()
    {
        int ruleScore = 0;
        
        return ruleScore;
    }

    // TODO
    private int EvalMountainsNearby()
    {
        int ruleScore = 0;
        
        return ruleScore;
    }

    // TODO
    private int EvalIrrigatedTile()
    {
        int ruleScore = 0;

        return ruleScore;
    }

    private bool EvalForbiddenCase()
    {
        bool boolOutput = false;
        boolOutput |= AreAllTilesAroundOccupied();
        boolOutput |= IsMountain();
        boolOutput |= IsDugged();
        boolOutput |= HasSprout();
        boolOutput |= HasSource();
        boolOutput |= IsNextToSource();
        boolOutput |= IsNextToLake();
        boolOutput |= IsNextToThreeSproutNearby();

        return boolOutput;
    }

    // TODO : Change TileType.other into "Mountains" when implemented
    private bool AreAllTilesAroundOccupied()
    {
        bool output = false;
        foreach(GameTile iTile in tile.neighbors)
        {
            output |= (iTile.isDuged || (iTile.type == TileType.other));
        }
        return output;
    }

    // TODO : Change TileType.other into "Mountains" when implemented
    private bool IsMountain()
    {
        return (tile.type == TileType.other);
    }

    private bool IsDugged()
    {
        return tile.isDuged;
    }

    private bool HasSprout()
    {
        return (tile.element is Plant);
    }

    private bool HasSource()
    {
        return (tile.element is WaterSource);
    }

    private bool IsNextToSource()
    {
        bool output = false;
        foreach (GameTile iTile in tile.neighbors)
        {
            output |= (iTile.element is WaterSource);
        }
        return output;
    }

    // TODO
    private bool IsNextToLake()
    {
        return false;
    }

    // TODO
    private bool IsNextToThreeSproutNearby()
    {
        return false;
    }
}
