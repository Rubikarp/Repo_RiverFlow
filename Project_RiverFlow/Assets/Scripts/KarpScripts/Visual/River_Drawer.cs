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
    public GameObject lineRendererTemplate;
    public List<LineRenderer> linesRender = new List<LineRenderer>();
    public List<Vector3[]> riverLine = new List<Vector3[]>();

    Vector3[] _tempRiver;
    private void Start()
    {
        grid = GameGrid.instance;
    }

    void Update()
    {
        RiverGeneration();
        RiverCurve();
    }

    public void RiverGeneration()
    {
        //Faudra faire du pooling
        if (linesRender.Count < riverHandler.canals.Count)
        {
            linesRender.Add(Instantiate(lineRendererTemplate, transform.position, Quaternion.identity, transform).GetComponent<LineRenderer>());
        }
        else
        if (linesRender.Count > riverHandler.canals.Count)
        {
            for (int i = riverHandler.canals.Count; i < linesRender.Count; i++)
            {
                Destroy(linesRender[i].gameObject);
            }
        }
    }
    public void RiverCurve()
    {
        for (int i = 0; i < linesRender.Count; i++)
        {
            //Tiles.Count + 2 => (tiles + startNode + endNode)
            int lineSize = riverHandler.canals[i].canalTiles.Count + 2;
            linesRender[i].positionCount = lineSize;
            _tempRiver = new Vector3[lineSize];

            //Set points
            _tempRiver[0] = grid.GetTile(riverHandler.canals[i].startNode).worldPos;
            for (int j = 0; j < riverHandler.canals[i].canalTiles.Count; j++)
            {
                _tempRiver[j+1] = grid.GetTile(riverHandler.canals[i].canalTiles[j]).worldPos;

            }
            _tempRiver[_tempRiver.Length-1] = grid.GetTile(riverHandler.canals[i].endNode).worldPos;

            linesRender[i].SetPositions(_tempRiver);
        }
    }
}

