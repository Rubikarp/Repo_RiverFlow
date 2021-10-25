using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Plane = UnityEngine.Plane;

public class Pointer : MonoBehaviour
{
    public Camera cam;
    public Transform camTransf;

    public GameObject testTemplate;

    private void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Plane inputSurf = new Plane(Vector3.back, Vector3.zero);
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            float hitDist;

            if (inputSurf.Raycast(ray, out hitDist))
            {
                Vector3 hitPoint = camTransf.forward * hitDist;
            }
            else
            {
                Debug.Log("Ray parrallèle to plane");
            }

            Instantiate(testTemplate, hitPoint, Quaternion.identity,)
        }
    }
}
