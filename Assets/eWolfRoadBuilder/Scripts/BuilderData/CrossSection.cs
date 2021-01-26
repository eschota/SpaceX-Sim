using System;
using UnityEngine;

namespace eWolfRoadBuilder
{
    [Serializable]
    public class CrossSection : ICrossSection
    {
        #region Public Fields
        /// <summary>
        /// The width of the road
        /// </summary>
        public float RoadWidth = 13;

        /// <summary>
        /// Has the road got curbs
        /// </summary>
        public bool WithCurb = true;

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

        #region Public properties
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
        #endregion

        #region Public Static Methods
        /// <summary>
        /// Lerp between the two sets of values
        /// </summary>
        /// <param name="cA">The from section</param>
        /// <param name="cB">The To section</param>
        /// <param name="v">The percentage between the first and the seconded</param>
        /// <returns>The cross section at the percentage</returns>
        public static ICrossSection Lerp(ICrossSection cA, ICrossSection cB, float v)
        {
            CrossSection cs = new CrossSection();
            cs.RoadWidth = Mathf.Lerp(cA.RoadWidthValue, cB.RoadWidthValue, v);

            cs.WithCurb = cA.WithCurbValue;
            cs.CurbLipHeight = Mathf.Lerp(cA.CurbLipHeightValue, cB.CurbLipHeightValue, v);
            cs.CurbLipSlope = Mathf.Lerp(cA.CurbLipSlopeValue, cB.CurbLipSlopeValue, v);
            cs.CurbWidth = Mathf.Lerp(cA.CurbWidthValue, cB.CurbWidthValue, v);
            cs.CurbEdgeDrop = Mathf.Lerp(cA.CurbEdgeDropValue, cB.CurbEdgeDropValue, v);
            return cs;
        }
        #endregion
    }
}
