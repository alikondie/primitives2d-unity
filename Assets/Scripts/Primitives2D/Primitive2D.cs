using UnityEngine;
using System.Collections;

namespace Primitives2D {

	public abstract class Primitive2D : MonoBehaviour {

		public Material material;
		public Color color;			// Color is only assigned to the primitive if no material is specified.

		protected Mesh m_Mesh;
		protected MeshRenderer m_Renderer;
		protected Vector3[] m_Vertices;
		protected int[] m_Triangles;


		/// <summary>
		/// Creates and adds the MeshFilter component that is responsible for defining the shape of the primitive.
		/// </summary>
		/// <returns>The mesh with added MeshFilter component.</returns>
		public Mesh CreateMesh ()
		{
			m_Mesh = new Mesh();
			GetComponent<MeshFilter>().mesh = m_Mesh;
			GetComponent<MeshFilter>().sharedMesh = m_Mesh;
			m_Mesh.Clear ();

			return m_Mesh;
		}

		/// <summary>
		/// Calculates the vertices that define the shape of the mesh. Vertices are calculated in a clockwise direction.
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
		public virtual void AddMaterial ()
		{
			if (material == null) {
				material = new Material(Shader.Find ("Diffuse"));
				material.color = color;
			}

			m_Renderer = GetComponent<MeshRenderer>();
			m_Renderer.material = material;
			m_Renderer.castShadows = false;
			m_Renderer.receiveShadows = false;
		}

		/// <summary>
		/// Create the primitive shape by adding the required Unity components.
		/// </summary>
		public void Setup ()
		{
			if (m_Mesh == null)
				CreateMesh();

			if (m_Vertices == null) {
				CalculateVertices ();
				CalculateUVs ();
			}

			if (m_Triangles == null)
				CalculateTriangles ();
			
			m_Mesh.RecalculateNormals ();
			m_Mesh.RecalculateBounds ();
			
			AddMaterial ();
		}


		// MonoBehavior method
		void Reset ()
		{
			Setup();
		}

	}

}
