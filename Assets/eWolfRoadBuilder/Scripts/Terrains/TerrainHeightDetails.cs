namespace eWolfRoadBuilder.Terrains
{
    /// <summary>
    /// Holders the terrain details for each part of the terrain map
    /// </summary>
    public class TerrainHeightDetails
    {
        /// <summary>
        /// The original height
        /// </summary>
        public float OriginalHeight;

        /// <summary>
        /// The new height
        /// </summary>
        public float Height;

        /// <summary>
        /// Weather it was a plateau or not (don't override if it's set)
        /// </summary>
        public bool IsPlateau;
    }
}