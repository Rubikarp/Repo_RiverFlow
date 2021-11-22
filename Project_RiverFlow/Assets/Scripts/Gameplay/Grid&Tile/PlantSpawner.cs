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
    public int scoreTerrainIsGrass = 5;
    public int scoreTerrainIsClay = 3;
    public int scoreTerrainIsSand = 1;
    public int scoreTerrainIsOther = 0;
    public int weightTerrainType = 1;
    public int scoreGoodSpawnArea = 1;
    public int scoreBadSpawnArea = 1;
    public int weightSpawnArea = 1;
    public int scorePlantsNearby = 1; // Unused
    public int scoreMountainsNearby = 1; // Unused
    public int scoreIrrigatedTile = 1; // Unused

    public int currentSpawnArea = 1;
    public ThreatState threatState = ThreatState.NEUTRAL;

    public void EvaluateTiles()
    {
        // For each Tiles
        // Evaluate spawning score
    }
}
