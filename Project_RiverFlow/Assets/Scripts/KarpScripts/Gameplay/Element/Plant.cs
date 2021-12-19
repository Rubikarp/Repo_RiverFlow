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
        {
            gridPos = tileOn.gridPos;
            tileOn = value; 
        }
    }
    //Don't overide other methode because is not linkable
    #endregion

    [Header("Plant Data")]
    public PlantState currentState = PlantState.Young;
    public bool isIrrigated = false;
    private bool IsAlive { get { return currentState != PlantState.Dead; } }
    public bool hasDiedRecently;

    public List<int> closeRivers;
    private FlowStrenght bestRiverStrenght = 0;
    private Plant_Drawer plantDrawer;
    
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
    private bool neighborHasFruit = false;

    [Header("MagicTree")]
    private int plantsForMagicTree = 0;
    public GameObject magicTree;
    private bool hasMagicTree = false;

    [Header("Event")]
    public BoolEvent onStateChange;

    public GameTime gameTime;

    [Header("FX")]
    public bool previousIrrigation;
    public ParticleSystem WaveIrrigate;

    private void Start()
    {
        previousIrrigation = isIrrigated;

        gameTime = GameTime.Instance;

        if (tileOn.haveElement)
        {
            tileOn.element = this;
        }

        irrigatedNeighbors = new bool[tileOn.neighbors.Length];

        for (int x = 0; x < irrigatedNeighbors.Length; x++)
        {
            irrigatedNeighbors[x] = false;
        }

        elementHandler = GetComponentInParent<ElementHandler>();
        plantDrawer = GetComponent<Plant_Drawer>();
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

            if (currentState >= PlantState.Young && hasMagicTree == false)
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

        //Determine si on vient de changer d'etat
        if(previousIrrigation != isIrrigated)
        {
            //determine si on vient d'etre irrigué
            if (isIrrigated == true)
            {
                WaveIrrigate.Play();
            }

            previousIrrigation = isIrrigated;
        }

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

            if(currentState == PlantState.Dead)
            {
                hasDiedRecently = true;
            }
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
                    neighborHasFruit = false;
                    for (int i = 0; i < tileOn.neighbors.Length; i++)
                    {
                        if (tileOn.neighbors[i].element is Plant)
                        {
                            if (tileOn.neighbors[i].element.GetComponent<Plant>().isFruitTree == true)
                            {
                                Debug.Log("FRUITS!");
                                neighborHasFruit = true;
                            }
                        }
                    }
                    if (neighborHasFruit == false)
                    {
                        isFruitTree = true;

                        currentState = (PlantState)Mathf.Clamp((int)(currentState + 1), 0, (int)PlantState.FruitTree);
                        onStateChange?.Invoke(true);

                        Debug.Log("Evolution !");
                    }
                }
            }
            else
            {
                timer = 1f;

                if (closeRivers.Count >= closeRiverTilesNeeded && isFruitTree == false)
                {
                    neighborHasFruit = false;
                    for(int i = 0; i < tileOn.neighbors.Length; i++)
                    {
                        if (tileOn.neighbors[i].element is Plant)
                        {
                            if (tileOn.neighbors[i].element.GetComponent<Plant>().isFruitTree == true)
                            {
                                Debug.Log("FRUITS!");
                                neighborHasFruit = true;
                            }
                        }
                    }
                    if (neighborHasFruit == false)
                    {
                        isFruitTree = true;

                        currentState = (PlantState)Mathf.Clamp((int)(currentState + 1), 0, (int)PlantState.FruitTree);
                        onStateChange?.Invoke(true);

                        Debug.Log("Evolution !");
                    }

                }
                else if (closeRivers.Count < closeRiverTilesNeeded && isFruitTree == true)
                {
                    isFruitTree = false;

                    currentState = (PlantState)Mathf.Clamp((int)(currentState - 1), 0, (int)PlantState.FruitTree);
                    onStateChange?.Invoke(true);
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
                if (tileOn.neighbors[a].linkAmount == 0 && tileOn.neighbors[a].element == null)
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
            if (tileOn.neighbors[3].element.GetComponent<Plant>().currentState >= PlantState.Young && tileOn.neighbors[3].element.GetComponent<Plant>().currentState >= PlantState.Young)
            {
                for (int g = 0; g < tileOn.neighbors[1].neighbors.Length; g++)
                {
                    if (tileOn.neighbors[1].neighbors[g].element is Plant)
                    {
                        if(tileOn.neighbors[1].neighbors[g].element.GetComponent<Plant>().currentState >= PlantState.Young)
                        {
                            plantsForMagicTree++;
                        }
                    }
                }
            }

            if (plantsForMagicTree == 8)
            {
                elementHandler.SpawnMagicTreeAt(tileOn.neighbors[1].gridPos);
                hasMagicTree = true;
            }
            else
            {
                plantsForMagicTree = 0;
            }
        }
            
    }
}
