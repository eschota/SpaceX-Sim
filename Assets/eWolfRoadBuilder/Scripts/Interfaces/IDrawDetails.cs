using eWolfRoadBuilder.Terrains;

namespace eWolfRoadBuilder
{
    /// <summary>
    /// The draw details interfase
    /// </summary>
    public interface IDrawDetails
    {
        /// <summary>
        /// Draw the mesh
        /// </summary>
        /// <param name="roadBuilderObject">The base object</param>
        void DrawMesh(IRoadBuildData roadBuilderObject);

        /// <summary>
        /// Update the end points
        /// </summary>
        /// <param name="roadBuilderObject">The base object</param>
        void UpdateEndPoints(IRoadBuildData roadBuilderObject);

        /// <summary>
        /// modify the terrain
        /// </summary>
        /// <param name="roadBuilderObject">The base object</param>
        /// <param name="tm">The terrain modifier</param>
        void ModifyTerrain(IRoadBuildData roadBuilderObject, TerrainModifier tm);
    }
}
