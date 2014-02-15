using UnityEngine;
using System.Collections;

namespace Primitives2D {

	[ExecuteInEditMode, RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
	public class Square2D : Primitive2D {

		public float sideLength = 2f;
		public float rotationSpeed = 0f;
		

		/// <summary>
		/// Do setup in this method as opposed to Start() since updating a prefab calls OnDisable/OnEnable. This way we intercept both initialization
		/// and prefab updates in one place.
		/// </summary>
		void OnEnable ()
		{
			Setup ();
			m_Mesh.name = "Square Mesh";
		}

		void Update ()
		{
			if (rotationSpeed != 0f)
				transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
		}

		public override void CalculateVertices ()
		{
			// Construct square using current position as the center, and assign vertices going clockwise from bottom left point.
			
			float halfLength = sideLength / 2f;
			
			m_Vertices = new Vector3[4];
			m_Vertices[0] = new Vector3(-halfLength, -halfLength, 0f);
			m_Vertices[1] = new Vector3(-halfLength, halfLength, 0f);
			m_Vertices[2] = new Vector3(halfLength, halfLength, 0f);
			m_Vertices[3] = new Vector3(halfLength, -halfLength, 0f);

			m_Mesh.vertices = m_Vertices;
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
		
		/*public override void AddCollider ()
		{
			gameObject.AddComponent<BoxCollider2D>();
			
			BoxCollider2D boxColl = GetComponent<BoxCollider2D>();
			boxColl.center = Vector2.zero;
			boxColl.size = new Vector2(sideLength, sideLength);
			boxColl.isTrigger = true;
		}*/

		private void LogSquareDataToConsole ()
		{
			Debug.Log ("Square data...");
			Debug.Log ("vertex[0] = " + m_Vertices[0].x + ", " + m_Vertices[0].y);
			Debug.Log ("vertex[1] = " + m_Vertices[1].x + ", " + m_Vertices[1].y);
			Debug.Log ("vertex[2] = " + m_Vertices[2].x + ", " + m_Vertices[2].y);
			Debug.Log ("Vertex[3] = " + m_Vertices[3].x + ", " + m_Vertices[3].y);
		}

	}

}
