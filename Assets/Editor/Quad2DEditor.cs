using UnityEngine;
using UnityEditor;
using System.Collections;
using Primitives2D;

namespace Primitives2D {

	[CustomEditor(typeof(Quad2D))]
	public class Quad2DEditor : Editor {
		
		private SerializedObject primitive;
		private SerializedProperty color, rotationSpeed;
		
		void OnEnable ()
		{
			primitive = new SerializedObject(target);
			color = primitive.FindProperty("color");
			rotationSpeed = primitive.FindProperty("rotationSpeed");
		}
		
		public override void OnInspectorGUI ()
		{
			primitive.Update();

			EditorGUILayout.PropertyField(color);
			EditorGUILayout.PropertyField(rotationSpeed);
			
			if (primitive.ApplyModifiedProperties()) {
				if (PrefabUtility.GetPrefabType(target) != PrefabType.Prefab) {
					(target as Quad2D).UpdateMesh();
					(target as Quad2D).UpdateColor();
				}
			}
		}
		
	}

}
