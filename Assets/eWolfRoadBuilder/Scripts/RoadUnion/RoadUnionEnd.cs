using System;
using System.Collections.Generic;
using eWolfRoadBuilder.Terrains;
using eWolfRoadBuilderHelpers;
using UnityEngine;

namespace eWolfRoadBuilder
{
    /// <summary>
    /// The road union end road
    /// </summary>
    public class RoadUnionEnd : IRoadUnion
    {
        /// <summary>
        /// Standard Constructor
        /// </summary>
        /// <param name="node">The road network node</param>
        public RoadUnionEnd(RoadNetworkNode node)
        {
            _roadNetworkNode = node;
        }

        #region Public Methods
        /// <summary>
        /// Gets the connection set index
        /// </summary>
        public int ConnectionSetIndex
        {
            get
            {
                return _connectionSet;
            }
        }

        /// <summary>
        /// Created the intersections of the roads
        /// </summary>
        /// <param name="roadBuilderObject">The road builder object</param>
        public void CreateLayout(RoadBuilder roadBuilderObject)
        {
            float angleRoadStart = RoadUnionHelper.AngleClamp(RoadUnionHelper.GetAngleOfRoad(_roadNetworkNode, 0));
            Vector3 pos = _roadNetworkNode.transform.position;

            ICrossSection sc = _roadNetworkNode.gameObject.GetComponent<ICrossSection>();
            if (sc == null)
                sc = RoadConstructorHelper.CrossSectionDetails;

            IMaterialFrequency mf = _roadNetworkNode.gameObject.GetComponent<IMaterialFrequency>();
            if (mf == null)
                mf = RoadConstructorHelper.MaterialFrequencySet;

            RoadCrossSection rA = new RoadCrossSection(pos, angleRoadStart, sc, mf);

            StreetManager.Instance.AddStreetEnd(_roadNetworkNode.name, _roadNetworkNode.Details.Roads[0].name, sc, mf, rA);
        }

        /// <summary>
        /// Update the layout for all the streets
        /// </summary>
        /// <param name="roadBuilderObject"></param>
        public void CreateStreetLayout(RoadBuilder roadBuilderObject)
        {
            RoadNetworkNodeHelper.CreateAllStreets(_roadNetworkNode);
        }

        /// <summary>
        /// Create the mesh for this Union and streets
        /// </summary>
        /// <param name="roadBuilderObject">The object to update the mesh for</param>
        public void CreateMesh(IRoadBuildData roadBuilderObject)
        {
            RoadNetworkNodeHelper.MeshStreets(roadBuilderObject, _streetNames);
        }

        /// <summary>
        /// Add the name of the street to the list
        /// </summary>
        /// <param name="streetFullName">The street name to add</param>
        public void AddStreetList(string streetFullName)
        {
            _streetNames.Add(streetFullName);
        }

        /// <summary>
        /// Apply to terrain
        /// </summary>
        /// <param name="TerrainModifier">The Terrain Modifier helper</param>
        public void ModifiyTerrain(TerrainModifier tm)
        {
            foreach (RoadNetworkNode rnn in _roadNetworkNode.Details.Roads)
            {
                string[] stringArray = new string[2] { _roadNetworkNode.name, rnn.name };
                Array.Sort(stringArray);

                string streetFullName = string.Join("-", stringArray);
                StreetData sd = StreetManager.Instance[streetFullName];
                
                RoadCrossSection rsc = sd.GetFirst;
                if (rsc == null)
                    continue;

                RoadCrossSection rsca = sd.GetSecond;
                if (rsca == null)
                    continue;

                tm.ApplyToTerrain(rsc, rsca);
            }
        }
        #endregion

        #region Private Fields
        /// <summary>
        /// The road network parent
        /// </summary>
        private RoadNetworkNode _roadNetworkNode;

        /// <summary>
        /// The index of the connect set
        /// </summary>
        private int _connectionSet = -1;

        /// <summary>
        /// The list of steet names for this node
        /// </summary>
        private List<string> _streetNames = new List<string>();
        #endregion
    }
}