using UnityEngine;
using UnityEditor;
using System.Collections;
using Primitives2D;


namespace Primitives2D {

	[CustomEditor(typeof(Quad2D))]
	public class Quad2DEditor : Editor {

		private SerializedObject primitive;
		private SerializedProperty color, rotationSpeed;
		private SerializedProperty snapValue, snapVertexPositions;

		private Vector3 snapPoint = Vector3.one * 0.1f;

		
		void OnEnable ()
		{
			primitive = new SerializedObject(target);
			color = primitive.FindProperty("color");
			rotationSpeed = primitive.FindProperty("rotationSpeed");
			snapValue = primitive.FindProperty("snapValue");
			snapVertexPositions = primitive.FindProperty("snapVertexPositions");
		}
		
		public override void OnInspectorGUI ()
		{
			primitive.Update();

			// This undo action seems to leak the material due to the object being passed to RecordObject being copied
			//Undo.RecordObject((target as Quad2D), "Modify Quad");

			EditorGUILayout.PropertyField(color);
			EditorGUILayout.PropertyField(rotationSpeed);
			EditorGUILayout.Space();

			if (GUILayout.Button("Add Collider")) {
				(target as Quad2D).AddCollider(4);
			}
			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(snapVertexPositions);
			if ((target as Quad2D).snapVertexPositions) {
				EditorGUILayout.PropertyField(snapValue);
			}
			EditorGUILayout.EndHorizontal();
			
			if (primitive.ApplyModifiedProperties() ||
			    (Event.current.type == EventType.ValidateCommand && Event.current.commandName == "UndoRedoPerformed")) {
				if (PrefabUtility.GetPrefabType(target) != PrefabType.Prefab) {
					(target as Quad2D).UpdateMesh();
				}
			}
		}

		void OnSceneGUI ()
		{
			Quad2D quad = (Quad2D)target;

			Undo.RecordObject(quad, "Move Quad Point");

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
