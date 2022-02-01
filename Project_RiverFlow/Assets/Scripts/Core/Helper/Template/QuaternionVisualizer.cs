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

    private void Update()
    {
        //Rotate 2D
        //Draw.Cone(transform.position, new Quaternion(Mathf.Sin(angle), Mathf.Cos(angle), z, w).normalized, 0.5f, 2f);

        //Default
        transform.rotation = new Quaternion(x, y, z, w).normalized;

    }

    [NaughtyAttributes.Button]
    public void Normalized()
    {
        x = transform.rotation.x;
        y = transform.rotation.y;
        z = transform.rotation.z;
        w = transform.rotation.w;
    }
}
