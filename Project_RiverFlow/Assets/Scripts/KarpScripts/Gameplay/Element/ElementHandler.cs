using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NaughtyAttributes;

public class ElementHandler : MonoBehaviour
{
    public GameGrid grid;
    [Space(10)]
    public GameObject waterSource_Template;
    public GameObject cloud_Template;
    public GameObject lake_Template;
    public GameObject plant_Template;
    public GameObject magicTree_Template;
    [Space(10)]
    public Transform elementContainer;

    [Header("Tool"), HorizontalLine]
    public List<Plant> allPlants = new List<Plant>();
    [Space(10)]
    public List<WaterSource> allSources = new List<WaterSource>();
    [Space(10)]
    public List<Cloud> allClouds = new List<Cloud>();
    [Space(10)]
    public List<Lake> allLakes = new List<Lake>();
    [Space(10)]
    public List<MagicTree> allMagicTrees = new List<MagicTree>();

    [Header("Tool"), HorizontalLine]
    [SerializeField] int posX;
    [SerializeField] int posY;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SpawnPlantAt(Vector2Int grisPos)
    {
        if (!grid.GetTile(grisPos).isElement)
        {
#if UNITY_EDITOR
            GameObject go = PrefabUtility.InstantiatePrefab(plant_Template, elementContainer)as GameObject;
#else
            GameObject go = Instantiate(plant_Template, elementContainer);
#endif 

            go.transform.position = grid.TileToPos(new Vector2Int(grisPos.x, grisPos.y));
            go.name = "Plant_" + grisPos;

            //Check if Plant
            Plant plant = go.GetComponent<Plant>();
            if (plant == null)
            {
                Debug.LogError("can't Find Plant on the object", go);
                return;
            }

            allPlants.Add(plant);

            //Link Element and Tile
            plant.tileOn = grid.GetTile(grisPos);
            grid.GetTile(grisPos).element = plant;
        }
    }
    public void SpawnWaterSourceAt(Vector2Int grisPos)
    {
        if (!grid.GetTile(grisPos).isElement)
        {
#if UNITY_EDITOR
            GameObject go = PrefabUtility.InstantiatePrefab(waterSource_Template, elementContainer) as GameObject;
#else
            GameObject go = Instantiate(waterSource_Template, elementContainer);
#endif 
            go.transform.position = grid.TileToPos(new Vector2Int(grisPos.x, grisPos.y));
            go.name = "Source_" + grisPos;

            //Check if Plant
            WaterSource source = go.GetComponent<WaterSource>();
            if (source == null)
            {
                Debug.LogError("can't Find WaterSource on the object", go);
            }

            allSources.Add(source);
            //Link Element and Tile
            source.tileOn = grid.GetTile(grisPos);
            grid.GetTile(grisPos).element = source;
        }
    }
    public void SpawnCloudAt(Vector2Int grisPos)
    {
        if (!grid.GetTile(grisPos).isElement)
        {
#if UNITY_EDITOR
            GameObject go = PrefabUtility.InstantiatePrefab(cloud_Template, elementContainer) as GameObject;
#else
            GameObject go = Instantiate(cloud_Template, elementContainer);
#endif 
            go.transform.position = grid.TileToPos(new Vector2Int(grisPos.x, grisPos.y));
            go.name = "Cloud_" + grisPos;

            //Check if Plant
            Cloud cloud = go.GetComponent<Cloud>();
            if (cloud == null)
            {
                Debug.LogError("can't Find WaterSource on the object", go);
            }

            allClouds.Add(cloud);
            //Link Element and Tile
            cloud.tileOn = grid.GetTile(grisPos);
            grid.GetTile(grisPos).element = cloud;
        }
    }

    public void SpawnMagicTreeAt(Vector2Int grisPos)
    {
        if (!grid.GetTile(grisPos).isElement)
        {
#if UNITY_EDITOR
            GameObject go = PrefabUtility.InstantiatePrefab(magicTree_Template, elementContainer) as GameObject;
#else
            GameObject go = Instantiate(magicTree_Template, elementContainer);
#endif 
            go.transform.position = grid.TileToPos(new Vector2Int(grisPos.x, grisPos.y));
            go.name = "MagicTree_" + grisPos;

            //Check if MagicTree
            MagicTree magicTree = go.GetComponent<MagicTree>();
            if (magicTree == null)
            {
                Debug.LogError("can't Find WaterSource on the object", go);
            }

            allMagicTrees.Add(magicTree);
            //Link Element and Tile
            magicTree.tileOn = grid.GetTile(grisPos);
            grid.GetTile(grisPos).element = magicTree;
        }
    }

    public void SpawnLakeAt(Vector2Int grisPos, bool vertical)
    {
        GameTile CurrentTile = grid.GetTile(grisPos);
        Debug.Log(grid.GetTile(grisPos));

        if (CurrentTile.receivedFlow > FlowStrenght._00_)
        {
            if (CurrentTile.linkedTile.Count == 2)
            {

#if UNITY_EDITOR
                GameObject go = PrefabUtility.InstantiatePrefab(lake_Template, elementContainer) as GameObject;
#else
            GameObject go = Instantiate(lake_Template, elementContainer);
#endif 
                go.transform.position = grid.TileToPos(new Vector2Int(grisPos.x, grisPos.y));
                go.name = "Lake_" + grisPos;
                //Check if Plant
                Lake lake = go.GetComponent<Lake>();
                if (lake == null)
                {
                    Debug.LogError("can't Find WaterSource on the object", go);
                }

                allLakes.Add(lake);
                //Link Element and Tile
                lake.tileOn = grid.GetTile(grisPos);
                //lake get les tiles
                grid.GetTile(grisPos).element = lake;
                if (vertical == true)
                {
                    lake.isVertical = true;
                    lake.allTilesOn = new GameTile[3];
                    lake.allTilesOn[0] = CurrentTile.neighbors[1];
                    lake.allTilesOn[1] = CurrentTile;
                    lake.allTilesOn[2] = CurrentTile.neighbors[5];
                }
                else
                {
                    lake.isVertical = false;
                    lake.allTilesOn = new GameTile[3];
                    lake.allTilesOn[0] = CurrentTile.neighbors[3];
                    lake.allTilesOn[1] = CurrentTile;
                    lake.allTilesOn[2] = CurrentTile.neighbors[7];
                }
                //assigner le lac au tiles
                for (int i = 0; i < lake.allTilesOn.Length; i++)
                {
                    lake.allTilesOn[i].element = lake;
                }
            }

        }


    }

#if UNITY_EDITOR
    [Button("Spawn Plant At")]
    private void UXspawnPlant()
    {
        SpawnPlantAt(new Vector2Int(posX, posY));
    }
    [Button("Spawn WaterSource At")]
    private void UXspawnWaterSource()
    {
        SpawnWaterSourceAt(new Vector2Int(posX, posY));
    }

    private void OnDrawGizmosSelected()
    {
        if (grid != null)
        {
            Gizmos.color = new Color(0.8f,0,0,0.5f);

         Gizmos.DrawCube(grid.GetTile(new Vector2Int(posX, posY)).worldPos, Vector3.one * grid.cellSize);
        }
    }
#endif
}
