using UnityEngine;
using System.Collections;

namespace Primitives2D {

	[ExecuteInEditMode, RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
	public class Circle2D : Primitive2D {

		public float radius = 2f;
		public int numPoints = 30;
		
		private const float TWO_PI = Mathf.PI * 2f;
		

		/// <summary>
		/// Do setup in this method as opposed to Start() since updating a prefab calls OnDisable/OnEnable. This way we intercept both initialization
		/// and prefab updates in one place.
		/// </summary>
		void OnEnable ()
		{
			Setup ();
			m_Mesh.hideFlags = HideFlags.HideAndDontSave;
			m_Mesh.name = "Circle Mesh";
		}

		public override void CalculateVertices ()
		{
			// Construct circle using current position as the center
			// The first vertex is the center of the circle, which is used to construct the triangles of the mesh, and we
			// don't include it in the calculation of the angle increment.
			
			float x, y;
			float angle = 0f;
			float angleIncr = TWO_PI / (numPoints - 1);
			
			m_Vertices = new Vector3[numPoints];
			m_Vertices[0] = new Vector3(0f, 0f, 0f);
			for (int i = 1; i < numPoints; ++i)
			{
				x = radius * Mathf.Cos (angle);
				y = -radius * Mathf.Sin (angle);
				m_Vertices[i] = new Vector3(x, y, 0f);
				angle += angleIncr;
			}

			m_Mesh.vertices = m_Vertices;
		}

		public override void CalculateUVs ()
		{
			Vector2[] uvs = new Vector2[numPoints];
			for (int i = 0; i < numPoints; ++i)
			{
				uvs[i] = new Vector2(m_Vertices[i].x, m_Vertices[i].y);
			}
			m_Mesh.uv = uvs;
			m_Mesh.uv1 = uvs;
			m_Mesh.uv2 = uvs;
		}
		
		public override void CalculateTriangles ()
		{
			// All triangles have a common point at the center of the circle, and use two points on its circumference.
			
			int index;
			
			m_Triangles = new int[numPoints * 3];
			for (int i = 0; i < numPoints - 2; ++i)
			{
				index = i * 3;
				m_Triangles[index + 0] = 0;
				m_Triangles[index + 1] = i + 1;
				m_Triangles[index + 2] = i + 2;
			}
			
			index = m_Triangles.Length - 3;
			m_Triangles[index + 0] = 0;
			m_Triangles[index + 1] = numPoints - 1;
			m_Triangles[index + 2] = 1;

			m_Mesh.triangles = m_Triangles;
		}
		
		/*public override void AddCollider ()
		{
			gameObject.AddComponent<CircleCollider2D>();
			
			CircleCollider2D circleColl = GetComponent<CircleCollider2D>();
			circleColl.center = new Vector2(0f, 0f);
			circleColl.radius = radius;
			circleColl.isTrigger = true;
		}*/
		
		private void LogCircleDataToConsole ()
		{
			Debug.Log ("Circle data...");
			Debug.Log ("radius is " + radius);
			for (int i = 0; i < numPoints; ++i)
			{
				Debug.Log ("vertex" + i + "] = " + m_Vertices[i].x + ", " + m_Vertices[i].y);
			}
		}

	}

}
