using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitTree : MonoBehaviour
{
    public Plant plantScript;
    private bool transformed = false;
    public GameTile[] closeTiles;

    private float fruitTimer;
    public float fruitSpawnTime = 20;

    private bool [] irrigatedNeighbors;
    private int goodTilesForFruit;
    private int chosenTileForFruit;

    public GameObject plantToSpawn;
    public ElementHandler elementHandler;

    private bool spawnTileFound = false;
    public int closeRiverTilesNeeded = 3;

    private TimeManager gameTime;

    private void Awake()
    {
        gameTime = TimeManager.Instance;

        closeTiles = plantScript.TileOn.neighbors;
        irrigatedNeighbors = new bool[closeTiles.Length];

        for (int x = 0; x < irrigatedNeighbors.Length; x++)
        {
            irrigatedNeighbors[x] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transformed == true)
        {
            FruitSpawn();
        }

        if (plantScript.currentState == PlantState.Senior && transformed == false)
        {
            FruitTreeTransformation();
        }

        if (transformed == true && plantScript.currentState != PlantState.Senior)
        {
            transformed = false;

            //Mettre ici les changements de graph. 
        }
    }

    private void FruitTreeTransformation()
    {
        if (plantScript.closeRivers.Count >= closeRiverTilesNeeded)
        {
            transformed = true;

            //Mettre ici les changements de graph.

            //Debug.Log("Evolution !");
        }
    }

    private void FruitSpawn()
    {
        fruitTimer += gameTime.DeltaSimulTime;

        if (fruitTimer >= fruitSpawnTime)
        {
            for (int a = 0; a < closeTiles.Length; a++)
            {
                if (closeTiles[a].linkAmount == 0 && closeTiles[a].element == null)
                {
                    for (int b = 0; b < closeTiles[a].neighbors.Length; b++)
                    {
                        if (closeTiles[a].neighbors[b].ReceivedFlow() >= FlowStrenght._25_)
                        {
                            irrigatedNeighbors[a] = true;

                            goodTilesForFruit++;
                        }
                    }
                }
            }

            if (goodTilesForFruit > 0)
            {
                while (spawnTileFound == false)
                {
                    chosenTileForFruit = Random.Range(0, closeTiles.Length);

                    if (irrigatedNeighbors[chosenTileForFruit] == true)
                    {
                        spawnTileFound = true;
                    }
                }
                Debug.Log("spawn : " + plantScript.TileOn.type.ToString());
                switch (plantScript.TileOn.type)
                {
                    case TileType.grass:
                        
                        plantScript.fruitDropForest.Play(true);
                        break;
                    case TileType.clay:
                        
                        plantScript.fruitDropSavanna.Play(true);
                        break;
                    case TileType.sand:
                        
                        plantScript.fruitDropDesert.Play(true);
                        break;
                    default:
                        Debug.Log("default");
                        break;
                }
                elementHandler.SpawnPlantAt(closeTiles[chosenTileForFruit].gridPos);
                
                spawnTileFound = false;
                fruitTimer = 0;
                goodTilesForFruit = 0;
            }

            for (int y = 0; y < irrigatedNeighbors.Length; y++)
            {
                irrigatedNeighbors[y] = false;
            }
        }
    }

    private bool isValidSpawnTarget(GameTile targetTile)
    {
        bool targetIsValid = false; 

        switch (targetTile.type)
        {
            case TileType.mountain:

                targetIsValid = false;

                break;

            case TileType.grass:

                if (targetTile.ReceivedFlow() >= FlowStrenght._25_)
                {
                    targetIsValid = true;
                }
                else
                {
                    targetIsValid = false;
                }

                break;

            case TileType.clay:

                if (targetTile.ReceivedFlow() >= FlowStrenght._50_)
                {
                    targetIsValid = true;
                }
                else
                {
                    targetIsValid = false;
                }

                break;

            case TileType.sand:

                if (targetTile.ReceivedFlow() >= FlowStrenght._75_)
                {
                    targetIsValid = true;
                }
                else
                {
                    targetIsValid = false;
                }

                break;

            case TileType.other:

                targetIsValid = false;

                break;
        }

        return targetIsValid;
    }
}
