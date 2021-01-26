using System;
using System.Collections.Generic;
using eWolfRoadBuilderHelpers;
using UnityEngine;

namespace eWolfRoadBuilder
{
    /// <summary>
    /// Class to order the roads for the cross road
    /// </summary>
    public class RoadCrossRoadOrder
	{
		/// <summary>
		/// Standard Constructor
		/// </summary>
		/// <param name="a">Guid A</param>
		/// <param name="nodeA">Node A</param>
		/// <param name="b">Guid B</param>
		/// <param name="nodeB">Node B</param>
		/// <param name="c">Guid C</param>
		/// <param name="nodeC">Node C</param>
		/// <param name="d">Guid D</param>
		/// <param name="nodeD">Node D</param>
		public RoadCrossRoadOrder(Guid a, RoadNetworkNode nodeA,
								Guid b, RoadNetworkNode nodeB,
								Guid c, RoadNetworkNode nodeC,
								Guid d, RoadNetworkNode nodeD)
		{
			_orderedAngles = new List<float>();
			_angles = new Dictionary<float, RoadCrossSection>();
			_nodes = new Dictionary<float, RoadNetworkNode>();

            AddAngle(a, nodeA);

            AddAngle(b, nodeB);

            AddAngle(c, nodeC);

            AddAngle(d, nodeD);

            _orderedAngles.Sort();
		}

        private void AddAngle(Guid id, RoadNetworkNode node)
        {
            RoadCrossSection road = IntersectionManager.Instance[id];
            float angle = MathsHelper.ClampAngle(road.Angle);
            while (_angles.ContainsKey(angle))
            {
                angle = angle + 0.001f;
                Debug.Log("Overlap node " + node.name);
            }
               
            _angles.Add(angle, road);
            _nodes.Add(angle, node);
            _orderedAngles.Add(angle);
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
        /// Reset the Opposite cross section
        /// </summary>
        /// <param name="crossSection">The new cross section to use</param>
        public void ReSetOpposite(RoadCrossSection crossSection)
        {
            _angles[_orderedAngles[_opposite]] = crossSection;
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
		/// Gets the opposite road
		/// </summary>
		public RoadCrossSection OppositeRoad
		{
			get { return _angles[_orderedAngles[_opposite]]; }
		}

		/// <summary>
		/// Gets the Opposite road node
		/// </summary>
		public RoadNetworkNode OppositeRoadNode
		{
			get { return _nodes[_orderedAngles[_opposite]]; }
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
		private int _middle = 1;
		private int _left = 0;
		private int _right = 2;
		private int _opposite = 3;
		private List<float> _orderedAngles;
		private Dictionary<float, RoadCrossSection> _angles;
		private Dictionary<float, RoadNetworkNode> _nodes;
		#endregion
	}
}
