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
        if(tileOn.flowOut.Count >= 2)
        {
            visual.SetActive(false);
        }
        else
        {
            visual.SetActive(true);

            // Rotate quaternion
            dir = tileOn.flowOut[0].dirValue;
            dir.y = dir.y * -1; //trouver un meilleur fix
            angle = Vector2.SignedAngle(Vector2.right, dir);
            rot = new Quaternion(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad), 0, 0).normalized;
            arrowA.transform.rotation = rot;

            dir = tileOn.flowOut[1].dirValue;
            dir.y = dir.y * -1; //trouver un meilleur fix
            angle = Vector2.SignedAngle(Vector2.right, dir);
            rot = new Quaternion(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad), 0, 0).normalized;
            arrowB.transform.rotation = rot;
        }
    }
}
