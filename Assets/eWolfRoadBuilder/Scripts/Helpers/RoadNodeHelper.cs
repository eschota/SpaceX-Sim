using System;
using System.Collections.Generic;
using UnityEngine;
using eWolfRoadBuilder;

namespace eWolfRoadBuilderHelpers
{
	/// <summary>
	/// Road network node helper class
	/// </summary>
	public static class RoadNetworkNodeHelper
	{
		/// <summary>
		/// Create a basic node
		/// </summary>
		/// <param name="vec">The position to create the node at</param>
		/// <param name="name">The name of the node</param>
		/// <param name="baseObj">The parent object</param>
		/// <returns>A new Node object</returns>
		public static GameObject CreateBasicNode(Vector3 vec, string name, GameObject baseObj)
		{
			GameObject go = (GameObject)GameObject.Instantiate(Resources.Load("RoadNode_pf"),
				vec,
				Quaternion.identity);

			go.name = name;
			go.transform.parent = baseObj.transform.parent;
			go.transform.position = vec;
			return go;
		}

		/// <summary>
		/// Remove the first road from the second.
		/// </summary>
		/// <param name="roadNetworkNodeA">The road to remove</param>
		/// <param name="roadNetworkNodeB">The road to remove from</param>
		public static void RemoveRoadAFromB(RoadNetworkNode roadNetworkNodeA, RoadNetworkNode roadNetworkNodeB)
		{
			for (int i = 0; i < roadNetworkNodeB.Details.Roads.Count; i++)
			{
				if (roadNetworkNodeB.Details.Roads[i] == roadNetworkNodeA)
					roadNetworkNodeB.Details.Roads[i] = null;
			}

			roadNetworkNodeB.Details.CompressRoads();
		}

		/// <summary>
		/// Create all the streets for this node
		/// </summary>
		/// <param name="node">The road network node</param>
		public static void CreateAllStreets(RoadNetworkNode node)
		{
			foreach (RoadNetworkNode rnn in node.Details.Roads)
			{
				if (rnn == null)
					continue;

				if (rnn.RoadUnion == null)
					continue;

				string[] stringArray = new string[2] { node.name, rnn.name };
				Array.Sort(stringArray);

				string streetFullName = string.Join("-", stringArray);
				StreetData sd = StreetManager.Instance[streetFullName];
				if (sd != null)
				{
					if (!sd.Used)
					{
						node.AddStreetList(streetFullName);
						sd.CreateStreetLayout(rnn.Details.Subdivide);
						sd.Used = true;
					}
				}
			}
		}

		/// <summary>
		/// Create the mesh for all of the given streets
		/// </summary>
		/// <param name="_roadNetworkNode">The build object</param>
		/// <param name="_streetNames">The list of streets to add</param>
		public static void MeshStreets(IRoadBuildData roadBuilderObject, List<string> _streetNames)
		{
			foreach (string streetName in _streetNames)
			{
				StreetData sd = StreetManager.Instance[streetName];
				if (sd != null)
					sd.CreateMesh(roadBuilderObject);
			}
		}
	}
}