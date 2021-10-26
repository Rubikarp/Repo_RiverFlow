using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Plane = UnityEngine.Plane;

public class Pointer : MonoBehaviour
{
    [Header("reférence")]
    public Camera cam;
    public Transform camTransf;
    [Space(10)]
    public GameGrid grid;

    private void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Tile tileSelected = grid.GetTile(grid.PosToTile(GetHitPos()));
            tileSelected.state = TileState.Hole;
        }
        else
        if (Input.GetMouseButton(1))
        {
            Tile tileSelected = grid.GetTile(grid.PosToTile(GetHitPos()));
            tileSelected.state = TileState.Full;
        }
    }

    public Vector3 GetHitPos()
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
            Debug.LogError("Ray parrallèle to plane", this);
        }

        return hitPoint;
    }

}
