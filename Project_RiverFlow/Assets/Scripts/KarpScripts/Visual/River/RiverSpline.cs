using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class RiverSpline : MonoBehaviour
{
    public List<RiverPoint> points = new List<RiverPoint>() { new RiverPoint(Vector2.zero, Vector3.up), new RiverPoint(Vector2.zero, Vector3.up) };
    public RiverPalette_SCO riverData;
    public float lineThicknessFactor = 1f;
    [Range(1, 16)] public int pointPerLine = 8;
    //
    PolylinePath path;
    //
    public Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
    }

    void OnDrawGizmos()
    {
        Draw.PolylineGeometry = PolylineGeometry.Flat2D;
        using (var path = new PolylinePath())
        {
            path.AddPoint(points[0].ToPolyLine());
            for (int i = 1; i < points.Count; i++)
            {
                path.AddPoints(MultiplyLine(points[i - 1], points[i], pointPerLine));
            }
            Draw.Polyline(path, false, lineThicknessFactor, PolylineJoins.Round, Color.white);
        }

        #region Bezier
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
        #endregion

    }

    private PolylinePoint[] MultiplyLine(RiverPoint lastPoint, RiverPoint newPoint, int iter)
    {
        PolylinePoint[] result = new  PolylinePoint[iter];
        float step = 1f / (float)iter;
        for (int i = 0; i < iter; i++)
        {
            result[i] = RiverPoint.Lerp(lastPoint, newPoint, (i+1) * step).ToPolyLine();
        }
        return result;
    }

    public void SetPointCount(int pointNbr)
    {
        //Floor to 0
        Mathf.Max( 0, pointNbr);

        if (points.Count < pointNbr)
        {
            for (int i = points.Count; i < pointNbr; i = points.Count)
            {
                if (points.Count == 0)
                {
                    points.Add(new RiverPoint(Vector2.zero, Vector3.up));
                }
                else
                {
                    points.Add(points[points.Count - 1]);
                }
            }
        }
        else if (points.Count > pointNbr)
        {
            for (int i = points.Count; i > pointNbr; i = points.Count)
            {
                points.Remove(points[points.Count-1]);
            }
        }
    }
    public void DefinePoint(int index, Vector3 pos, FlowStrenght riverStregth)
    {
        RiverPoint newPoints = new RiverPoint(pos);

        switch (riverStregth)
        {
            case FlowStrenght._00_:
                newPoints.thickness = riverData.debit_00;
                newPoints.color = riverData.color_00;
                break;
            case FlowStrenght._25_:
                newPoints.thickness = riverData.debit_25;
                newPoints.color = riverData.color_25;

                break;
            case FlowStrenght._50_:
                newPoints.thickness = riverData.debit_50;
                newPoints.color = riverData.color_50;

                break;
            case FlowStrenght._75_:
                newPoints.thickness = riverData.debit_75;
                newPoints.color = riverData.color_75;

                break;
            case FlowStrenght._100_:
                newPoints.thickness = riverData.debit_100;
                newPoints.color = riverData.color_100;

                break;
            default:
                break;
        }

        points[index] = newPoints;

    }

}