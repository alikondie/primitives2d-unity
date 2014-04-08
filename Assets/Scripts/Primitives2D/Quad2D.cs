using UnityEngine;
using System.Collections;


namespace Primitives2D {

	[ExecuteInEditMode, RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
	public class Quad2D : Primitive2D
	{
		public float snapValue = 0.5f;
		public bool snapVertexPositions = false;

		private const float UNIT_LENGTH = 1f;


		/// <summary>
		/// Do initialization here instead of Start because it will ensure proper behavior when creating a prefab, which calls OnDisable/OnEnable.
		/// </summary>
		void OnEnable ()
		{
			UpdateMesh ();
			m_Mesh.name = "Quad Mesh";
			if (!useCustomMaterial)
				AddDefaultMaterial();
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
			}

			m_Mesh.vertices = m_Vertices;
			
			CalculateUVs();
			CalculateTriangles();
		}

		public override void CalculateUVs ()
		{
			m_Mesh.uv = new Vector2[4]{
				// Bias the UVs array by 0.5 to make range [0..1].
				new Vector2(m_Vertices[0].x + 0.5f, m_Vertices[0].y + 0.5f), 
				new Vector2(m_Vertices[1].x + 0.5f, m_Vertices[1].y + 0.5f), 
				new Vector2(m_Vertices[2].x + 0.5f, m_Vertices[2].y + 0.5f),
				new Vector2(m_Vertices[3].x + 0.5f, m_Vertices[3].y + 0.5f),
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


// MonoBehavior methods -------------------------------------------------------------

		/// <summary>
		/// This method is called when resetting the script component in the inspector.
		/// </summary>
		void Reset ()
		{
			ResetPrimitive();
		}

	}

}
