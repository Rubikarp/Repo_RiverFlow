using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ThreatState
{
    PEACEFUL,
    CALM,
    NEUTRAL,
    THREATENING,
    CHAOTIC
}

public class PlantSpawner : MonoBehaviour
{
    [Header("Scores & Weights")]
    [Header("Terrain Type")]
    public int scoreTerrainIsGrass = 5;
    public int scoreTerrainIsClay = 3;
    public int scoreTerrainIsSand = 1;
    public int scoreTerrainIsOther = 0;
    public int weightTerrainType = 1;
    [Space(8)]
    [Header("Spawn Area")]
    public int scoreGoodSpawnArea = 3;
    public int scoreBadSpawnArea = 1;
    public int weightSpawnArea = 1;
    [Space(8)]
    [Header("Plant Nearby")]
    public int scorePlantsNearby = 1;
    public int weightPlantNearby = 1;
    [Space(8)]
    [Header("Mountains Nearby")]
    public int scoreMountainsNearby = 1;
    public int weightMountainsNearby = 1;
    public int scoreIrrigatedTile100 = 4;
    [Space(8)]
    [Header("Irrigated Tiles")]
    public int scoreIrrigatedTile75 = 4;
    public int scoreIrrigatedTile50 = 3;
    public int scoreIrrigatedTile25 = 2;
    public int scoreIrrigatedTile0 = 0;
    public int weightIrrigatedTile = 1;
    [Space(10)]
    public int currentSpawnArea = 1;
    public ThreatState threatState = ThreatState.NEUTRAL;
    [Space(10)]
    public Element plantGameObject;

    private GameGrid gameGrid;
    private List<TileInfoScore> tileScores;
    private ElementHandler elementHandler;

    private void Start()
    {
        gameGrid = GameObject.Find("Grid").GetComponent<GameGrid>();
        tileScores = new List<TileInfoScore>();
        elementHandler = GameObject.Find("Element").GetComponent<ElementHandler>();
    }

    public void SpawnPlant()
    {
        this.EvaluateTiles();
        Debug.Log(this.tileScores[0].tile.gridPos);
        elementHandler.SpawnPlantAt(this.tileScores[0].tile.gridPos);
    }

    public void EvaluateTiles()
    {
        foreach(GameTile iTile in gameGrid.tiles)
        {
            TileInfoScore tmp = iTile.spawnScore.Evaluate();
            if(tmp.spawnable)
            {
                tileScores.Add(tmp);
            }
        }
        QuickSort(tileScores, 0, tileScores.Count - 1);
    }

    public void QuickSort(List<TileInfoScore> list, int low, int high)
    {
        if(low < high)
        {
            int pi = Partition(list, low, high);
            QuickSort(list, low, pi - 1);
            QuickSort(list, pi + 1, high);
        }
    }

    private void Swap(List<TileInfoScore> list, int idxA, int idxB)
    {
        TileInfoScore tmp = list[idxA];
        list[idxA] = list[idxB];
        list[idxB] = tmp;
    }

    private int Partition(List<TileInfoScore> list, int low, int high)
    {
        int pivot = list[high].score;
        int i = low - 1;
        for(int j = low; j<=high-1; j++)
        {
            if(list[j].score < pivot)
            {
                i++;
                Swap(list, i, j);
            }
        }
        Swap(list, i + 1, high);
        return i + 1;
    }
}
