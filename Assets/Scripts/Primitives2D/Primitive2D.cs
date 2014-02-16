using UnityEngine;
using System.Collections;


namespace Primitives2D {

	public abstract class Primitive2D : MonoBehaviour
	{
		public Color color;

		// These need to be public in order to persist from the scene view to the game, otherwise the primitive will always
		// be reinitialized at runtime with default vertices, and will cause leaks with the material.
		public Vector3[] m_Vertices;
		public Material m_Material;

		protected Mesh m_Mesh;
		protected MeshRenderer m_Renderer;
		protected int[] m_Triangles;


		/// <summary>
		/// Creates and adds the MeshFilter component that is responsible for defining the shape of the primitive.
		/// </summary>
		/// <returns>The mesh with added MeshFilter component.</returns>
		public void CreateMesh ()
		{
			if (m_Mesh == null) {
				m_Mesh = new Mesh();
				GetComponent<MeshFilter>().mesh = m_Mesh;
				m_Mesh.hideFlags = HideFlags.HideAndDontSave;
			}
		}

		/// <summary>
		/// Calculates the vertices that define the shape of the mesh. Vertices are calculated in a clockwise direction. Calculates uv texture coordinates
		/// and the mesh's triangles from the vertices.
		/// </summary>
		public abstract void CalculateVertices();
		/// <summary>
		/// Assigns the calculated vertices to the UV map of the primitive.
		/// </summary>
		public abstract void CalculateUVs();
		/// <summary>
		/// Calculates the triangles that make up the shape of the mesh. All meshes are a collection of triangles using the calculated 
		/// vertices of the mesh to define the "face" of the object.
		/// </summary>
		public abstract void CalculateTriangles();

		/// <summary>
		/// Adds the material onto the shape's mesh renderer. If no material is specified in the inspector, the default "diffuse" shader is used
		/// with the given color.
		/// </summary>
		public void AddMaterial ()
		{
			if (m_Material == null) {
				m_Material = new Material(Shader.Find ("Diffuse"));
				m_Material.color = color;

				renderer.material = m_Material;
				renderer.castShadows = false;
				renderer.receiveShadows = false;
			}
		}

		/// <summary>
		/// All three primitives need a polygon collider in order for it to match the exact shape of the mesh since the vertices can be moved. 
		/// The collider needs only one path, which is set to the same positions as the primitive's vertices.
		/// The Circle2D shape overrides this method to handle its more particular case.
		/// </summary>
		/// <param name="numSides">Number of sides to match the primitive.</param>
		public virtual void AddCollider (int numSides)
		{
			gameObject.AddComponent<PolygonCollider2D>();
			
			PolygonCollider2D coll = GetComponent<PolygonCollider2D>();
			coll.CreatePrimitive(numSides);
			coll.pathCount = 1;
			
			Vector2[] collPoints = new Vector2[numSides];
			for (int i = 0; i < numSides; ++i) {
				collPoints[i] = m_Vertices[i];
			}
			coll.SetPath(0, collPoints);
		}

		/// <summary>
		/// Create the primitive shape by adding the required Unity components.
		/// </summary>
		public void UpdateMesh ()
		{
			CreateMesh();
			CalculateVertices ();

			m_Mesh.RecalculateNormals ();
			m_Mesh.RecalculateBounds ();

			if (m_Material != null) {
				m_Material.color = color;
			}
		}

		/// <summary>
		/// This method is called from the editor when a vertex is moved in the scene view, updating its position as well as the path that
		/// defines the collider (if there is one).
		/// </summary>
		/// <param name="index">Index of the vertex to update.</param>
		/// <param name="position">New position in local space.</param>
		public void UpdateVertex (int index, Vector2 position)
		{
			m_Vertices[index] = position;

			if (collider2D != null) {
				PolygonCollider2D coll = (PolygonCollider2D)collider2D;
				Vector2[] collPoints = new Vector2[coll.GetPath(0).Length];
				for (int i = 0; i < collPoints.Length; ++i) {
					collPoints[i] = m_Vertices[i];
				}
				coll.SetPath(0, collPoints);
			}
		}

		/// <summary>
		/// Instances call this base method from the MonoBehavior Reset method, which is invoked when resetting the script component in the inspector.
		/// </summary>
		public void ResetPrimitive ()
		{
			transform.position = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			
			color = Color.black;
			m_Vertices = null;
			
			UpdateMesh();
		}


// MonoBehavior methods -------------------------------------------------------------

		void OnDestroy ()
		{
			// Still need to verify this for memmory leak.
			if (Application.isPlaying) {
				Destroy (m_Material);
			} else {
				DestroyImmediate(m_Material);
			}
		}

	}

}
