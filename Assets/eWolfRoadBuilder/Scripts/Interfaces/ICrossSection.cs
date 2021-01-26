namespace eWolfRoadBuilder
{
    /// <summary>
    /// The cross section interface
    /// </summary>
    public interface ICrossSection
    {
        /// <summary>
        /// The width of the road
        /// </summary>
        float RoadWidthValue { get; }

        /// <summary>
		/// Has the road got curbs
		/// </summary>
		bool WithCurbValue { get; }

        /// <summary>
        /// Do we need to add the curb ploys
        /// </summary>
        bool HasCurbDataValue { get; }

        /// <summary>
        /// The height of the curb
        /// </summary>
        float CurbLipHeightValue { get; }

        /// <summary>
        /// The slop of the curb. 0 for normal
        /// </summary>
        float CurbLipSlopeValue { get; }

        /// <summary>
        /// The thinkness / width of the curb
        /// </summary>
        float CurbWidthValue { get; }

        /// <summary>
        /// The drop of the curb
        /// </summary>
        float CurbEdgeDropValue { get; }
    }
}
