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
        UpdateMeshLine();
    }
    void Update()
    {
        UpdateMeshLine();
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
        Vector3 right = Vector3.Cross(Vector3.back, (vertices[1] - vertices[0])).normalized;
        vertices[2] = points[0].pos - (right * points[0].thickness);
        vertices[4] = points[1].pos - (right * points[1].thickness);
        //
        vertices[3] = points[0].pos + (right * points[0].thickness);
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
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        //
        triangles[3] = 0;
        triangles[4] = 1;
        triangles[5] = 3;
        //
        triangles[6] = 1;
        triangles[7] = 2;
        triangles[8] = 4;
        //
        triangles[9] = 1;
        triangles[10] = 3;
        triangles[11] = 5;
        #endregion

        mesh.SetVertices(vertices);
        mesh.SetNormals(normals);
        mesh.SetColors(verticesColors);
        mesh.SetUVs(0, uvs);
        mesh.SetTriangles(triangles, 0);
        mesh.MarkDynamic();

        mf.mesh = mesh;
    }
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

        #region Vertices
        vertices[0] = points[0].pos;
        vertices[1] = points[1].pos;
        Vector3 right = Vector3.Cross(Vector3.back, (vertices[1] - vertices[0])).normalized;
        vertices[2] = points[0].pos - (right * points[0].thickness);
        vertices[4] = points[0].pos - (right * points[0].thickness);
        //
        vertices[3] = points[1].pos + (right * points[0].thickness);
        vertices[5] = points[1].pos + (right * points[0].thickness);
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
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        //
        triangles[3] = 0;
        triangles[4] = 1;
        triangles[5] = 3;
        //
        triangles[6] = 1;
        triangles[7] = 2;
        triangles[8] = 4;
        //
        triangles[9] = 1;
        triangles[10] = 3;
        triangles[11] = 5;
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
