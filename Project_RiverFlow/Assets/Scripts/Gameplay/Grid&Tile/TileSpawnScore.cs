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

        scoreValue += EvalTerrainType();
        scoreValue += EvalSpawnArea();
        scoreValue += EvalPlantsNearby();
        scoreValue += EvalMountainsNearby();
        scoreValue += EvalIrrigatedTile();
    }

    private int EvalTerrainType()
    {
        int ruleScore = 0;
        // Evaluate rule here
        // Modify ruleScore from that
        return ruleScore;
    }

    private int EvalSpawnArea()
    {
        int ruleScore = 0;
        // Evaluate rule here
        // Modify ruleScore from that
        return ruleScore;
    }

    private int EvalPlantsNearby()
    {
        int ruleScore = 0;
        // Evaluate rule here
        // Modify ruleScore from that
        return ruleScore;
    }

    private int EvalMountainsNearby()
    {
        int ruleScore = 0;
        // Evaluate rule here
        // Modify ruleScore from that
        return ruleScore;
    }

    private int EvalIrrigatedTile()
    {
        int ruleScore = 0;
        // Evaluate rule here
        // Modify ruleScore from that
        return ruleScore;
    }
}
