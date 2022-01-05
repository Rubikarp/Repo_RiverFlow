using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum ThreatState
{
    CALM,
    NEUTRAL,
    CHAOTIC
}

public class PlantSpawner : MonoBehaviour
{
    [Header("Scores & Weights")]
    [Header("Terrain Type")]
    public int scoreCalmTerrainIsGrass = 5;
    public int scoreCalmTerrainIsClay = 3;
    public int scoreCalmTerrainIsSand = 1;
    public int scoreNeutralTerrainIsGrass = 5;
    public int scoreNeutralTerrainIsClay = 3;
    public int scoreNeutralTerrainIsSand = 1;
    public int scoreChaoticTerrainIsGrass = 5;
    public int scoreChaoticTerrainIsClay = 3;
    public int scoreChaoticTerrainIsSand = 1;
    public int weightTerrainType = 1;
    [Space(8)]
    [Header("Plant Nearby")]
    public int scoreCalmPlantsFar = 2;
    public int scoreCalmPlantsClose = -1;
    public int scoreNeutralPlantsFar = 2;
    public int scoreNeutralPlantsClose = -1;
    public int scoreChaoticPlantsFar = 2;
    public int scoreChaoticPlantsClose = -1;
    public int weightPlantNearby = 1;
    [Space(8)]
    [Header("River Nearby")]
    public int scoreCalmRiverFar = 2;
    public int scoreCalmRiverClose = -1;
    public int scoreNeutralRiverFar = 2;
    public int scoreNeutralRiverClose = -1;
    public int scoreChaoticRiverFar = 2;
    public int scoreChaoticRiverClose = -1;
    public int weightRiverNearby = 1;
    [Space(8)]
    [Header("Mountains Nearby")]
    public int scoreMountainsNearby = 1;
    public int weightMountainsNearby = 1;

    [Space(8)]
    [Header("Noise")]
    public int scoreRangeNoise = 4;
    [Space(10)]
    public int currentSpawnArea = 1;
    public ThreatState threatState = ThreatState.NEUTRAL;
    [Space(10)]
    public Element plantGameObject;

    private GameGrid gameGrid;
    private List<TileInfoScore> tileScores;
    private ElementHandler elementHandler;
    public bool newZone;

    public InventoryManager inventory;
    private int difficultyScore;
    private ThreatState lastThreastState;
    public GameTime gametime;

    [Header("Difficulty Score")]
    public float digNumberMultiplier;
    public int nonIrrigatedPlantScore;
    public int deadPlantScore;
    public int lastThreatStateCalmScore;
    public int lastThreatStateNeutralScore;
    public int lastThreatStateChaoticScore;
    public int difficultyScoreToPassInNeutral;
    public int difficultyScoreToPassInChaotic;

    [Header("First Plant")]
    bool isFirstPlant = true;
    public List<GameObject> firstPlants;
    int randomFirstPlant;

    [Header("DistanceGrid")]
    public DistanceField distanceField;

    private void Start()
    {
        gameGrid = GameObject.Find("Grid").GetComponent<GameGrid>();
        tileScores = new List<TileInfoScore>();
        elementHandler = GameObject.Find("Element").GetComponent<ElementHandler>();
    }

    public void SpawnPlant()
    {
        if(isFirstPlant == true)
        {
            randomFirstPlant = Random.Range(0, firstPlants.Count);
            firstPlants[randomFirstPlant].SetActive(true);
            isFirstPlant = false;
        }
        else
        {
            if(distanceField != null)
            {
                distanceField.GenerateArray();
            }
            DifficultyScoreCalcul();
            EvaluateTiles();
            for (int i = tileScores.Count - 1; i > 0; i--)
            {
                if (tileScores[i].spawn)
                {
                    elementHandler.SpawnPlantAt(tileScores[i].tile.gridPos);
                    break;
                }
            }
        }
    }

    public void EvaluateTiles()
    {
        tileScores.Clear();
        foreach(GameTile iTile in gameGrid.tiles)
        {
            TileInfoScore tmp = iTile.spawnScore.Evaluate();
            if(tmp.spawn)
            {
                tileScores.Add(tmp);
            }
        }
        tileScores = tileScores.OrderBy(e => e.score).ToList();
    }

    private void DifficultyScoreCalcul()
    {
        lastThreastState = threatState;

        difficultyScore = 0;

        difficultyScore = (int) (inventory.digAmmount * digNumberMultiplier);

        foreach (Plant plant in elementHandler.allPlants)
        {
            if(plant.currentState == PlantState.Baby || plant.currentState == PlantState.Agony)
            {
                difficultyScore -= nonIrrigatedPlantScore;
            }
            if (plant.hasDiedRecently == true)
            {
                difficultyScore -= deadPlantScore;
                plant.hasDiedRecently = false;
            }
        }

        if(lastThreastState == ThreatState.CALM)
        {
            difficultyScore += lastThreatStateCalmScore;
        }
        else if (lastThreastState == ThreatState.NEUTRAL)
        {
            difficultyScore -= lastThreatStateNeutralScore;
        }
        else if (lastThreastState == ThreatState.CHAOTIC)
        {
            difficultyScore -= lastThreatStateChaoticScore;
        }

        ChooseThreadState();
    }

    private void ChooseThreadState()
    {
        if(difficultyScore < difficultyScoreToPassInNeutral)
        {
            threatState = ThreatState.CALM;
        }
        if (difficultyScoreToPassInNeutral <= difficultyScore)
        {
            threatState = ThreatState.NEUTRAL;
        }
        if (difficultyScoreToPassInChaotic <= difficultyScore)
        {
            threatState = ThreatState.CHAOTIC;
        }
        //Debug.Log(threatState);
    }
}
