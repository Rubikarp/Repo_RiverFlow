using Shapes;
using UnityEngine;
using System.Collections.Generic;

public class RiverSpline : MonoBehaviour
{
    private GameGrid grid;
    [Header("Shapes")]
    public RiverMesh customLine;
    [Space(5)]
    public Disc startDisk;
    public Disc endDisk;

    [Header("Parameter")]
    [Range(2, 16)] public int pointPerSegment = 8;
    public RiverPalette_SCO riverData;

    [Header("Data")]
    public Canal canalTreated;
    public List<RiverPoint> points = new List<RiverPoint>() {};

    private void Start()
    {
        grid = GameGrid.Instance;
    }

    void FixedUpdate()
    {
        ReCalculCurve();
    }

    private RiverPoint[] LinerInterpolation(RiverPoint lastPoint, RiverPoint newPoint, int iter)
    {
        RiverPoint[] result = new RiverPoint[iter];
        float step = 1f / (float)iter;
        for (int i = 0; i < iter; i++)
        {
            result[i] = RiverPoint.Lerp(lastPoint, newPoint, (i+1) * step);
        }
        return result;
    }
    private RiverPoint[] BezierInterpolation(CatmullRiverSegment curve, RiverPoint From, RiverPoint To)
    {
        RiverPoint[] result = new RiverPoint[pointPerSegment - 1];
        RiverPoint temp = From;

        for (int i = 1; i < pointPerSegment; i++)
        {
            float t = i / (pointPerSegment - 1f);

            //TODOBezier
            temp = RiverPoint.Lerp(From, To, t);
            temp.pos = curve.GetPointPos(t);

            result[i - 1] = temp;
        }
        return result;
    }
    private RiverPoint[] CatmullInterpolation(CatmullRiverSegment curve, RiverPoint From, RiverPoint To)
    {
        RiverPoint[] result = new RiverPoint[pointPerSegment - 1];

        RiverPoint temp = From;

        for (int i = 1; i < pointPerSegment; i++)
        {
            float t = i / (pointPerSegment - 1f);
            temp = RiverPoint.Lerp(From, To, t);
            temp.pos = curve.GetPointPos(t);
            result[i-1] = temp;
        }
        return result;
    }
    private CatmullRiverSegment GetCurve(int i)
    {
        return new CatmullRiverSegment(points[i-1].pos, points[i].pos, points[i + 1].pos, points[i + 2].pos);
    }
    //
    private GameTile temp;
    public void ReCalculCurve()
    {
        #region Set Points
        SetPointCount(canalTreated.Lenght);
        //Set First Point
        temp = grid.GetTile(canalTreated.startNode);
        DefinePoint(0,
            temp.worldPos,
            temp.riverStrenght,
            temp.element is Lake);

        //Set Middle Point
        for (int j = 0; j < canalTreated.canalTiles.Count; j++)
        {
            temp = grid.GetTile(canalTreated.canalTiles[j]);
            DefinePoint(j + 1,
                    temp.worldPos,
                    temp.riverStrenght,
                    temp.element is Lake);
        }
        //Set Last Point
        temp = grid.GetTile(canalTreated.endNode);
        DefinePoint(canalTreated.Lenght - 1,
                    temp.worldPos,
                    temp.riverStrenght,
                    temp.element is Lake);
        #endregion

        GameTile startTile = grid.GetTile(canalTreated.startNode);
        Vector3 previousPos = startTile.worldPos;
        if(startTile.flowIn.Count > 0)
        {
            previousPos += new Vector3(startTile.flowIn[0].dirValue.x, startTile.flowIn[0].dirValue.y,0) * grid.cellSize;
        }
        else
        {
            previousPos += startTile.worldPos - grid.GetTile(grid.PosToTile(points[1].pos)).worldPos;
        }

        GameTile endTile = grid.GetTile(canalTreated.endNode);
        Vector3 afterPos = endTile.worldPos;
        if (endTile.flowOut.Count > 0)
        {
            afterPos += new Vector3(endTile.flowOut[0].dirValue.x, endTile.flowOut[0].dirValue.y, 0) * grid.cellSize;
        }
        else
        {
            afterPos += endTile.worldPos - grid.GetTile(grid.PosToTile(points[points.Count - 2].pos)).worldPos;
        }

        customLine.points.Clear();
        if (points.Count > 2)
        {
            customLine.AddPoint(points[0]);
            customLine.AddPoints(CatmullInterpolation(
                new CatmullRiverSegment(
                    previousPos,
                    points[0].pos,
                    points[1].pos,
                    points[2].pos)
                , points[0], points[1]));

            for (int i = 1; i < points.Count - 2; i++)
            {
                customLine.AddPoints(CatmullInterpolation(GetCurve(i), points[i], points[i + 1]));
            }

            customLine.AddPoints(CatmullInterpolation(
                 new CatmullRiverSegment(
                     points[points.Count - 3].pos,
                     points[points.Count - 2].pos,
                     points[points.Count - 1].pos,
                     afterPos)
                   , points[points.Count - 2], points[points.Count - 1]));
        }
        else if (points.Count == 2)
        {
            customLine.AddPoint(points[0]);
            customLine.AddPoints(
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

        UpdatePoints();
    }
    public void UpdatePoints()
    {
        startDisk.Radius = points[0].thickness * 0.5f * customLine.scale;
        startDisk.Color = points[0].color;
        startDisk.transform.position = points[0].pos;

        endDisk.Radius = points[points.Count - 1].thickness * 0.5f * customLine.scale;
        endDisk.Color = points[points.Count - 1].color;
        endDisk.transform.position = points[points.Count - 1].pos;
    }
    /* public void ArrowFlowDir()
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
    */

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
    public void DefinePoint(int index, Vector3 pos, FlowStrenght riverStregth, bool lake)
    {
        RiverPoint newPoints = new RiverPoint(pos);
        newPoints.lake = lake? 1.0f:0.0f;

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