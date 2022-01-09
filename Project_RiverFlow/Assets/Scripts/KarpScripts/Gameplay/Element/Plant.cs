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
    [Header("Element Data"), SerializeField]
    private GameTile tileOn;
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
            tileOn = value; 
            gridPos = tileOn.gridPos;
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
    public ParticleSystem waveIrrigate;
    public ParticleSystem butterflyScore;
    public float timeWithoutIrrigation =0;
    public ParticleSystem fruitDropForest;
    public ParticleSystem fruitDropSavanna;
    public ParticleSystem fruitDropDesert;

    [Header("Scoring")]
    public int youngTreeScoring;
    public int adultTreeScoring;
    public int seniorTreeScoring;
    private ScoreManager scoreManager;
    public float scoringTick;
    private float scoringTimer = 0.0f;

    [Header("Flower")]
    public GameObject flowerTemplate;
    public List<GameObject> myFlower = new List<GameObject>();
    List<Collider2D> flowerSpawnAeras = new List<Collider2D>();
    public List<Collider2D> savanaFlowerSpawnAeras = new List<Collider2D>();
    public List<Collider2D> forestFlowerSpawnAeras = new List<Collider2D>();
    public List<Collider2D> desertFlowerSpawnAeras = new List<Collider2D>();
    int frontAeraLeft = 1;

    private void Start()
    {
        previousIrrigation = isIrrigated;
        scoreManager = ScoreManager.Instance;
        gameTime = GameTime.Instance;
        

        if (!TileOn.haveElement)
        {
            TileOn.element = this;
        }

        irrigatedNeighbors = new bool[TileOn.neighbors.Length];



        for (int x = 0; x < irrigatedNeighbors.Length; x++)
        {
            irrigatedNeighbors[x] = false;
        }

        elementHandler = GetComponentInParent<ElementHandler>();
        plantDrawer = GetComponent<Plant_Drawer>();

        if (TileOn.type == TileType.grass)
        {
            flowerSpawnAeras = forestFlowerSpawnAeras;
        }else if(TileOn.type == TileType.sand)
        {
            flowerSpawnAeras = desertFlowerSpawnAeras;
        }
        else if(TileOn.type == TileType.clay)
        {
            flowerSpawnAeras = savanaFlowerSpawnAeras;
        }
    }

    void Update()
    {
        if (IsAlive && !gameTime.isPaused)
        {
            CheckNeighboringRivers();
            StateUpdate();
            Scoring();
            if (isFruitTree == true)
            {
                FruitSpawn();
            }

            if (currentState >= PlantState.Young && hasMagicTree == false)
            {
                MagicTreeVerif();
            }
            if (isIrrigated == false)
            {
                timeWithoutIrrigation += Time.deltaTime * gameTime.gameTimeSpeed;
            }
        }
    }

    private void CheckNeighboringRivers()
    {
        //Cherche all irrigated Neighbor
        for (int i = 0; i < TileOn.neighbors.Length; i++)
        {
            if (TileOn.neighbors[i].isRiver)
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
            if (TileOn.neighbors[closeRivers[j]].riverStrenght > bestRiverStrenght)
            {
                bestRiverStrenght = TileOn.neighbors[closeRivers[j]].riverStrenght;
            }
        }

        //Determine if irrigated
        isIrrigated = TileOn.IsIrrigate;

        //Determine si on vient de changer d'etat
        if(previousIrrigation != isIrrigated)
        {
            //determine si on vient d'etre irrigué
            if (isIrrigated == true)
            {
                timeWithoutIrrigation = 0;
                waveIrrigate.Play();
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
                                //Debug.Log("FRUITS!");
                                neighborHasFruit = true;
                            }
                        }
                    }
                    if (neighborHasFruit == false)
                    {
                        isFruitTree = true;

                        currentState = (PlantState)Mathf.Clamp((int)(currentState + 1), 0, (int)PlantState.FruitTree);
                        onStateChange?.Invoke(true);

                        //Debug.Log("Evolution !");
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
                                //Debug.Log("FRUITS!");
                                neighborHasFruit = true;
                            }
                        }
                    }
                    if (neighborHasFruit == false)
                    {
                        isFruitTree = true;

                        currentState = (PlantState)Mathf.Clamp((int)(currentState + 1), 0, (int)PlantState.FruitTree);
                        onStateChange?.Invoke(true);

                        //Debug.Log("Evolution !");
                    }

                }
                else if (closeRivers.Count < closeRiverTilesNeeded && isFruitTree == true)
                {
                    isFruitTree = false;

                    currentState = (PlantState)Mathf.Clamp((int)(currentState - 1), 0, (int)PlantState.FruitTree);
                    onStateChange?.Invoke(true);
                }
            }
            SpawnFlower();
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
                        if (tileOn.neighbors[a].neighbors[b].ReceivedFlow() >= FlowStrenght._25_)
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
                //Debug.Log("spawn : " +TileOn.type.ToString());
                switch (TileOn.type)
                {
                    case TileType.grass:

                        fruitDropForest.Play(true);
                        break;
                    case TileType.clay:

                        fruitDropSavanna.Play(true);
                        break;
                    case TileType.sand:

                        fruitDropDesert.Play(true);
                        break;
                    default:
                        //Debug.Log("default");
                        break;
                }
                StartCoroutine(SpawnFruitTree());
                //Debug.Log("spawn");
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

    IEnumerator SpawnFruitTree()
    {
        yield return new WaitForSeconds(1);
        elementHandler.SpawnPlantAt(tileOn.neighbors[chosenTileForFruit].gridPos);
    }

    private void MagicTreeVerif()
    {
        if (tileOn.neighbors[1].element is null || tileOn.neighbors[1].element is Plant)
        {
            for (int g = 1; g < tileOn.neighbors[1].neighbors.Length; g = g + 2)
            {
                if (tileOn.neighbors[1].neighbors[g].element is Plant)
                {
                    if(tileOn.neighbors[1].neighbors[g].element.GetComponent<Plant>().currentState >= PlantState.Young)
                    {
                        plantsForMagicTree++;
                    }
                }
            }


            if (plantsForMagicTree == 4)
            {
                if (tileOn.neighbors[1].element is Plant)
                {
                    Destroy(tileOn.neighbors[1].element.gameObject);
                    tileOn.neighbors[1].element = null;
                    Debug.Log("MAGIC");
                    elementHandler.SpawnMagicTreeAt(tileOn.neighbors[1].gridPos);
                    hasMagicTree = true;
                }
                else
                {
                    Debug.Log("MAGIC2");
                    elementHandler.SpawnMagicTreeAt(tileOn.neighbors[1].gridPos);
                    hasMagicTree = true;
                }
            }
            else
            {
                plantsForMagicTree = 0;
            }
        }
            
    }


    private void Scoring()
    {
        if (isIrrigated == true)
        {
            scoringTimer += Time.deltaTime * gameTime.gameTimeSpeed;

            if (scoringTimer >= scoringTick)
            {


                CalculateScore();

                scoringTimer = 0;
            }
        }
    }

    private void CalculateScore()
    {
        switch (currentState)
        {
            case PlantState.Young:
                scoreManager.gameScore += youngTreeScoring;
                butterflyScore.Play(true);
                break;
            case PlantState.Adult:
                scoreManager.gameScore += adultTreeScoring;
                butterflyScore.Play(true);
                break;
            case PlantState.Senior:
                scoreManager.gameScore += seniorTreeScoring;
                butterflyScore.Play(true);
                break;
            case PlantState.FruitTree:
                scoreManager.gameScore += seniorTreeScoring;
                butterflyScore.Play(true);
                break;
            default:
                break;

        }
    }

    /*public void SpawnFlowers()
    {
        
        int numberOfFlowers = 0;
        float flowerXCoordinate;
        float flowerYCoordinate;

        if (currentState == PlantState.Adult)
        {
            numberOfFlowers = 3;
        }
        if(currentState == PlantState.Senior)
        {
            numberOfFlowers = 6;
        }
        for(int i = myFlower.Count; i < numberOfFlowers; i++)
        {
            flowerYCoordinate = Random.Range(-0.5f, 0.5f);
            if(flowerSpawnLeft == false)
            {
                flowerXCoordinate = Random.Range(0.2f, 0.5f);
                flowerSpawnLeft = true;
            }
            else 
            {
                flowerXCoordinate = Random.Range(-0.5f, -0.2f);
                flowerSpawnLeft = false;
            }


            Vector3 flowerCoordinate = new Vector3(flowerXCoordinate, flowerYCoordinate, 0);
            Sprite[] flowers = plantDrawer.visual.grass_plantGrowth.flowers;

            GameObject go = Instantiate(flowerTemplate, transform);
            go.transform.position = tileOn.worldPos + flowerCoordinate;
            go.name = "Flower_" + myFlower.Count;
            myFlower.Add(go);

            //Check if Plant
            FlowerVisual flower = go.GetComponent<FlowerVisual>();
            if (flower == null)
            {
                Debug.LogError("can't Find flower on the object", go);
                return;
            }
            else
            {
                flower.type = tileOn.type;
                flower.GenerateVisual();
            }
            if(flowerYCoordinate <= 0.17)
            {
                flower.renderer.sortingOrder = transform.GetComponent<SpriteRenderer>().sortingOrder + 1;
            }
            else
            {
                flower.renderer.sortingOrder = transform.GetComponent<SpriteRenderer>().sortingOrder - 1;
            }
            Debug.Log("pweaseflowerz");
        }

    }*/


    public void SpawnFlower()
    {
        int numberOfFlowers = 0;
        if (currentState == PlantState.Adult)
        {
            numberOfFlowers = 3;
        }
        if (currentState == PlantState.Senior)
        {
            numberOfFlowers = 6;
        }
        for (int i = myFlower.Count; i < numberOfFlowers; i++)
        {
            
            int chooseCollider = (int)Random.Range(0, flowerSpawnAeras.Count);
            

            float flowerXCoordinate;
            float flowerYCoordinate;

            flowerXCoordinate = Random.Range(flowerSpawnAeras[chooseCollider].bounds.max.x, flowerSpawnAeras[chooseCollider].bounds.min.x);
            flowerYCoordinate = Random.Range(flowerSpawnAeras[chooseCollider].bounds.max.y, flowerSpawnAeras[chooseCollider].bounds.min.y);
            if (Mathf.Sign(flowerXCoordinate) == 1)
            {
                flowerXCoordinate = flowerXCoordinate - Mathf.Abs(transform.position.x);
            }
            else if (Mathf.Sign(flowerXCoordinate) == -1)
            {
                flowerXCoordinate = flowerXCoordinate + Mathf.Abs(transform.position.x);
            }

            if (Mathf.Sign(flowerYCoordinate) == 1)
            {
                flowerYCoordinate = flowerYCoordinate - Mathf.Abs(transform.position.y);
            }
            else if (Mathf.Sign(flowerYCoordinate) == -1)
            {
                flowerYCoordinate = flowerYCoordinate + Mathf.Abs(transform.position.y);
            }
            Vector3 flowerCoordinate = new Vector3(flowerXCoordinate, flowerYCoordinate, 0);
            Sprite[] flowers = plantDrawer.visual.grass_plantGrowth.flowers;

            GameObject go = Instantiate(flowerTemplate, transform);
            go.transform.position = tileOn.worldPos + flowerCoordinate;
            go.name = "Flower_" + myFlower.Count;
            myFlower.Add(go);

            //Check if Plant
            FlowerVisual flower = go.GetComponent<FlowerVisual>();
            if (flower == null)
            {
                Debug.LogError("can't Find flower on the object", go);
                return;
            }
            else
            {
                flower.type = tileOn.type;
                flower.GenerateVisual();
            }
            if (chooseCollider <= frontAeraLeft )
            {

                flower.renderer.sortingOrder = transform.GetComponent<SpriteRenderer>().sortingOrder + 1;
                frontAeraLeft--;
            }
            else
            {
                flower.renderer.sortingOrder = transform.GetComponent<SpriteRenderer>().sortingOrder - 1;
            }
            flowerSpawnAeras.RemoveAt(chooseCollider);
        }
    }
}
