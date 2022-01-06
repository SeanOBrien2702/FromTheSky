using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SP.Grid
{
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
	public class HexMesh : MonoBehaviour
	{

		Mesh hexMesh;
		MeshCollider meshCollider;
		List<Vector3> vertices;
		List<Vector4> tangats;
		List<int> triangles;

		void Awake()
		{
			GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
			meshCollider = GetComponent<MeshCollider>();
			hexMesh.name = "Hex Mesh";
			tangats = new List<Vector4>();
			vertices = new List<Vector3>();
			triangles = new List<int>();
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

		public void Triangulate(HexCell cell)
		{
			Vector3 center = Vector3.zero;//cell.transform.localPosition;
			//Debug.Log("center: " + center);
			for (int i = 0; i < 6; i++)
			{
				AddTriangle(
				center,
				center + HexMetrics.corners[i],
				center + HexMetrics.corners[i+1]
			);
			}
			hexMesh.vertices = vertices.ToArray();
			hexMesh.triangles = triangles.ToArray();
			//hexMesh.tangents = tangats.ToArray();
			Vector2[] uvs = new Vector2[hexMesh.vertices.Length];

			for (int i = 0; i < uvs.Length; i++)
			{
				uvs[i] = new Vector2(vertices[i].z, vertices[i].x);
			}
			hexMesh.uv = uvs;
			hexMesh.RecalculateNormals();
			meshCollider.sharedMesh = hexMesh;
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
}
