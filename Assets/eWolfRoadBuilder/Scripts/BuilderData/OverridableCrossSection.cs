using UnityEngine;

namespace eWolfRoadBuilder
{
    public class OverridableCrossSection : MonoBehaviour, ICrossSection
    {
         #region Public Fields
        /// <summary>
        /// The width of the road
        /// </summary>
        public float RoadWidth = 13;

        /// <summary>
        /// Has the road got curbs
        /// </summary>
        private bool WithCurb = true;

        /// <summary>
        /// The height of the curb
        /// </summary>
        public float CurbLipHeight = 0.5f;

        /// <summary>
        /// The slop of the curb. 0 for normal
        /// </summary>
        public float CurbLipSlope = 0.0f;

        /// <summary>
        /// The thinkness / width of the curb
        /// </summary>
        public float CurbWidth = 6.0f;

        /// <summary>
        /// The drop of the curb
        /// </summary>
        public float CurbEdgeDrop = 0.0f;
        #endregion

        /// <summary>
        /// The width of the road
        /// </summary>
        public float RoadWidthValue
        {
            get { return RoadWidth; }
        }

        /// <summary>
		/// Has the road got curbs
		/// </summary>
		public bool WithCurbValue
        {
            get { return WithCurb; }
        }

        /// <summary>
        /// Do we need to add the curb ploys
        /// </summary>
        public bool HasCurbDataValue
        {
            get { return (CurbLipHeight != 0) || (CurbLipSlope != 0); }
        }

        /// <summary>
        /// The height of the curb
        /// </summary>
        public float CurbLipHeightValue
        {
            get { return CurbLipHeight; }
        }

        /// <summary>
        /// The slop of the curb. 0 for normal
        /// </summary>
        public float CurbLipSlopeValue
        {
            get { return CurbLipSlope; }
        }

        /// <summary>
        /// The thinkness / width of the curb
        /// </summary>
        public float CurbWidthValue
        {
            get { return CurbWidth; }
        }

        /// <summary>
        /// The drop of the curb
        /// </summary>
        public float CurbEdgeDropValue
        {
            get { return CurbEdgeDrop; }
        }

        /// <summary>
        /// Copy the cross sections detials
        /// </summary>
        /// <param name="crossSection">The master cross section</param>
        public void Copy(ICrossSection crossSection)
        {
            RoadWidth = crossSection.RoadWidthValue;
            WithCurb = crossSection.WithCurbValue;
            CurbLipHeight = crossSection.CurbLipHeightValue;
            CurbLipSlope = crossSection.CurbLipSlopeValue;
            CurbWidth = crossSection.CurbWidthValue;
            CurbEdgeDrop = crossSection.CurbEdgeDropValue;
        }
    }
}