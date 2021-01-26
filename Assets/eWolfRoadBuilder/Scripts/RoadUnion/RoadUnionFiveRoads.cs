using System;
using System.Collections.Generic;
using eWolfRoadBuilder.Terrains;
using eWolfRoadBuilderHelpers;

namespace eWolfRoadBuilder
{
    /// <summary>
    /// Five Road Union class
    /// </summary>
    public class RoadUnionFiveRoads : IRoadUnion
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="node">The node</param>
        public RoadUnionFiveRoads(RoadNetworkNode node)
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

        #region Public Methods
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
            if (_roadNetworkNode.Details.Roads[3] == null)
                return;
            if (_roadNetworkNode.Details.Roads[4] == null)
                return;

            CreateFiveRoads(roadBuilderObject, _roadNetworkNode.Details.Sections);
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
            CreateFiveRoadsTerrain(_roadNetworkNode.Details.Sections, tm);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Create the road object
        /// </summary>
        /// <param name="roadObject">The base object to add the road to</param>
        /// <param name="sections">The number of sections to use for this road</param>
        private void CreateFiveRoads(RoadBuilder roadObject, int sections)
        {
            float couveSize = 2.5f;
            _roadNetworkNode.OrderRoads();

            IMaterialFrequency materialFrequency = _roadNetworkNode.GetComponent<OverridableMaterialFrequency>();
            if (materialFrequency == null)
                materialFrequency = RoadConstructorHelper.MaterialFrequencySet;

            RoadNetworkNode roadA, roadB, roadC, roadD, roadE;
            RoadCrossSection rA, rB, rC, rD, rE;
            RoadUnionHelper.DefineCrossSectionOffSet(couveSize, 0, _roadNetworkNode, out roadA, out rA);
            RoadUnionHelper.DefineCrossSectionOffSet(couveSize, 1, _roadNetworkNode, out roadB, out rB);
            RoadUnionHelper.DefineCrossSectionOffSet(couveSize, 2, _roadNetworkNode, out roadC, out rC);
            RoadUnionHelper.DefineCrossSectionOffSet(couveSize, 3, _roadNetworkNode, out roadD, out rD);
            RoadUnionHelper.DefineCrossSectionOffSet(couveSize, 4, _roadNetworkNode, out roadE, out rE);

            int connectionSet = IntersectionManager.Instance.AddLinkedIntersecions(rA, rB, rC, rD, rE);
            _meshSection.AddFiveRoad(connectionSet, _roadNetworkNode, RoadConstructorHelper.GetMainMaterial(materialFrequency));
            _meshSection.UpdateEndPoints(roadObject);

            StreetManager.Instance.AddStreetEnd(_roadNetworkNode.name, _roadNetworkNode.Details.Roads[0].name, RoadConstructorHelper.CrossSection(_roadNetworkNode.Details.Roads[0]), RoadConstructorHelper.Materials(roadA), rA);
            StreetManager.Instance.AddStreetEnd(_roadNetworkNode.name, _roadNetworkNode.Details.Roads[1].name, RoadConstructorHelper.CrossSection(_roadNetworkNode.Details.Roads[1]), RoadConstructorHelper.Materials(roadB), rB);
            StreetManager.Instance.AddStreetEnd(_roadNetworkNode.name, _roadNetworkNode.Details.Roads[2].name, RoadConstructorHelper.CrossSection(_roadNetworkNode.Details.Roads[2]), RoadConstructorHelper.Materials(roadC), rC);
            StreetManager.Instance.AddStreetEnd(_roadNetworkNode.name, _roadNetworkNode.Details.Roads[3].name, RoadConstructorHelper.CrossSection(_roadNetworkNode.Details.Roads[3]), RoadConstructorHelper.Materials(roadD), rD);
            StreetManager.Instance.AddStreetEnd(_roadNetworkNode.name, _roadNetworkNode.Details.Roads[4].name, RoadConstructorHelper.CrossSection(_roadNetworkNode.Details.Roads[4]), RoadConstructorHelper.Materials(roadE), rE);

            List<Guid> list = IntersectionManager.Instance[connectionSet];
            RoadCrossSection rscA = IntersectionManager.Instance[list[0]];
            RoadCrossSection rscB = IntersectionManager.Instance[list[1]];
            RoadCrossSection rscC = IntersectionManager.Instance[list[2]];
            RoadCrossSection rscD = IntersectionManager.Instance[list[3]];
            RoadCrossSection rscE = IntersectionManager.Instance[list[4]];

            StreetManager.Instance.ReplaceRoadWithId(_roadNetworkNode.name, _roadNetworkNode.Details.Roads[0].name,
                 RoadConstructorHelper.CrossSection(_roadNetworkNode.Details.Roads[0]), rscA);

            StreetManager.Instance.ReplaceRoadWithId(_roadNetworkNode.name, _roadNetworkNode.Details.Roads[1].name,
                RoadConstructorHelper.CrossSection(_roadNetworkNode.Details.Roads[1]), rscB);

            StreetManager.Instance.ReplaceRoadWithId(_roadNetworkNode.name, _roadNetworkNode.Details.Roads[2].name,
                RoadConstructorHelper.CrossSection(_roadNetworkNode.Details.Roads[2]), rscC);

            StreetManager.Instance.ReplaceRoadWithId(_roadNetworkNode.name, _roadNetworkNode.Details.Roads[3].name,
                RoadConstructorHelper.CrossSection(_roadNetworkNode.Details.Roads[3]), rscD);

            StreetManager.Instance.ReplaceRoadWithId(_roadNetworkNode.name, _roadNetworkNode.Details.Roads[4].name,
                RoadConstructorHelper.CrossSection(_roadNetworkNode.Details.Roads[4]), rscE);
        }

        /// <summary>
        /// Create the terrain for the road
        /// </summary>
        /// <param name="sections">The number of sections</param>
        /// <param name="tm">The terrain modifier</param>
        private void CreateFiveRoadsTerrain(int sections, TerrainModifier tm)
        {
            float couveSize = 2.5f;

            IMaterialFrequency materialFrequency = _roadNetworkNode.GetComponent<OverridableMaterialFrequency>();
            if (materialFrequency == null)
                materialFrequency = RoadConstructorHelper.MaterialFrequencySet;

            RoadNetworkNode roadA, roadB, roadC, roadD, roadE;
            RoadCrossSection rA, rB, rC, rD, rE;
            RoadUnionHelper.DefineCrossSectionOffSet(couveSize, 0, _roadNetworkNode, out roadA, out rA);
            RoadUnionHelper.DefineCrossSectionOffSet(couveSize, 1, _roadNetworkNode, out roadB, out rB);
            RoadUnionHelper.DefineCrossSectionOffSet(couveSize, 2, _roadNetworkNode, out roadC, out rC);
            RoadUnionHelper.DefineCrossSectionOffSet(couveSize, 3, _roadNetworkNode, out roadD, out rD);
            RoadUnionHelper.DefineCrossSectionOffSet(couveSize, 4, _roadNetworkNode, out roadE, out rE);

            int connectionSet = IntersectionManager.Instance.AddLinkedIntersecions(rA, rB, rC, rD, rE);

            DrawDetailsFiveRoad drs = new DrawDetailsFiveRoad(connectionSet, _roadNetworkNode, RoadConstructorHelper.GetMainMaterial(materialFrequency));
            drs.ModifyTerrain(_roadNetworkNode.BuildData, tm);

            RoadConstructorHelper.ApplyLeadingStrights(_roadNetworkNode, tm, 0);
            RoadConstructorHelper.ApplyLeadingStrights(_roadNetworkNode, tm, 1);
            RoadConstructorHelper.ApplyLeadingStrights(_roadNetworkNode, tm, 2);
            RoadConstructorHelper.ApplyLeadingStrights(_roadNetworkNode, tm, 3);
            RoadConstructorHelper.ApplyLeadingStrights(_roadNetworkNode, tm, 4);
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
        /// The mesh section
        /// </summary>
        private MeshSectionDetails _meshSection = new MeshSectionDetails();
        #endregion
    }
}
