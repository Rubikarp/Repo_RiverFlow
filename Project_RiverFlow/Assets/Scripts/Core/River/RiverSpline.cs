using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class RiverSpline : MonoBehaviour
{
    [Header("Shapes")]
    public bool custom;
    public Polyline line;
    public RiverMesh customLine;
    [Space(5)]
    public Cone cone;
    [Space(5)]
    public Disc startDisk;
    public Disc endDisk;

    [Header("Data")]
    [Range(2, 16)] public int pointPerSegment = 8;
    public RiverPalette_SCO riverData;
    public List<RiverPoint> points = new List<RiverPoint>() {};

    [Header("other")]
    private GameGrid grid;

    private void Start()
    {
        grid = GameGrid.Instance;
    }

    void Update()
    {
        if(points.Count != line.points.Count)
        {
            ReCalculCurve();
        }
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
    private PolylinePoint[] BezierInterpolation(CatmullRiverSegment curve, RiverPoint From, RiverPoint To)
    {
        PolylinePoint[] result = new PolylinePoint[pointPerSegment - 1];
        RiverPoint temp = From;

        for (int i = 1; i < pointPerSegment; i++)
        {
            float t = i / (pointPerSegment - 1f);

            //TODOBezier
            temp = RiverPoint.Lerp(From, To, t);
            temp.pos = curve.GetPointPos(t);

            result[i - 1] = temp.ToPolyLine();
        }
        return result;
    }
    private PolylinePoint[] CatmullInterpolation(CatmullRiverSegment curve, RiverPoint From, RiverPoint To)
    {
        PolylinePoint[] result = new PolylinePoint[pointPerSegment - 1];

        RiverPoint temp = From;

        for (int i = 1; i < pointPerSegment; i++)
        {
            float t = i / (pointPerSegment - 1f);
            temp = RiverPoint.Lerp(From, To, t);
            temp.pos = curve.GetPointPos(t);
            result[i-1] = temp.ToPolyLine();
        }
        return result;
    }
    private CatmullRiverSegment GetCurve(int i)
    {
        return new CatmullRiverSegment(points[i-1].pos, points[i].pos, points[i + 1].pos, points[i + 2].pos);
    }
    //
    public void ReCalculCurve()
    {
        GameTile startTile = grid.GetTile(grid.PosToTile(points[0].pos));
        Vector3 previousPos = startTile.worldPos;
        if(startTile.flowIn.Count > 0)
        {
            previousPos += new Vector3(startTile.flowIn[0].dirValue.x, startTile.flowIn[0].dirValue.y,0) * grid.cellSize;
        }
        else
        {
            previousPos += startTile.worldPos - grid.GetTile(grid.PosToTile(points[1].pos)).worldPos;
        }

        GameTile endTile = grid.GetTile(grid.PosToTile(points[points.Count - 1].pos));
        Vector3 afterPos = endTile.worldPos;
        if (endTile.flowOut.Count > 0)
        {
            afterPos += new Vector3(endTile.flowOut[0].dirValue.x, endTile.flowOut[0].dirValue.y, 0) * grid.cellSize;
        }
        else
        {
            afterPos += endTile.worldPos - grid.GetTile(grid.PosToTile(points[points.Count - 2].pos)).worldPos;
        }

        line.points.Clear();
        if (points.Count > 2)
        {
            line.AddPoint(points[0].ToPolyLine());
            line.AddPoints(CatmullInterpolation(
                new CatmullRiverSegment(
                    previousPos,
                    points[0].pos,
                    points[1].pos,
                    points[2].pos)
                , points[0], points[1]));

            for (int i = 1; i < points.Count - 2; i++)
            {
                line.AddPoints(CatmullInterpolation(GetCurve(i), points[i], points[i + 1]));
            }

            line.AddPoints(CatmullInterpolation(
                 new CatmullRiverSegment(
                     points[points.Count - 3].pos,
                     points[points.Count - 2].pos,
                     points[points.Count - 1].pos,
                     afterPos)
                   , points[points.Count - 2], points[points.Count - 1]));
        }
        else if (points.Count == 2)
        {
            line.AddPoint(points[0].ToPolyLine());
            line.AddPoints(
                CatmullInterpolation(
                new CatmullRiverSegment(
                    previousPos,
                    points[0].pos,
                    points[1].pos,
                    afterPos
                    ), points[0], points[1]));
        }
        else
        {
            Debug.LogWarning("Need more point", this);
        }

        customLine.linePoints = line.points;

        UpdateEndPoint();
        ArrowFlowDir();
    }
    public void UpdateEndPoint()
    {
        startDisk.Radius = points[0].thickness * 0.5f * line.Thickness;
        startDisk.Color = points[0].color;
        startDisk.transform.position = points[0].pos;

        endDisk.Radius = points[points.Count - 1].thickness * 0.5f * line.Thickness;
        endDisk.Color = points[points.Count - 1].color;
        endDisk.transform.position = points[points.Count - 1].pos;
    }
    public void ArrowFlowDir()
    {
        Vector2 dir = (points[1].pos - points[0].pos).normalized * Mathf.PI;
        dir.y = dir.y * -1; //trouver un meilleur fix
        float angle = Vector2.SignedAngle(Vector2.right, dir);
        Quaternion rot = new Quaternion(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad), 0, 1).normalized;
        cone.Radius = 0.3f * line.Thickness;
        cone.Length = 0.6f;
        cone.transform.position = points[1].pos;
        cone.transform.rotation = rot.normalized;
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