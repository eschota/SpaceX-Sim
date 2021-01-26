using System;
using System.Collections.Generic;
using eWolfRoadBuilderHelpers;
using UnityEngine;

namespace eWolfRoadBuilder
{
    /// <summary>
    /// The main road network node
    /// </summary>
    [ExecuteInEditMode]
    public class RoadNetworkNode : MonoBehaviour
    {
        public enum UNION_TYPE
        {
            NONE,
            CORNER,
            JUNCTION,
            CROSSROADS,
            END,
            FIVEROADS,
            SIXROADS
        }

        #region Public Fields
        /// <summary>
        /// Road Detaails
        /// </summary>
        public UnionDetails Details = new UnionDetails();
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the road Build data
        /// </summary>
        public IRoadBuildData BuildData
        {
            get { return _roadBuildData; }
        }

        /// <summary>
        /// Gets the Road union
        /// </summary>
        public IRoadUnion RoadUnion
        {
            get { return _roadUnion; }
        }

        /// <summary>
        /// Created by the tool
        /// </summary>
        public bool Creator
        {
            get { return _creator; }
            set { _creator = value; }
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Create the mesh.
        /// </summary>
        public void CreateMesh()
        {
            RoadBuilder rb = gameObject.GetComponentInParent<RoadBuilder>();
            if (rb != null)
                rb.CreateMesh(true);
        }

        /// <summary>
        /// Delete the this node
        /// </summary>
        public void DeleteNode()
        {
            RoadNetworkLayout.RemoveLinkFrom(this);
            DestroyImmediate(gameObject);
        }

        /// <summary>
        /// Connect the nodes to make a road.
        /// </summary>
        /// <param name="nodes">The list of selected node</param>
        public void ConnectNodes(List<RoadNetworkNode> nodes)
        {
            if (nodes.Count != 2)
                return;

            RoadNetworkLayout.AddRoadToNode(nodes[0], nodes[1]);
            RoadNetworkLayout.AddRoadToNode(nodes[1], nodes[0]);
        }

        /// <summary>
        /// Add the Cross section overridables
        /// </summary>
        public void AddOverridableCrossSections()
        {
            gameObject.AddComponent<OverridableCrossSection>();
            Details.Modified = true;
        }

        /// <summary>
        /// Remove any cross sections from this node
        /// </summary>
        public void RemoveOverridableCrossSections()
        {
            if (HasOverridenCrossSection())
            {
                gameObject.GetComponent<OverridableCrossSection>().enabled = false;
                DestroyImmediate(gameObject.GetComponent<OverridableCrossSection>());
                Details.Modified = true;
            }
        }

        /// <summary>
        /// Add the Material Frequency overridables
        /// </summary>
        public void AddOverridableMaterialsDetails()
        {
            gameObject.AddComponent<OverridableMaterialFrequency>();
            OverridableMaterialFrequency omf = gameObject.GetComponent<OverridableMaterialFrequency>();
            RoadNetworkLayout rnl = gameObject.GetComponentInParent<RoadNetworkLayout>();
            omf.PopulateDefaultMaterials(rnl);
            Details.Modified = true;
        }

        /// <summary>
        /// Remove any Material Frequency from this node
        /// </summary>
        public void RemoveOverridableMaterialDetails()
        {
            if (HasOverridenMaterialDetails())
            {
                gameObject.GetComponent<OverridableMaterialFrequency>().enabled = false;
                DestroyImmediate(gameObject.GetComponent<OverridableMaterialFrequency>());
                Details.Modified = true;
            }
        }

        /// <summary>
        /// Do we have a Cross section override?
        /// </summary>
        /// <returns>Whether or not we have cross section details</returns>
        public bool HasOverridenCrossSection()
        {
            OverridableCrossSection ocs = gameObject.GetComponent<OverridableCrossSection>();
            return ocs != null;
        }

        /// <summary>
        /// Do we have a Cross section override?
        /// </summary>
        /// <returns>Whether or not we have cross section details</returns>
        public bool HasOverridenMaterialDetails()
        {
            OverridableMaterialFrequency omf = gameObject.GetComponent<OverridableMaterialFrequency>();
            return omf != null;
        }

        /// <summary>
        /// Insect a new node between the two nodes
        /// </summary>
        /// <param name="nodes">The list of selected node</param>
        /// <returns>The newly inserted node</returns>
        public GameObject InsertNewNode(List<RoadNetworkNode> nodes)
        {
            Vector3 pos = (nodes[0].transform.position + nodes[1].transform.position) / 2;
            GameObject rnnNew = RoadNetworkNodeHelper.CreateBasicNode(pos, RoadConstructorHelper.GetUniqueRoadName(this.gameObject), this.gameObject);

            RoadNetworkNodeHelper.RemoveRoadAFromB(nodes[0], nodes[1]);
            RoadNetworkNodeHelper.RemoveRoadAFromB(nodes[1], nodes[0]);

            RoadNetworkLayout.AddRoadToNode(nodes[0], rnnNew.GetComponent<RoadNetworkNode>());
            RoadNetworkLayout.AddRoadToNode(rnnNew.GetComponent<RoadNetworkNode>(), nodes[0]);

            RoadNetworkLayout.AddRoadToNode(nodes[1], rnnNew.GetComponent<RoadNetworkNode>());
            RoadNetworkLayout.AddRoadToNode(rnnNew.GetComponent<RoadNetworkNode>(), nodes[1]);

            return rnnNew;
        }

        /// <summary>
        /// Extrude the road out, by creating a new node
        /// </summary>
        /// <returns>The newly created road section</returns>
        public GameObject ExtrudeRoad()
        {
            Details.CompressRoads();
            Details.Modified = true;

            float roadAngle = GetCurrentAngle() + (float)(Math.PI / 2);

            if (Details.Union != UNION_TYPE.END)
                roadAngle += (float)(Math.PI / 2);

            Vector3 pos = MathsHelper.OffSetVector(transform.position, roadAngle, 100);
            GameObject newNode = RoadNetworkNodeHelper.CreateBasicNode(pos, RoadConstructorHelper.GetUniqueRoadName(gameObject), gameObject);

            GameObject rnnB = gameObject;
            RoadNetworkLayout.AddRoadToNode(newNode.GetComponent<RoadNetworkNode>(), rnnB.GetComponent<RoadNetworkNode>());
            RoadNetworkLayout.AddRoadToNode(rnnB.GetComponent<RoadNetworkNode>(), newNode.GetComponent<RoadNetworkNode>());

            ICrossSection sc = gameObject.GetComponent<ICrossSection>();
            if (sc != null)
            {
                newNode.AddComponent<OverridableCrossSection>();
                OverridableCrossSection ocs = newNode.GetComponent<OverridableCrossSection>();
                ocs.Copy(sc);
            }

            IMaterialFrequency fm = gameObject.GetComponent<IMaterialFrequency>();
            if (fm != null)
            {
                newNode.AddComponent<OverridableMaterialFrequency>();
                OverridableMaterialFrequency ocs = newNode.GetComponent<OverridableMaterialFrequency>();
                ocs.Copy(fm);
            }

            return newNode;
        }

        /// <summary>
        /// Add the name of the street to the list
        /// </summary>
        /// <param name="streetFullName">The street name to add</param>
        public void AddStreetList(string streetFullName)
        {
            _roadUnion.AddStreetList(streetFullName);
        }

        /// <summary>
        /// Order the roads - clockwise by angle
        /// </summary>
        public void OrderRoads()
        {
            if (Details.Roads.Count < 3)
                return;

            RoadNetworkNode a = Details.Roads[0];
            RoadNetworkNode b = Details.Roads[1];
            RoadNetworkNode c = Details.Roads[2];

            float angleA = a.GetRoadAngle(this.transform.position);
            float angleB = b.GetRoadAngle(this.transform.position) - angleA;
            float angleC = c.GetRoadAngle(this.transform.position) - angleA;

            if (angleC > angleB)
            {
                Details.Roads[1] = c;
                Details.Roads[2] = b;
            }
        }

        /// <summary>
        /// Called when the node has changed / moved
        /// </summary>
        void OnValidate()
        {
            CheckNodeAreValid();
            CreateRoadArray();
        }

        void Start()
        {
            _ignoreTransFirstChange = true;
        }

        /// <summary>
        /// Normal Game Update
        /// </summary>
        void Update()
        {
#if DEBUG
            if (transform.hasChanged)
            {
                if (!_ignoreTransFirstChange)
                {
                    _ignoreTransFirstChange = false;
                    transform.hasChanged = false;
                    return;
                }
                Details.Modified = true;
                transform.hasChanged = false;
                TestForAutoBuildOption();
            }
#endif
        }

        /// <summary>
        /// Gets Sets the angle of the road
        /// </summary>
        public float RoadAngle
        {
            get { return _roadAngle; }
            set { _roadAngle = value; }
        }

        /// <summary>
        /// Set the union details
        /// </summary>
        public void CreateUnion()
        {
            _roadUnion = RoadUnionHelper.Creator(this);
            _roadBuildData = new RoadBuildData();
        }

        /// <summary>
        /// Unityed Editor shoe the road link
        /// </summary>
        public void OnDrawGizmosSelected()
        {
            RoadNetworkLayout rnl = gameObject.GetComponentInParent<RoadNetworkLayout>();
            if (rnl != null)
            {
                rnl.OnDrawGizmosSelected();
            }
        }

        /// <summary>
        /// Editor draw helper
        /// </summary>
        public void DrawGizmosSelected()
        {
            Details.DrawRoads(gameObject);
        }

        /// <summary>
        /// Set the intersecion for this road
        /// </summary>
        /// <param name="index">The road section index</param>
        /// <param name="section">The road crosss section to use</param>
        public void SetRoadCrossSection(int index, RoadCrossSection section)
        {
            if (_roadCrossSections == null || _roadCrossSections.Length != Details.Roads.Count)
                _roadCrossSections = new RoadCrossSection[Details.Roads.Count];

            _roadCrossSections[index] = section;
        }

        /// <summary>
        /// Update the road array
        /// </summary>
        public void CreateRoadArray()
        {
            int numberOfRoads = UnionHelper.GetRoadCount(Details.Union);
            Details.UpdateLinksnumber(numberOfRoads);
        }

        /// <summary>
        /// Calculate the position of the inner corner
        /// </summary>
        /// <param name="roadIndexA">The first road index</param>
        /// <param name="roadIndexB">The second road index</param>
        /// <param name="magnitude">The magnitude of the corner</param>
        /// <param name="posOut">The position of the innre corner</param>
        /// <returns>True if we have found a cross over</returns>
        public bool GetInnerCorner(int roadIndexA, int roadIndexB, float magnitude, out Vector3 posOut)
        {
            Vector3 pos = gameObject.transform.position;
            RoadNetworkNode rnn = Details.Roads[roadIndexA].GetComponent<RoadNetworkNode>();
            Vector3 roadPointA = rnn.GetOffSetDownRoad(pos, magnitude);

            RoadNetworkNode rnnB = Details.Roads[roadIndexB].GetComponent<RoadNetworkNode>();
            Vector3 roadPointB = rnnB.GetOffSetDownRoad(pos, magnitude);

            Vector3 roadPointAEnd = MathsHelper.OffSetVector(
                roadPointA,
                Details.Roads[roadIndexA].GetAngleOfRoad(pos) + (float)(Math.PI / 2),
                magnitude);

            Vector3 roadPointBEnd = MathsHelper.OffSetVector(
                roadPointB,
                Details.Roads[roadIndexB].GetAngleOfRoad(pos) + (float)(Math.PI / 2),
                magnitude);

            posOut = new Vector3();
            Vector3 outB = new Vector3();
            Vector3 diffA = roadPointA - roadPointAEnd;
            Vector3 diffB = roadPointB - roadPointBEnd;
            if (MathsHelper.ClosestPointsOnTwoLines(out posOut, out outB, roadPointA, diffA.normalized, roadPointB, diffB.normalized))
                return true;

            posOut = (roadPointA + roadPointB) / 2;
            return false;
        }

        /// <summary>
        /// Appiles the off set of the road
        /// </summary>
        /// <param name="baseObject">The base road to apply the off set to</param>
        /// <param name="magitude">The size (distance) of the off set</param>
        /// <returns>The off set postion</returns>
        public Vector3 GetOffSetDownRoad(Vector3 baseObject, float magitude)
        {
            float angle = GetAngleOfRoad(baseObject);
            return MathsHelper.OffSetVector(baseObject, angle, magitude);
        }

        /// <summary>
        /// Get the angle of the road
        /// </summary>
        /// <param name="baseObject">The road to get the angle from</param>
        /// <returns>The angle of the road</returns>
        public float GetAngleOfRoad(Vector3 baseObject)
        {
            Vector3 endPosition = transform.position;
            Vector3 diff = endPosition - baseObject;
            return MathsHelper.GetAngleFrom(diff.x, diff.z) - Mathf.PI / 2;
        }

        /// <summary>
        /// Have we got a link to the given node
        /// </summary>
        /// <param name="rnn">The node to look for</param>
        /// <returns>Wheater we have a link to the node or not</returns>
        public bool HasLinksToNode(RoadNetworkNode rnn)
        {
            if (Details == null)
                return false;

            if (Details.Roads == null)
                return false;

            foreach (RoadNetworkNode rnnTest in Details.Roads)
            {
                if (rnnTest == rnn)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Check all the node are valid 
        /// </summary>
        public void CheckNodeAreValid()
        {
            foreach (RoadNetworkNode rnn in Details.Roads)
            {
                if (rnn == this)
                {
                    Debug.Log("We are linked to us -> removing node");
                    RoadNetworkLayout.RemoveLinkFrom(rnn);
                    CreateRoadArray();
                    break;
                }
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Get the angle of the road
        /// </summary>
        /// <param name="position">The base position</param>
        /// <returns>The angle of the road</returns>
        private float GetRoadAngle(Vector3 position)
        {
            Vector3 pos = position - transform.position;
            return MathsHelper.GetAngleFrom(pos.x, pos.z);
        }

        /// <summary>
        /// Get the best angle from all the roads
        /// </summary>
        /// <returns>The best angle</returns>
        private float GetCurrentAngle()
        {
            if (Details.Union == UNION_TYPE.NONE)
                return 0;

            Vector3 pos = Details.Roads[0].transform.position - transform.position;
            return MathsHelper.GetAngleFrom(pos.x, pos.z);
        }

        /// <summary>
        /// Check if we need to auto build the mesh
        /// </summary>
        private void TestForAutoBuildOption()
        {
            RoadBuilder rb = gameObject.GetComponentInParent<RoadBuilder>();
            if (rb == null)
                return;
            
            if (rb.AutoBuild)
            {
                if (rb.DropToGround)
                {
                    Debug.Log("Can't use Auto Build when Drop to ground is enabled.");
                    return;
                }
                if (!rb.MeshPerNode)
                {
                    Debug.Log("Can only use Auto Build when Mesh Pre Node is used.");
                    return;
                }

                rb.CreateMesh(true);
            }
        }
        #endregion

        #region Private Fields
        private RoadCrossSection[] _roadCrossSections;
        private float _roadAngle = 0;
        private bool _creator = false;
        private IRoadUnion _roadUnion = null;
        private IRoadBuildData _roadBuildData;
        private bool _ignoreTransFirstChange;
        #endregion
    }
}