using UnityEngine;
using UnityEditor;
using System.Collections;
using Primitives2D;


namespace Primitives2D {

	[CustomEditor(typeof(Quad2D))]
	public class Quad2DEditor : Editor {

		private SerializedObject primitive;
		private SerializedProperty color;
		private SerializedProperty snapVertexPositions;

		// For handling vertex snapping.
		private float editorSnapValue;
		private bool snapValuesSame;
		private bool snapValueInRange;

		
		void OnEnable ()
		{
			primitive = new SerializedObject(target);
			color = primitive.FindProperty("color");

			snapVertexPositions = primitive.FindProperty("snapVertexPositions");
			editorSnapValue = (target as Quad2D).snapValue;
			snapValuesSame = true;
			snapValueInRange = true;
		}
		
		public override void OnInspectorGUI ()
		{
			Quad2D quadTarget = (Quad2D)target;

			primitive.Update();

			// This undo action seems to leak the material due to the object being passed to RecordObject being copied
			//Undo.RecordObject((target as Quad2D), "Modify Quad");

			// Material.
			if (!quadTarget.useCustomMaterial) {
				if (GUILayout.Button("Use Custom Material")) {
					quadTarget.RemoveDefaultMaterial();
				}
				EditorGUILayout.PropertyField(color);
			} else
			{
				if (GUILayout.Button("Use Default Material")) {
					quadTarget.AddDefaultMaterial();
				}
			}
			EditorGUILayout.Space();

			// Handle vertex snapping option.
			// New snap values must be applied in order to avoid erratic movement of the mesh vertices.
			// Thus, changes to the snap value are not directly made to the primitive's value, but instead go through a temporary 
			// variable (editorSnapValue) local to the editor script.
			EditorGUILayout.PropertyField(snapVertexPositions);
			if (quadTarget.snapVertexPositions) {

				EditorGUILayout.BeginHorizontal();
				editorSnapValue = EditorGUILayout.FloatField("Snap Value", editorSnapValue);

				if (editorSnapValue == quadTarget.snapValue)
					snapValuesSame = true;
				else
					snapValuesSame = false;

				// Only enable the Apply button if the snap value in the inspector is different from the primitive's value.
				GUI.enabled = !snapValuesSame;
				if (GUILayout.Button("Apply")) {
					snapValueInRange = VertexSnapper.Clamp(ref editorSnapValue, ref quadTarget.snapValue);
				}
				GUI.enabled = true;
				EditorGUILayout.EndHorizontal();
			}

			if (!snapValueInRange)
				EditorGUILayout.HelpBox("Snap value must be between 0.1 and 1.0", MessageType.Error, true);

			EditorGUILayout.Space();

			// Collider.
			if (GUILayout.Button("Add Collider")) {
				quadTarget.AddCollider(4);
			}

			// Handle updating of the primitive's mesh when changes are made.
			if (primitive.ApplyModifiedProperties() ||
			    (Event.current.type == EventType.ValidateCommand && Event.current.commandName == "UndoRedoPerformed")) {
				if (PrefabUtility.GetPrefabType(target) != PrefabType.Prefab) {
					quadTarget.UpdateMesh();
				}
			}
		}

		void OnSceneGUI ()
		{
			Quad2D quad = (Quad2D)target;

			Undo.RecordObject(quad, "Move Quad Point");

			// Handle manual vertex movement by user.
			for (int i = 0; i < 4; ++i) {
				Vector3 oldPoint = quad.transform.TransformPoint(quad.GetVertex(i));
				Vector3 newPoint = Handles.FreeMoveHandle(oldPoint, Quaternion.identity, VertexSnapper.VERTEX_HANDLE_SIZE, Vector3.one * 0.1f, Handles.DotCap);

				if (quad.snapVertexPositions) {
					VertexSnapper.SnapTo(quad.snapValue, ref newPoint);
				}

				if (oldPoint != newPoint) {
					quad.UpdateVertex(i, quad.transform.InverseTransformPoint(newPoint));
					quad.UpdateMesh();
				}
			}
		}
		
	}

}
