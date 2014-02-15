using UnityEngine;
using UnityEditor;
using System.Collections;
using Primitives2D;


namespace Primitives2D {

	[CustomEditor(typeof(Triangle2D))]
	public class Triangle2DEditor : Editor {

		private SerializedObject primitive;
		private SerializedProperty color, rotationSpeed;

		private Vector3 snapPoint = Vector3.one * 0.1f;


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
					(target as Triangle2D).UpdateMesh();
					(target as Triangle2D).UpdateColor();
				}
			}
		}

		void OnSceneGUI ()
		{
			Triangle2D tri = (Triangle2D)target;
			Transform triTransform = tri.transform;
			
			for (int i = 0; i < 3; ++i) {
				Vector3 oldPoint = triTransform.TransformPoint(tri.m_Vertices[i]);
				Vector3 newPoint = Handles.FreeMoveHandle(oldPoint, Quaternion.identity, 0.04f, snapPoint, Handles.DotCap);
				if (oldPoint != newPoint) {
					tri.m_Vertices[i] = triTransform.InverseTransformPoint(newPoint);
					tri.UpdateMesh();
				}
			}
		}

	}

}

