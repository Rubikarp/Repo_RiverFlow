using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivergeanceChoice : MonoBehaviour
{
    public Vector2Int gridPos;
    public GameTile tileOn;
    [Space(10)]
    public GameObject visual;
    [Space(10)]
    public GameObject arrowA;
    public GameObject arrowB;

    Vector2 dir;
    float angle;
    Quaternion rot;
    public void UpdateChoice()
    {
        if(tileOn.flowOut.Count < 2)
        {
            visual.SetActive(false);
        }
        else
        {
            visual.SetActive(true);

            // Rotate quaternion
            rot = Direction.ToQuat2D(tileOn.flowOut[0].dirEnum);
            arrowA.transform.rotation = rot;
            //
            rot = Direction.ToQuat2D(tileOn.flowOut[0].dirEnum);
            arrowB.transform.rotation = rot;
        }
    }
}
