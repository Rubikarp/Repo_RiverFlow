using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomManager : MonoBehaviour
{

    public Camera Cam;
    public TimeManager Timer;
    public float minZoom;
    public float maxZoom;
    public float zoomIncrement;
    private float startZoom;

    void Start()
    {
        startZoom = minZoom;
        Cam.orthographicSize = startZoom;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Cam.orthographicSize = Mathf.Clamp(startZoom + zoomIncrement * (Timer.weekNumber-1), minZoom, maxZoom);
    }
}
