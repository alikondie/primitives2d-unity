using UnityEngine;
using UnityEditor;
using System.Collections;
using Primitives2D;


namespace Primitives2D {

	[CustomEditor(typeof(Triangle2D))]
	public class Triangle2DEditor : Editor {

		private SerializedObject primitive;
		private SerializedProperty color;
		private SerializedProperty snapVertexPositions;

		// For vertex snapping.
		private float editorSnapValue;
		private bool snapValuesSame;
		private bool snapValueInRange;


		void OnEnable ()
		{
			primitive = new SerializedObject(target);
			color = primitive.FindProperty("color");

			snapVertexPositions = primitive.FindProperty("snapVertexPositions");
			editorSnapValue = (target as Triangle2D).snapValue;
			snapValuesSame = true;
			snapValueInRange = true;
		}
		
		public override void OnInspectorGUI ()
		{
			Triangle2D triTarget = (Triangle2D)target;

			primitive.Update();

			// This undo action seems to leak the material due to the object being passed to RecordObject being copied
			// Undo.RecordObject(target, "Modify Triangle");

			// Material.
			if (!triTarget.useCustomMaterial) {
				if (GUILayout.Button("Use Custom Material")) {
					triTarget.RemoveDefaultMaterial();
				}
				EditorGUILayout.PropertyField(color);
			} else
			{
				if (GUILayout.Button("Use Default Material")) {
					triTarget.AddDefaultMaterial();
				}
			}
			EditorGUILayout.Space();

			// Handle vertex snapping option.
			// New snap values must be applied in order to avoid erratic movement of the mesh vertices.
			// Thus, changes to the snap value are not directly made to the primitive's value, but instead go through a temporary 
			// variable (editorSnapValue) local to the editor script.
			EditorGUILayout.PropertyField(snapVertexPositions);
			if (triTarget.snapVertexPositions) {
				
				EditorGUILayout.BeginHorizontal();
				editorSnapValue = EditorGUILayout.FloatField("Snap Value", editorSnapValue);
				
				if (editorSnapValue == triTarget.snapValue)
					snapValuesSame = true;
				else
					snapValuesSame = false;
				
				// Only enable the Apply button if the snap value in the inspector is different from the primitive's value.
				GUI.enabled = !snapValuesSame;
				if (GUILayout.Button("Apply")) {
					snapValueInRange = VertexSnapper.Clamp(ref editorSnapValue, ref triTarget.snapValue);
				}
				GUI.enabled = true;
				EditorGUILayout.EndHorizontal();
			}

			if (!snapValueInRange)
				EditorGUILayout.HelpBox("Snap value must be between 0.1 and 1.0", MessageType.Error, true);

			EditorGUILayout.Space();

			// Collider.
			if (GUILayout.Button("Add Collider")) {
				triTarget.AddCollider(3);
			}

			// Handle updating of the primitive's mesh when changes are made.
			if (primitive.ApplyModifiedProperties() ||
			    (Event.current.type == EventType.ValidateCommand && Event.current.commandName == "UndoRedoPerformed")) {
				if (PrefabUtility.GetPrefabType(target) != PrefabType.Prefab) {
					triTarget.UpdateMesh();
				}
			}
		}

		void OnSceneGUI ()
		{
			Triangle2D tri = (Triangle2D)target;

			Undo.RecordObject(tri, "Move Triangle Point");

			// Handle manual vertex movement by user.
			for (int i = 0; i < 3; ++i) {
				Vector3 oldPoint = tri.transform.TransformPoint(tri.GetVertex(i));
				Vector3 newPoint = Handles.FreeMoveHandle(oldPoint, Quaternion.identity, VertexSnapper.VERTEX_HANDLE_SIZE, Vector3.one * 0.1f, Handles.DotCap);

				if (tri.snapVertexPositions) {
					VertexSnapper.SnapTo(tri.snapValue, ref newPoint);
				}

				if (oldPoint != newPoint) {
					tri.UpdateVertex(i, tri.transform.InverseTransformPoint(newPoint));
					tri.UpdateMesh();
				}
			}
		}

	}

}

