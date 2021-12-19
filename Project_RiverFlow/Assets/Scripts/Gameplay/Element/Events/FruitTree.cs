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

    private GameTime gameTime;

    private void Awake()
    {
        gameTime = GameTime.Instance;

        closeTiles = plantScript.tileOn.neighbors;
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

            Debug.Log("Evolution !");
        }
    }

    private void FruitSpawn()
    {
        fruitTimer += Time.deltaTime * gameTime.gameTimeSpeed;

        if (fruitTimer >= fruitSpawnTime)
        {
            for (int a = 0; a < closeTiles.Length; a++)
            {
                if (closeTiles[a].linkAmount == 0 && closeTiles[a].element == null)
                {
                    for (int b = 0; b < closeTiles[a].neighbors.Length; b++)
                    {
                        if (closeTiles[a].neighbors[b].receivedFlow >= FlowStrenght._25_)
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
}
