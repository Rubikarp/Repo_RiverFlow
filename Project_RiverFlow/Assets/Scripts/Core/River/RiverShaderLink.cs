using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverShaderLink : MonoBehaviour
{
    [Header("reference")]
    public Material riverMat;

    [Header("Parameter")]
    [SerializeField] private float riverTime = 0f;
    [Space(10)]
    [Range(0.1f, 1.0f), SerializeField] private float speedFactor = 1f;

    [Header("Internal")]
    private TimeManager clock;

    void Start()
    {
        clock = TimeManager.Instance;
        riverTime = 0f;
    }

    void Update()
    {
        if (!clock.isPaused)
        {
            riverTime += clock.DeltaSimulTime;
        }
        riverMat.SetFloat("_RiverTime", riverTime);
    }

    private void OnDestroy()
    {
        riverMat.SetFloat("_RiverTime", 0);
    }
}
