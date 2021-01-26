namespace eWolfRoadBuilder
{
    /// <summary>
    /// Holds the array of outter and inner cross sections - use my junctions with 5 or more roads
    /// </summary>
    public class OutterInner
    {
        /// <summary>
        /// The array of cross sections for the outter side
        /// </summary>
        public RoadCrossSection[] Outter = new RoadCrossSection[8];

        /// <summary>
        /// The array of cross sections for the inner side
        /// </summary>
        public RoadCrossSection[] Inner = new RoadCrossSection[8];
    }

}
