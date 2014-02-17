using UnityEngine;
using System.Collections;


namespace Primitives2D {

	public static class VertexSnapper {

		public const float VERTEX_HANDLE_SIZE = 0.04f;

		private const float MIN_SNAP_VALUE = 0.1f;
		private const float MAX_SNAP_VALUE = 1.0f;


		/// <summary>
		/// Clamp the snap value set in the inspector to the min/max allowed, and only update the object's snap value if it is within range. 
		/// This method returns false if the value falls outside the range, signalling that an error message should be displayed in the inspector.
		/// </summary>
		/// <param name="editorValue">The temporary snap value displayed in the inspector.</param>
		/// <param name="objectValue">The primitive's snap value.</param>
		public static bool Clamp (ref float editorValue, ref float objectValue)
		{
			bool inRange;

			if (editorValue < MIN_SNAP_VALUE) {
				editorValue = MIN_SNAP_VALUE;
				inRange = false;
			}
			else if (editorValue > MAX_SNAP_VALUE) {
				editorValue = MAX_SNAP_VALUE;
				inRange = false;
			}
			else {
				objectValue = editorValue;
				inRange = true;
			}

			return inRange;
		}

		/// <summary>
		/// Snaps the given vertex point to the snap value of the object.
		/// </summary>
		/// <param name="snapValue">Snap value set and applied to the primitive in the inspector.</param>
		/// <param name="point">The vertex point to snap into place.</param>
		public static void SnapTo (float snapValue, ref Vector3 point)
		{
			point.x = (float)Mathf.RoundToInt(point.x / snapValue) * snapValue;
			point.y = (float)Mathf.RoundToInt(point.y / snapValue) * snapValue;
		}

	}

}

