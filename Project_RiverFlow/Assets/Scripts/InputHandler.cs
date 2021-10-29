using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputHandler : MonoBehaviour
{
    [Header("reférence")]
    public Camera cam;
    public Transform camTransf;
    [Space(10)]
    public GameGrid grid;
    [Space(10)]
    public LineRenderer line;

    [Header("Event")]
    public UnityEvent onLeftClickDown;
    public UnityEvent onRightClickDown;
    [Space(5)]
    public UnityEvent onLeftClicking;
    public UnityEvent onRightClicking;
    [Space(5)]
    public UnityEvent onLeftClickUp;
    public UnityEvent onRightClickUp;

    [Header("Internal Value")]
    public Plane inputSurf = new Plane(Vector3.back, Vector3.zero);

    [SerializeField] public Ray ray;
    [SerializeField] public float hitDist = 0f;
    [SerializeField] public Vector3 hitPoint = Vector3.zero;

    [Header("Digging")]
    [SerializeField] public Tile startSelectTile;
    [SerializeField] public TileGround startSelectTileGround;
    [SerializeField] public Vector3 startSelectTilePos;

    [SerializeField] public Tile endSelectTile;
    [SerializeField] public TileGround endSelectTileGround;
    [SerializeField] public Vector3 endSelectPos;

    [SerializeField] public Vector3 dragPos;
    [SerializeField] public Vector3 dragVect;

    [Header("Eraser")]
    [SerializeField] public Tile eraserSelectTile;
    [SerializeField] public TileGround eraserSelectTileGround;

    private void Start()
    {
        onLeftClickDown.AddListener(OnLeftClickPress);
        onLeftClicking.AddListener(OnLeftClicking);
        onLeftClickUp.AddListener(OnLeftClickRelease);

        onRightClickUp.AddListener(OnRighClicking);
    }

    void Update()
    {
        CheckInput();
    }

    public void OnLeftClickPress()
    {
        if (startSelectTile is TileGround)
        {
            ///Line Rendering Part to remove in his own script
            line.positionCount = 1;
            line.SetPosition(0, startSelectTilePos);
        }

    }
    public void OnLeftClicking()
    {
        ///Line Rendering Part to remove in his own script
        line.positionCount = 2;
        line.SetPosition(1, dragPos);

        //Have drag a certainDistance        
        if (Mathf.Abs(dragVect.x) > grid.cellSize || Mathf.Abs(dragVect.y) > grid.cellSize)
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
            if (startSelectTileGround != null && endSelectTileGround != null)
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
    }
    public void OnLeftClickRelease()
    {
        ///Line Rendering Part to remove in his own script
        line.positionCount = 0;
    }
    public void OnRighClicking()
    {
        eraserSelectTileGround.isDuged = false;
    }

    public void CheckInput()
    {
        #region leftClick
        //OnPress
        if (Input.GetMouseButtonDown(0))
        {
            //Initialise start select
            startSelectTile = grid.GetTile(grid.PosToTile(GetHitPos()));
            startSelectTilePos = grid.TileToPos(startSelectTile.position);
            //Check for TileGround
            if (startSelectTile is TileGround)
            {
                startSelectTileGround = startSelectTile.GetComponent<TileGround>();
            }
            else
            {
                startSelectTileGround = null;
            }

            onLeftClickDown?.Invoke();
        }
        //OnDrag
        if (Input.GetMouseButton(0))
        {
            dragPos = GetHitPos();
            dragVect = (dragPos - startSelectTilePos);

            onLeftClicking?.Invoke();
        }
        //OnRelease
        if (Input.GetMouseButtonUp(0))
        {
            onLeftClickUp?.Invoke();

            //Reset
            startSelectTile = null;
            startSelectTileGround = null;
            startSelectTilePos = Vector3.zero;
            //
            endSelectTile = null;
            endSelectTileGround = null;
            endSelectPos = Vector3.zero;
            //
            dragPos = Vector3.zero;
            dragVect = Vector3.zero;

        }
        #endregion
        #region rightClick
        //OnPress
        if (Input.GetMouseButtonDown(1))
        {
            onRightClickDown?.Invoke();
        }
        //OnDrag
        if (Input.GetMouseButton(1))
        {
            eraserSelectTile = grid.GetTile(grid.PosToTile(GetHitPos()));
            if (eraserSelectTile is TileGround)
            {
                eraserSelectTileGround = eraserSelectTile.GetComponent<TileGround>();
            }
            onRightClicking?.Invoke();
        }
        //OnRelease
        if (Input.GetMouseButtonUp(1))
        {
            onRightClickUp?.Invoke();
        }
        #endregion

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
