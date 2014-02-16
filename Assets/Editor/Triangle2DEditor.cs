using UnityEngine;
using UnityEditor;
using System.Collections;
using Primitives2D;


namespace Primitives2D {

	[CustomEditor(typeof(Triangle2D))]
	public class Triangle2DEditor : Editor {

		private SerializedObject triangle;
		private SerializedProperty color, rotationSpeed;
		private SerializedProperty snapValue, snapVertexPositions;

		private Vector3 snapPoint = Vector3.one * 0.1f;


		void OnEnable ()
		{
			triangle = new SerializedObject(target);
			color = triangle.FindProperty("color");
			rotationSpeed = triangle.FindProperty("rotationSpeed");
			snapValue = triangle.FindProperty("snapValue");
			snapVertexPositions = triangle.FindProperty("snapVertexPositions");
		}
		
		public override void OnInspectorGUI ()
		{
			triangle.Update();

			EditorGUILayout.PropertyField(color);
			EditorGUILayout.PropertyField(rotationSpeed);
			EditorGUILayout.Space();

			if (GUILayout.Button("Add Collider")) {
				(target as Triangle2D).AddCollider(3);
			}
			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(snapVertexPositions);
			if ((target as Triangle2D).snapVertexPositions) {
				EditorGUILayout.PropertyField(snapValue);
			}
			EditorGUILayout.EndHorizontal();

			// This undo action seems to leak the material due to the object being passed to RecordObject being copied
			// Undo.RecordObject(target, "Modify Triangle");

			if (triangle.ApplyModifiedProperties() ||
			    (Event.current.type == EventType.ValidateCommand && Event.current.commandName == "UndoRedoPerformed")) {
				if (PrefabUtility.GetPrefabType(target) != PrefabType.Prefab) {
					(target as Triangle2D).UpdateMesh();
				}
			}
		}

		void OnSceneGUI ()
		{
			Triangle2D tri = (Triangle2D)target;

			Undo.RecordObject(tri, "Move Triangle Point");
			
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

