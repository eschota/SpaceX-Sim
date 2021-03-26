using eWolfRoadBuilderHelpers;
using UnityEngine;

namespace eWolfRoadBuilder
{
	/// <summary>
	/// The road network layout
	/// </summary>
	public class RoadNetworkLayout : MonoBehaviour
	{
		/// <summary>
		/// Remove all the nodes from this road
		/// </summary>
		public void RemoveAllNodes()
		{
			ClearCurrentNetwork();
		}

		/// <summary>
		/// Create the a starting node
		/// </summary>
        /// <returns>The newly created road section</returns>
		public GameObject StartNewRoad()
		{
			GameObject rnnA = RoadNetworkNodeHelper.CreateBasicNode(new Vector3(0, 5, 0), "R_0000", this.gameObject);
			rnnA.transform.parent = this.gameObject.transform;

            return rnnA;
		}

        /// <summary>
        /// can we remove all the nodes
        /// </summary>
        /// <returns>Whether we have child road nodes</returns>
        public bool CanRemoveAllNodes()
        {
            foreach (Transform child in gameObject.transform)
            {
                RoadNetworkNode rnr = child.gameObject.GetComponent<RoadNetworkNode>();
                if (rnr != null)
                {
                    return true;
                }
            }

            return false;
        }

		/// <summary>
		/// Remove node from all links
		/// </summary>
		/// <param name="node">The node to remove</param>
		internal static void RemoveLinkFrom(RoadNetworkNode node)
		{
			for (int r = 0; r < node.Details.Roads.Count; r++)
			{
				RoadNetworkNode innerRoad = node.Details.Roads[r];

				for (int i = 0; i < innerRoad.Details.Roads.Count; i++)
				{
                    if (innerRoad.Details.Roads[i] == node)
                    {
                        innerRoad.Details.Roads[i] = null;
                        node.Details.Modified = true;
                    }
				}

				innerRoad.Details.CompressRoads();
			}
		}

		/// <summary>
		/// Add a road
		/// </summary>
		/// <param name="node">The node to add the road too</param>
		/// <param name="nodeToAdd">The node to add</param>
		public static void AddRoadToNode(RoadNetworkNode node, RoadNetworkNode nodeToAdd)
		{
            if (node.HasLinksToNode(nodeToAdd))
            {
                return;
            }
            
            int roadCount = UnionHelper.GetRoadCount(node.Details.Union) + 1;
            node.Details.Union = UnionHelper.SetRoadUnionTypeFromRoadCount(roadCount);

            node.CreateRoadArray();
			for (int r = 0; r < node.Details.Roads.Count; r++)
			{
				if (node.Details.Roads[r] == null)
				{
					node.Details.Roads[r] = nodeToAdd;
                    node.Details.Modified = true;
                    return;
				}
			}
		}

		/// <summary>
		/// Clear the current network
		/// </summary>
		private void ClearCurrentNetwork()
		{
			bool more = true;
			while (more)
			{
				more = false;
				foreach (Transform child in gameObject.transform)
				{
					RoadNetworkNode rnr = child.gameObject.GetComponent<RoadNetworkNode>();
					if (rnr != null)
					{
						DestroyImmediate(child.gameObject);
						more = true;
					}
				}
			}
		}

		/// <summary>
		/// On Draw Gizmo selection. Draw when selected
		/// </summary>
		public void OnDrawGizmosSelected()
		{
			RoadBuilder rb = GetComponent<RoadBuilder>();
			RoadConstructorHelper.CrossSectionDetails = rb.CrossSectionDetails;
            RoadConstructorHelper.Lighting = rb.Lighting;
            RoadConstructorHelper.MaterialFrequencySet = rb;
			RoadConstructorHelper.BaseNodeLayoutNode = GetComponent<RoadNetworkLayout>();

			foreach (Transform child in gameObject.transform)
			{
				RoadNetworkNode rnr = child.gameObject.GetComponent<RoadNetworkNode>();
				if (rnr != null)
					rnr.DrawGizmosSelected();
			}
		}
	}
}