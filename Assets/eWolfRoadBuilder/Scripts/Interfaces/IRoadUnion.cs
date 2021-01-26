using eWolfRoadBuilder.Terrains;

namespace eWolfRoadBuilder
{
    /// <summary>
    /// not sure about this yet
    /// </summary>
    public interface IRoadUnion
	{
		/// <summary>
		/// Gets the connection set index
		/// </summary>
		int ConnectionSetIndex { get; }

		/// <summary>
		/// Created the intersections of the roads
		/// </summary>
		/// <param name="roadBuilderObject">The road builder object</param>
		void CreateLayout(RoadBuilder roadBuilderObject);

		/// <summary>
		/// Update the layout for all the streets
		/// </summary>
		/// <param name="roadBuilderObject">The road builder object</param>
		void CreateStreetLayout(RoadBuilder roadBuilderObject);

		/// <summary>
		/// Create the mesh
		/// </summary>
		/// <param name="roadBuilderObject">The builder object</param>
		void CreateMesh(IRoadBuildData roadBuilderObject);

        /// <summary>
		/// Apply to terrain
		/// </summary>
		/// <param name="TerrainModifier">The Terrain Modifier helper</param>
		void ModifiyTerrain(TerrainModifier tm);

        /// <summary>
        /// Add the name of the street to the list
        /// </summary>
        /// <param name="streetFullName">The street name to add</param>
        void AddStreetList(string streetFullName);
    }
}