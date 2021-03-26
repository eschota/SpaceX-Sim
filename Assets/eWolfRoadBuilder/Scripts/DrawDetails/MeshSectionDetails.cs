using System.Collections.Generic;

namespace eWolfRoadBuilder
{
    /// <summary>
    /// The Details on how to draw a section.
    /// </summary>
    public class MeshSectionDetails
	{
		/// <summary>
		/// Add a basic road section
		/// </summary>
		/// <param name="connectionSet">The connection index</param>
		internal void AddBasicRoad(int connectionSet)
		{
			AddBasicRoad(connectionSet, string.Empty, 0);
		}

		/// <summary>
		/// Add a basic road section
		/// </summary>
		/// <param name="connectionSet">The connection index</param>
		/// <param name="materialName">The name of the material to use</param>
        /// <param name="subDivide">How much to subdivide</param>
		internal void AddBasicRoad(int connectionSet, string materialName, int subDivide)
		{
			DrawDetailsBasicRoad drs = new DrawDetailsBasicRoad(connectionSet, materialName, subDivide);
			_drawDetails.Add(drs);
		}

		/// <summary>
		/// Add a junction road section
		/// </summary>
		/// <param name="connectionSet">The connection index</param>
		/// <param name="roadNetworkNode">THe master node for the junction</param>
		/// <param name="materialName">The name of the material to use</param>
		internal void AddJunctionRoad(int connectionSet, RoadNetworkNode roadNetworkNode, string materialName)
		{
			DrawDetailsJunctionRoad drs = new DrawDetailsJunctionRoad(connectionSet, roadNetworkNode, materialName);
			_drawDetails.Add(drs);
		}

		/// <summary>
		/// Add a cross road section
		/// </summary>
		/// <param name="connectionSet">The connection index</param>
		/// <param name="roadNetworkNode">THe master node for the cross road</param>
		/// <param name="materialName">The name of the material to use</param>
		internal void AddCrossRoad(int connectionSet, RoadNetworkNode roadNetworkNode, string materialName)
		{
            DrawDetailsCrossRoad drs = new DrawDetailsCrossRoad(connectionSet, roadNetworkNode, materialName);
			_drawDetails.Add(drs);
		}

        /// <summary>
        /// Add a five road section
        /// </summary>
        /// <param name="connectionSet">The connection index</param>
        /// <param name="roadNetworkNode">THe master node for the five roads</param>
        /// <param name="materialName">The name of the material to use</param>
        internal void AddFiveRoad(int connectionSet, RoadNetworkNode roadNetworkNode, string materialName)
        {
            DrawDetailsFiveRoad drs = new DrawDetailsFiveRoad(connectionSet, roadNetworkNode, materialName);
            _drawDetails.Add(drs);
        }

        /// <summary>
        /// Create the mesh for this section
        /// </summary>
        /// <param name="roadBuilderObject">The object to create the mesh on</param>
        public void CreateMesh(IRoadBuildData roadBuilderObject)
		{
			foreach (IDrawDetails dd in _drawDetails)
			{
				dd.DrawMesh(roadBuilderObject);
			}
		}

        /// <summary>
        /// Update the end points
        /// </summary>
        /// <param name="roadBuilderObject">The base object</param>
        public void UpdateEndPoints(IRoadBuildData roadBuilderObject)
        {
            foreach (IDrawDetails dd in _drawDetails)
            {
                dd.UpdateEndPoints(roadBuilderObject);
            }
        }

		#region Private Fields
		/// <summary>
		/// The list of each section
		/// </summary>
		private List<IDrawDetails> _drawDetails = new List<IDrawDetails>();
		#endregion
	}
}
