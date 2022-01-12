using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class QuaternionVisualizer : MonoBehaviour
{
    //Rotate 2D
    //public float angle = 0f;
    [Space(10)]
    public float x = 0f;
    public float y = 0f;
    public float z = 0f;
    public float w = 1f;

    private void OnDrawGizmos()
    {
        //Rotate 2D
        //Draw.Cone(transform.position, new Quaternion(Mathf.Sin(angle), Mathf.Cos(angle), z, w).normalized, 0.5f, 2f);

        //Default
        Draw.Cone(transform.position, new Quaternion(x, y, z, w).normalized, 0.5f, 2f);
    }
}
