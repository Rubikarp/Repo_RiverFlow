using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer))]
public class RiverMesh : MonoBehaviour
{
    public MeshFilter mf;

    [Header("Mesh Data")]
    public Mesh mesh;
    public Color[] verticesColors = new Color[6];
    public Vector3[] vertices = new Vector3[6];
    public Vector3[] normals = new Vector3[6];
    public Vector2[] uvs = new Vector2[6];
    public int[] triangles = new int[12];

    [Header("River Data")]
    [Range(2, 16)] public int pointPerSegment = 8;
    public RiverPalette_SCO riverData;
    public List<RiverPoint> points = new List<RiverPoint>() { };

    [Header("other")]
    private GameGrid grid;

    void Awake()
    {
        UpdateMesh();
    }
    void Update()
    {
        UpdateMesh();
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

        int pointCount = points.Count;
        verticesColors = new Color[pointCount * 3];
        vertices = new Vector3[pointCount * 3];
        normals = new Vector3[pointCount * 3];
        uvs = new Vector2[pointCount * 3];

        int segmentCount = points.Count -1;
        triangles = new int[segmentCount * 12];

        #region Vertices
        Vector3 previousDir = (points[1].pos - points[0].pos).normalized;
        Vector3 nextDir = (points[1].pos - points[0].pos).normalized;
        Vector3 dir = (previousDir + nextDir) * 0.5f;
        Vector3 right = Vector3.Cross(Vector3.back, dir).normalized;
        //
        vertices[0] = points[0].pos;
        vertices[1] = points[0].pos - (right * points[0].thickness);
        vertices[2] = points[0].pos + (right * points[0].thickness);

        for (int i = 1; i < pointCount-1; i++)
        {
            previousDir = (points[i].pos - points[i - 1].pos).normalized;
            nextDir = (points[i+1].pos - points[i].pos).normalized;
            dir = (previousDir + nextDir) * 0.5f;
            right = Vector3.Cross(Vector3.back, dir).normalized;
            //
            vertices[(i * 3) + 0] = points[i].pos;
            vertices[(i * 3) + 1] = points[i].pos - (right * points[i].thickness);
            vertices[(i * 3) + 2] = points[i].pos + (right * points[i].thickness);
        }

        previousDir = (points[(pointCount - 1)].pos - points[(pointCount - 2)].pos).normalized;
        nextDir = (points[(pointCount - 1)].pos - points[(pointCount - 2)].pos).normalized;
        dir = (previousDir + nextDir) * 0.5f;
        right = Vector3.Cross(Vector3.back, dir).normalized;
        //
        vertices[((pointCount - 1 ) * 3) + 0] = points[(pointCount - 1)].pos;
        vertices[((pointCount - 1 ) * 3) + 1] = points[(pointCount - 1)].pos - (right * points[(pointCount - 1)].thickness);
        vertices[((pointCount - 1 )* 3) + 2] = points[(pointCount - 1)].pos + (right * points[(pointCount - 1)].thickness);
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
        vertices[0] = points[0].pos;
        vertices[1] = points[1].pos;
        //
        Vector3 right = Vector3.Cross(Vector3.back, (vertices[1] - vertices[0])).normalized;
        vertices[2] = points[0].pos - (right * points[0].thickness);
        vertices[3] = points[0].pos + (right * points[0].thickness);
        //
        vertices[4] = points[1].pos - (right * points[1].thickness);
        vertices[5] = points[1].pos + (right * points[1].thickness);
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

}
