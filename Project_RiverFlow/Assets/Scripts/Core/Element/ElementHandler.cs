using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NaughtyAttributes;
using UnityEngine.Events;

public class ElementHandler : Singleton<ElementHandler>
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

    [Header("Sounds"), HorizontalLine]
    public string sourceSound = "Source";
    public string cloudSound = "Cloud";
    public string lakeSound = "Lake";

    private void Awake()
    {
        for (int i = 0; i < allPlants.Count; i++)
        {
            LinkElementToGrid(allPlants[i]);
        }
        for (int i = 0; i < allSources.Count; i++)
        {
            LinkElementToGrid(allSources[i]);
        }
    }
    #region Spawn
    public void SpawnPlantAt(Vector2Int gridPos)
    {
        if (!grid.GetTile(gridPos).haveElement && grid.GetTile(gridPos).canalsIn.Count == 0)
        {
            GameObject go = InstanciateElement(plant_Template);
            go.transform.position = grid.TileToPos(new Vector2Int(gridPos.x, gridPos.y));
            go.name = "Plant_" + gridPos;

            //Check if Plant
            Plant plant = go.GetComponent<Plant>();
            if (plant == null)
            {
                Debug.LogError("can't Find Plant on the object", go);
                return;
            }

            allPlants.Add(plant);

            //Link Element and Tile
            plant.gridPos = gridPos;
            LinkElementToGrid(plant);

            PositionRendererSorter.SortTreePositionOderInLayer(plant.GetComponent<SpriteRenderer>(), plant.GetComponent<Transform>());

        }
    }
    public void SpawnWaterSourceAt(Vector2Int grisPos)
    {
        if (!grid.GetTile(grisPos).haveElement)
        {
            GameObject go = InstanciateElement(waterSource_Template);
            go.transform.position = grid.TileToPos(new Vector2Int(grisPos.x, grisPos.y));
            go.name = "Source_" + grisPos;

            //sound
            LevelSoundboard.Instance.PlaySourceEffectSound(sourceSound);
            //Check if Plant
            WaterSource source = go.GetComponent<WaterSource>();
            if (source == null)
            {
                Debug.LogError("can't Find WaterSource on the object", go);
            }

            allSources.Add(source);

            //Link Element and Tile
            source.gridPos = grisPos;
            source.TileOn = grid.GetTile(grisPos);

            grid.GetTile(grisPos).element = source;
        }
    }
    public void SpawnCloudAt(Vector2Int grisPos)
    {
        GameTile aimedTile = grid.GetTile(grisPos);

                GameObject go = InstanciateElement(cloud_Template);
                go.transform.position = grid.TileToPos(new Vector2Int(grisPos.x, grisPos.y));
                go.name = "Cloud_" + grisPos;

        //sound
        LevelSoundboard.Instance.PlayCloudEffectSound(cloudSound);
        //Check if Plant
        Cloud cloud = go.GetComponent<Cloud>();
                if (cloud == null)
                {
                    Debug.LogError("can't Find WaterSource on the object", go);
                }

                allClouds.Add(cloud);
                //Link Element and Tile
                cloud.gridPos = grisPos;
                cloud.TileOn = aimedTile;
                aimedTile.element = cloud;

    }
    public void SpawnMagicTreeAt(Vector2Int grisPos)
    {
        if (!grid.GetTile(grisPos).haveElement)
        {
            Debug.Log("MAGIC SPAWN");
            GameObject go = InstanciateElement(magicTree_Template);
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
            magicTree.gridPos = grisPos;
            magicTree.tileOn = grid.GetTile(grisPos);
            grid.GetTile(grisPos).element = magicTree;
        }
    }
    public void SpawnLakeAt(Vector2Int grisPos, bool vertical,GameTile previousTile)
    {

        GameObject go = InstanciateElement(lake_Template);
        go.transform.position = grid.TileToPos(new Vector2Int(grisPos.x, grisPos.y));
        go.name = "Lake_" + grisPos;

        //sound
        LevelSoundboard.Instance.PlayLakeEffectSound(lakeSound);
        //Check if Lake
        Lake lake = go.GetComponent<Lake>();
        lake.entryTile = previousTile;
        if (lake == null)
        {
            Debug.LogError("can't Find WaterSource on the object", go);
        }
        allLakes.Add(lake);

        //Link Element and Tile
        lake.gridPos = grisPos;
        lake.tileOn = grid.GetTile(grisPos);

        //lake get les tiles
        GameTile CurrentTile = grid.GetTile(grisPos);
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
    /// 
    public GameObject InstanciateElement(GameObject template)
    {
#if UNITY_EDITOR
        return PrefabUtility.InstantiatePrefab(template, elementContainer) as GameObject;
#else
        return Instantiate(template, elementContainer);
#endif 
    }
    public void LinkElementToGrid(Element element)
    {
        GameTile elementTile = grid.GetTile(pos: element.gridPos);
        //Link Element and Tile
        elementTile.element = element;
        element.TileOn = elementTile;
    }
    public void UnLinkElementToGrid(Element element)
    {
        //Link Element and Tile
        grid.GetTile(element.gridPos).element = null;
        element.TileOn = null;
    }
    #endregion

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
    public void UpdateSoundRiver()
    {
        LevelSoundboard.Instance.riverPresent = false;
        for (int i = 0; i < allSources.Count; i++)
        {
            if (allSources[i].TileOn.linkAmount > 0)
            {
                LevelSoundboard.Instance.riverPresent = true;
            }
        }
        LevelSoundboard.Instance.ChangeBackGroundSoundVolume();

    }
}
