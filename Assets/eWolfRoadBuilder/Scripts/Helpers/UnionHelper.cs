using UnityEngine;

namespace eWolfRoadBuilder
{
	public static class UnionHelper
	{
		/// <summary>
		/// Get the number of roads for this Union type
		/// </summary>
		/// <returns>The number of roads</returns>
		public static int GetRoadCount(RoadNetworkNode.UNION_TYPE ut)
		{
			int numberOfRoads = 0;

			if (ut == RoadNetworkNode.UNION_TYPE.END)
				numberOfRoads = 1;

			if (ut == RoadNetworkNode.UNION_TYPE.CORNER)
				numberOfRoads = 2;

			if (ut == RoadNetworkNode.UNION_TYPE.JUNCTION)
				numberOfRoads = 3;

			if (ut == RoadNetworkNode.UNION_TYPE.CROSSROADS)
				numberOfRoads = 4;

            if (ut == RoadNetworkNode.UNION_TYPE.FIVEROADS)
                numberOfRoads = 5;

            if (ut == RoadNetworkNode.UNION_TYPE.SIXROADS)
                numberOfRoads = 6;

            return numberOfRoads;
		}

		/// <summary>
		/// gets the Union type from the number of roads
		/// </summary>
		/// <param name="numberOfRoads">The number of roads</param>
        /// <returns>The union type for the number of roads</returns>
		public static RoadNetworkNode.UNION_TYPE SetRoadUnionTypeFromRoadCount(int numberOfRoads)
		{
			RoadNetworkNode.UNION_TYPE Union = RoadNetworkNode.UNION_TYPE.NONE;
			switch (numberOfRoads)
			{
				case 1:
					Union = RoadNetworkNode.UNION_TYPE.END;
					break;
				case 2:
					Union = RoadNetworkNode.UNION_TYPE.CORNER;
					break;
				case 3:
					Union = RoadNetworkNode.UNION_TYPE.JUNCTION;
					break;
				case 4:
					Union = RoadNetworkNode.UNION_TYPE.CROSSROADS;
					break;
                case 5:
                    Union = RoadNetworkNode.UNION_TYPE.FIVEROADS;
                    break;
                case 6:
                    Union = RoadNetworkNode.UNION_TYPE.SIXROADS;
                    break;
            }

			return Union;
		}
	}
}