using System.Collections.Generic;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using Plane = UnityEngine.Plane;

public class InputManager : MonoBehaviour
{
    [Header("Camera")]
    public Camera mainCamera;

    [Header("Game")]
    public LayerMask selectableEntityLayer;
    [HideInInspector] public bool touchingGame;
    [HideInInspector] public bool getEntityTarget;
    [HideInInspector] public bool gettingEntityTarget;
    [HideInInspector] public bool canUseCam;
    [HideInInspector] public bool canZoomCam;
    [HideInInspector] public bool canMoveCam;

    //Touch inputs
    [HideInInspector]public Vector2 touchedSeaPosition = new Vector2(-9999, -9999);
    private Touch touch;
    private float distance = 0;
    private float lastDistance;

    //Inertie
    [SerializeField] private bool activateInertie = true;
    private float dragStartTime = 0;
    [SerializeField] private float inertialVelocity = 0;
    private Vector2 move = Vector2.zero;
    private Vector2 dragStartPos = Vector2.zero;

    //Raycasting
    List<RaycastResult> raycastResults = new List<RaycastResult>();
    EventSystem currentEventSystem;
    PointerEventData pointerData;

    //Map Limits
    [HideInInspector] public float minX;
    [HideInInspector] public float maxX;
    [HideInInspector] public float minY;
    [HideInInspector] public float maxY;


    void Start()
    {

    }

    void Update()
    {
        //Tap Input
        if(Input.touchCount == 1)
        {
            touch = Input.GetTouch(0);

            pointerData = new PointerEventData(currentEventSystem);
            pointerData.position = touch.position;
            currentEventSystem.RaycastAll(pointerData, raycastResults);

            if(raycastResults.Count < 1 || raycastResults.Count == 1)
            {
                touchingGame = true;
                
            }

            if (touch.deltaPosition.magnitude < 5f)
            {

            }
        }
        //Glide Input
        else if (Input.touchCount == 2)
        {

        }
        //No fingers on screen.
        else if (Input.touchCount == 0)
        {
            touchingGame = false;
        }
    }

    public Vector2 GetSeaPosition()
    {
        touch = Input.GetTouch(0);
        Ray touchRay;
        touchRay = mainCamera.ScreenPointToRay(touch.position);

        Plane ground = new Plane(Vector3.up, new Vector3(0, 0, 0));
        float distance;
        ground.Raycast(touchRay, out distance);

        return Coordinates.ConvertWorldToVector2(touchRay.GetPoint(distance));
    }
}
public struct Coordinates
{
    public Vector2 position { get; set; }
    public Vector2 direction { get; set; }
    public float rotation { get; set; }

    /// <summary>
    /// Default constructor for the Coordinates struct. 
    /// </summary>
    /// <param name="position">OceanEntity position</param>
    /// <param name="direction">OceanEntity direction</param>
    /// <param name="rotation">OceanEntity rotation</param>
    public Coordinates(Vector3 position, Vector2 direction, float rotation)
    {
        this.position = ConvertWorldToVector2(position);
        this.direction= direction;
        this.rotation = rotation;
    }

    public static Vector2 ConvertWorldToVector2(Vector3 vector)
    {
        return new Vector2(vector.x, vector.z);
    }

    public static Vector3 ConvertVector2ToWorld(Vector2 vector)
    {
        return new Vector3(vector.x, 0, vector.y);
    }

}