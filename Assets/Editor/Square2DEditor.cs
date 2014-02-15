using UnityEngine;
using UnityEditor;
using System.Collections;
using Primitives2D;

namespace Primitives2D {

	[CustomEditor(typeof(Square2D))]
	public class Square2DEditor : Editor {
		
		private SerializedObject primitive;
		
		
		void OnEnable ()
		{
			primitive = new SerializedObject(target);
		}
		
		public override void OnInspectorGUI ()
		{
			DrawDefaultInspector();
			
			if (primitive.ApplyModifiedProperties()) {
				(target as Square2D).Setup();
			}
		}
		
	}

}
