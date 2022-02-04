using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoManager : MonoBehaviour
{
    [Header("R�ference")]
    private GameGrid grid;
    private InputHandler input;
    private TimeManager gameTime;
    private ElementHandler elements;
    private RiverManager riverManager;
    public DigingHandler digging;

    [Header("Info")]
    public WaterSource tutoSource;
    public Plant firstPlant;

    void Start()
    {
        grid = GameGrid.Instance;
        input = InputHandler.Instance;
        gameTime = TimeManager.Instance;
        elements = ElementHandler.Instance;
        riverManager = RiverManager.Instance;

        //Cons�quence
        StopAllCoroutines();
        StartCoroutine(BeginCreation());

    }


    public IEnumerator BeginCreation()
    {
        //Set-Up
        Debug.Log("BeginCreation");

        //Attente d'action
        do
        {


            yield return new WaitForEndOfFrame();
        }
        //Tant que le joueur n'a pas cr�er de canal
        while (riverManager.canals.Count < 1);

        //Cons�quence
        StopAllCoroutines();
        StartCoroutine(IrrigateCreation());
    }
    public IEnumerator IrrigateCreation()
    {
        //Set-Up
        Debug.Log("IrrigateCreation");

        //Attente d'action
        do
        {


            yield return new WaitForEndOfFrame();
        }
        //Tant que le joueur n'a pas cr�er de canal
        while 
        (  (int)grid.GetTile(riverManager.canals[0].endNode).riverStrenght < 1
        && tutoSource.TileOn.linkAmount < 1);

        //Cons�quence
        StopAllCoroutines();
        StartCoroutine(IrrigatePlant());
    }
    public IEnumerator IrrigatePlant()
    {
        //Set-Up
        Debug.Log("IrrigatePlant");

        elements.SpawnPlantAt(new Vector2Int(25, 15));
        firstPlant = (Plant)grid.GetTile(new Vector2Int(25, 15)).element;

        //Attente d'action
        do
        {


            yield return new WaitForEndOfFrame();
        }
        //Tant que le joueur n'a pas cr�er de canal
        while (!firstPlant.isIrrigated);

        //Cons�quence
        StopAllCoroutines();
        StartCoroutine(LookForSplit());
    }
    public IEnumerator LookForSplit()
    {
        //Set-Up
        Debug.Log("LookForSplit");


        //Attente d'action
        do
        {


            yield return new WaitForEndOfFrame();
        }
        //Tant que le joueur n'a pas cr�er de canal
        while (riverManager.canals.Count > 3);

        //Cons�quence
        StopAllCoroutines();
        //StartCoroutine();
    }
}
