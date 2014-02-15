﻿using UnityEngine;
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
			
			if (primitive.ApplyModifiedProperties()) {
				if (PrefabUtility.GetPrefabType(target) != PrefabType.Prefab) {
					(target as Circle2D).UpdateMesh();
					(target as Circle2D).UpdateColor();
				}
			}
		}
		
	}

}