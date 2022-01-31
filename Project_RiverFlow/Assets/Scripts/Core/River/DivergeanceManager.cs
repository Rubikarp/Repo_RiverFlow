using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif 

public class DivergeanceManager : MonoBehaviour
{
    public RiverManager riverManager;
    public GameGrid grid;

    public GameObject dixVergeTemplate;
    public List<DivergeanceChoice> divergeances;

    void Start()
    {
        riverManager = RiverManager.Instance;
        grid = GameGrid.Instance;
    }


    void Update()
    {
        GameTile lookedTile;
        foreach (Canal canal in riverManager.canals)
        {
            lookedTile = grid.GetTile(canal.startNode);
            if (lookedTile.flowOut.Count >= 2)
            {
                DivergeanceChoice choice = divergeances.Find(x => x.gridPos == lookedTile.gridPos);
                //check if not already here
                if (choice != null)
                {
                    // already here
                    if (lookedTile.linkAmount < 3)
                    {
                        divergeances.Remove(choice);
                        Destroy(choice.gameObject);
                    }
                    else
                    {
                        choice.UpdateChoice();
                    }
                }
                else
                {
                    SpawnDivergeance(lookedTile);
                }
            }
        }
    }

    public void SpawnDivergeance(GameTile tile)
    {
        GameObject go = InstanciateObject(dixVergeTemplate, tile);
        go.transform.position = grid.TileToPos(new Vector2Int(tile.gridPos.x, tile.gridPos.y));
        go.name = "Source_" + tile.gridPos;

        //Check if Plant
        DivergeanceChoice choice = go.GetComponent<DivergeanceChoice>();
        if (choice == null)
        {
            Debug.LogError("can't Find DivergeanceChoice on the object", go);
        }

        divergeances.Add(choice);

        //Link Element and Tile
        choice.gridPos = tile.gridPos;
        choice.tileOn = tile;
    }

    public GameObject InstanciateObject(GameObject template,GameTile tile)
    {
#if UNITY_EDITOR
        return PrefabUtility.InstantiatePrefab(template, tile.transform) as GameObject;
#else
        return Instantiate(template, elementContainer);
#endif 
    }
}
