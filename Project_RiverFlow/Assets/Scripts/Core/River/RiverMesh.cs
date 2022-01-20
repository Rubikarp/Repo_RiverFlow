using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer))]
public class RiverMesh : MonoBehaviour
{
    public MeshFilter mf;

    [Header("Mesh Data")]
    public Mesh mesh;
    public Color[] verticesColors = new Color[6];
    public Vector3[] vertices = new Vector3[6];
    public Vector3[] normals = new Vector3[6];
    public Vector3[] uvs = new Vector3[6];
    public int[] triangles = new int[12];

    [Header("River Data")]
    [Range(0.01f, 4)] public float scale = 1;
    [Range(1, 16)] public int subdividePerSegment = 1;
    public List<RiverPoint> points = new List<RiverPoint>() { new RiverPoint(Vector3.zero, Color.red, 1), new RiverPoint(Vector3.zero, Color.red, 1) };
    public RiverPoint previousPoint ;
    public RiverPoint nextPoint;

    [Header("other")]
    private GameGrid grid;

    void Update()
    {
        UpdateMesh();
    }

    public void AddPoint(RiverPoint point)
    {
        this.points.Add(point);
    }
    public void AddPoints(IEnumerable<RiverPoint> points)
    {
        this.points.AddRange(points);
    }

    [Button]
    public void UpdateMesh()
    {
        if (mesh != null)
        {
            mesh.Clear();
        }
        else
        {
            mesh = new Mesh();
        }

        if (points.Count < 2)
        {
            //Error Catching
            Debug.LogError("pas assez de point");
            return;
        }

        #region Compute array size
        int pointCount = points.Count;
        verticesColors = new Color[pointCount * 3];
        vertices = new Vector3[pointCount * 3];
        normals = new Vector3[pointCount * 3];
        uvs = new Vector3[pointCount * 3];
        //
        int segmentCount = points.Count - 1;
        triangles = new int[segmentCount * 3 * 4];
        #endregion

        #region Vertices
        Vector3 previousDir = (points[1].pos - points[0].pos).normalized;
        Vector3 nextDir = (points[1].pos - points[0].pos).normalized;
        Vector3 dir = (previousDir + nextDir) * 0.5f;
        Vector3 right = Vector3.Cross(Vector3.back, dir).normalized;
        float thickness = 0.5f * scale;
        //
        vertices[0] = points[0].pos;
        vertices[1] = points[0].pos - (right * thickness);
        vertices[2] = points[0].pos + (right * thickness);

        for (int i = 1; i < pointCount - 1; i++)
        {
            previousDir = (points[i].pos - points[i - 1].pos).normalized;
            nextDir = (points[i + 1].pos - points[i].pos).normalized;
            dir = (previousDir + nextDir) * 0.5f;
            right = Vector3.Cross(Vector3.back, dir).normalized;
            thickness = 0.5f * scale;
            //
            vertices[(i * 3) + 0] = points[i].pos;
            vertices[(i * 3) + 1] = points[i].pos - (right * thickness);
            vertices[(i * 3) + 2] = points[i].pos + (right * thickness);
        }

        previousDir = (points[(pointCount - 1)].pos - points[(pointCount - 2)].pos).normalized;
        nextDir = (points[(pointCount - 1)].pos - points[(pointCount - 2)].pos).normalized;
        dir = (previousDir + nextDir) * 0.5f;
        right = Vector3.Cross(Vector3.back, dir).normalized;
        thickness = 0.5f * scale;
        //
        vertices[((pointCount - 1) * 3) + 0] = points[pointCount - 1].pos;
        vertices[((pointCount - 1) * 3) + 1] = points[pointCount - 1].pos - (right * thickness);
        vertices[((pointCount - 1) * 3) + 2] = points[pointCount - 1].pos + (right * thickness);
        #endregion
        #region Normal
        for (int i = 0; i < pointCount; i++)
        {
            normals[(i * 3) + 0] = Vector3.back;
            normals[(i * 3) + 1] = Vector3.back;
            normals[(i * 3) + 2] = Vector3.back;
        }
        #endregion
        #region Vertex Colors
        Color temp;
        for (int i = 0; i < pointCount; i++)
        {
            temp = points[i].color;
            verticesColors[(i * 3) + 0] = temp;
            verticesColors[(i * 3) + 1] = temp;
            verticesColors[(i * 3) + 2] = temp;
        }
        #endregion
        #region UV
        float distTravell = 0;
        uvs[0] = new Vector3(distTravell, 0.5f, points[0].thickness);
        uvs[1] = new Vector3(distTravell, 1.0f, points[0].thickness);
        uvs[2] = new Vector3(distTravell, 0.0f, points[0].thickness);

        for (int i = 1; i < pointCount; i++)
        {
            distTravell += (points[i].pos - points[i-1].pos).magnitude;

            uvs[(i * 3) + 0] = new Vector3(distTravell, 0.5f, points[i].thickness);
            uvs[(i * 3) + 1] = new Vector3(distTravell, 1.0f, points[i].thickness);
            uvs[(i * 3) + 2] = new Vector3(distTravell, 0.0f, points[i].thickness);
        }
        #endregion
        #region Triangle
        for (int i = 0; i < pointCount - 1; i++)
        {
            triangles[(i * 12) + 00] = (i * 3) + 0;
            triangles[(i * 12) + 01] = (i * 3) + 1;
            triangles[(i * 12) + 02] = (i * 3) + 3;
            //
            triangles[(i * 12) + 03] = (i * 3) + 0;
            triangles[(i * 12) + 04] = (i * 3) + 3;
            triangles[(i * 12) + 05] = (i * 3) + 2;
            //
            triangles[(i * 12) + 06] = (i * 3) + 3;
            triangles[(i * 12) + 07] = (i * 3) + 1;
            triangles[(i * 12) + 08] = (i * 3) + 4;
            //
            triangles[(i * 12) + 09] = (i * 3) + 2;
            triangles[(i * 12) + 10] = (i * 3) + 3;
            triangles[(i * 12) + 11] = (i * 3) + 5;
        }
        #endregion

        mesh.SetVertices(vertices);
        mesh.SetNormals(normals);
        mesh.SetColors(verticesColors);
        mesh.SetUVs(0, uvs);
        mesh.SetTriangles(triangles, 0);
        mesh.MarkDynamic();

        mf.mesh = mesh;
    }
    [Button]
    public void UpdateData()
    {
        if (mesh == null || points.Count < 2)
        {
            UpdateMesh();
            return;
        }

        #region Compute array size
        int pointCount = points.Count;
        verticesColors = new Color[pointCount * 3];
        uvs = new Vector3[pointCount * 3];
        #endregion

        #region Vertex Colors
        Color temp;
        for (int i = 0; i < pointCount; i++)
        {
            temp = points[i].color;
            verticesColors[(i * 3) + 0] = temp;
            verticesColors[(i * 3) + 1] = temp;
            verticesColors[(i * 3) + 2] = temp;
        }
        #endregion
        #region UV
        float distTravell = 0;
        uvs[0] = new Vector3(distTravell, 0.5f, points[0].thickness);
        uvs[1] = new Vector3(distTravell, 1.0f, points[0].thickness);
        uvs[2] = new Vector3(distTravell, 0.0f, points[0].thickness);

        for (int i = 1; i < pointCount; i++)
        {
            distTravell += (points[i].pos - points[i - 1].pos).magnitude;

            uvs[(i * 3) + 0] = new Vector3(distTravell, 0.5f, points[i].thickness);
            uvs[(i * 3) + 1] = new Vector3(distTravell, 1.0f, points[i].thickness);
            uvs[(i * 3) + 2] = new Vector3(distTravell, 0.0f, points[i].thickness);
        }
        #endregion

        mesh.SetColors(verticesColors);
        mesh.SetUVs(0, uvs);

        mesh.MarkDynamic();
        mf.mesh = mesh;
    }

    #region legacy code
    /*
    [Button]
    public void UpdateMeshSimple()
    {
        if (mesh != null)
        {
            mesh.Clear();
        }
        else
        {
            mesh = new Mesh();
        }

        if (points.Count < 2)
        {
            //Error Catching
            Debug.LogError("pas assez de point");
            return;
        }

        int pointCount = points.Count;
        verticesColors = new Color[pointCount * 3];
        vertices = new Vector3[pointCount * 3];
        normals = new Vector3[pointCount * 3];
        uvs = new Vector3[pointCount * 3];

        int segmentCount = points.Count -1;
        triangles = new int[segmentCount * 12];

        #region Vertices
        Vector3 previousDir = (points[1].point - points[0].point).normalized;
        Vector3 nextDir = (points[1].point - points[0].point).normalized;
        Vector3 dir = (previousDir + nextDir) * 0.5f;
        Vector3 right = Vector3.Cross(Vector3.back, dir).normalized;
        //
        vertices[0] = points[0].point;
        vertices[1] = points[0].point - (right * points[0].thickness);
        vertices[2] = points[0].point + (right * points[0].thickness);

        for (int i = 1; i < pointCount-1; i++)
        {
            previousDir = (points[i].point - points[i - 1].point).normalized;
            nextDir = (points[i+1].point - points[i].point).normalized;
            dir = (previousDir + nextDir) * 0.5f;
            right = Vector3.Cross(Vector3.back, dir).normalized;
            //
            vertices[(i * 3) + 0] = points[i].point;
            vertices[(i * 3) + 1] = points[i].point - (right * points[i].thickness);
            vertices[(i * 3) + 2] = points[i].point + (right * points[i].thickness);
        }

        previousDir = (points[(pointCount - 1)].point - points[(pointCount - 2)].point).normalized;
        nextDir = (points[(pointCount - 1)].point - points[(pointCount - 2)].point).normalized;
        dir = (previousDir + nextDir) * 0.5f;
        right = Vector3.Cross(Vector3.back, dir).normalized;
        //
        vertices[((pointCount - 1 ) * 3) + 0] = points[(pointCount - 1)].point;
        vertices[((pointCount - 1 ) * 3) + 1] = points[(pointCount - 1)].point - (right * points[(pointCount - 1)].thickness);
        vertices[((pointCount - 1 ) * 3) + 2] = points[(pointCount - 1)].point + (right * points[(pointCount - 1)].thickness);
        #endregion
        #region Normal
        for (int i = 0; i < pointCount; i++)
        {
            normals[(i * 3) + 0] = Vector3.back;
            normals[(i * 3) + 1] = Vector3.back;
            normals[(i * 3) + 2] = Vector3.back;
        }
        #endregion
        #region Vertex Colors
        Color temp;
        for (int i = 0; i < pointCount; i++)
        {
            temp = points[i].color;
            verticesColors[(i * 3) + 0] = temp;
            verticesColors[(i * 3) + 1] = temp;
            verticesColors[(i * 3) + 2] = temp;
        }
        #endregion
        #region UV
        float distTravell = 0;
        for (int i = 0; i < pointCount; i++)
        {
            uvs[(i * 3) + 0] = new Vector2(distTravell, 0);
            uvs[(i * 3) + 1] = new Vector2(distTravell, 1);
            uvs[(i * 3) + 2] = new Vector2(distTravell, 1);

            distTravell += dir.magnitude;
        }
        #endregion
        #region Triangle
        for (int i = 0; i < pointCount-1; i++)
        {
            triangles[(i * 12) + 00] = (i * 3) + 0;
            triangles[(i * 12) + 01] = (i * 3) + 1;
            triangles[(i * 12) + 02] = (i * 3) + 3;
            //
            triangles[(i * 12) + 03] = (i * 3) + 0;
            triangles[(i * 12) + 04] = (i * 3) + 3;
            triangles[(i * 12) + 05] = (i * 3) + 2;
            //
            triangles[(i * 12) + 06] = (i * 3) + 3;
            triangles[(i * 12) + 07] = (i * 3) + 1;
            triangles[(i * 12) + 08] = (i * 3) + 4;
            //
            triangles[(i * 12) + 09] = (i * 3) + 2;
            triangles[(i * 12) + 10] = (i * 3) + 3;
            triangles[(i * 12) + 11] = (i * 3) + 5;
        }
        #endregion

        mesh.SetVertices(vertices);
        mesh.SetNormals(normals);
        mesh.SetColors(verticesColors);
        mesh.SetUVs(0, uvs);
        mesh.SetTriangles(triangles, 0);
        mesh.MarkDynamic();

        mf.mesh = mesh;
    }
    [Button]
    public void UpdateMeshLine()
    {
        if (mesh != null)
        {
            mesh.Clear();
        }
        else
        {
            mesh = new Mesh();
        }

        if (points.Count < 2)
        {
            //Error Catching
            Debug.LogError("pas assez de point");
            return;
        }

        #region Vertices
        vertices[0] = points[0].point;
        vertices[1] = points[1].point;
        //
        Vector3 right = Vector3.Cross(Vector3.back, (vertices[1] - vertices[0])).normalized;
        vertices[2] = points[0].point - (right * points[0].thickness);
        vertices[3] = points[0].point + (right * points[0].thickness);
        //
        vertices[4] = points[1].point - (right * points[1].thickness);
        vertices[5] = points[1].point + (right * points[1].thickness);
        #endregion
        #region Normal
        normals[0] = Vector3.back;
        normals[1] = Vector3.back;
        //           
        normals[2] = Vector3.back;
        normals[4] = Vector3.back;
        //           
        normals[3] = Vector3.back;
        normals[5] = Vector3.back;
        #endregion
        #region Vertex Colors
        Color a = points[0].color;
        Color b = points[1].color;
        //
        verticesColors[0] = a;
        verticesColors[2] = a;
        verticesColors[3] = a;
        //
        verticesColors[1] = b;
        verticesColors[4] = b;
        verticesColors[5] = b;
        #endregion
        #region UV
        uvs[0] = new Vector2(0, 0);
        uvs[1] = new Vector2(1, 0);
        //
        uvs[2] = new Vector2(0, 1);
        uvs[4] = new Vector2(1, 1);
        //
        uvs[3] = new Vector2(0, 1);
        uvs[5] = new Vector2(1, 1);
        #endregion
        #region Triangle
        triangles[00] = 2;
        triangles[01] = 1;
        triangles[02] = 0;
        //
        triangles[03] = 0;
        triangles[04] = 1;
        triangles[05] = 3;
        //
        triangles[06] = 1;
        triangles[07] = 2;
        triangles[08] = 4;
        //
        triangles[09] = 1;
        triangles[10] = 5;
        triangles[11] = 3;
        #endregion

        mesh.SetVertices(vertices);
        mesh.SetNormals(normals);
        mesh.SetColors(verticesColors);
        mesh.SetUVs(0, uvs);
        mesh.SetTriangles(triangles, 0);

        mesh.MarkDynamic();

        mf.mesh = mesh;
    }
    */
    #endregion
}
