using System;
using UnityEngine;

namespace eWolfRoadBuilderHelpers
{
	public static class MathsHelper
	{
        /// <summary>
        /// Gets the cloest potin from two lines
        /// </summary>
        /// <param name="closestPointLine1">The cloest point from line 1</param>
        /// <param name="closestPointLine2">The cloest point from line 2</param>
        /// <param name="linePoint1">The start of line 1</param>
        /// <param name="lineVec1">The direction of line 1</param>
        /// <param name="linePoint2">The start of line 2</param>
        /// <param name="lineVec2">The direction of line 2</param>
        /// <returns>True if we have found a cloest point</returns>
		public static bool ClosestPointsOnTwoLines(out Vector3 closestPointLine1, out Vector3 closestPointLine2, Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2)
		{
			closestPointLine1 = Vector3.zero;
			closestPointLine2 = Vector3.zero;

			float a = Vector3.Dot(lineVec1, lineVec1);
			float b = Vector3.Dot(lineVec1, lineVec2);
			float e = Vector3.Dot(lineVec2, lineVec2);

			float d = (a * e) - (b * b);
            
            float roundedDown = (float)Math.Round(d, 5);

			// lines are not parallel
			if (roundedDown != 0.0f)
			{
				Vector3 r = linePoint1 - linePoint2;
				float c = Vector3.Dot(lineVec1, r);
				float f = Vector3.Dot(lineVec2, r);

				float s = ((b * f) - (c * e)) / d;
				float t = ((a * f) - (c * b)) / d;

				closestPointLine1 = linePoint1 + (lineVec1 * s);
				closestPointLine2 = linePoint2 + (lineVec2 * t);

				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Off set the vector with angle and distance
		/// </summary>
		/// <param name="start">The starting vector</param>
		/// <param name="angle">The angle of the off set</param>
		/// <param name="magnitude">The magnitude off set</param>
		/// <returns>The new off set positon</returns>
		public static Vector3 OffSetVector(Vector3 start, float angle, float magnitude)
		{
			Vector3 offSetPosition = start;
			float x = Mathf.Sin((float)angle) * magnitude;
			float z = Mathf.Cos((float)angle) * magnitude;
			offSetPosition.x -= x;
			offSetPosition.z += z;
			return offSetPosition;
		}

		/// <summary>
		/// Off set the vector with angle and distance
		/// </summary>
		/// <param name="start">The starting vector</param>
		/// <param name="angle">The angle of the off set</param>
		/// <param name="magnitude">The magnitude off set</param>
		/// <returns>The new off set positon</returns>
		public static Vector3 OffSetVector90(Vector3 start, float angle, float magnitude)
		{
			Vector3 offSetPosition = start;
			float x = Mathf.Sin((float)angle) * magnitude;
			float z = Mathf.Cos((float)angle) * magnitude;
			offSetPosition.x += x;
			offSetPosition.z -= z;
			return offSetPosition;
		}

        /// <summary>
        /// Get the normal from the given triangle
        /// </summary>
        /// <param name="a">point a</param>
        /// <param name="b">point b</param>
        /// <param name="c">point c</param>
        /// <returns>The noraml of the 3 points</returns>
		public static Vector3 NormalTri(Vector3 a, Vector3 b, Vector3 c)
		{
			Vector3 nor = new Vector3(0, 0, 0);
			Vector3 sideA = b - a;
			Vector3 sideB = c - a;

			nor.x = (sideA.y * sideB.z) - (sideA.z * sideB.y);
			nor.y = (sideA.z * sideB.x) - (sideA.x * sideB.z);
			nor.z = (sideA.x * sideB.y) - (sideA.y * sideB.x);

			return nor;
		}

		/// <summary>
		/// Get the angle between the two points
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns>The angle of the two points</returns>
		static public float GetAngleFrom(float x, float y)
		{
			return (float)Mathf.Atan2(y, x);
		}

		/// <summary>
		/// Normalize the angle
		/// </summary>
		/// <param name="angle">The angle to normalize</param>
		/// <returns>The normalize angle</returns>
		static public float ClampAngle(float angle)
		{
			if (angle > (float)Mathf.PI * 2)
				angle -= (float)Mathf.PI * 2;

			if (angle < 0)
				angle += (float)Mathf.PI * 2;

			return angle;
		}
	}
}