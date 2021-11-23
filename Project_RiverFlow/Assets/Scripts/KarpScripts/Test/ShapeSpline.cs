using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class ShapeSpline : MonoBehaviour
{
    public Vector3 a = new Vector3(0, 0, 0);
    public Vector3 b = new Vector3(-2, 1, 1);
    public Vector3 c = new Vector3(2, 1, 3);
    public Vector3 d = new Vector3(0, 0, 4);
    public float lineThickness = 0.05f;
    public bool polyline = false;
    public int pointCount = 48;

    void OnDrawGizmos()
    {

        Draw.PolylineGeometry = PolylineGeometry.Flat2D;
        using (var path = new PolylinePath())
        {
            path.AddPoint(a);
            path.BezierTo(b, c, d, pointCount);
            Draw.Polyline(path, lineThickness, PolylineJoins.Round);
        }
    }

    static Vector3 GetBezierPt(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
        float omt = 1f - t;
        float omt2 = omt * omt;
        return a * (omt2 * omt) + b * (3f * omt2 * t) + c * (3f * omt * t * t) + d * (t * t * t);
    }

}
