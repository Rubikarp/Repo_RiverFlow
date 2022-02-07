using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.Universal;

public class NightDayLight : MonoBehaviour
{
    [Header("reference")]
    public TimeManager time;

    [Header("Parameter")]
    [SerializeField] private float dayTime = 0;
    public Gradient cycleLight;

    [Header("Target")]
    public Light2D globalLight;

    void Awake()
    {
        time = TimeManager.Instance;
    }

    void Start()
    {
        dayTime = (time.gameTimer % time.weekDuration) / time.weekDuration;
        globalLight.color = cycleLight.Evaluate(dayTime);
    }

    void Update()
    {
        dayTime = (time.gameTimer % time.weekDuration) / time.weekDuration;
        globalLight.color = cycleLight.Evaluate(dayTime);
    }
}
