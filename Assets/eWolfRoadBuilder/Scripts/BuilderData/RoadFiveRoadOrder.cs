using System;
using System.Collections.Generic;
using eWolfRoadBuilderHelpers;
using UnityEngine;

namespace eWolfRoadBuilder
{
    /// <summary>
    /// Class to order the roads for the five road
    /// </summary>
    public class RoadFiveRoadOrder
    {
        /// <summary>
        /// Standared constructor
        /// </summary>
        /// <param name="roadIds">The list of ids</param>
        /// <param name="roadNodes">The list of roads</param>
        public RoadFiveRoadOrder(List<Guid> roadIds, List<RoadNetworkNode> roadNodes)
        {
            _orderedAngles = new List<float>();
            _angles = new Dictionary<float, RoadCrossSection>();
            _nodes = new Dictionary<float, RoadNetworkNode>();

            for (int i = 0; i < roadIds.Count; i++)
            {
                RoadCrossSection road = IntersectionManager.Instance[roadIds[i]];
                float angle = MathsHelper.ClampAngle(road.Angle);
                _angles.Add(angle, road);
                _nodes.Add(angle, roadNodes[i]);
                _orderedAngles.Add(angle);
            }
            _orderedAngles.Sort();
        }

        /// <summary>
        /// Reset the indexed cross section
        /// </summary>
        /// <param name="crossSection">The new cross section to use</param>
        public void ReSet(int i, RoadCrossSection roadCrossSection)
        {
            _angles[_orderedAngles[i]] = roadCrossSection;
        }
        
        /// <summary>
        /// Gets the indexed road cross section
        /// </summary>
        /// <param name="index">The index of the road cross section</param>
        /// <returns>The cross section of the road</returns>
        public RoadCrossSection Road(int index)
        {
            return _angles[_orderedAngles[index]];
        }

        /// <summary>
        /// Gets the indexed road node
        /// </summary>
        /// <param name="index">The index of the road</param>
        /// <returns>The node for the road</returns>
        internal RoadNetworkNode RoadNode(int index)
        {
            return _nodes[_orderedAngles[index]];
        }

        /// <summary>
        /// Get the road index to the right
        /// </summary>
        /// <param name="mainRoad">The current road index</param>
        /// <returns>Road index to the right</returns>
        public  int GetNextRightIndex(int mainRoad)
        {
            int rightRoad = mainRoad + 1;
            if (rightRoad > _orderedAngles.Count-1)
                rightRoad -= _orderedAngles.Count;

            return rightRoad;
        }

        /// <summary>
        /// Get the road index to the left
        /// </summary>
        /// <param name="mainRoad">The current road index</param>
        /// <returns>Road index to the left</returns>
        public int GetNextLeftIndex(int mainRoad)
        {
            int leftRoad = mainRoad - 1;
            if (leftRoad < 0)
                leftRoad += _orderedAngles.Count;

            return leftRoad;
        }
        
        #region Private Fields
        private List<float> _orderedAngles;
        private Dictionary<float, RoadCrossSection> _angles;
        private Dictionary<float, RoadNetworkNode> _nodes;
        #endregion
    }
}
