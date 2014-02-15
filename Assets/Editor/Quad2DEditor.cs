using UnityEngine;
using UnityEditor;
using System.Collections;
using Primitives2D;

namespace Primitives2D {

	[CustomEditor(typeof(Quad2D))]
	public class Quad2DEditor : Editor {
		
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
					(target as Quad2D).UpdateMesh();
					(target as Quad2D).UpdateColor();
				}
			}
		}

		void OnSceneGUI ()
		{
			Quad2D quad = (Quad2D)target;
			Transform quadTransform = quad.transform;

			for (int i = 0; i < 4; ++i) {
				Vector3 oldPoint = quadTransform.TransformPoint(quad.m_Vertices[i]);
				Vector3 newPoint = Handles.FreeMoveHandle(oldPoint, Quaternion.identity, 0.04f, snapPoint, Handles.DotCap);
				if (oldPoint != newPoint) {
					quad.m_Vertices[i] = quadTransform.InverseTransformPoint(newPoint);
					quad.UpdateMesh();
				}
			}
		}
		
	}

}
