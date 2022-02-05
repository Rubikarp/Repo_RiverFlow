using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class TutoManager : MonoBehaviour
{
    [Header("R�ference")]
    private GameGrid grid;
    private InputHandler inputHandler;
    private TimeManager gameTime;
    private ElementHandler elements;
    private RiverManager riverManager;
    public DigingHandler digging;

    [Header("Info")]
    public WaterSource tutoSource;
    public Plant firstPlant;

    [Header("ToolTips")]
    public List<RectTransform> tooltips;
    private RectTransform currentTooltipTransform;
    bool alreadyDone;

    int currentStep = 0;

    void Start()
    {
        grid = GameGrid.Instance;
        inputHandler = InputHandler.Instance;
        gameTime = TimeManager.Instance;
        elements = ElementHandler.Instance;
        riverManager = RiverManager.Instance;

        //Cons�quence
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
        //Tant que le joueur n'a pas cr�er de canal
        while (riverManager.canals.Count < 1);
        DissapearToolTip();
        

        //Cons�quence
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
        //Tant que le joueur n'a pas cr�er de canal
        while 
        (  (int)grid.GetTile(riverManager.canals[0].endNode).riverStrenght < 1
        && tutoSource.TileOn.linkAmount < 1);
        DissapearToolTip();
        //Cons�quence
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
        //Cons�quence
        StopAllCoroutines();
        currentStep++;
        StartCoroutine(IrrigatePlant());
    }
    public IEnumerator IrrigatePlant()
    {
        //Set-Up
        Debug.Log("IrrigatePlant");

        elements.SpawnPlantAt(new Vector2Int(25, 15));
        firstPlant = (Plant)grid.GetTile(new Vector2Int(25, 15)).element;
        SpawnToolTip(currentStep);
        //Attente d'action
        do
        {


            yield return new WaitForEndOfFrame();
        }
        //Tant que le joueur n'a pas cr�er de canal
        while (!firstPlant.isIrrigated);
        DissapearToolTip();
        //Cons�quence
        StopAllCoroutines();
        currentStep++;
        StartCoroutine(LookForSplit());
    }
    public IEnumerator LookForSplit()
    {
        //Set-Up
        Debug.Log("LookForSplit");

        SpawnToolTip(currentStep);

        //Attente d'action
        do
        {


            yield return new WaitForEndOfFrame();
        }
        //Tant que le joueur n'a pas cr�er de canal
        while (!(riverManager.canals.Count > 3));
        DissapearToolTip();
        //Cons�quence
        StopAllCoroutines();
        currentStep++;
        //StartCoroutine();
    }


    void SpawnToolTip(int currentStep)
    {
        currentTooltipTransform = tooltips[currentStep];
        currentTooltipTransform.DOScaleY(1f, 0.2f).SetEase(Ease.InOutBack);
        currentTooltipTransform.DOScaleX(1f, 0.2f).SetEase(Ease.InOutBack);

    }
    void DissapearToolTip()
    {
        currentTooltipTransform.DOScaleY(0f, 0.2f).SetEase(Ease.InOutBack);
        currentTooltipTransform.DOScaleX(0f, 0.2f).SetEase(Ease.InOutBack);
    }
}
