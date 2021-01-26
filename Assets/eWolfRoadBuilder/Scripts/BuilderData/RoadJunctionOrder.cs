using System;
using System.Collections.Generic;
using eWolfRoadBuilderHelpers;
using UnityEngine;

namespace eWolfRoadBuilder
{
	/// <summary>
	/// The class to hold the road order
	/// </summary>
	public class RoadJunctionOrder
	{
		/// <summary>
		/// Standard Constructor
		/// </summary>
		/// <param name="a">Guid A</param>
		/// <param name="nodeA">Node a</param>
		/// <param name="b">Guid B</param>
		/// <param name="nodeB">Node B</param>
		/// <param name="c">Giud C</param>
		/// <param name="nodeC">Node C</param>
		public RoadJunctionOrder(Guid a, RoadNetworkNode nodeA, Guid b, RoadNetworkNode nodeB, Guid c, RoadNetworkNode nodeC)
		{
			_orderedAngles = new List<float>();
			_angles = new Dictionary<float, RoadCrossSection>();
			_nodes = new Dictionary<float, RoadNetworkNode>();

			RoadCrossSection road = IntersectionManager.Instance[a];
			float angle = RoadUnionHelper.AngleClamp(road.Angle);
            
			_angles.Add(angle, road); 
			_nodes.Add(angle, nodeA);
			_orderedAngles.Add(angle);

			road = IntersectionManager.Instance[b];
			angle = RoadUnionHelper.AngleClamp(road.Angle);
            if (_angles.ContainsKey(angle))
                angle += 0.001f;
			_angles.Add(angle, road);
			_nodes.Add(angle, nodeB);
			_orderedAngles.Add(angle);

			road = IntersectionManager.Instance[c];
			angle = RoadUnionHelper.AngleClamp(road.Angle);
            if (_angles.ContainsKey(angle))
                angle += 0.002f;
             if (_angles.ContainsKey(angle))
                angle += 0.001f;
			_angles.Add(angle, road);
			_nodes.Add(angle, nodeC);
			_orderedAngles.Add(angle);

			_orderedAngles.Sort();

			AnglesGaps agA = new AnglesGaps(_orderedAngles[0], _orderedAngles[1], 2);
			AnglesGaps agB = new AnglesGaps(_orderedAngles[1], _orderedAngles[2], 0);
			AnglesGaps agC = new AnglesGaps(_orderedAngles[2], _orderedAngles[0], 1);

			Dictionary<float, AnglesGaps> angleGapArray = new Dictionary<float, AnglesGaps>();

            if (agA.RoadAngleGap == agB.RoadAngleGap)
                agB.RoadAngleGap += 0.001f;

            if (agB.RoadAngleGap == agC.RoadAngleGap)
                agC.RoadAngleGap += 0.001f;

            if (agA.RoadAngleGap == agC.RoadAngleGap)
                agC.RoadAngleGap += 0.002f;
            
            angleGapArray.Add(agA.RoadAngleGap, agA);
			angleGapArray.Add(agB.RoadAngleGap, agB);
			angleGapArray.Add(agC.RoadAngleGap, agC);

			List<float> orderedGapAngles = new List<float>();
			orderedGapAngles.Add(agA.RoadAngleGap);
			orderedGapAngles.Add(agB.RoadAngleGap);
			orderedGapAngles.Add(agC.RoadAngleGap);
			orderedGapAngles.Sort();

			_middle = angleGapArray[orderedGapAngles[2]].RoadIndex;
			_left = _middle - 1;
			if (_left < 0)
				_left = 2;

			_right = _middle + 1;
			if (_right > 2)
				_right = 0;
		}

        /// <summary>
        /// Reset the left cross section
        /// </summary>
        /// <param name="crossSection">The new cross section to use</param>
        public void ReSetLeft(RoadCrossSection crossSection)
        {
            _angles[_orderedAngles[_left]] = crossSection;
        }

        /// <summary>
        /// Reset the right cross section
        /// </summary>
        /// <param name="crossSection">The new cross section to use</param>
        public void ReSetRight(RoadCrossSection crossSection)
        {
            _angles[_orderedAngles[_right]] = crossSection;
        }

        /// <summary>
        /// Reset the middle cross section
        /// </summary>
        /// <param name="crossSection">The new cross section to use</param>
        public void ReSetMiddle(RoadCrossSection crossSection)
        {
            _angles[_orderedAngles[_middle]] = crossSection;
        }
        
        /// <summary>
        /// Holder the gaps of the angles
        /// </summary>
        private class AnglesGaps
		{
			public AnglesGaps(float angleA, float angleB, int roadIndex)
			{
				_roadIndex = roadIndex;

				Radian rA = new Radian(angleA);
				Radian rB = new Radian(angleB);

				rA.Value = rA.Value - rB.Value;
				float diff = rA.Value;

				if (diff > Math.PI)
					diff = (float)(Math.PI * 2) - diff;

				_roadGap = diff;
			}

			/// <summary>
			/// Gets the opersite road index
			/// </summary>
			public int RoadIndex
			{
				get { return _roadIndex; }
			}

			/// <summary>
			/// Gets or Sets the road angles
			/// </summary>
			public float RoadAngleGap
			{
				get { return _roadGap; }
                set { _roadGap = value; }
			}

			#region Private Fields
			private float _roadGap;
			private int _roadIndex;
			#endregion
		}

		/// <summary>
		/// Gets the middle road
		/// </summary>
		public RoadCrossSection MiddleRoad
		{
			get { return _angles[_orderedAngles[_middle]]; }
		}

		/// <summary>
		/// Gets the middle road node
		/// </summary>
		public RoadNetworkNode MiddleRoadNode
		{
			get { return _nodes[_orderedAngles[_middle]]; }
		}

		/// <summary>
		/// Gets the left road
		/// </summary>
		public RoadCrossSection LeftRoad
		{
			get { return _angles[_orderedAngles[_left]]; }
		}

		/// <summary>
		/// Gets the left road node
		/// </summary>
		public RoadNetworkNode LeftRoadNode
		{
			get { return _nodes[_orderedAngles[_left]]; }
		}

		/// <summary>
		/// Gets the right road
		/// </summary>
		public RoadCrossSection RightRoad
		{
			get { return _angles[_orderedAngles[_right]]; }
		}

		/// <summary>
		/// Gets the right road node
		/// </summary>
		public RoadNetworkNode RightRoadNode
		{
			get { return _nodes[_orderedAngles[_right]]; }
		}

		#region Private Fields
		private int _middle = -1;
		private int _left = -1;
		private int _right = -1;
		private List<float> _orderedAngles;
		private Dictionary<float, RoadCrossSection> _angles;
		private Dictionary<float, RoadNetworkNode> _nodes;
		#endregion
	}
}
