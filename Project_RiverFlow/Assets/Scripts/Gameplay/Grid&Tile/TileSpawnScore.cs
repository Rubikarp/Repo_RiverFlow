using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfoScore
{
    public GameTile tile;
    public int score;
    public bool spawn;

    public TileInfoScore(GameTile tile, int score, bool spawnable)
    {
        this.tile = tile;
        this.score = score;
        this.spawn = spawnable;
    }
}

public class TileSpawnScore : MonoBehaviour
{
    public int scoreValue = 0;
    public bool spawnable = false;
    
    private GameTile tile;
    private PlantSpawner plantSpawner;

    private void Start()
    {
        tile = GetComponent<GameTile>();
        plantSpawner = GameObject.Find("PlantSpawner").GetComponent<PlantSpawner>();
    }

    public TileInfoScore Evaluate()
    {

        spawnable = false;
        scoreValue = 0;

        if (tile.spawnArea <= plantSpawner.currentSpawnArea && tile.spawnArea != 0)
        {

            //Debug.Log("evaluate");
            if (plantSpawner.newZone == true)
            {
                scoreValue += EvalSpawnArea() * CastThreatToInt(1, 0) + 10000;
                Debug.Log("Le calcul se fait bien");
            }

            scoreValue += EvalTerrainType() * CastThreatToInt(1, 0);
            scoreValue += EvalPlantsNearby() * CastThreatToInt(1, 0);
            scoreValue += EvalMountainsNearby() * CastThreatToInt(1, 0);
            scoreValue += EvalIrrigatedTile() * CastThreatToInt(1, 0);
            scoreValue += EvalNoise();
            spawnable = EvalForbiddenCase();

        }

        return new TileInfoScore(tile, scoreValue, spawnable);
    }

    private int CastThreatToInt()
    {
        int castInt = 1;
        switch(plantSpawner.threatState)
        {
            case ThreatState.CALM:
                castInt = 1;
                break;
            case ThreatState.NEUTRAL:
                castInt = 2;
                break;
            case ThreatState.CHAOTIC:
                castInt = 3;
                break;
            default:
                castInt = 2;
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

        if (tile.spawnArea == plantSpawner.currentSpawnArea)
        {
            ruleScore = plantSpawner.scoreGoodSpawnArea;
        }
        else
        {
            ruleScore = plantSpawner.scoreBadSpawnArea;
        }
        ruleScore *= plantSpawner.weightSpawnArea;

        return ruleScore;
    }

    private int EvalPlantsNearby()
    {
        int ruleScore = 0;
        foreach(GameTile iTile in tile.neighbors)
        {
            if(iTile != null)
            {
                if (iTile.element is Plant)
                {
                    ruleScore += plantSpawner.scorePlantsNearby;
                }
            }
        }
        ruleScore *= plantSpawner.weightPlantNearby;
        return ruleScore;
    }

    private int EvalMountainsNearby()
    {
        int ruleScore = 0;
        foreach (GameTile iTile in tile.neighbors)
        {
            if (iTile != null)
            {
                if (iTile.type != TileType.mountain)
                {
                    ruleScore += plantSpawner.scoreMountainsNearby;
                }
            }
        }
        ruleScore *= plantSpawner.weightMountainsNearby;
        return ruleScore;
    }

    private int EvalIrrigatedTile()
    {
        int ruleScore = 0;
        foreach (GameTile iTile in tile.neighbors)
        {
            if (iTile != null)
            {
                switch (iTile.riverStrenght)
                {
                    case FlowStrenght._100_:
                        ruleScore = plantSpawner.scoreIrrigatedTile100;
                        break;
                    case FlowStrenght._75_:
                        ruleScore = ruleScore > plantSpawner.scoreIrrigatedTile75 ? ruleScore : plantSpawner.scoreIrrigatedTile75;
                        break;
                    case FlowStrenght._50_:
                        ruleScore = ruleScore > plantSpawner.scoreIrrigatedTile50 ? ruleScore : plantSpawner.scoreIrrigatedTile50;
                        break;
                    case FlowStrenght._25_:
                        ruleScore = ruleScore > plantSpawner.scoreIrrigatedTile25 ? ruleScore : plantSpawner.scoreIrrigatedTile25;
                        break;
                    case FlowStrenght._00_:
                        ruleScore = ruleScore > plantSpawner.scoreIrrigatedTile0 ? ruleScore : plantSpawner.scoreIrrigatedTile0;
                        break;
                    default:
                        ruleScore = ruleScore > plantSpawner.scoreIrrigatedTile0 ? ruleScore : plantSpawner.scoreIrrigatedTile0;
                        break;
                }
            }
        }
        ruleScore *= plantSpawner.weightIrrigatedTile;
        return ruleScore;
    }

    private int EvalNoise()
    {
        return Random.Range(-4, 4);
    }

    // IsNextToThreeSproutNearby commented : Uncomment when index are fixed
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
        //boolOutput |= IsNextToThreeSproutNearby();

        return !boolOutput;
    }

    private bool AreAllTilesAroundOccupied()
    {
        bool output = false;
        foreach(GameTile iTile in tile.neighbors)
        {
            if (iTile != null)
            {
                output |= (iTile.isDuged || (iTile.type == TileType.mountain));
            }
        }
        return output;
    }

    private bool IsMountain()
    {
        return (tile.type == TileType.mountain);
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
            if (iTile != null)
            {
                output |= (iTile.element is WaterSource);
            }
        }
        return output;
    }

    private bool IsNextToLake()
    {
        bool output = false;
        foreach (GameTile iTile in tile.neighbors)
        {
            if (iTile != null)
            {
                output |= (iTile.element is Lake);
            }
        }
        return output;
    }

    private bool IsNextToThreeSproutNearby()
    {
        bool output = false;
        for(int i=0;i<=8;i+=2)
        {
            if ((tile.neighbors[(i - 1) % 8].element is Plant) && (tile.neighbors[i % 8].element is Plant) && (tile.neighbors[(i + 1) % 8].element is Plant))
            {
                output |= true;
            }
            else
            {
                output |= false;
            }
        }
        return output;
    }
}