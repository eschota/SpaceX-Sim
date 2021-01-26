using System;
using System.Collections.Generic;
using UnityEngine;
using eWolfRoadBuilder;

namespace eWolfRoadBuilderHelpers
{
	/// <summary>
	/// Drop to the ground helper class
	/// </summary>
	public static class DropToGroundHelper
	{
		/// <summary>
		/// Drop the all of the road to the ground - in steps
		/// </summary>
		public static void StepDropRoad()
		{
			for (int j = 0; j < 100; j++)
			{
				foreach (Guid g in IntersectionManager.Instance.Intersections.Keys)
				{
					TryDropSection(g);
				}
			}
		}

		/// <summary>
		/// Try and drop this roadSection
		/// </summary>
		/// <param name="g">The road section to drop</param>
		private static void TryDropSection(Guid g)
		{
			RoadCrossSection master = (RoadCrossSection)IntersectionManager.Instance[g];
			RoadCrossSection clone = (RoadCrossSection)master.Clone();

			clone.DropToGroundSection();
			bool canDrop = true;

			int count = IntersectionManager.Instance.LinksCount;
			for (int i = 0; i < count; i++)
			{
				List<Guid> list = IntersectionManager.Instance[i];
				foreach (Guid id in list)
				{
					if (id == g)
					{
						if (!CanUseDropRoad(list, clone))
						{
							canDrop = false;
						}

						continue;
					}
				}
			}

			if (canDrop)
				master.DropToGroundSection();
		}

		/// <summary>
		/// Test if we can use this dropped road sections
		/// </summary>
		/// <param name="list">The list of roadSections</param>
		/// <param name="clone">The clone of the road section to replace</param>
		/// <returns>True if we can use this new position</returns>
		private static bool CanUseDropRoad(List<Guid> list, RoadCrossSection clone)
		{
			RoadCrossSection a = (RoadCrossSection)IntersectionManager.Instance[list[0]].Clone();
			RoadCrossSection b = (RoadCrossSection)IntersectionManager.Instance[list[1]].Clone();
			if (a.ID == clone.ID)
				a = clone;

			if (b.ID == clone.ID)
				b = clone;

			return !IntersectingGround(a.Left, a.Right, b.Left, b.Right);
		}

		/// <summary>
		/// Test to see if we are intersecting any ground object.
		/// </summary>
		/// <param name="a">Corner a</param>
		/// <param name="b">Corner b</param>
		/// <param name="c">Corner c</param>
		/// <param name="d">Corner d</param>
		/// <returns>True if we are intersecting</returns>
		private static bool IntersectingGround(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
		{
			if (!IsLineClear(a, b))
				return true;

			if (!IsLineClear(b, c))
				return true;

			if (!IsLineClear(c, d))
				return true;

			if (!IsLineClear(d, a))
				return true;

			if (!IsLineClear(a, c))
				return true;

			if (!IsLineClear(b, d))
				return true;

			return false;
		}

		/// <summary>
		/// Is the line clear - EG  no collision between the two points
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="posEnd"></param>
		/// <returns></returns>
		public static bool IsLineClear(Vector3 pos, Vector3 posEnd)
		{
			Vector3 diff = posEnd - pos;
			float mag = diff.magnitude;
			diff.Normalize();

			RaycastHit hitInfo;
			if (Physics.Raycast(pos, diff, out hitInfo, mag, 1 << LayerMask.NameToLayer("Ground")))
			{
				return false;
			}
			return true;
		}
	}
}