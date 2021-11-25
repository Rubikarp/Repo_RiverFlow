using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class RiverSpline : MonoBehaviour
{
    public List<RiverPoint> points;
    public float lineThicknessFactor = 1f;
    public int pointPerLine = 8;

    void Start()
    {

    }

    void Update()
    {
        /*
        Draw.PolylineGeometry = PolylineGeometry.Flat2D;
        using (var path = new PolylinePath())
        {

            path.AddPoint(points[0].pos, points[0].thickness, points[0].color);
            for (int i = 0; i < points.Count; i++)
            {
                if ((i + 1) < points.Count)
                {
                    int next = (i + 1);

                    Vector3 startTangPos = points[i + 0].pos + points[i + 0].tangDir * points[i + 0].tangStrenght;
                    Vector3 endTangPos = points[i + 1].pos - points[i + 1].tangDir * points[i + 1].tangStrenght;

                    //Draw the path
                    path.BezierTo(startTangPos, endTangPos, points[next].pos, pointPerLine);

                    Draw.Polyline(path, false, lineThicknessFactor, PolylineJoins.Round);

                }
            }
        }
        */
    }

    void OnDrawGizmos()
    {
        Draw.PolylineGeometry = PolylineGeometry.Flat2D;
        using (var path = new PolylinePath())
        {

            path.AddPoint(points[0].pos, points[0].thickness, points[0].color);
            for (int i = 0; i < points.Count; i++)
            {
                if ((i + 1) < points.Count)
                {
                    int next = (i + 1);

                    Vector3 startTangPos = points[i + 0].pos + points[i + 0].tangDir * points[i + 0].tangStrenght;
                    Vector3 endTangPos = points[i + 1].pos - points[i + 1].tangDir * points[i + 1].tangStrenght;

                    //Draw the path
                    path.BezierTo(startTangPos, endTangPos, points[next].pos, pointPerLine);

                    Draw.Polyline(path, true, lineThicknessFactor, PolylineJoins.Round, Color.white);

                }
            }
        }
    }
}