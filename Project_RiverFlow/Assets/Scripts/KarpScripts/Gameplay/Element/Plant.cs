using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class BoolEvent : UnityEvent<bool>
{ }

public class Plant : Element
{
    #region Element
    [Header("Element Data")]
    public GameTile tileOn;
    public override GameTile TileOn
    {
        get
        {
            if (tileOn == null)
            {
                Debug.LogError("Can't find the tile where is this WaterSource", this);
                return null ;
            }
            return tileOn;
        }
        set
        { tileOn = value; }
    }
    public override GameTile[] TilesOn
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
    //Don't overide other methode because is not linkable
    #endregion

    [Header("Plant Data")]
    public PlantState currentState = PlantState.Young;
    public bool isIrrigated = false;
    private bool IsAlive { get { return currentState != PlantState.Dead; } }

    public List<int> closeRivers;
    private FlowStrenght bestRiverStrenght = 0;
    
    [Header("Living")]
    [Range(0f,1f)] public float timer = 1.0f;
    public float stateUpgradeTime = 60f;
    public float stateDowngradeTime = 15f;

    [Header("Event")]
    public BoolEvent onStateChange;

    public GameTime gameTime;

    private void Start()
    {
        gameTime = GameTime.Instance;

        if (tileOn.isElement)
        {
            tileOn.element = this;
        }
    }

    void Update()
    {
        if (IsAlive && !gameTime.isPaused)
        {
            CheckNeighboringRivers();
            StateUpdate();
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
        isIrrigated = tileOn.IsIrrigate;
    }

    private void StateUpdate()
    {
        if (isIrrigated)
        {
            timer += Time.deltaTime * (1 / stateUpgradeTime) * gameTime.gameTimeSpeed;
        }
        else
        {
            timer -= Time.deltaTime * (1 / stateDowngradeTime) * gameTime.gameTimeSpeed;
        }

        //Lvl Drop
        if (timer < 0)
        {
            timer += 1f;
            currentState = (PlantState)Mathf.Clamp((int)(currentState - 1), 0, (int)PlantState.Senior);
            onStateChange?.Invoke(false);
            //Debug.Log("testcridown");
        }
        else
        //Lvl Up
        if (timer > 1)
        {
            //Si pas au niveau max
            if (currentState < PlantState.Senior)
            {
                //Debug.Log("testcriup");
                timer -= 1f;
                currentState = (PlantState)Mathf.Clamp((int)(currentState + 1), 0, (int)PlantState.Senior);
                onStateChange?.Invoke(true);
            }
            else
            {
                timer = 1f;
            }
        }
    }
}
