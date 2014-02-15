using UnityEngine;
using UnityEditor;
using System.Collections;
using Primitives2D;

namespace Primitives2D {

	[CustomEditor(typeof(Circle2D))]
	public class Circle2DEditor : Editor {
		
		private SerializedObject primitive;
		
		
		void OnEnable ()
		{
			primitive = new SerializedObject(target);
		}
		
		public override void OnInspectorGUI ()
		{
			DrawDefaultInspector();
			
			if (primitive.ApplyModifiedProperties()) {
				(target as Circle2D).Setup();
			}
		}
		
	}

}
