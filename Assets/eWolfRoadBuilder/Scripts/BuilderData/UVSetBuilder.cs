using System.Collections.Generic;

namespace eWolfRoadBuilder
{
    /// <summary>
    /// The UV set to use
    /// </summary>
    public enum UV_SET
	{
		/// <summary>
		/// Road with pavement
		/// </summary>
		RoadPavement,

		/// <summary>
		/// Road only (No Pavement)
		/// </summary>
		RoadOnly,

        /// <summary>
        /// Road with pavement extended better uv mapping
        /// </summary>
        RoadPavementExtended,

        /// <summary>
        /// Road with pavement extended for smother corners set A
        /// </summary>
        RoadPavementInnerCurveA,

        /// <summary>
        /// Road with pavement extended for smother corners set B
        /// </summary>
        RoadPavementInnerCurveB,

        /// <summary>
        /// Road with pavement extended for smother corners set C
        /// </summary>
        RoadPavementInnerCurveC,
    }

    /// <summary>
    /// Builder class to populate the UV set
    /// </summary>
    public static class UVSetBuilder
	{
		/// <summary>
		/// Create the uv set from the UV type selected
		/// </summary>
		/// <param name="uvSet">The UV set to use</param>
		/// <returns>The property bag of the UV set</returns>
		internal static Dictionary<string, float> CreateUVSet(UV_SET uvSet)
		{
			Dictionary<string, float> array = new Dictionary<string, float>();
			switch (uvSet)
			{
				case UV_SET.RoadPavement:
					array = SetRoadsWithPavments();
					break;
				case UV_SET.RoadOnly:
					array = SetRoadsWithOutPavments();
					break;
                case UV_SET.RoadPavementExtended:
                    array = SetRoadsWithPavments_Extended();
                    break;
                case UV_SET.RoadPavementInnerCurveA:
                    array = SetRoadsWithPavments_CurveA();
                    break;
                case UV_SET.RoadPavementInnerCurveB:
                    array = SetRoadsWithPavments_CurveB();
                    break;
                case UV_SET.RoadPavementInnerCurveC:
                    array = SetRoadsWithPavments_CurveC();
                    break;
            }

			return array;
		}

		/// <summary>
		/// Road with pavement
		/// </summary>
		/// <returns>The property bag of the UV set</returns>
		private static Dictionary<string, float> SetRoadsWithPavments()
		{
			Dictionary<string, float> array = new Dictionary<string, float>();
            array["JUNCTION_START"] = 0.33f;
            array["JUNCTION_LENGTH"] = 0.49f;
            array["JUNCTION_LENGTH_KERB"] = 0.49f;

            array["STRAIGHT_START"] = 0;
			array["STRAIGHT_LENGTH"] = 0.49f;

			array["CRUB_L_OUTTER"] = 1;
			array["CRUB_L_INNER"] = 0.775f; 
			array["CRUB_L_LIP_INNER"] = 0.75f;

			array["CRUB_R_OUTTER"] = 0;
			array["CRUB_R_INNER"] = 0.225f;
			array["CRUB_R_LIP_INNER"] = 0.25f;

			array["JUNCTION_INTERSECTION_START"] = 0.5f;
			array["JUNCTION_INTERSECTION_MID"] = 0.75f;
			array["JUNCTION_INTERSECTION_END"] = 1f;

			array["JUNCTIONA_START"] = 0;
			array["JUNCTIONA_LENGTH"] = 0.25f;
			array["JUNCTIONA_L_LIP_INNER"] = 0.5f;
			array["JUNCTIONA_R_LIP_INNER"] = 1f;

			array["JUNCTIONB_START"] = 1;
			array["JUNCTIONB_LENGTH"] = 0.75f;
			array["JUNCTIONB_L_LIP_INNER"] = 1f;
			array["JUNCTIONB_R_LIP_INNER"] = 0.5f;
			return array;
		}

        private static Dictionary<string, float> SetRoadsWithPavments_Extended()
        {
            // TODO (MUST): Set UV set Extended
            return SetRoadsWithPavments_CurveA();
        }

        private static Dictionary<string, float> SetRoadsWithPavments_CurveB()
        {
            return SetRoadsWithPavments_CurveA();
        }

        private static Dictionary<string, float> SetRoadsWithPavments_CurveC()
        {
            return SetRoadsWithPavments_CurveA();
        }

        /// <summary>
        /// Road with pavement
        /// </summary>
        /// <returns>The property bag of the UV set</returns>
        private static Dictionary<string, float> SetRoadsWithPavments_CurveA()
        {
            Dictionary<string, float> array = new Dictionary<string, float>();
            array["JUNCTION_START"] = 0.33f;
            array["JUNCTION_LENGTH"] = 0.33f + 0.1666f - 0.015f + 0.0015f;
            array["JUNCTION_LENGTH_KERB"] = 0.5f;

            array["STRAIGHT_START"] = 0;
            array["STRAIGHT_LENGTH"] = 0.33f; // 0.49f;

            array["CRUB_L_OUTTER"] = 1;
            array["CRUB_L_INNER"] = 0.775f;
            array["CRUB_L_LIP_INNER"] = 0.75f - 0.0015f;

            array["CRUB_R_OUTTER"] = 0;
            array["CRUB_R_INNER"] = 0.225f;
            array["CRUB_R_LIP_INNER"] = 0.25f + 0.0015f;

            array["JUNCTION_INTERSECTION_START"] = 0.5f;
            array["JUNCTION_INTERSECTION_MID"] = 0.666f;
            array["JUNCTION_INTERSECTION_END"] = 0.8333f - 0.0015f;

            array["JUNCTIONA_START"] = 0;
            array["JUNCTIONA_LENGTH"] = 0.25f;
            array["JUNCTIONA_L_LIP_INNER"] = 0.5f;
            array["JUNCTIONA_R_LIP_INNER"] = 0.8333f - 0.0015f;

            array["JUNCTIONB_START"] = 1;
            array["JUNCTIONB_LENGTH"] = 0.75f;
            array["JUNCTIONB_L_LIP_INNER"] = 0.8333f - 0.0015f;
            array["JUNCTIONB_R_LIP_INNER"] = 0.5f;
            return array;
        }

        /// <summary>
        /// Road only (No Pavement)
        /// </summary>
        /// <returns>The property bag of the UV set</returns>
        private static Dictionary<string, float> SetRoadsWithOutPavments()
		{
			Dictionary<string, float> array = new Dictionary<string, float>();
			array["JUNCTION_START"] = 0.0f;
			array["JUNCTION_LENGTH"] = 0.3f;
            array["JUNCTION_LENGTH_KERB"] = 0.3f;

            array["STRAIGHT_START"] = 0;
			array["STRAIGHT_LENGTH"] = 0.49f;

			array["CRUB_L_OUTTER"] = 1;
			array["CRUB_L_INNER"] = 1;
			array["CRUB_L_LIP_INNER"] = 1;

			array["CRUB_R_OUTTER"] = 0;
			array["CRUB_R_INNER"] = 0;
			array["CRUB_R_LIP_INNER"] = 0;

			array["JUNCTION_INTERSECTION_START"] = 0.5f;
			array["JUNCTION_INTERSECTION_MID"] = 0.75f;
			array["JUNCTION_INTERSECTION_END"] = 1f;

			array["JUNCTIONA_START"] = 0;
			array["JUNCTIONA_LENGTH"] = 0.25f;
			array["JUNCTIONA_L_LIP_INNER"] = 0;
			array["JUNCTIONA_R_LIP_INNER"] = 1;

			array["JUNCTIONB_START"] = 0;
			array["JUNCTIONB_LENGTH"] = 0.25f;
			array["JUNCTIONB_L_LIP_INNER"] = 1f;
			array["JUNCTIONB_R_LIP_INNER"] = 0f;
			return array;
		}
	}
}
