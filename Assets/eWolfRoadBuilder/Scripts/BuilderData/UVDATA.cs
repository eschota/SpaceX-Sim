namespace eWolfRoadBuilder
{
    /// <summary>
    /// The UV facade class - easy access to the property bag of the UVs
    /// </summary>
    public static class UVDATA
	{
		/// <summary>
		/// Whether the UV sets is the width of the full texture for the road
		/// </summary>
		public static bool JunctionAcrossUV
		{
			get
			{
				return (RoadConstructorHelper.RoadUVSet != UV_SET.RoadOnly);
			}
		}

        /// <summary>
        /// gets the junction start
        /// </summary>
		public static float JunctionStart { get { return RoadConstructorHelper.UVSET["JUNCTION_START"]; } }

        /// <summary>
        /// gets the junction length
        /// </summary>
		public static float JunctionLength { get { return RoadConstructorHelper.UVSET["JUNCTION_LENGTH"]; } }

        /// <summary>
        /// gets the junction kerb length
        /// </summary>
        public static float JunctionLengthKerb { get { return RoadConstructorHelper.UVSET["JUNCTION_LENGTH_KERB"]; } }

        /// <summary>
        /// gets the straight start
        /// </summary>
        public static float StraightStart { get { return RoadConstructorHelper.UVSET["STRAIGHT_START"]; } }

        /// <summary>
        /// gets the straight length
        /// </summary>
		public static float StraightLength { get { return RoadConstructorHelper.UVSET["STRAIGHT_LENGTH"]; } }

        /// <summary>
        /// gets the curb left ouuter
        /// </summary>
		public static float CurbLeftOutter { get { return RoadConstructorHelper.UVSET["CRUB_L_OUTTER"]; } }

        /// <summary>
        /// Gets the curb left inner
        /// </summary>
		public static float CurbLeftInner { get { return RoadConstructorHelper.UVSET["CRUB_L_INNER"]; } }

        /// <summary>
        /// Gets the curb left lip inner
        /// </summary>
		public static float CurbLeftLipInner { get { return RoadConstructorHelper.UVSET["CRUB_L_LIP_INNER"]; } }

        /// <summary>
        /// Gets the curb right ouuter
        /// </summary>
		public static float CurbRightOutter { get { return RoadConstructorHelper.UVSET["CRUB_R_OUTTER"]; } }

        /// <summary>
        /// Gets the right inner
        /// </summary>
		public static float CurbRightInner { get { return RoadConstructorHelper.UVSET["CRUB_R_INNER"]; } }

        /// <summary>
        /// Gets the curn right lip inner
        /// </summary>
		public static float CurbRightLipInner { get { return RoadConstructorHelper.UVSET["CRUB_R_LIP_INNER"]; } }
        
        /// <summary>
        /// Gets the junction intersection start
        /// </summary>
		public static float JunctionIntersectionStart { get { return RoadConstructorHelper.UVSET["JUNCTION_INTERSECTION_START"]; } }

        /// <summary>
        /// Gets the junction intersection middle
        /// </summary>
		public static float JunctionIntersectionMiddle { get { return RoadConstructorHelper.UVSET["JUNCTION_INTERSECTION_MID"]; } }

        /// <summary>
        /// Gets the junction intersection end
        /// </summary>
		public static float JunctionIntersectionEnd { get { return RoadConstructorHelper.UVSET["JUNCTION_INTERSECTION_END"]; } }

        /// <summary>
        /// Gets the junction bottom start
        /// </summary>
		public static float JunctionBottomStart { get { return RoadConstructorHelper.UVSET["JUNCTIONA_START"]; } }
        /// <summary>
        /// Gets the junction bottom end
        /// </summary>
        public static float JunctionBottomEnd { get { return RoadConstructorHelper.UVSET["JUNCTIONA_LENGTH"]; } }

        /// <summary>
        /// Gets the junction left inner
        /// </summary>
        public static float JunctionBottomLeftInner { get { return RoadConstructorHelper.UVSET["JUNCTIONA_L_LIP_INNER"]; } }

        /// <summary>
        /// Gets the junction left inner
        /// </summary>
        public static float JunctionBottomRightInner { get { return RoadConstructorHelper.UVSET["JUNCTIONA_R_LIP_INNER"]; } }

        /// <summary>
        /// Gets the junction top start
        /// </summary>
        public static float JunctionTopStart { get { return RoadConstructorHelper.UVSET["JUNCTIONB_START"]; } }

        /// <summary>
        /// Gets the junction top end
        /// </summary>
		public static float JunctionTopEnd { get { return RoadConstructorHelper.UVSET["JUNCTIONB_LENGTH"]; } }

        /// <summary>
        /// Gets the junction top left start
        /// </summary>
		public static float JunctionTopLeftInner { get { return RoadConstructorHelper.UVSET["JUNCTIONB_L_LIP_INNER"]; } }

        /// <summary>
        /// Gets the junction top right start
        /// </summary>
		public static float JunctionTopRightInner { get { return RoadConstructorHelper.UVSET["JUNCTIONB_R_LIP_INNER"]; } }
	}
}
