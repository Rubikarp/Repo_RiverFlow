using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour, Element
{
    [Header("Variable")]
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

    public List<int> closeRivers;
    private RiverStrenght bestRiverStrenght = 0;
    public float noChangeTimer = 0.0f;
    public float stateDowngradeTimer = 15f;
    public float stateUpgradeTimer = 60f;

    private bool irrigated = false;
    public string[] state = new string[6];
    public string currentState;
    private int stateIndex = 2;
    private bool alive = true;

    public GameObject[] allSkins;

    private void Awake()
    {
        state[0] = "Dead";
        state[1] = "Agony";
        state[2] = "Young";
        state[3] = "Tree1";
        state[4] = "Tree2";
        state[5] = "Tree3";

        currentState = state[2];
        UpdateSkin();
    }

    void Update()
    {
        if (alive == true)
        {
            CheckNeighboringRivers();
            noChangeTimer += Time.deltaTime;
            StateUpdate();
        }
    }

    private void CheckNeighboringRivers()  
    {
        for (int a = 0; a < tileOn.neighbors.Length; a++)
        {
            if (tileOn.neighbors[a].isRiver == true)
            {
                closeRivers.Add(a);
            }
        }

        if (closeRivers.Count > 0)
        {
            for (int b = 0; b < closeRivers.Count; b++)
            {
                if (tileOn.neighbors[b].riverStrenght >= bestRiverStrenght)
                {
                    bestRiverStrenght = tileOn.neighbors[b].riverStrenght;
                }
            }

            VerifyIrrigation();
        }
        else
        {
            irrigated = false;
        }
    }

    private void VerifyIrrigation()
    {
        switch (bestRiverStrenght)
        {
            case RiverStrenght._00_:
                irrigated = false;
                break;

            case RiverStrenght._25_:
                if (tileOn.type == TileType.soil || tileOn.type == TileType.other)
                {
                    irrigated = true;
                }
                else
                {
                    irrigated = false;
                }
                break;

            case RiverStrenght._50_:
                if (tileOn.type == TileType.soil || tileOn.type == TileType.clay || tileOn.type == TileType.other)
                {
                    irrigated = true;
                }
                else
                {
                    irrigated = false;
                }
                break;

            case RiverStrenght._75_:
                if (tileOn.type == TileType.soil || tileOn.type == TileType.clay || tileOn.type == TileType.sand || tileOn.type == TileType.other)
                {
                    irrigated = true;
                }
                else
                {
                    irrigated = false;
                }
                break;

            case RiverStrenght._100_:
                if (tileOn.type == TileType.soil || tileOn.type == TileType.clay || tileOn.type == TileType.sand)
                {
                    irrigated = true;
                }
                else
                {
                    irrigated = false;
                }
                break;
        }
    } 

    private void StateUpdate()
    {
        if (irrigated == false)
        {
            if (noChangeTimer >= stateDowngradeTimer)
            {
                stateIndex -= 1;
                currentState = state[stateIndex];
                noChangeTimer = 0.0f;
                UpdateSkin();

                if (currentState == state[0])
                {
                    alive = false;
                }
            }
        }
        else if (irrigated == true)
        {
            if (stateIndex <= 2)
            {
                stateIndex = 3;
                currentState = state[stateIndex];
                noChangeTimer = 0.0f;
                UpdateSkin();
            }
            else if (stateIndex <= 4)
            {
                if (noChangeTimer >= stateUpgradeTimer)
                {
                    stateIndex += 1;
                    currentState = state[stateIndex];
                    noChangeTimer = 0.0f;
                    UpdateSkin();
                }
            }
            else if (stateIndex == 5)
            {
                noChangeTimer = 0.0f;
            }
        }
    }

    private void UpdateSkin()
    {
        for (int c = 0; c < allSkins.Length; c++)
        {
            allSkins[c].SetActive(false);
        }

        allSkins[stateIndex].SetActive(true);
    }
}
