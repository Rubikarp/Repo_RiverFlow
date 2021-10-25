using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Plane = UnityEngine.Plane;

public class Pointer : MonoBehaviour
{
    public Camera cam;
    public Transform camTransf;

    public GameObject testTemplate;

    public GameGrid grid;

    private void Start()
    {
        
    }

    void Update()
    {
        //TODO : Make it a proper methode to re-use
        if (Input.GetMouseButtonDown(0))
        {
            Plane inputSurf = new Plane(Vector3.back, Vector3.zero);
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            float hitDist;

            Vector3 hitPoint = Vector3.zero;

            if (inputSurf.Raycast(ray, out hitDist))
            {
                hitPoint = ray.GetPoint(hitDist);
            }
            else
            {
                Debug.Log("Ray parrallèle to plane");
            }

            Instantiate(testTemplate, grid.TileToPos(grid.PosToTile(hitPoint)), Quaternion.identity);
        }
    }
}
