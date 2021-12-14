using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    public bool newZone;

    private void Start()
    {
        gameGrid = GameObject.Find("Grid").GetComponent<GameGrid>();
        //Debug.Log(gameGrid.tiles);
        tileScores = new List<TileInfoScore>();
        elementHandler = GameObject.Find("Element").GetComponent<ElementHandler>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EvaluateTiles();
        }
    }

    public void SpawnPlant()
    {
        EvaluateTiles();
        for (int i = this.tileScores.Count - 1; i>0;i--)
        {
            //Debug.Log(tileScores[i].spawn);
            //Debug.Log(tileScores[i].score);
            if (this.tileScores[i].spawn)
            {
                //Debug.Log(this.tileScores[i].tile.gridPos);
                elementHandler.SpawnPlantAt(this.tileScores[i].tile.gridPos);
                break;
            }
        }
    }

    public void EvaluateTiles()
    {
        //tileScores = new List<TileInfoScore>();
        tileScores.Clear();
        //Debug.Log(gameGrid.tiles.Length);
        foreach(GameTile iTile in gameGrid.tiles)
        {
            //Debug.Log(iTile);
            TileInfoScore tmp = iTile.spawnScore.Evaluate();
            if(tmp.spawn)
            {
                tileScores.Add(tmp);
            }
        }
        tileScores = tileScores.OrderBy(e => e.score).ToList();
        //tileScores.Sort(SortByScore);
        //QuickSort(tileScores, 0, tileScores.Count - 1);
    }

    static int SortByScore(TileInfoScore t1, TileInfoScore t2)
    {
        return t1.score.CompareTo(t2.score);
    }

    //public void QuickSort(List<TileInfoScore> list, int low, int high)
    //{
    //    if(low < high)
    //    {
    //        int pi = Partition(list, low, high);
    //        QuickSort(list, low, pi - 1);
    //        QuickSort(list, pi + 1, high);
    //    }
    //}

    //private void Swap(List<TileInfoScore> list, int idxA, int idxB)
    //{
    //    TileInfoScore tmp = list[idxA];
    //    list[idxA] = list[idxB];
    //    list[idxB] = tmp;
    //}

    //private int Partition(List<TileInfoScore> list, int low, int high)
    //{
    //    int pivot = list[high].score;
    //    int i = low - 1;
    //    for(int j = low; j<=high-1; j++)
    //    {
    //        if(list[j].score < pivot)
    //        {
    //            i++;
    //            Swap(list, i, j);
    //        }
    //    }
    //    Swap(list, i + 1, high);
    //    return i + 1;
    //}
}
