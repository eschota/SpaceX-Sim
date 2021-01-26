using System;

namespace eWolfRoadBuilder.Terrains
{
    [Serializable]
    public class TerrainModifierDetails
    {
        /// <summary>
        /// The terrain off set from the height (downward)
        /// </summary>
        public float OffSet = 0.001f;

        /// <summary>
        /// The amount of detail in the terrain
        /// </summary>
        public float Divider = 200;
    }
}