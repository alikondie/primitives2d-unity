using UnityEngine;
using System.Collections;

namespace Primitives2D {

	[ExecuteInEditMode, RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
	public class Triangle2D : Primitive2D
	{
		public float rotationSpeed = 0f;

		private const float UNIT_LENGTH = 1f;


		/// <summary>
		/// Do initialization here instead of Start because it will ensure proper behavior when creating a prefab, which calls OnDisable/OnEnable.
		/// </summary>
		void OnEnable ()
		{
			UpdateMesh ();
			m_Mesh.name = "Triangle Mesh";
			AddMaterial();
		}

		public override void CalculateVertices ()
		{
			// Construct triangle using current position as the circumcenter, and going clockwise from bottom left point.

			if (m_Vertices == null) {
				float altitude = 0.5f * Mathf.Sqrt(3f) * UNIT_LENGTH;
				float circumcenter = UNIT_LENGTH / Mathf.Sqrt(3f);
				float offsetX = UNIT_LENGTH / 2f;

				m_Mesh.Clear();
				
				m_Vertices = new Vector3[3];
				m_Vertices[0] = new Vector3(-offsetX, circumcenter - altitude, 0f);
				m_Vertices[1] = new Vector3(0f, circumcenter, 0f);
				m_Vertices[2] = new Vector3(offsetX, circumcenter - altitude, 0f);
			}

			m_Mesh.vertices = m_Vertices;
			
			CalculateUVs();
			CalculateTriangles();
		}

		public override void CalculateUVs ()
		{
			m_Mesh.uv = new Vector2[3]{
				new Vector2(m_Vertices[0].x, m_Vertices[0].y), 
				new Vector2(m_Vertices[1].x, m_Vertices[1].y), 
				new Vector2(m_Vertices[2].x, m_Vertices[2].y)
			};
			m_Mesh.uv1 = m_Mesh.uv;
			m_Mesh.uv2 = m_Mesh.uv;
		}

		public override void CalculateTriangles ()
		{
			m_Triangles = new int[3]{0, 1, 2};
			m_Mesh.triangles = m_Triangles;
		}

		private void LogTriangleDataToConsole ()
		{
			Debug.Log ("Triangle data...");
			Debug.Log ("vertex[0] = " + m_Vertices[0].x + ", " + m_Vertices[0].y);
			Debug.Log ("vertex[1] = " + m_Vertices[1].x + ", " + m_Vertices[1].y);
			Debug.Log ("vertex[2] = " + m_Vertices[2].x + ", " + m_Vertices[2].y);
		}


		void Update ()
		{
			if (rotationSpeed != 0f)
				transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
		}

		void Reset ()
		{
			m_Vertices = null;
			color = Color.black;
			UpdateMesh();
			UpdateColor();
		}

	}

}
