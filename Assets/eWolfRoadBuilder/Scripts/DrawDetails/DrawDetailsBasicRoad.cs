using System;
using System.Collections.Generic;
using eWolfRoadBuilder.Terrains;

namespace eWolfRoadBuilder
{
    /// <summary>
    /// The basic road draw details
    /// </summary>
    public sealed class DrawDetailsBasicRoad : IDrawDetails
	{
		/// <summary>
		/// Standard Constructor
		/// </summary>
		/// <param name="connectionSet">The connection set</param>
		/// <param name="materialName">The material to use</param>
        /// <param name="subDivide">The subdivide</param>
		public DrawDetailsBasicRoad(int connectionSet, string materialName, int subDivide)
		{
			_connectionSet = connectionSet;
			_materialName = materialName;
            _subDivide = subDivide;
		}

		#region Public Methods
		/// <summary>
		/// Create the mesh for this section
		/// </summary>
		/// <param name="roadBuilderObject">The object to create the mesh on</param>
		public void DrawMesh(IRoadBuildData roadBuilderObject)
		{
			List<Guid> list = IntersectionManager.Instance[_connectionSet];

			MeshBuilderSection jb = new MeshBuilderSection(
				roadBuilderObject,
				list,
				_materialName,
                _subDivide);

			jb.Build();
		}

        /// <summary>
        /// Update the end points
        /// </summary>
        /// <param name="roadBuilderObject">The base object</param>
        public void UpdateEndPoints(IRoadBuildData roadBuilderObject)
        {
            List<Guid> list = IntersectionManager.Instance[_connectionSet];
            MeshBuilderSection jb = new MeshBuilderSection(
                roadBuilderObject,
                list,
                _materialName,
                _subDivide);
            // TODO: Do we need this for basic roads
            // jb.UpdateEndPoints();
        }

        /// <summary>
        /// Modifiy the terrain
        /// </summary>
        /// <param name="roadBuilderObject">The road object</param>
        /// <param name="tm">The terrain modifier</param>
        public void ModifyTerrain(IRoadBuildData roadBuilderObject, TerrainModifier tm)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Private Fields
        private int _connectionSet;
		private string _materialName;
        private int _subDivide;
        #endregion
    }
}