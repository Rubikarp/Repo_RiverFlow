using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class RiverSpline : MonoBehaviour
{
    public List<RiverPoint> points = new List<RiverPoint>() { new RiverPoint(Vector2.zero), new RiverPoint(Vector2.zero) };
    public RiverPalette_SCO riverData;
    public float lineThicknessFactor = 1f;
    [Range(2, 16)] public int pointPerLine = 8;
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
            //Line
            if (points.Count > 2)
            {

                path.AddPoint(points[0].ToPolyLine());
                path.AddPoints(DrawCurveSegment(
                    new CatmullRiverSegment(
                        points[0].pos + (points[1].pos - points[0].pos),
                        points[0].pos,
                        points[1].pos,
                        points[2].pos)
                    , points[0], points[1]));

                for (int i = 1; i < points.Count - 2; i++)
                {
                    path.AddPoints(DrawCurveSegment(GetCurve(i), points[i], points[i + 1]));
                }

                path.AddPoints(DrawCurveSegment(
                     new CatmullRiverSegment(
                         points[points.Count - 3].pos,
                         points[points.Count - 2].pos,
                         points[points.Count - 1].pos,
                         points[points.Count - 1].pos + (points[points.Count - 1].pos - points[points.Count - 2].pos))
                       , points[points.Count - 2], points[points.Count - 1]));
            }
            else if (points.Count == 2)
            {
                path.AddPoint(points[0].ToPolyLine());
                path.AddPoints(DrawCurveSegment(
                    new CatmullRiverSegment(
                        points[0].pos + (points[1].pos - points[0].pos),
                        points[0].pos,
                        points[1].pos,
                        points[1].pos + (points[0].pos - points[1].pos))
                    , points[0], points[1]));
            }
            else
            {
                Debug.LogWarning("Need more point", this);
            }

            //EndCap
            Draw.Disc(points[0].pos, Vector3.back, points[0].thickness * 0.5f * lineThicknessFactor, (DiscColors)points[0].color);
            Draw.Disc(points[points.Count-1].pos, Vector3.back, points[points.Count - 1].thickness * 0.5f * lineThicknessFactor, (DiscColors)points[points.Count - 1].color);

            //Arrow
            Vector2 dir = points[1].pos - points[0].pos;
            //Rotation un peu foireuse
            Quaternion rot = new Quaternion(1,0,Vector2.Dot(Vector2.right,dir),1);
            Draw.Cone(points[1].pos, rot.normalized, 0.5f * lineThicknessFactor, 1f);

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

    private PolylinePoint[] LinerInterpolation(RiverPoint lastPoint, RiverPoint newPoint, int iter)
    {
        PolylinePoint[] result = new  PolylinePoint[iter];
        float step = 1f / (float)iter;
        for (int i = 0; i < iter; i++)
        {
            result[i] = RiverPoint.Lerp(lastPoint, newPoint, (i+1) * step).ToPolyLine();
        }
        return result;
    }

    CatmullRiverSegment GetCurve(int i)
    {
        return new CatmullRiverSegment(points[i-1].pos, points[i].pos, points[i + 1].pos, points[i + 2].pos);
    }
    private PolylinePoint[] DrawCurveSegment(CatmullRiverSegment curve, RiverPoint From, RiverPoint To)
    {
        PolylinePoint[] result = new PolylinePoint[pointPerLine - 1];

        RiverPoint temp = From;

        for (int i = 1; i < pointPerLine; i++)
        {
            float t = i / (pointPerLine - 1f);
            temp = RiverPoint.Lerp(From, To, t);
            temp.pos = curve.GetPointPos(t);
            result[i-1] = temp.ToPolyLine();
        }
        return result;
    }

    private PolylinePoint[] DrawCurveSegmentRatio(CatmullRiverSegment curve, RiverPoint From, RiverPoint To)
    {
        //Position
        Vector3[] resultPoses = new Vector3[pointPerLine - 1];
        for (int i = 1; i < pointPerLine; i++)
        {
            float t = i / (pointPerLine - 1f);
            resultPoses[i - 1] = curve.GetPointPos(t);
        }

        //Points
        float totDist = 0;
        for (int i = 0; i < resultPoses.Length-1; i++)
        {
            totDist += (resultPoses[i + 1] - resultPoses[i]).magnitude;
        }
        float step = 1f/ totDist;

        PolylinePoint[] result = new PolylinePoint[pointPerLine - 1];
        RiverPoint temp = From;
        for (int i = 0; i < resultPoses.Length; i++)
        {
            temp = RiverPoint.Lerp(From, To, i*step/totDist);
            temp.pos = resultPoses[i];
            result[i] = temp.ToPolyLine();
        }

        return result;
    }
    //
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
                    points.Add(new RiverPoint(Vector2.zero));
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