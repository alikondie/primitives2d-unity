using UnityEngine;
using UnityEditor;
using System.Collections;
using Primitives2D;


namespace Primitives2D {

	[CustomEditor(typeof(Triangle2D))]
	public class Triangle2DEditor : Editor {

		private SerializedObject triangle;
		private SerializedProperty color, rotationSpeed;
		private SerializedProperty snapVertexPositions;

		private Vector3 snapPoint = Vector3.one * 0.1f;
		private float editorSnapValue;
		private bool snapValuesSame;
		private bool displaySnapValueError;


		void OnEnable ()
		{
			triangle = new SerializedObject(target);
			color = triangle.FindProperty("color");
			rotationSpeed = triangle.FindProperty("rotationSpeed");
			snapVertexPositions = triangle.FindProperty("snapVertexPositions");
			editorSnapValue = (target as Triangle2D).snapValue;
			snapValuesSame = true;
			displaySnapValueError = false;
		}
		
		public override void OnInspectorGUI ()
		{
			Triangle2D triTarget = (Triangle2D)target;

			triangle.Update();

			// This undo action seems to leak the material due to the object being passed to RecordObject being copied
			// Undo.RecordObject(target, "Modify Triangle");

			// Draw basic property fields defined in base class.
			EditorGUILayout.PropertyField(color);
			EditorGUILayout.PropertyField(rotationSpeed);
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
					if (editorSnapValue < 0.1f) {
						editorSnapValue = 0.1f;
						displaySnapValueError = true;
					}
					else if (editorSnapValue > 1.0f) {
						editorSnapValue = 1.0f;
						displaySnapValueError = true;
					}
					else {
						triTarget.snapValue = editorSnapValue;
						displaySnapValueError = false;
					}
				}
				GUI.enabled = true;
				EditorGUILayout.EndHorizontal();
			}

			if (displaySnapValueError)
				EditorGUILayout.HelpBox("Snap value must be between 0.1 and 1.0", MessageType.Error, true);

			EditorGUILayout.Space();

			// Option for adding a collider to the primitive.
			if (GUILayout.Button("Add Collider")) {
				triTarget.AddCollider(3);
			}

			// Handle updating of the primitive's mesh when changes are made.
			if (triangle.ApplyModifiedProperties() ||
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
				Vector3 oldPoint = tri.transform.TransformPoint(tri.m_Vertices[i]);
				Vector3 newPoint = Handles.FreeMoveHandle(oldPoint, Quaternion.identity, 0.04f, snapPoint, Handles.DotCap);

				if (tri.snapVertexPositions) {
					newPoint.x = (float)Mathf.RoundToInt(newPoint.x / tri.snapValue) * tri.snapValue;
					newPoint.y = (float)Mathf.RoundToInt(newPoint.y / tri.snapValue) * tri.snapValue;
				}

				if (oldPoint != newPoint) {
					tri.UpdateVertex(i, tri.transform.InverseTransformPoint(newPoint));
					tri.UpdateMesh();
				}
			}
		}

	}

}

