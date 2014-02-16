using UnityEngine;
using UnityEditor;
using System.Collections;
using Primitives2D;


namespace Primitives2D {

	[CustomEditor(typeof(Circle2D))]
	public class Circle2DEditor : Editor {
		
		private SerializedObject primitive;
		private SerializedProperty numPoints, color;
		
		
		void OnEnable ()
		{
			primitive = new SerializedObject(target);
			numPoints = primitive.FindProperty("numPoints");
			color = primitive.FindProperty("color");
		}
		
		public override void OnInspectorGUI ()
		{
			primitive.Update();

			EditorGUILayout.PropertyField(color);
			EditorGUILayout.PropertyField(numPoints);
			EditorGUILayout.Space();

			if (GUILayout.Button("Add Collider")) {
				(target as Circle2D).AddCollider((target as Circle2D).numPoints);
			}

			// This undo action seems to leak the material due to the object being passed to RecordObject being copied
			//Undo.RecordObject(target, "Modify Circle");
			
			if (primitive.ApplyModifiedProperties() || 
			    (Event.current.type == EventType.ValidateCommand && Event.current.commandName == "UndoRedoPerformed")) {
				if (PrefabUtility.GetPrefabType(target) != PrefabType.Prefab) {
					(target as Circle2D).UpdateMesh();
				}
			}
		}
		
	}

}
