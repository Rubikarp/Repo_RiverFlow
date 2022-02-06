using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoManager : MonoBehaviour
{
    [Header("Réference")]
    private GameGrid grid;
    private InputHandler inputHandler;
    private TimeManager gameTime;
    private ElementHandler elements;
    private RiverManager riverManager;
    public DigingHandler digging;
    public ZoomManager cameraZoom;
    public InventoryManager inventory;

    [Header("Info")]
    public WaterSource tutoSource;
    public Plant firstPlant;
    public Plant secondPlant;
    public Plant desertPlant;

    [Header("ToolTips")]
    public List<RectTransform> tooltips;
    private RectTransform currentTooltipTransform;
    bool alreadyDone;

    [Header("Indicators")]
    public RectTransform step8Indicator;

    int currentStep = 0;

    void Start()
    {
        grid = GameGrid.Instance;
        inputHandler = InputHandler.Instance;
        gameTime = TimeManager.Instance;
        elements = ElementHandler.Instance;
        riverManager = RiverManager.Instance;

        //Conséquence
        StopAllCoroutines();
        StartCoroutine(BeginCreation());

    }

    private void Update()
    {
        Debug.Log(currentStep);
    }

    public IEnumerator BeginCreation()
    {
        //Set-Up
        Debug.Log("BeginCreation");
        SpawnToolTip(currentStep);
        //Attente d'action
        do
        {
           

            yield return new WaitForEndOfFrame();
        }
        //Tant que le joueur n'a pas créer de canal
        while (riverManager.canals.Count < 1);
        DissapearToolTip();
        

        //Conséquence
        StopAllCoroutines();
        currentStep++;
        StartCoroutine(IrrigateCreation());
    }
    public IEnumerator IrrigateCreation()
    {
        //Set-Up
        Debug.Log("IrrigateCreation");
        SpawnToolTip(currentStep);
        //Attente d'action
        do
        {


            yield return new WaitForEndOfFrame();
        }
        //Tant que le joueur n'a pas créer de canal
        while 
        (  (int)grid.GetTile(riverManager.canals[0].endNode).riverStrenght < 1
        && tutoSource.TileOn.linkAmount < 1);
        DissapearToolTip();
        //Conséquence
        StopAllCoroutines();
        currentStep++;
        StartCoroutine(InfoRiverFlow());
    }

    //exemple de qd il fo cliquer
    public IEnumerator InfoRiverFlow()
    {
        bool hasRealeasedKey = false;
        //Set-Up
        Debug.Log("InfoRiverFlow");

       
        SpawnToolTip(currentStep);
        //Attente d'action
        do
        {
            if (!UnityEngine.Input.GetMouseButton(0))
            {
                Debug.Log("released");
                hasRealeasedKey = true;
            }
            yield return new WaitForEndOfFrame();
        }
        while (!UnityEngine.Input.GetMouseButton(0)||!hasRealeasedKey);
        DissapearToolTip();
        //Conséquence
        StopAllCoroutines();
        currentStep++;
        StartCoroutine(IrrigatePlant());
    }
    public IEnumerator IrrigatePlant()
    {
        //Set-Up
        Debug.Log("IrrigatePlant");

        elements.SpawnPlantAt(new Vector2Int(25, 13));
        firstPlant = (Plant)grid.GetTile(new Vector2Int(25, 13)).element;
        SpawnToolTip(currentStep);
        //Attente d'action
        do
        {


            yield return new WaitForEndOfFrame();
        }
        //Tant que le joueur n'a pas créer de canal
        while (!firstPlant.isIrrigated);
        DissapearToolTip();
        //Conséquence
        StopAllCoroutines();
        currentStep++;
        StartCoroutine(InfoIrrigate());
    }
    public IEnumerator InfoIrrigate()
    {
        bool hasRealeasedKey = false;
        //Set-Up
        Debug.Log("InfoIrrigate");


        SpawnToolTip(currentStep);
        //Attente d'action
        do
        {
            if (!UnityEngine.Input.GetMouseButton(0))
            {
                Debug.Log("released");
                hasRealeasedKey = true;
            }
            yield return new WaitForEndOfFrame();
        }
        while (!UnityEngine.Input.GetMouseButton(0) || !hasRealeasedKey);
        DissapearToolTip();
        //Conséquence
        StopAllCoroutines();
        currentStep++;
        StartCoroutine(InfoButterfly());
    }
    public IEnumerator InfoButterfly()
    {
        bool hasRealeasedKey = false;
        //Set-Up
        Debug.Log("InfoButterfly");


        SpawnToolTip(currentStep);
        //Attente d'action
        do
        {
            if (!UnityEngine.Input.GetMouseButton(0))
            {
                Debug.Log("released");
                hasRealeasedKey = true;
            }
            yield return new WaitForEndOfFrame();
        }
        while (!UnityEngine.Input.GetMouseButton(0) || !hasRealeasedKey);
        DissapearToolTip();
        //Conséquence
        StopAllCoroutines();
        currentStep++;
        StartCoroutine(LookForSplit());
    }
    public IEnumerator LookForSplit()
    {
        //Set-Up
        Debug.Log("LookForSplit");

        elements.SpawnPlantAt(new Vector2Int(24, 17));
        secondPlant = (Plant)grid.GetTile(new Vector2Int(24, 17)).element;
        SpawnToolTip(currentStep);

        //Attente d'action
        do
        {


            yield return new WaitForEndOfFrame();
        }
        //Tant que le joueur n'a pas splitté + irrigué
        while (!(riverManager.canals.Count >= 3) || !secondPlant.isIrrigated);
        DissapearToolTip();
        //Conséquence
        StopAllCoroutines();
        currentStep++;
        StartCoroutine(InfoDivideForShovels());
    }
    public IEnumerator InfoDivideForShovels()
    {
        bool hasRealeasedKey = false;
        //Set-Up
        Debug.Log("InfoDivideForShovels");

        SpawnToolTip(currentStep);
        SpawnIndicator(step8Indicator, 0.65f);
        //Attente d'action
        do
        {
            if (!UnityEngine.Input.GetMouseButton(0))
            {
                Debug.Log("released");
                hasRealeasedKey = true;
            }
            yield return new WaitForEndOfFrame();
        }
        while (!UnityEngine.Input.GetMouseButton(0) || !hasRealeasedKey);
        DissapearToolTip();
        DissapearIndicator(step8Indicator);
        //Conséquence
        StopAllCoroutines();
        currentStep++;
        StartCoroutine(InfoCarefulDivide());
    }
    public IEnumerator InfoCarefulDivide()
    {
        bool hasRealeasedKey = false;
        //Set-Up
        Debug.Log("InfoCarefulDivide");

        SpawnToolTip(currentStep);
        //Attente d'action
        do
        {
            if (!UnityEngine.Input.GetMouseButton(0))
            {
                Debug.Log("released");
                hasRealeasedKey = true;
            }
            yield return new WaitForEndOfFrame();
        }
        while (!UnityEngine.Input.GetMouseButton(0) || !hasRealeasedKey);
        DissapearToolTip();
        //Conséquence
        StopAllCoroutines();
        currentStep++;
        StartCoroutine(DesertReveal());
    }
    public IEnumerator DesertReveal()
    {
        bool hasRealeasedKey = false;
        //Set-Up
        Debug.Log("DesertReveal");


        SpawnToolTip(currentStep);
        cameraZoom.Zoom();
        //Attente d'action
        do
        {
            if (!UnityEngine.Input.GetMouseButton(0))
            {
                Debug.Log("released");
                hasRealeasedKey = true;
            }
            yield return new WaitForEndOfFrame();
        }
        while (!UnityEngine.Input.GetMouseButton(0) || !hasRealeasedKey);
        DissapearToolTip();
        //Conséquence
        StopAllCoroutines();
        currentStep++;
        StartCoroutine(MergeForDesertPhase1());
    }
    public IEnumerator MergeForDesertPhase1()
    {
        bool firstPartDone = false;
        //Set-Up
        Debug.Log("MergeForDesertPhase1");

        elements.SpawnPlantAt(new Vector2Int(20, 15));
        desertPlant = (Plant)grid.GetTile(new Vector2Int(20, 15)).element;
        SpawnToolTip(currentStep);
        //Attente d'action
        do
        {
            for (int i = 0; i < desertPlant.TileOn.neighbors.Length; i++)
            {
                if (desertPlant.TileOn.neighbors[i].isDuged == true)
                {
                    firstPartDone = true;
                }
            }

            yield return new WaitForEndOfFrame();
        }
        //Tant que le joueur n'a pas irrigué la pousse désertique (et donc mergé)
        while (!firstPartDone);
        //Conséquence
        StopAllCoroutines();
        StartCoroutine(MergeForDesertPhase2());
    }
    public IEnumerator MergeForDesertPhase2()
    {
        //Set-Up
        Debug.Log("MergeForDesertPhase2");

        SpawnToolTip(currentStep);
        //Attente d'action
        do
        {


            yield return new WaitForEndOfFrame();
        }
        //Tant que le joueur n'a pas irrigué la pousse désertique (et donc mergé)
        while (!desertPlant.isIrrigated);
        DissapearToolTip();
        //Conséquence
        StopAllCoroutines();
        currentStep++;
        StartCoroutine(InfoDieCondition());
    }
    public IEnumerator InfoDieCondition()
    {
        bool hasRealeasedKey = false;
        //Set-Up
        Debug.Log("InfoDieCondition");


        SpawnToolTip(currentStep);
        //Attente d'action
        do
        {
            if (!UnityEngine.Input.GetMouseButton(0))
            {
                Debug.Log("released");
                hasRealeasedKey = true;
            }
            yield return new WaitForEndOfFrame();
        }
        while (!UnityEngine.Input.GetMouseButton(0) || !hasRealeasedKey);
        DissapearToolTip();
        //Conséquence
        StopAllCoroutines();
        currentStep++;
        StartCoroutine(ErasePhase1());
    }
    public IEnumerator ErasePhase1()
    {
        //Set-Up
        Debug.Log("ErasePhase1");

        SpawnToolTip(currentStep);
        //Attente d'action
        do
        {


            yield return new WaitForEndOfFrame();
        }
        //Tant que le joueur n'a pas irrigué la pousse désertique (et donc mergé)
        while (!(riverManager.canals.Count <= 3));
        //Conséquence
        StopAllCoroutines();
        StartCoroutine(ErasePhase2());
    }
    public IEnumerator ErasePhase2()
    {
        //Set-Up
        Debug.Log("ErasePhase2");

        SpawnToolTip(currentStep);
        //Attente d'action
        do
        {


            yield return new WaitForEndOfFrame();
        }
        //Tant que le joueur n'a pas irrigué la pousse désertique (et donc mergé)
        while (!(riverManager.canals.Count <= 2));
        //Conséquence
        StopAllCoroutines();
        StartCoroutine(OptimiseRiver());
    }
    public IEnumerator OptimiseRiver()
    {
        //Set-Up
        Debug.Log("OptimiseRiver");

        SpawnToolTip(currentStep);
        //Attente d'action
        do
        {


            yield return new WaitForEndOfFrame();
        }
        //Tant que le joueur n'a pas irrigué la pousse désertique (et donc mergé)
        while (!desertPlant.isIrrigated || !secondPlant.isIrrigated || !firstPlant.isIrrigated);
        DissapearToolTip();
        //Conséquence
        StopAllCoroutines();
        currentStep++;
        StartCoroutine(InfoTime());
    }
    public IEnumerator InfoTime()
    {
        bool hasRealeasedKey = false;
        //Set-Up
        Debug.Log("InfoTime");


        SpawnToolTip(currentStep);
        //Attente d'action
        do
        {
            if (!UnityEngine.Input.GetMouseButton(0))
            {
                Debug.Log("released");
                hasRealeasedKey = true;
            }
            yield return new WaitForEndOfFrame();
        }
        while (!UnityEngine.Input.GetMouseButton(0) || !hasRealeasedKey);
        DissapearToolTip();
        //Conséquence
        StopAllCoroutines();
        StartCoroutine(WaitForNewDay());
    }
    public IEnumerator WaitForNewDay()
    {
        //Set-Up
        Debug.Log("WaitForNewDay");

        //Attente d'action
        do
        {


            yield return new WaitForEndOfFrame();
        }
        //Tant que le joueur n'a pas irrigué la pousse désertique (et donc mergé)
        while (!(gameTime.weekNumber >= 2));
        //Conséquence
        StopAllCoroutines();
        currentStep++;
        StartCoroutine(RewardLake());
    }
    public IEnumerator RewardLake()
    {
        //Set-Up
        Debug.Log("RewardLake");

        //Attente d'action
        do
        {

            yield return new WaitForEndOfFrame();
        }
        //Tant que le joueur n'a pas irrigué la pousse désertique (et donc mergé)
        while (!(inventory.lakesAmmount >= 1));
        DissapearToolTip();
        //Conséquence
        StopAllCoroutines();
        currentStep++;
        StartCoroutine(InfoLake());
    }
    public IEnumerator InfoLake()
    {
        bool hasRealeasedKey = false;
        //Set-Up
        Debug.Log("InfoLake");


        SpawnToolTip(currentStep);
        //Attente d'action
        do
        {
            if (!UnityEngine.Input.GetMouseButton(0))
            {
                Debug.Log("released");
                hasRealeasedKey = true;
            }
            yield return new WaitForEndOfFrame();
        }
        while (!UnityEngine.Input.GetMouseButton(0) || !hasRealeasedKey);
        DissapearToolTip();
        //Conséquence
        StopAllCoroutines();
        currentStep++;
        //StartCoroutine(InfoTime());
    }



    void SpawnToolTip(int currentStep)
    {
        currentTooltipTransform = tooltips[currentStep];
        currentTooltipTransform.DOScaleY(0.5f, 0.2f).SetEase(Ease.InOutBack);
        currentTooltipTransform.DOScaleX(0.5f, 0.2f).SetEase(Ease.InOutBack);

    }
    void DissapearToolTip()
    {
        currentTooltipTransform.DOScaleY(0f, 0.2f).SetEase(Ease.InOutBack);
        currentTooltipTransform.DOScaleX(0f, 0.2f).SetEase(Ease.InOutBack);
    }
    void SpawnIndicator(RectTransform Indicator, float scale)
    {
        Indicator.DOScaleY(scale, 0.2f).SetEase(Ease.InOutBack);
        Indicator.DOScaleX(scale, 0.2f).SetEase(Ease.InOutBack);
    }
    void DissapearIndicator(RectTransform Indicator)
    {
        Indicator.DOScaleY(0f, 0.2f).SetEase(Ease.InOutBack);
        Indicator.DOScaleX(0f, 0.2f).SetEase(Ease.InOutBack);
    }
}
