using UnityEngine;
using System.Collections;

namespace Primitives2D {

	[ExecuteInEditMode, RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
	public class Triangle2D : Primitive2D {

		public float sideLength = 5f;
		public float rotationSpeed = 0f;


		/// <summary>
		/// Do setup in this method as opposed to Start() since updating a prefab calls OnDisable/OnEnable. This way we intercept both initialization
		/// and prefab updates in one place.
		/// </summary>
		void OnEnable ()
		{
			Setup ();
			m_Mesh.name = "Triangle Mesh";
		}

		void Update ()
		{
			if (rotationSpeed != 0f)
				transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
		}

		public override void CalculateVertices ()
		{
			// Construct triangle using current position as the circumcenter, and going clockwise from bottom left point.
			
			float altitude = 0.5f * Mathf.Sqrt(3f) * sideLength;
			float circumcenter = sideLength / Mathf.Sqrt(3f);
			float offsetX = sideLength / 2f;
			
			m_Vertices = new Vector3[3];
			m_Vertices[0] = new Vector3(-offsetX, circumcenter - altitude, 0f);
			m_Vertices[1] = new Vector3(0f, circumcenter, 0f);
			m_Vertices[2] = new Vector3(offsetX, circumcenter - altitude, 0f);

			m_Mesh.vertices = m_Vertices;
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
		
		/*public override void AddCollider ()
		{
			gameObject.AddComponent<PolygonCollider2D>();
			
			PolygonCollider2D polyColl = GetComponent<PolygonCollider2D>();
			float scale = sideLength / Mathf.Sqrt(3f);
			polyColl.CreatePrimitive(3, new Vector2(scale, scale), new Vector2(0f, 0f));
			polyColl.isTrigger = true;
		}*/

		private void LogTriangleDataToConsole ()
		{
			Debug.Log ("Triangle data...");
			Debug.Log ("vertex[0] = " + m_Vertices[0].x + ", " + m_Vertices[0].y);
			Debug.Log ("vertex[1] = " + m_Vertices[1].x + ", " + m_Vertices[1].y);
			Debug.Log ("vertex[2] = " + m_Vertices[2].x + ", " + m_Vertices[2].y);
		}

	}

}
