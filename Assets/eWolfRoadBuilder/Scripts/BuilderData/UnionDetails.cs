using System;
using UnityEngine;
using System.Collections.Generic;
using eWolfRoadBuilderHelpers;

namespace eWolfRoadBuilder
{
    [Serializable]
    public class UnionDetails
    {
        /// <summary>
        /// The union type
        /// </summary>
        public RoadNetworkNode.UNION_TYPE Union;

        /// <summary>
        /// The array of nodes we are connected to
        /// </summary>
        public List<RoadNetworkNode> Roads;

        /// <summary>
        /// The number of sections for the mesh
        /// </summary>
        public int Sections = 5;

        /// <summary>
        /// The amount to sub divide each polygon
        /// </summary>
        public int Subdivide = 0;

        /// <summary>
        /// Has the node changed in someway
        /// </summary>
        public bool Modified = true;

        /// <summary>
        /// Second hand modified, by neighbours neighbour has changed
        /// </summary>
        public bool ModifiedSecondHand
        {
            get; set;
        }

        /// <summary>
        /// Draw all the roads linked
        /// </summary>
        /// <param name="gameObject"></param>
        internal void DrawRoads(GameObject gameObject)
        {
            if (Roads == null)
                return;

            Vector3 roadCorner = gameObject.transform.position;

            for (int i = 0; i < Roads.Count; i++)
            {
                if (Roads[i] == null)
                    return;

                Vector3 roadA = Roads[i].gameObject.transform.position;
                Gizmos.color = Color.yellow;
                if (Modified || Roads[i].Details.Modified)
                    Gizmos.color = Color.cyan;

                Gizmos.DrawLine(roadA, roadCorner);
                float roadWidth = RoadConstructorHelper.CrossSectionDetails.RoadWidthValue / 2;

                Gizmos.color = Color.green;
                Vector3 intersectionDown = Roads[i].GetOffSetDownRoad(roadCorner, 0);
                float a = Roads[i].GetAngleOfRoad(roadCorner);
                Vector3 endA = MathsHelper.OffSetVector(intersectionDown, a + (Mathf.PI / 2), roadWidth);
                Vector3 endB = MathsHelper.OffSetVector(intersectionDown, a - (Mathf.PI / 2), roadWidth);

                Gizmos.DrawLine(endA, endB);
            }
        }

        /// <summary>
        /// Update linked roads
        /// </summary>
        /// <param name="roadNode">The road To link to</param>
        public void UpdateLinkedRoads(RoadNetworkNode roadNode)
        {
            for (int i = 0; i < Roads.Count; i++)
            {
                if (Roads[i] != null)
                    Roads[i].Details.BackLink(roadNode);
            }
        }

        /// <summary>
        /// Compress the road array - remove any null roads
        /// </summary>
        public void CompressRoads()
        {
            for (int i = 0; i < Roads.Count - 1; i++)
            {
                if (Roads[i] == null)
                {
                    if (Roads[i + 1] != null)
                    {
                        Roads[i] = Roads[i + 1];
                        Roads[i + 1] = null;
                        Modified = true;
                    }
                }
            }

            int numberOfRoads = 0;
            for (int i = 0; i < Roads.Count; i++)
            {
                if (Roads[i] != null)
                    numberOfRoads++;
            }

            int roadsFromUnion = UnionHelper.GetRoadCount(Union);
            if (roadsFromUnion == numberOfRoads)
                return;

            Union = UnionHelper.SetRoadUnionTypeFromRoadCount(numberOfRoads);
            UpdateLinksnumber(numberOfRoads);
        }

        /// <summary>
        /// Link to the road node
        /// </summary>
        /// <param name="roadNode">The road To link to</param>
        private void BackLink(RoadNetworkNode roadNode)
        {
            foreach (RoadNetworkNode n in Roads)
            {
                if (n == roadNode)
                    return;
            }

            for (int i = 0; i < Roads.Count; i++)
            {
                if (Roads[i] == null)
                {
                    Roads[i] = roadNode;
                    Modified = true;
                    return;
                }
            }
        }

        /// <summary>
        /// Update the number of links
        /// </summary>
        /// <param name="roadCount">The number of links for this node</param>
        public void UpdateLinksnumber(int roadCount)
        {
            if (Roads == null)
            {
                Roads = new List<RoadNetworkNode>();
            }

            if (Roads.Count > roadCount)
            {
                int count = Roads.Count - roadCount;
                for (int i = 0; i < count; i++)
                {
                    Roads.RemoveAt(roadCount);
                    Modified = true;
                }
            }

            while (Roads.Count < roadCount)
            {
                Roads.Add(null);
                Modified = true;
            }
        }

        /// <summary>
        /// Can we extrude another road from this node
        /// </summary>
        /// <returns>True, we can add another road</returns>
        public bool CanExtrudeRoad()
        {
            return (Roads.Count < 6);
        }

        /// <summary>
        /// Is me or of the neighbours modified
        /// </summary>
        public bool IsNeighbourModified
        {
            get
            {
                if (Modified || ModifiedSecondHand)
                    return true;

                foreach (RoadNetworkNode rnn in Roads)
                {
                    if (rnn != null)
                    {
                        if (rnn.Details != null)
                        {
                            if (rnn.Details.Modified || rnn.Details.ModifiedSecondHand)
                                return true;
                        }
                    }
                }
                return false;
            }
        }

    }
}