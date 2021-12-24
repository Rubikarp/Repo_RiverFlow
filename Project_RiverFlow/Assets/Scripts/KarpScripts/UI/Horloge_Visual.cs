using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horloge_Visual : MonoBehaviour
{
    public GameTime time;
    private Transform self;

    public float value;

    void Start()
    {
        time = GameTime.Instance;
        self = transform;
    }

    void Update()
    {
        float val = time.gameTimer * (1/ time.weekDuration) * 180;
        self.rotation = new Quaternion(Mathf.Sin(val * Mathf.Deg2Rad), Mathf.Cos(val * Mathf.Deg2Rad), 0, 0).normalized; ;
    }
}
