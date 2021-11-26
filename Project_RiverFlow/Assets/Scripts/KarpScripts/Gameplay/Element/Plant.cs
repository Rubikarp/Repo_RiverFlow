using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class BoolEvent : UnityEvent<bool>
{ }

public class Plant : Element
{
    #region Element
    [Header("Element Data")]
    public GameTile tileOn;
    public override GameTile TileOn
    {
        get
        {
            if (tileOn == null)
            {
                Debug.LogError("Can't find the tile where is this WaterSource", this);
                return null ;
            }
            return tileOn;
        }
        set
        { tileOn = value; }
    }
    public override GameTile[] TilesOn
    {
        get
        {
            if (tileOn == null)
            {
                Debug.LogError("Can't find the tile where is this WaterSource", this);
                return new GameTile[1] { null };
            }
            return new GameTile[1] { tileOn };
        }
        set { tileOn = value[0]; }
    }
    //Don't overide other methode because is not linkable
    #endregion

    [Header("Plant Data")]
    public PlantState currentState = PlantState.Young;
    public bool isIrrigated = false;
    private bool IsAlive { get { return currentState != PlantState.Dead; } }

    public List<int> closeRivers;
    private FlowStrenght bestRiverStrenght = 0;
    
    [Header("Living")]
    [Range(0f,1f)] public float timer = 1.0f;
    public float stateUpgradeTime = 60f;
    public float stateDowngradeTime = 15f;

    [Header("FruitTree")]
    private bool isFruitTree = false;
    private float fruitTimer;
    public float fruitSpawnTime = 20;
    private bool[] irrigatedNeighbors;
    private int goodTilesForFruit;
    private int chosenTileForFruit;
    public ElementHandler elementHandler;
    private bool spawnTileFound = false;
    public int closeRiverTilesNeeded = 3;

    [Header("MagicTree")]
    private int plantsForMagicTree = 0;
    public GameObject magicTree;

    [Header("Event")]
    public BoolEvent onStateChange;

    public GameTime gameTime;

    private void Start()
    {
        gameTime = GameTime.Instance;

        if (tileOn.isElement)
        {
            tileOn.element = this;
        }

        irrigatedNeighbors = new bool[tileOn.neighbors.Length];

        for (int x = 0; x < irrigatedNeighbors.Length; x++)
        {
            irrigatedNeighbors[x] = false;
        }
    }

    void Update()
    {
        if (IsAlive && !gameTime.isPaused)
        {
            CheckNeighboringRivers();
            StateUpdate();

            if (isFruitTree == true)
            {
                FruitSpawn();
            }

            if (currentState >= PlantState.Young)
            {
                MagicTreeVerif();
            }
        }
    }

    private void CheckNeighboringRivers()
    {
        //Cherche all irrigated Neighbor
        for (int i = 0; i < tileOn.neighbors.Length; i++)
        {
            if (tileOn.neighbors[i].isRiver)
            {
                if (!closeRivers.Contains(i))
                {
                    closeRivers.Add(i);
                }
            }
            else
            //if (!tileOn.neighbors[i].isRiver)
            {
                if (closeRivers.Contains(i))
                {
                    closeRivers.Remove(closeRivers.IndexOf(i));
                }
            }
        }

        //Cherche best river Strenght
        bestRiverStrenght = 0;
        for (int j = 0; j < closeRivers.Count; j++)
        {
            if (tileOn.neighbors[closeRivers[j]].riverStrenght > bestRiverStrenght)
            {
                bestRiverStrenght = tileOn.neighbors[closeRivers[j]].riverStrenght;
            }
        }

        //Determine if irrigated
        isIrrigated = tileOn.IsIrrigate;
    }

    private void StateUpdate()
    {
        if (isIrrigated)
        {
            timer += Time.deltaTime * (1 / stateUpgradeTime) * gameTime.gameTimeSpeed;
        }
        else
        {
            timer -= Time.deltaTime * (1 / stateDowngradeTime) * gameTime.gameTimeSpeed;
        }

        //Lvl Drop
        if (timer < 0)
        {
            timer += 1f;
            currentState = (PlantState)Mathf.Clamp((int)(currentState - 1), 0, (int)PlantState.Senior);
            onStateChange?.Invoke(false);
            //Debug.Log("testcridown");

            if (isFruitTree == true)
            {
                isFruitTree = false;
            }
        }
        else
        //Lvl Up
        if (timer > 1)
        {
            //Si pas au niveau max
            if (currentState < PlantState.Senior)
            {
                //Debug.Log("testcriup");
                timer -= 1f;
                currentState = (PlantState)Mathf.Clamp((int)(currentState + 1), 0, (int)PlantState.Senior);
                onStateChange?.Invoke(true);

                if (closeRivers.Count >= closeRiverTilesNeeded && isFruitTree == false && currentState == PlantState.Senior)
                {
                    isFruitTree = true;

                    Debug.Log("Evolution !");
                }
            }
            else
            {
                timer = 1f;

                if (closeRivers.Count >= closeRiverTilesNeeded && isFruitTree == false)
                {
                    isFruitTree = true;

                    Debug.Log("Evolution !");
                }
            }
        }
    }

    private void FruitSpawn()
    {
        fruitTimer += Time.deltaTime * gameTime.gameTimeSpeed;

        if (fruitTimer >= fruitSpawnTime)
        {
            for (int a = 0; a < tileOn.neighbors.Length; a++)
            {
                if (tileOn.neighbors[a].linkedTile.Count == 0 && tileOn.neighbors[a].element == null)
                {
                    for (int b = 0; b < tileOn.neighbors[a].neighbors.Length; b++)
                    {
                        if (tileOn.neighbors[a].neighbors[b].receivedFlow >= FlowStrenght._25_)
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
                    chosenTileForFruit = Random.Range(0, tileOn.neighbors.Length);

                    if (irrigatedNeighbors[chosenTileForFruit] == true)
                    {
                        spawnTileFound = true;
                    }
                }

                elementHandler.SpawnPlantAt(tileOn.neighbors[chosenTileForFruit].gridPos);

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

    private void MagicTreeVerif()
    {
        if (tileOn.neighbors[3].element is Plant && tileOn.neighbors[7].element is Plant && tileOn.neighbors[1].element == null)
        {
            for (int g = 0; g < tileOn.neighbors[1].neighbors.Length; g++)
            {
                if (tileOn.neighbors[1].neighbors[g].element is Plant)
                {
                    plantsForMagicTree++;
                }
            }

            if (plantsForMagicTree == 8)
            {
                Instantiate(magicTree, tileOn.neighbors[1].worldPos, Quaternion.identity, elementHandler.transform);
            }
        }
            
    }
}
