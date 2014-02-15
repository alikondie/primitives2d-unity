using UnityEngine;
using System.Collections;

namespace Primitives2D {

	[ExecuteInEditMode, RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
	public class Quad2D : Primitive2D {

		public float rotationSpeed = 0f;

		private const float UNIT_LENGTH = 1f;


		void Start ()
		{
			UpdateMesh();
			m_Mesh.name = "Quad Mesh";
			AddMaterial();
		}

		/// <summary>
		/// Updating the mesh here ensures proper behavior when creating a prefab, since it calls OnDisable/OnEnable.
		/// </summary>
		void OnEnable ()
		{
			UpdateMesh ();
		}

		public override void CalculateVertices ()
		{
			// Construct square using current position as the center, and assign vertices going clockwise from bottom left point.

			if (m_Vertices == null) {
				float halfLength = UNIT_LENGTH / 2f;

				m_Mesh.Clear();
				
				m_Vertices = new Vector3[4];
				m_Vertices[0] = new Vector3(-halfLength, -halfLength, 0f);
				m_Vertices[1] = new Vector3(-halfLength, halfLength, 0f);
				m_Vertices[2] = new Vector3(halfLength, halfLength, 0f);
				m_Vertices[3] = new Vector3(halfLength, -halfLength, 0f);
				
				m_Mesh.vertices = m_Vertices;

				CalculateUVs();
				CalculateTriangles();
			}
		}

		public override void CalculateUVs ()
		{
			m_Mesh.uv = new Vector2[4]{
				new Vector2(m_Vertices[0].x, m_Vertices[0].y), 
				new Vector2(m_Vertices[1].x, m_Vertices[1].y), 
				new Vector2(m_Vertices[2].x, m_Vertices[2].y),
				new Vector2(m_Vertices[3].x, m_Vertices[3].y),
			};
			m_Mesh.uv1 = m_Mesh.uv;
			m_Mesh.uv2 = m_Mesh.uv;
		}

		public override void CalculateTriangles ()
		{
			// Tiangles must be defined in the same order (clockwise) as the calculation of the vertices, or else the mesh will face 
			// the wrong way.

			m_Triangles = new int[6]{0, 1, 2, 0, 2, 3};
			m_Mesh.triangles = m_Triangles;
		}

		private void LogSquareDataToConsole ()
		{
			Debug.Log ("Quad data...");
			Debug.Log ("vertex[0] = " + m_Vertices[0].x + ", " + m_Vertices[0].y);
			Debug.Log ("vertex[1] = " + m_Vertices[1].x + ", " + m_Vertices[1].y);
			Debug.Log ("vertex[2] = " + m_Vertices[2].x + ", " + m_Vertices[2].y);
			Debug.Log ("Vertex[3] = " + m_Vertices[3].x + ", " + m_Vertices[3].y);
		}


		void Update ()
		{
			if (rotationSpeed != 0f)
				transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
		}

		void Reset ()
		{
			color = Color.black;
			UpdateMesh();
			UpdateColor();
		}

	}

}
