using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class River_Drawer : MonoBehaviour
{
    [Header("References")]
    public GameGrid grid;
    public RiverManager riverHandler;

    [Header("Color")]
    [SerializeField] TilePalette_SCO palette;

    [Header("Parameter")]
    public GameObject riverRendererTemplate;
    public List<RiverSpline> riverRender = new List<RiverSpline>();
    public List<Vector3[]> riverLine = new List<Vector3[]>();

    Vector3[] _tempRiver;
    private void Start()
    {
        grid = GameGrid.instance;
    }

    void Update()
    {
        UpdateRiverVisual();
    }
    public void UpdateRiverVisual()
    {
        RiverGeneration();
        RiverCurve();
    }
    public void RiverGeneration()
    {
        //Faudra faire du pooling
        if (riverRender.Count < riverHandler.canals.Count)
        {
            riverRender.Add(Instantiate(riverRendererTemplate, transform.position, Quaternion.identity, transform).GetComponent<RiverSpline>());
        }
        else
        if (riverRender.Count > riverHandler.canals.Count)
        {
            for (int i = riverHandler.canals.Count; i < riverRender.Count; i++)
            {
                GameObject go = riverRender[i].gameObject;
                riverRender.Remove(riverRender[i]);
                Destroy(go);
            }
        }
    }
    public void RiverCurve()
    {
        for (int i = 0; i < riverRender.Count; i++)
        {
            if (riverHandler.canals[i] != null)
            {
                //Tiles.Count + 2 => (tiles + startNode + endNode)
                riverRender[i].SetPointCount(riverHandler.canals[i].canalTiles.Count + 2);

                //Set First Point
                riverRender[i].DefinePoint(0,
                    grid.GetTile(riverHandler.canals[i].startNode).worldPos,
                    grid.GetTile(riverHandler.canals[i].startNode).riverStrenght);
                //Set Middle Point
                for (int j = 0; j < riverHandler.canals[i].canalTiles.Count; j++)
                {
                    riverRender[i].DefinePoint(j + 1,
                            grid.GetTile(riverHandler.canals[i].canalTiles[j]).worldPos,
                            grid.GetTile(riverHandler.canals[i].canalTiles[j]).riverStrenght);
                }
                //Set Last Point
                riverRender[i].DefinePoint(riverRender[i].points.Count - 1,
                        grid.GetTile(riverHandler.canals[i].endNode).worldPos,
                        grid.GetTile(riverHandler.canals[i].endNode).riverStrenght);
            }
        }
    }
}

