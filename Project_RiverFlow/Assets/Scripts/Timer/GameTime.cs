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
    [Range(60, 360)] public float weekDuration = 180f;

    [Header("FlowSimulation")]
    [SerializeField] float simulTimer = 0;
    [Range(0.1f, 1.2f)] public float iterationStepDur = 0.2f;

    [Header("Event")]
    public UnityEvent onWaterSimulationStep;

    void Start()
    {
        
    }

    void Update()
    {
        if (!isPaused)
        {
            WaterSimulation();

            gameTimer += Time.deltaTime;
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
