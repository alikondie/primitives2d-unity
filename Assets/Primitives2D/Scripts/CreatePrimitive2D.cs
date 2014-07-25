using UnityEngine;
using UnityEditor;
using System.Collections;


namespace Primitives2D {

	/// <summary>
	/// Utility class for creating Primitive2D objects from Unity's 'GameObject/Create Other' menu.
	/// </summary>
	public static class CreatePrimitive2D
	{
		[MenuItem("GameObject/Create Other/Primitives2D/Create Quad")]
		public static void CreateQuad ()
		{
			GameObject newQuad = CreatePrimitive<Quad2D>("Quad2D");
			Selection.activeGameObject = newQuad;
		}

		[MenuItem("GameObject/Create Other/Primitives2D/Create Triangle")]
		public static void CreateTriangle ()
		{
			GameObject newTri = CreatePrimitive<Triangle2D>("Triangle2D");
			Selection.activeGameObject = newTri;
		}

		[MenuItem("GameObject/Create Other/Primitives2D/Create Circle")]
		public static void CreateCircle ()
		{
			GameObject newCircle = CreatePrimitive<Circle2D>("Circle2D");
			Selection.activeGameObject = newCircle;
		}


		private static GameObject CreatePrimitive<T> (string name) where T : Primitive2D
		{
			GameObject obj = new GameObject();
			obj.name = name;
			obj.AddComponent<T>();
			obj.transform.position = Vector3.zero;
			obj.transform.rotation = Quaternion.identity;
			obj.transform.localScale = new Vector3(1f, 1f, 1f);

			return obj;
		}
	}

} // Primitives2D namespace
