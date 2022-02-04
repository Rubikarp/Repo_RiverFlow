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
    private Camera mainCamera;
    private PlantSpawner plantSpawner;
    private DistanceField distanceField;
    private int spawnMargin;
    public int occupiedTilesLimit = 4;

    private void Start()
    {
        tile = GetComponent<GameTile>();
        mainCamera = Camera.main;
        plantSpawner = GameObject.Find("PlantSpawner").GetComponent<PlantSpawner>();
        distanceField = GameObject.Find("PlantSpawner").GetComponent<DistanceField>();
    }

    public TileInfoScore Evaluate()
    {

        spawnable = false;
        scoreValue = 0;

        scoreValue += EvalTerrainType();
        scoreValue += EvalPlantsNearby();
        scoreValue += EvalRiverNearby();
        scoreValue += EvalMountainsNearby();
        scoreValue += EvalNoise();
        spawnable = EvalForbiddenCase();

        return new TileInfoScore(tile, scoreValue, spawnable);
    }

    private int EvalTerrainType()
    {
        int ruleScore = 0;
        switch (plantSpawner.threatState)
        {
            case ThreatState.CALM:
                switch (tile.type)
                {
                    case TileType.grass:
                        ruleScore = plantSpawner.scoreCalmTerrainIsGrass;
                        break;
                    case TileType.clay:
                        ruleScore = plantSpawner.scoreCalmTerrainIsClay;
                        break;
                    case TileType.sand:
                        ruleScore = plantSpawner.scoreCalmTerrainIsSand;
                        break;
                    default:
                        break;
                }
                break;

            case ThreatState.NEUTRAL:
                switch (tile.type)
                {
                    case TileType.grass:
                        ruleScore = plantSpawner.scoreNeutralTerrainIsGrass;
                        break;
                    case TileType.clay:
                        ruleScore = plantSpawner.scoreNeutralTerrainIsClay;
                        break;
                    case TileType.sand:
                        ruleScore = plantSpawner.scoreNeutralTerrainIsSand;
                        break;
                    default:
                        break;
                }
                break;

            case ThreatState.CHAOTIC:
                switch (tile.type)
                {
                    case TileType.grass:
                        ruleScore = plantSpawner.scoreChaoticTerrainIsGrass;
                        break;
                    case TileType.clay:
                        ruleScore = plantSpawner.scoreChaoticTerrainIsClay;
                        break;
                    case TileType.sand:
                        ruleScore = plantSpawner.scoreChaoticTerrainIsSand;
                        break;
                    default:
                        break;
                }
                break;

            default:
                break;
        }
        ruleScore *= plantSpawner.weightTerrainType;
        return ruleScore;
    }

    private int EvalPlantsNearby()
    {
        int ruleScore = 0;
        int distanceValue = distanceField.treeArray[tile.gridPos.x, tile.gridPos.y];
        switch (plantSpawner.threatState)
        {
            case ThreatState.CALM:
                if(distanceValue <= 2)
                {
                    ruleScore = plantSpawner.scoreCalmPlantsClose;
                }
                else
                {
                    ruleScore = plantSpawner.scoreCalmPlantsFar;
                }
                break;

            case ThreatState.NEUTRAL:
                if (distanceValue >= 4)
                {
                    ruleScore = plantSpawner.scoreNeutralPlantsFar;
                }
                else
                {
                    ruleScore = plantSpawner.scoreNeutralPlantsClose;
                }
                break;

            case ThreatState.CHAOTIC:
                if (distanceValue >= 5)
                {
                    ruleScore = plantSpawner.scoreChaoticPlantsFar;
                }
                else
                {
                    ruleScore = plantSpawner.scoreChaoticPlantsClose;
                }
                break;

            default:
                break;
        }
        ruleScore *= plantSpawner.weightPlantNearby;
        return ruleScore;
    }

    private int EvalRiverNearby()
    {
        int ruleScore = 0;
        int distanceValue = distanceField.riverArray[tile.gridPos.x, tile.gridPos.y];
        switch (plantSpawner.threatState)
        {
            case ThreatState.CALM:
                if (2 <= distanceValue && distanceValue <= 3)
                {
                    ruleScore = plantSpawner.scoreCalmRiverClose;
                }
                else
                {
                    ruleScore = plantSpawner.scoreCalmRiverFar;
                }
                break;

            case ThreatState.NEUTRAL:
                if (distanceValue >= 4)
                {
                    ruleScore = plantSpawner.scoreNeutralRiverFar;
                }
                else
                {
                    ruleScore = plantSpawner.scoreNeutralRiverClose;
                }
                break;

            case ThreatState.CHAOTIC:
                if (distanceValue >= 5)
                {
                    ruleScore = plantSpawner.scoreChaoticRiverFar;
                }
                else
                {
                    ruleScore = plantSpawner.scoreChaoticRiverClose;
                }
                break;

            default:
                break;
        }
        ruleScore *= plantSpawner.weightRiverNearby;
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

    private int EvalNoise()
    {
        return Random.Range(-plantSpawner.scoreRangeNoise, plantSpawner.scoreRangeNoise);
    }

    private bool EvalForbiddenCase()
    {
        bool boolOutput = false;
        boolOutput |= IsOutsideOfCamera();
        boolOutput |= AreAllTilesAroundOccupied();
        boolOutput |= IsMountain();
        boolOutput |= IsDugged();
        boolOutput |= HasSprout();
        boolOutput |= HasSource();
        boolOutput |= IsNextToSource();
        boolOutput |= IsNextToLake();

        return !boolOutput;
        
    }

    private bool IsOutsideOfCamera()
    {
        bool output = false;
        spawnMargin = plantSpawner.marginValue;

        int maxX = (int)Mathf.Floor(((float)Screen.width / (float)Screen.height) * mainCamera.orthographicSize) - spawnMargin;
        int maxY = (int)Mathf.Floor(mainCamera.orthographicSize) - spawnMargin;

        if(!(-maxX <= tile.worldPos2D.x && tile.worldPos2D.x <= maxX) || !(-maxY <= tile.worldPos2D.y && tile.worldPos2D.y <= maxY))
        {
            output = true;
        }

        return output;
    }

    private bool AreAllTilesAroundOccupied()
    {
        int blockedNeighbors = 0;
        bool output = false;
        foreach(GameTile iTile in tile.neighbors)
        {
            if (iTile != null)
            {
                if (iTile.type == TileType.mountain || iTile.element != null)
                {
                    blockedNeighbors++;
                }
                if (iTile.isDuged)
                {
                    output = true;
                }
            }

            if (blockedNeighbors > occupiedTilesLimit)
            {
                output = true;
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
}