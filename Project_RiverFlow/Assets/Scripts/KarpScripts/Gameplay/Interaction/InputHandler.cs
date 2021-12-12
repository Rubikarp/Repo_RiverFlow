using UnityEngine;
using UnityEngine.Events;

public class InputHandler : MonoBehaviour
{
    [Header("reférence")]
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

    void Update()
    {
        CheckInput();
    }

    //Méthodes
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
    public void CheckInput()
    {
        #region leftClick
        //OnPress
        if (Input.GetMouseButtonDown(0))
        {
            onLeftClickDown?.Invoke();
        }
        //OnDrag
        if (Input.GetMouseButton(0))
        {
            onLeftClicking?.Invoke();
        }
        //OnRelease
        if (Input.GetMouseButtonUp(0))
        {
            onLeftClickUp?.Invoke();
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
