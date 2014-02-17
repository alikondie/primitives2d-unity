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
			Circle2D circleTarget = (Circle2D)target;

			primitive.Update();

			// This undo action seems to leak the material due to the object being passed to RecordObject being copied
			//Undo.RecordObject(target, "Modify Circle");

			// Draw basic property fields defined in base class.
			EditorGUILayout.PropertyField(color);
			EditorGUILayout.PropertyField(numPoints);
			EditorGUILayout.Space();

			// Option for adding a collider to the primitive.
			if (GUILayout.Button("Add Collider")) {
				circleTarget.AddCollider(circleTarget.numPoints);
			}

			// Handle updating of the primitive's mesh when changes are made.
			if (primitive.ApplyModifiedProperties() || 
			    (Event.current.type == EventType.ValidateCommand && Event.current.commandName == "UndoRedoPerformed")) {
				if (PrefabUtility.GetPrefabType(target) != PrefabType.Prefab) {
					circleTarget.UpdateMesh();
				}
			}
		}
		
	}

}
