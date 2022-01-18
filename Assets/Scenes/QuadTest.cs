using FTS.Grid;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuadTest : MonoBehaviour
{
    public Material ObjectMaterial;

    void Start()
    {

        //MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        //meshRenderer.material = ObjectMaterial;

        //MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

        //Mesh mesh = meshFilter.mesh;
        //mesh.Clear();

        //Vector3[] vertices = new Vector3[6]
        //{
        //        new Vector3(0, 0, 0),
        //        new Vector3(0, 1, 0),
        //        new Vector3(1, 0, 0),
        //        new Vector3(0, 1, 0),
        //        new Vector3(1, 1, 0),
        //        new Vector3(1, 0, 0),
        //};

        //Vector3[] normals = new Vector3[6]
        //{
        //        Vector3.Cross(vertices[1] - vertices[0], vertices[2] - vertices[0]).normalized,
        //        Vector3.Cross(vertices[1] - vertices[0], vertices[2] - vertices[0]).normalized,
        //        Vector3.Cross(vertices[1] - vertices[0], vertices[2] - vertices[0]).normalized,
        //        Vector3.Cross(vertices[4] - vertices[3], vertices[5] - vertices[3]).normalized,
        //        Vector3.Cross(vertices[4] - vertices[3], vertices[5] - vertices[3]).normalized,
        //        Vector3.Cross(vertices[4] - vertices[3], vertices[5] - vertices[3]).normalized,
        //};

        //mesh.vertices = vertices;
        //mesh.normals = normals;
        //mesh.triangles = Enumerable.Range(0, vertices.Length).ToArray();

        //mesh.uv = new Vector2[6]
        //{
        //        new Vector2(0f, 0f),
        //        new Vector2(0f, 1f),
        //        new Vector2(1f, 0f),
        //        new Vector2(0f, 1f),
        //        new Vector2(1f, 1f),
        //        new Vector2(1f, 0f),
        //};

        //// Define Tanget at each point based on the 3D direction the U of the UV values increases at each vertex.
        //// For more information on Tangents see here https://docs.unity3d.com/ScriptReference/Mesh-tangents.html
        //Vector3 tangent3 = (vertices[2] - vertices[0]).normalized;
        //Vector4 tangent4 = new Vector4(tangent3.x, tangent3.y, tangent3.z, -1);
        //mesh.tangents = new Vector4[6]
        //{
        //        tangent4, tangent4, tangent4, tangent4, tangent4, tangent4
        //};

        //// Alternatively, tangents can be calculated automatically, but only using the primary UV set.
        ////mesh.RecalculateTangents();
    }


	Mesh hexMesh;
	List<Vector3> vertices;
    //List<Vector4> tangats;
    //List<Vector2> uvs;
	List<int> triangles;

	void Awake()
	{
		GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
		hexMesh.name = "Hex Mesh";
	    //uvs = new List<Vector2>();
		vertices = new List<Vector3>();
		triangles = new List<int>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = ObjectMaterial;
        Triangulate();

    }

	//public void Triangulate(HexCell[] cells)
	//{
	//	hexMesh.Clear();
	//	vertices.Clear();
	//	triangles.Clear();
	//	for (int i = 0; i < cells.Length; i++)
	//	{
	//		Triangulate(cells[i]);
	//	}
	//	hexMesh.vertices = vertices.ToArray();
	//	hexMesh.triangles = triangles.ToArray();

	//	hexMesh.RecalculateNormals();
	//}

	public void Triangulate()
	{
		Vector3 center = Vector3.zero;
		for (int i = 0; i < 6; i++)
		{
			AddTriangle(
			center,
			center + HexMetrics.corners[i],
			center + HexMetrics.corners[i + 1]
            );
            //uvs.Add(new Vector2(HexMetrics.corners[i].x, HexMetrics.corners[i].y));

        }
		hexMesh.vertices = vertices.ToArray();
		hexMesh.triangles = triangles.ToArray();
        //hexMesh.uv = uvs.ToArray();
        Vector2[] uvs = new Vector2[hexMesh.vertices.Length];

        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].z, vertices[i].x);
        }
        hexMesh.uv = uvs.ToArray();
        //hexMesh.tangents = tangats.ToArray();

        //hexMesh.RecalculateNormals();
        hexMesh.RecalculateTangents();
		hexMesh.RecalculateNormals();
	}

	void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
	{
		int vertexIndex = vertices.Count;
		vertices.Add(v1);
		vertices.Add(v2);
		vertices.Add(v3);
		triangles.Add(vertexIndex);
		triangles.Add(vertexIndex + 1);
		triangles.Add(vertexIndex + 2);
	}
}


