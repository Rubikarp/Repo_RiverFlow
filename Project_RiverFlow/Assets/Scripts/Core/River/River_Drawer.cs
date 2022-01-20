using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    private void Start()
    {
        grid = GameGrid.Instance;
    }

    void FixedUpdate()
    {
        UpdateRiverVisual();
    }
    public void UpdateRiverVisual()
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

        //Lie chaque riverRender à son canal
        for (int i = 0; i < riverRender.Count; i++)
        {
            if (riverHandler.canals[i] != null)
            {
                riverRender[i].canalTreated = riverHandler.canals[i];
            }
        }
    }
}

