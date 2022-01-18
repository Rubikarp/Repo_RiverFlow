using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
        Timer = TimeManager.Instance;
        Timer.getMoreDig.AddListener(Zoom);
        startZoom = minZoom;
        Cam.orthographicSize = startZoom;

    }

    // Update is called once per frame
    public void Zoom()
    {
        Cam.DOOrthoSize(Mathf.Clamp(startZoom + zoomIncrement * (Timer.weekNumber - 1), minZoom, maxZoom), 1f);
    }
}
