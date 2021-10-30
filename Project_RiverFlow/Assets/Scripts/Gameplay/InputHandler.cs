using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputHandler : MonoBehaviour
{
    [Header("ref�rence")]
    public Camera cam;
    public Transform camTransf;
    [Space(10)]
    public GameGrid grid;

    #region Event
    [Header("Event")]
    public UnityEvent onLeftClickDown;
    public UnityEvent onRightClickDown;
    [Space(5)]
    public UnityEvent onLeftClicking;
    public UnityEvent onRightClicking;
    [Space(5)]
    public UnityEvent onLeftClickUp;
    public UnityEvent onRightClickUp;
    #endregion

    [Header("Internal Value")]
    public Plane inputSurf = new Plane(Vector3.back, Vector3.zero);
    [Space(10)]
    [SerializeField] public Ray ray;
    [SerializeField] public float hitDist = 0f;
    [SerializeField] public Vector3 hitPoint = Vector3.zero;

    [Header("Digging")]
    [SerializeField] public GameTile startSelectTile;
    [SerializeField] public Vector3 startSelectTilePos;
    [Space(10)]
    [SerializeField] public GameTile endSelectTile;
    [SerializeField] public Vector3 endSelectPos;
    [Space(10)]
    [SerializeField] public Vector3 dragPos;
    [SerializeField] public Vector3 dragVect;

    [Header("Eraser")]
    [SerializeField] public GameTile eraserSelectTile;

    private void Start()
    {
    }

    void Update()
    {
        CheckInput();
    }

    //M�thodes
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
            Debug.LogError("Ray parrall�le to plane", this);
        }
        return hitPoint;
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
            startSelectTilePos = Vector3.zero;
            //
            endSelectTile = null;
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

            onRightClicking?.Invoke();
        }
        //OnRelease
        if (Input.GetMouseButtonUp(1))
        {
            onRightClickUp?.Invoke();
        }
        #endregion
    }
    
}
