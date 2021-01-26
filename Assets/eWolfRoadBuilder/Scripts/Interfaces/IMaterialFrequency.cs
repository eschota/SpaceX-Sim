namespace eWolfRoadBuilder
{
    /// <summary>
    /// The material freqiency
    /// </summary>
    public interface IMaterialFrequency
    {
        /// <summary>
        /// Get the materials details
        /// </summary>
        /// <returns>The Material array</returns>
        MaterialFrequency[] GetDetails { get; }
    }
}