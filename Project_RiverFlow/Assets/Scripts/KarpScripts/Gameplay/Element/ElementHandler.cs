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
    public GameObject plant_Template;
    [Space(10)]
    public Transform elementContainer;

    [Header("Tool"), HorizontalLine]
    public List<Plant> allPlants = new List<Plant>();
    [Space(10)]
    public List<WaterSource> allSources = new List<WaterSource>();

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
            GameObject go = PrefabUtility.InstantiatePrefab(plant_Template, elementContainer)as GameObject;
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
            GameObject go = PrefabUtility.InstantiatePrefab(waterSource_Template, elementContainer) as GameObject;
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
