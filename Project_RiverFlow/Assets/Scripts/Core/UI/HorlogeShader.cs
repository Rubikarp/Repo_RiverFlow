using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HorlogeShader : MonoBehaviour//,IMeshModifier
{
    [Header("reference")]
    public TimeManager time;

    [Header("Parameter")]
    [SerializeField] private float dayTime = 0;

    [Header("Target")]
    public Material horlogeMat;

    void Awake()
    {
        time = TimeManager.Instance;
    }

    void Start()
    {
        dayTime = (time.gameTimer % time.weekDuration)/ time.weekDuration;
        horlogeMat.SetFloat("_DayTime", dayTime);
    }

    void Update()
    {
        dayTime = (time.gameTimer % time.weekDuration) / time.weekDuration;
        horlogeMat.SetFloat("_DayTime", dayTime);
    }

    private void OnDestroy()
    {
        horlogeMat.SetFloat("_DayTime", 0);
    }
}
