using System;
using System.Collections.Generic;
using eWolfRoadBuilder.Terrains;

namespace eWolfRoadBuilder
{
    /// <summary>
    /// The cross road draw details
    /// </summary>
    public sealed class DrawDetailsCrossRoad : IDrawDetails
	{
		/// <summary>
		/// Standard Constructor
		/// </summary>
		/// <param name="connectionSet"></param>
		/// <param name="roadNetworkNode">THe master node for the junction</param>
		/// <param name="materialName">The name of the material for this mesh</param>
		public DrawDetailsCrossRoad(int connectionSet, RoadNetworkNode roadNetworkNode, string materialName)
		{
			_connectionSet = connectionSet;
			_materialName = materialName;
			_roadNetworkNode = roadNetworkNode;
			_materialName = materialName;
		}

		#region Public Methods
		/// <summary>
		/// Create the mesh for this section
		/// </summary>
		/// <param name="roadBuilderObject">The object to create the mesh on</param>
		public void DrawMesh(IRoadBuildData roadBuilderObject)
		{
			List<Guid> list = IntersectionManager.Instance[_connectionSet];

			MeshBuilderCrossRoad jb = new MeshBuilderCrossRoad(
				roadBuilderObject,
				list,
				_roadNetworkNode,
				_materialName);

			jb.Build();
        }

        /// <summary>
        /// Update the end points
        /// </summary>
        /// <param name="roadBuilderObject">The base object</param>
        public void UpdateEndPoints(IRoadBuildData roadBuilderObject)
        {
            List<Guid> list = IntersectionManager.Instance[_connectionSet];
            MeshBuilderCrossRoad jb = new MeshBuilderCrossRoad(
                roadBuilderObject,
                list,
                _roadNetworkNode,
                _materialName);

            jb.UpdateEndPoints();
        }

        /// <summary>
        /// Modifiy the terrain
        /// </summary>
        /// <param name="roadBuilderObject">The road object</param>
        /// <param name="tm">The terrain modifier</param>
        public void ModifyTerrain(IRoadBuildData roadBuilderObject, TerrainModifier tm)
        {
            List<Guid> list = IntersectionManager.Instance[_connectionSet];

            MeshBuilderCrossRoad jb = new MeshBuilderCrossRoad(
                roadBuilderObject,
                list,
                _roadNetworkNode,
                _materialName);

            jb.ApplyTerrain(tm);
        }
        #endregion

        #region Private Fields
        private int _connectionSet;
		private string _materialName;
		private RoadNetworkNode _roadNetworkNode;
		#endregion
	}
}
