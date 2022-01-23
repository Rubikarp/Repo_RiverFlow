using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class TimeManager : Singleton<TimeManager>
{
    [Header("LongTime")]
    public bool isPaused = false;
    [SerializeField] public float gameTimer = 0;
    public float gameTimeSpeed = 1f;
    [Range(10, 360)] public float weekDuration = 180f;
    public int weekNumber = 1;
    public float DeltaSimulTime
    {
        get
        {
            return isPaused? 0: Time.deltaTime * gameTimeSpeed;
        }
    }
    public bool isFreeze
    {
        get
        {
            return gameTimeSpeed <= 0;
        }
    }

    [Header("FlowSimulation")]
    [Range(0.1f, 1.2f)] public float iterationStepDur = 0.2f;

    [Header("Plant Spawning")]
    public float plantSpawnRateCalm = 10;
    public float plantSpawnRateNeutral = 15;
    public float plantSpawnRateChaotic = 8;
    [Range(0.0f,1.0f)] public float spawnRateVariation = 0.2f;
    private float nextPlantSpawn;
    private PlantSpawner plantSpawner;


    [Header("Event")]
    public UnityEvent onWaterSimulationStep;
    public UnityEvent getMoreDig;


    void Start()
    {
        plantSpawner = GameObject.Find("PlantSpawner").GetComponent<PlantSpawner>();
        nextPlantSpawn = 2;
    }

    void Update()
    {
        if (!isPaused)
        {
            gameTimer += DeltaSimulTime;

            if (gameTimer > (weekDuration * weekNumber))
            {
                getMoreDig?.Invoke();
                weekNumber++;
                Pause();
            }

            //Cheat pour faire spawn instant une pousse
            if (Input.GetKeyDown(KeyCode.S))
            {
                nextPlantSpawn = 0;
            }

            nextPlantSpawn -= DeltaSimulTime;
            if (nextPlantSpawn < 0)
            {
                switch(plantSpawner.threatState) {
                    case ThreatState.CALM:
                        nextPlantSpawn = plantSpawnRateCalm + (plantSpawnRateCalm * spawnRateVariation);
                        break;

                    case ThreatState.NEUTRAL:
                        nextPlantSpawn = plantSpawnRateNeutral + (plantSpawnRateNeutral * spawnRateVariation);
                        break;
                    
                    case ThreatState.CHAOTIC:
                        nextPlantSpawn = plantSpawnRateChaotic + (plantSpawnRateChaotic * spawnRateVariation);
                        break;

                    default:
                        nextPlantSpawn = plantSpawnRateNeutral + (plantSpawnRateNeutral * spawnRateVariation);
                        break;
                }
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
        if (speed > 0)
        {
            RiverManager.Instance.FlowStep();
        }
        gameTimeSpeed = speed;
    }
}
