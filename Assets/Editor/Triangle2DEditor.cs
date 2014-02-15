using UnityEngine;
using UnityEditor;
using System.Collections;
using Primitives2D;


namespace Primitives2D {

	[CustomEditor(typeof(Triangle2D))]
	public class Triangle2DEditor : Editor {

		private SerializedObject primitive;


		void OnEnable ()
		{
			primitive = new SerializedObject(target);
		}
		
		public override void OnInspectorGUI ()
		{
			DrawDefaultInspector();

			if (primitive.ApplyModifiedProperties()) {
				if (PrefabUtility.GetPrefabType(target) != PrefabType.Prefab) {
					(target as Triangle2D).Setup();
				}
			}
		}

	}

}

