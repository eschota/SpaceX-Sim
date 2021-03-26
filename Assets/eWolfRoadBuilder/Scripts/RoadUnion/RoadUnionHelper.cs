using System;
using eWolfRoadBuilderHelpers;
using UnityEngine;

namespace eWolfRoadBuilder
{
    /// <summary>
    /// The Union base
    /// </summary>
    public class RoadUnionHelper
    {
        /// <summary>
        /// Creates the Union from the node union type
        /// </summary>
        /// <param name="roadNetworkNode">The node to create the union from</param>
        /// <returns>The union object</returns>
        public static IRoadUnion Creator(RoadNetworkNode roadNetworkNode)
        {
            IRoadUnion rub = null;
            switch (roadNetworkNode.Details.Union)
            {
                case RoadNetworkNode.UNION_TYPE.CORNER:
                    rub = new RoadUnionCorner(roadNetworkNode);
                    break;

                case RoadNetworkNode.UNION_TYPE.END:
                    rub = new RoadUnionEnd(roadNetworkNode);
                    break;

                case RoadNetworkNode.UNION_TYPE.JUNCTION:
                    rub = new RoadUnionJunction(roadNetworkNode);
                    break;

                case RoadNetworkNode.UNION_TYPE.CROSSROADS:
                    rub = new RoadUnionCross(roadNetworkNode);
                    break;

                case RoadNetworkNode.UNION_TYPE.FIVEROADS:
                    rub = new RoadUnionFiveRoads(roadNetworkNode);
                    break;

                case RoadNetworkNode.UNION_TYPE.SIXROADS:
                    rub = new RoadUnionSixRoads(roadNetworkNode);
                    break;
            }

            return rub;
        }

        /// <summary>
        /// Clamp the angle to radians
        /// </summary>
        /// <param name="an">The angle to clamp</param>
        /// <returns>The clamped value</returns>
        public static float AngleClamp(float an)
        {
            if (an < 0)
                an += (float)Math.PI * 2;
            if (an < 0)
                an += (float)Math.PI * 2;

            return an;
        }

        /// <summary>
        /// Get the angle of the road
        /// </summary>
        /// <param name="roadNetworkNode">The main node</param>
        /// <param name="index">Index of the road</param>
        /// <returns>The angle of the road</returns>
        public static float GetAngleOfRoad(RoadNetworkNode roadNetworkNode, int index)
        {
            GameObject StartObj = roadNetworkNode.gameObject;
            GameObject EndObj = roadNetworkNode.Details.Roads[index].gameObject;

            Vector3 startPosition = StartObj.transform.position;
            Vector3 endPosition = EndObj.transform.position;
            Vector3 diff = startPosition - endPosition;

            return MathsHelper.GetAngleFrom(diff.x, diff.z);
        }

        /// <summary>
        /// Get the angle of the road
        /// </summary>
        /// <param name="roadNetworkNode">The main node</param>
        /// <param name="index">Index of the road</param>
        /// <returns>The angle of the road</returns>
        public static float GetAngleOfRoadClampped(RoadNetworkNode roadNetworkNode, int index)
        {
            GameObject StartObj = roadNetworkNode.gameObject;
            GameObject EndObj = roadNetworkNode.Details.Roads[index].gameObject;

            Vector3 startPosition = StartObj.transform.position;
            Vector3 endPosition = EndObj.transform.position;
            Vector3 diff = startPosition - endPosition;

            return MathsHelper.ClampAngle(MathsHelper.GetAngleFrom(diff.x, diff.z));
        }

        /// <summary>
        /// Define the road and cross section
        /// </summary>
        /// <param name="couveSize">The size off set</param>
        /// <param name="index">The road index</param>
        /// <param name="roadNetworkNode">The main road node</param>
        /// <param name="road">The rode node to populate</param>
        /// <param name="crossSection">The cross section to populate</param>
        public static void DefineCrossSectionOffSet(float couveSize, int index, RoadNetworkNode roadNetworkNode, out RoadNetworkNode road, out RoadCrossSection crossSection)
        {
            road = roadNetworkNode.Details.Roads[index];
            Vector3 pos = roadNetworkNode.gameObject.transform.position;
            float angleA = RoadUnionHelper.AngleClamp(RoadUnionHelper.GetAngleOfRoad(roadNetworkNode, index) + (Mathf.PI / 2));
            Vector3 roadPointA = road.GetOffSetDownRoad(pos, (RoadConstructorHelper.CrossSectionDetails.RoadWidthValue * couveSize));
            crossSection = new RoadCrossSection(roadPointA, angleA - (float)(Math.PI / 2), RoadConstructorHelper.CrossSection(road), RoadConstructorHelper.Materials(road));
        }
    }
}