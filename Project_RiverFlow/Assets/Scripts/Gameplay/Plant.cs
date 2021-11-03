using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlantState
{
    Dead,
    Agony,
    Young,
    Tree1,
    Tree2,
    Tree3
}

public class Plant : MonoBehaviour, Element
{
    #region Element
    [Header("Element Data")]
    public GameTile tileOn;
    public GameTile[] TilesOn
    {
        get
        {
            if (tileOn == null)
            {
                Debug.LogError("Can't find the tile where is this WaterSource", this);
                return new GameTile[1] { null };
            }
            return new GameTile[1] { tileOn };
        }
        set { tileOn = value[0]; }
    }
    ///Not Likable
    public bool isLinkable { get { return false; } }
    #endregion

    [Header("Plant Data")]
    public PlantState currentState = PlantState.Young;
    [SerializeField] bool isIrrigated = false;
    [SerializeField] bool IsAliveVisu = true;
    private bool IsAlive { get { return currentState != PlantState.Dead; } }

    public List<int> closeRivers;
    private RiverStrenght bestRiverStrenght = 0;
    
    [Header("Living")]
    [Range(0f,1f)] public float timer = 1.0f;
    public float stateUpgradeTime = 60f;
    public float stateDowngradeTime = 15f;

    [Header("Living")]
    [SerializeField] Camera cam;
    [SerializeField] Canvas canvas;
    [SerializeField] Image imgTiller;

    [Header("Visual")]
    public GameObject[] allSkins;

    private void Start()
    {
        currentState = PlantState.Young;
        UpdateSkin();
        cam = Camera.main;
        canvas.worldCamera = cam;
    }

    void Update()
    {
        IsAliveVisu = IsAlive;
        if (IsAlive)
        {
            CheckNeighboringRivers();
            StateUpdate();

            imgTiller.fillAmount = timer;
        }
    }

    private void CheckNeighboringRivers()
    {
        //Cherche all irrigated Neighbor
        for (int i = 0; i < tileOn.neighbors.Length; i++)
        {
            if (tileOn.neighbors[i].isRiver)
            {
                if (!closeRivers.Contains(i))
                {
                    closeRivers.Add(i);
                }
            }
            else
            //if (!tileOn.neighbors[i].isRiver)
            {
                if (closeRivers.Contains(i))
                {
                    closeRivers.Remove(closeRivers.IndexOf(i));
                }
            }
        }

        //Cherche best river Strenght
        bestRiverStrenght = 0;
        for (int j = 0; j < closeRivers.Count; j++)
        {
            if (tileOn.neighbors[closeRivers[j]].riverStrenght > bestRiverStrenght)
            {
                bestRiverStrenght = tileOn.neighbors[closeRivers[j]].riverStrenght;
            }
        }

        //Determine if irrigated
        isIrrigated = VerifyIrrigation(bestRiverStrenght);
    }

    private bool VerifyIrrigation(RiverStrenght _bestRiverStrenght)
    {
        if(_bestRiverStrenght > RiverStrenght._00_)
        {
            return tileOn.type <= (TileType)_bestRiverStrenght;
        }
        else
        {
            return false;
        }
    } 

    private void StateUpdate()
    {
        if (isIrrigated)
        {
            timer += Time.deltaTime * (1/stateUpgradeTime);
        }
        else
        {
            timer -= Time.deltaTime * (1/stateDowngradeTime);
        }

        //Lvl Drop
        if (timer < 0)
        {
            timer += 1f;
            currentState = (PlantState)Mathf.Clamp((int)(currentState - 1), 0, (int)PlantState.Tree3);
            UpdateSkin();
        }
        else
        //Lvl Up
        if (timer > 1)
        {
            //Si pas au niveau max
            if(currentState < PlantState.Tree3)
            {
                timer -= 1f;
                currentState = (PlantState)Mathf.Clamp((int)(currentState + 1), 0, (int)PlantState.Tree3);
                UpdateSkin();
            }
            else
            {
                timer = 1f;
            }
        }
    }

    private void UpdateSkin()
    {
        for (int i = 0; i < allSkins.Length; i++)
        {
            allSkins[i].SetActive(false);
        }

        allSkins[(int)currentState].SetActive(true);
    }
}
