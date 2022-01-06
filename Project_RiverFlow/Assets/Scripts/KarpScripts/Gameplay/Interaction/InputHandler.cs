using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable] public class InputEvent : UnityEvent<InputMode> { }
public enum InputMode
{
    nothing = 0,
    diging = 1,
    eraser = 2,
    cloud = 3,
    lake = 4,
    source = 5,
}
public class InputHandler : Singleton<InputHandler>
{
    [Header("ref�rence")]
    public Camera cam;
    public Transform camTransf;
    [Space(10)]
    public GameGrid grid;

    #region Event
    [Header("Event")]
    public InputEvent onInputPress;
    public InputEvent onInputMaintain;
    public InputEvent onInputRelease;
    #endregion

    [Header("Internal Value")]
    public Plane inputSurf = new Plane(Vector3.back, Vector3.zero);
    [Space(10)]
    [SerializeField] public Ray ray;
    [SerializeField] public float hitDist = 0f;
    [SerializeField] public Vector3 hitPoint = Vector3.zero;

    [Header("Mode")]
    public KeyCode digMode = KeyCode.Alpha1;
    public KeyCode eraserMode = KeyCode.Alpha2;
    public KeyCode cloudMode = KeyCode.Alpha3;
    public KeyCode lakeMode = KeyCode.Alpha4;
    public KeyCode sourceMode = KeyCode.Alpha5;
    [Space(10)]
    public InputMode mode = InputMode.diging;
    //public InputMode secondaryMode = InputMode.eraser;

    private bool isMaintaining = false;
    private InputMode lastMode = InputMode.diging;

    void Update()
    {
        CheckMode();
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
    //
    private void CheckMode()
    {
        lastMode = mode;
        if (Input.GetKeyDown(digMode))
        {
            mode = InputMode.diging;
        }
        else 
        if (Input.GetKeyDown(eraserMode))
        {
            mode = InputMode.eraser;
        }
        else
        if (Input.GetKeyDown(cloudMode))
        {
            mode = InputMode.cloud;
        }
        else
        if (Input.GetKeyDown(lakeMode))
        {
            mode = InputMode.lake;
        }
        else
        if (Input.GetKeyDown(sourceMode))
        {
            mode = InputMode.source;
        }

        if(isMaintaining && lastMode != mode )
        {
            onInputRelease?.Invoke(lastMode);
            onInputPress?.Invoke(mode);
        }
    }
    private void CheckInput()
    {
        //OnPress
        if (Input.GetMouseButtonDown(0))
        {
            if (!KarpHelper.IsOverUI())
            {
                isMaintaining = true;
                onInputPress?.Invoke(mode);
            }
        }
        //OnDrag
        if (Input.GetMouseButton(0))
        {
            if (KarpHelper.IsOverUI())
            {
                onInputRelease?.Invoke(mode);
            }
            else 
            if(isMaintaining)
            {
                onInputMaintain?.Invoke(mode);
            }
        }
        //OnRelease
        if (Input.GetMouseButtonUp(0))
        {
            if (isMaintaining)
            {
                isMaintaining = false;
                onInputRelease?.Invoke(mode);
            }
        }
    }
    //
    public void ChangeMode(InputMode newMode)
    {
        lastMode = mode;
        mode = newMode;
        if (isMaintaining && lastMode != newMode)
        {
            onInputRelease?.Invoke(lastMode);
            onInputPress?.Invoke(mode);
        }
    }

}
