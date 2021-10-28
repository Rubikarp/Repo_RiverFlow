using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [Header("reférence")]
    public Camera cam;
    public Transform camTransf;
    [Space(10)]
    public Grid grid;
    [Space(10)]
    public LineRenderer line;

    [Header("Internal Value")]
    public Plane inputSurf = new Plane(Vector3.back, Vector3.zero);

    private Ray ray;
    private float hitDist = 0f;
    private Vector3 hitPoint = Vector3.zero;

    private Tile startSelectTile;
    private TileGround startSelectTileGround;
    private Vector3 startSelectTilePos;

    private Tile endSelectTile;
    private TileGround endSelectTileGround;
    private Vector3 endSelectPos;

    private Vector3 dragPos;

    void Update()
    {
        //OnClickDown
        if (Input.GetMouseButtonDown(0))
        {
            startSelectTile = grid.GetTile(grid.PosToTile(GetHitPos()));
            startSelectTilePos = grid.TileToPos(startSelectTile.position);
            if(startSelectTile is TileGround)
            {
                startSelectTileGround = startSelectTile.GetComponent<TileGround>();
                line.positionCount = 1;
                line.SetPosition(0, startSelectTilePos);
            }
            else
            {
                startSelectTileGround = null;
            }
        }

        //OnDrag
        if (Input.GetMouseButton(0))
        {
            dragPos = GetHitPos();
            line.positionCount = 2;
            line.SetPosition(1, dragPos);

            //Have drag a certainDistance
            Vector3 dragVect = (dragPos - startSelectTilePos);
            Debug.DrawLine(dragPos, startSelectTilePos, Color.blue);
            if(Mathf.Abs(dragVect.x) > grid.cellSize || Mathf.Abs(dragVect.y) > grid.cellSize)
            {
                //Check la ou je touche
                endSelectTile = grid.GetTile(grid.PosToTile(GetHitPos()));
                endSelectPos = dragPos;
                if (endSelectTile is TileGround)
                {
                    endSelectTileGround = endSelectTile.GetComponent<TileGround>();
                }
                else
                {
                    endSelectTileGround = null;
                }

                //Si j'ai bien 2 tile linkable
                if(startSelectTileGround != null && endSelectTileGround != null)
                {
                    //Make the Flow
                    startSelectTileGround.isDuged = true;
                    ///TODO :startSelectTileGround.flowOut.Add();
                    endSelectTileGround.isDuged = true;
                    ///TODO :endSelectTileGround.flowIn.Add();

                    //End became the new start
                    startSelectTile = endSelectTile;
                    startSelectTileGround = endSelectTileGround;
                    startSelectTilePos = grid.TileToPos(startSelectTile.position);
                    line.SetPosition(0, startSelectTilePos);

                }
            }

            /* EasyHole/Old Méthode
            if (startSelectTile is TileGround)
            {
                TileGround tileSelect = startSelectTile.GetComponent<TileGround>();
                tileSelect.isDuged = true;
            }
            */
        }

        //OnRelease
        if (Input.GetMouseButtonUp(0))
        {
            startSelectTile = null;
            startSelectTileGround = null;
            startSelectTilePos = Vector3.zero;

            endSelectTile = null;
            endSelectTileGround = null;
            endSelectPos = Vector3.zero;

            dragPos = Vector3.zero;
            line.positionCount = 0;
        }

        if (Input.GetMouseButton(1))
        {
            Tile tileSelected = grid.GetTile(grid.PosToTile(GetHitPos()));
            if (tileSelected is TileGround)
            {
                TileGround tileSelect = tileSelected.GetComponent<TileGround>();
                tileSelect.isDuged = false;
            }
        }
    }

    public Vector3 GetHitPos()
    {
        //Reset HitPoint
        hitPoint = Vector3.zero;
        //Get Ray
        ray = cam.ScreenPointToRay(Input.mousePosition);
        //Raycast
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
