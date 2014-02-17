using UnityEngine;
using UnityEditor;
using System.Collections;
using Primitives2D;


namespace Primitives2D {

	[CustomEditor(typeof(Quad2D))]
	public class Quad2DEditor : Editor {

		private SerializedObject primitive;
		private SerializedProperty color, rotationSpeed;
		private SerializedProperty snapVertexPositions;

		private Vector3 snapPoint = Vector3.one * 0.1f;
		private float editorSnapValue;
		private bool snapValuesSame;
		private bool displaySnapValueError;

		
		void OnEnable ()
		{
			primitive = new SerializedObject(target);
			color = primitive.FindProperty("color");
			rotationSpeed = primitive.FindProperty("rotationSpeed");
			snapVertexPositions = primitive.FindProperty("snapVertexPositions");
			editorSnapValue = (target as Quad2D).snapValue;
			snapValuesSame = true;
			displaySnapValueError = false;
		}
		
		public override void OnInspectorGUI ()
		{
			Quad2D quadTarget = (Quad2D)target;

			primitive.Update();

			// This undo action seems to leak the material due to the object being passed to RecordObject being copied
			//Undo.RecordObject((target as Quad2D), "Modify Quad");

			// Draw basic property fields defined in base class.
			EditorGUILayout.PropertyField(color);
			EditorGUILayout.PropertyField(rotationSpeed);
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
					if (editorSnapValue < 0.1f) {
						editorSnapValue = 0.1f;
						displaySnapValueError = true;
					}
					else if (editorSnapValue > 1.0f) {
						editorSnapValue = 1.0f;
						displaySnapValueError = true;
					}
					else {
						quadTarget.snapValue = editorSnapValue;
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
				Vector3 oldPoint = quad.transform.TransformPoint(quad.m_Vertices[i]);
				Vector3 newPoint = Handles.FreeMoveHandle(oldPoint, Quaternion.identity, 0.04f, snapPoint, Handles.DotCap);

				if (quad.snapVertexPositions) {
					newPoint.x = (float)Mathf.RoundToInt(newPoint.x / quad.snapValue) * quad.snapValue;
					newPoint.y = (float)Mathf.RoundToInt(newPoint.y / quad.snapValue) * quad.snapValue;
				}

				if (oldPoint != newPoint) {
					quad.UpdateVertex(i, quad.transform.InverseTransformPoint(newPoint));
					quad.UpdateMesh();
				}
			}
		}
		
	}

}
