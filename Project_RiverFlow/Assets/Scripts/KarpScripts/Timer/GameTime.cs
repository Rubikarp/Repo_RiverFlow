using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class GameTime : Singleton<GameTime>
{
    [Header("LongTime")]
    public bool isPaused = false;
    [SerializeField] float gameTimer = 0;
    public float gameTimeSpeed = 1f;
    [Range(10, 360)] public float weekDuration = 180f;
    public int weekNumber = 1;

    [Header("FlowSimulation")]
    [SerializeField] float simulTimer = 0;
    [Range(0.1f, 1.2f)] public float iterationStepDur = 0.2f;

    [Header("Plant Spawning")]
    public float plantSpawnRate = 10;
    private float nextPlantSpawn;
    private PlantSpawner plantSpawner;
    public List<int> timingsZones;

    [Header("Event")]
    public UnityEvent onWaterSimulationStep;
    public UnityEvent getMoreDig;


    void Start()
    {
        plantSpawner = GameObject.Find("PlantSpawner").GetComponent<PlantSpawner>();
        nextPlantSpawn = plantSpawnRate;
    }

    void Update()
    {
        if (!isPaused)
        {
            WaterSimulation();

            gameTimer += Time.deltaTime;

            if (gameTimer > (weekDuration * weekNumber))
            {
                
                getMoreDig?.Invoke();
                
                weekNumber++;
                if (((weekDuration * weekNumber)+gameTimer) == timingsZones[plantSpawner.currentSpawnArea-1])
                {
                    plantSpawner.threatState = ThreatState.NEWZONE;
                    plantSpawner.currentSpawnArea++;
                }
                else
                {
                    plantSpawner.threatState = ThreatState.PEACEFUL;
                }

                Pause();
            }

            nextPlantSpawn -= Time.deltaTime;
            if (nextPlantSpawn < 0)
            {
                nextPlantSpawn = plantSpawnRate;
                plantSpawner.SpawnPlant();

            }
            
        }
    }

    [Button]
    public void Pause()
    {
        isPaused = !isPaused;
    }
    public void SetPause(bool state)
    {
        isPaused = state;
    }
    public void SetSpeed(float speed)
    {
        gameTimeSpeed = speed;
    }


    public void WaterSimulation()
    {
        simulTimer += Time.deltaTime;

        if (simulTimer > iterationStepDur)
        {
            onWaterSimulationStep?.Invoke();
            simulTimer = 0f;
        }
    }

}
