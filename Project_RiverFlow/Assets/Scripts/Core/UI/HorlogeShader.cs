using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HorlogeShader : MonoBehaviour//,IMeshModifier
{
    [Header("reference")]
    public TimeManager time;
    public SpriteRenderer rend;
    private Transform self;

    [Header("Parameter")]
    [SerializeField] private float dayTime = 0;

    [Header("Internal")]
    private MaterialPropertyBlock propBlock;

    void Awake()
    {
        time = TimeManager.Instance;
        self = transform;
    }

    void Start()
    {
        //permet d'overide les param sans modif le mat ou créer d'instance
        propBlock = new MaterialPropertyBlock();
        //Recup Data
        rend.GetPropertyBlock(propBlock);
        //Edit
        #region EditZone
        dayTime = (time.gameTimer % time.weekDuration)/ time.weekDuration;
        propBlock.SetFloat("_DayTime", dayTime);
        #endregion
        //Push Data
        rend.SetPropertyBlock(propBlock);
    }

    void Update()
    {
        //permet d'overide les param sans modif le mat ou créer d'instance
        propBlock = new MaterialPropertyBlock();
        //Recup Data
        rend.GetPropertyBlock(propBlock);
        //Edit
        #region EditZone
        dayTime = (time.gameTimer % time.weekDuration) / time.weekDuration;
        propBlock.SetFloat("_DayTime", dayTime);
        #endregion
        //Push Data
        rend.SetPropertyBlock(propBlock);
    }
}
