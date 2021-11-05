using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    private bool IsAlive { get { return currentState != PlantState.Dead; } }

    public List<int> closeRivers;
    private RiverStrenght bestRiverStrenght = 0;
    
    [Header("Living")]
    [Range(0f,1f)] public float timer = 1.0f;
    public float stateUpgradeTime = 60f;
    public float stateDowngradeTime = 15f;

    [Header("Event")]
    public UnityEvent onStateChange;

    private void Start()
    {

    }

    void Update()
    {
        if (IsAlive)
        {
            CheckNeighboringRivers();
            StateUpdate();
        }
    }

    private void CheckNeighboringRivers()
    {
        //Cherche all irrigated Neighbor
        for (int i = 0; i < tileOn.data.neighbors.Length; i++)
        {
            if (tileOn.data.neighbors[i].isRiver)
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
            if (tileOn.data.neighbors[closeRivers[j]].riverStrenght > bestRiverStrenght)
            {
                bestRiverStrenght = tileOn.data.neighbors[closeRivers[j]].riverStrenght;
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
            currentState = (PlantState)Mathf.Clamp((int)(currentState - 1), 0, (int)PlantState.Senior);
            onStateChange?.Invoke();
        }
        else
        //Lvl Up
        if (timer > 1)
        {
            //Si pas au niveau max
            if(currentState < PlantState.Senior)
            {
                timer -= 1f;
                currentState = (PlantState)Mathf.Clamp((int)(currentState + 1), 0, (int)PlantState.Senior);
                onStateChange?.Invoke();
            }
            else
            {
                timer = 1f;
            }
        }
    }
}
