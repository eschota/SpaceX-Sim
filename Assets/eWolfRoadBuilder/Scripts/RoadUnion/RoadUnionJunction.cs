using System;
using System.Collections.Generic;
using eWolfRoadBuilder.Terrains;
using eWolfRoadBuilderHelpers;
using UnityEngine;

namespace eWolfRoadBuilder
{
    /// <summary>
    /// The road union junction class
    /// </summary>
    public class RoadUnionJunction : IRoadUnion
    {
        #region Public Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="node">The node</param>
        public RoadUnionJunction(RoadNetworkNode node)
        {
            _roadNetworkNode = node;
        }

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
            if (_roadNetworkNode.Details.Roads[0] == null)
                return;
            if (_roadNetworkNode.Details.Roads[1] == null)
                return;
            if (_roadNetworkNode.Details.Roads[2] == null)
                return;

            CreateJunctions(roadBuilderObject, _roadNetworkNode.Details.Sections);
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
            _meshSection.CreateMesh(roadBuilderObject);
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
            CreateJunctionsTerrain(_roadNetworkNode.Details.Sections, tm);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Create the road object
        /// </summary>
        /// <param name="roadObject">The base object to add the road to</param>
        /// <param name="sections">The number of sections to use for this road</param>
        private void CreateJunctions(RoadBuilder roadObject, int sections)
        {
            float couveSize = 2.5f;

            _roadNetworkNode.OrderRoads();

            IMaterialFrequency materialFrequency = _roadNetworkNode.GetComponent<OverridableMaterialFrequency>();
            if (materialFrequency == null)
                materialFrequency = RoadConstructorHelper.MaterialFrequencySet;

            RoadNetworkNode roadA, roadB, roadC;
            RoadCrossSection rA, rB, rC;
            RoadUnionHelper.DefineCrossSectionOffSet(couveSize, 0, _roadNetworkNode, out roadA, out rA);
            RoadUnionHelper.DefineCrossSectionOffSet(couveSize, 1, _roadNetworkNode, out roadB, out rB);
            RoadUnionHelper.DefineCrossSectionOffSet(couveSize, 2, _roadNetworkNode, out roadC, out rC);

            int connectionSet = IntersectionManager.Instance.AddLinkedIntersecions(rA, rB, rC);
            _meshSection.AddJunctionRoad(connectionSet, _roadNetworkNode, RoadConstructorHelper.GetMainMaterial(materialFrequency));
            _meshSection.UpdateEndPoints(roadObject);

            List<Guid> list = IntersectionManager.Instance[connectionSet];
            RoadCrossSection rscA = IntersectionManager.Instance[list[0]];
            RoadCrossSection rscB = IntersectionManager.Instance[list[1]];
            RoadCrossSection rscC = IntersectionManager.Instance[list[2]];

            StreetManager.Instance.AddStreetEnd(_roadNetworkNode.name, _roadNetworkNode.Details.Roads[0].name,
                    RoadConstructorHelper.CrossSection(_roadNetworkNode.Details.Roads[0]), RoadConstructorHelper.Materials(roadA), rA);

            StreetManager.Instance.AddStreetEnd(_roadNetworkNode.name, _roadNetworkNode.Details.Roads[1].name,
                    RoadConstructorHelper.CrossSection(_roadNetworkNode.Details.Roads[1]), RoadConstructorHelper.Materials(roadB), rB);

            StreetManager.Instance.AddStreetEnd(_roadNetworkNode.name, _roadNetworkNode.Details.Roads[2].name,
                    RoadConstructorHelper.CrossSection(_roadNetworkNode.Details.Roads[2]), RoadConstructorHelper.Materials(roadC), rC);

            StreetManager.Instance.ReplaceRoadWithId(_roadNetworkNode.name, _roadNetworkNode.Details.Roads[0].name,
                 RoadConstructorHelper.CrossSection(_roadNetworkNode.Details.Roads[0]), rscA);

            StreetManager.Instance.ReplaceRoadWithId(_roadNetworkNode.name, _roadNetworkNode.Details.Roads[1].name,
                RoadConstructorHelper.CrossSection(_roadNetworkNode.Details.Roads[1]), rscB);

            StreetManager.Instance.ReplaceRoadWithId(_roadNetworkNode.name, _roadNetworkNode.Details.Roads[2].name,
                RoadConstructorHelper.CrossSection(_roadNetworkNode.Details.Roads[2]), rscC);

            return;
        }

        /// <summary>
        /// Modifiy the terrain for the junction
        /// </summary>
        /// <param name="sections">The number of sections</param>
        /// <param name="tm">The terrain modifier</param>
        private void CreateJunctionsTerrain(int sections, TerrainModifier tm)
        {
            float couveSize = 2.5f;

            _roadNetworkNode.OrderRoads();

            IMaterialFrequency materialFrequency = _roadNetworkNode.GetComponent<OverridableMaterialFrequency>();
            if (materialFrequency == null)
                materialFrequency = RoadConstructorHelper.MaterialFrequencySet;

            RoadNetworkNode roadA, roadB, roadC;
            RoadCrossSection rA, rB, rC;
            RoadUnionHelper.DefineCrossSectionOffSet(couveSize, 0, _roadNetworkNode, out roadA, out rA);
            RoadUnionHelper.DefineCrossSectionOffSet(couveSize, 1, _roadNetworkNode, out roadB, out rB);
            RoadUnionHelper.DefineCrossSectionOffSet(couveSize, 2, _roadNetworkNode, out roadC, out rC);

            int connectionSet = IntersectionManager.Instance.AddLinkedIntersecions(rA, rB, rC);

            RoadNetworkNode roadNetworkNode = _roadNetworkNode;
            string materialName = RoadConstructorHelper.GetMainMaterial(materialFrequency);

            DrawDetailsJunctionRoad drs = new DrawDetailsJunctionRoad(connectionSet, roadNetworkNode, materialName);
            drs.ModifyTerrain(_roadNetworkNode.BuildData, tm);

            RoadConstructorHelper.ApplyLeadingStrights(_roadNetworkNode, tm, 0);
            RoadConstructorHelper.ApplyLeadingStrights(_roadNetworkNode, tm, 1);
            RoadConstructorHelper.ApplyLeadingStrights(_roadNetworkNode, tm, 2);
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

        /// <summary>
        /// The mesh section details
        /// </summary>
        private MeshSectionDetails _meshSection = new MeshSectionDetails();
        #endregion
    }
}