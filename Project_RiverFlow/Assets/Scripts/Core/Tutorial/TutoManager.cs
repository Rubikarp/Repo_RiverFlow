using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoManager : MonoBehaviour
{
    [Header("Réference")]
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
    }


    public IEnumerator BeginCreation()
    {
        //Set-Up
        /*
            Actu il y a rien
        */

        //Attente d'action
        do
        {


            yield return new WaitForEndOfFrame();
        }
        //Tant que le joueur n'a pas créer de canal
        while (riverManager.canals.Count < 1);

        //Conséquence
        StopAllCoroutines();
        StartCoroutine(IrrigateCreation());
    }
    public IEnumerator IrrigateCreation()
    {
        //Set-Up
        /*
            Actu il y a rien
        */

        //Attente d'action
        do
        {


            yield return new WaitForEndOfFrame();
        }
        //Tant que le joueur n'a pas créer de canal
        while 
        (  grid.GetTile(riverManager.canals[0].endNode).riverStrenght > 0
        && tutoSource.TileOn.linkAmount > 0);

        //Conséquence
        StopAllCoroutines();
        StartCoroutine(IrrigatePlant());
    }
    public IEnumerator IrrigatePlant()
    {
        //Set-Up
        elements.SpawnPlantAt(new Vector2Int(15, 15));
        firstPlant = (Plant)grid.GetTile(new Vector2Int(15, 15)).element;

        //Attente d'action
        do
        {


            yield return new WaitForEndOfFrame();
        }
        //Tant que le joueur n'a pas créer de canal
        while (!firstPlant.isIrrigated);

        //Conséquence
        StopAllCoroutines();
        StartCoroutine(LookForSplit());
    }
    public IEnumerator LookForSplit()
    {
        //Set-Up
        /*
            Actu il y a rien
        */

        //Attente d'action
        do
        {


            yield return new WaitForEndOfFrame();
        }
        //Tant que le joueur n'a pas créer de canal
        while (riverManager.canals.Count > 3);

        //Conséquence
        StopAllCoroutines();
        //StartCoroutine();
    }
}
