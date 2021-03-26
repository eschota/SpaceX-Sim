using UnityEngine;

namespace eWolfRoadBuilderHelpers
{
	/// <summary>
	/// The road builder helper
	/// </summary>
	public static class RoadBuilderHelper
	{
		/// <summary>
		/// Remove all the data from the mesh
		/// </summary>
		/// <remarks>This will not remove the mesh component - only the vertices, uv and trinalges</remarks>
		/// <param name="go">The object to remove the mesh data from</param>
		public static void RemoveMeshData(GameObject go)
		{
			Mesh mesh = new Mesh();
			go.GetComponent<MeshFilter>().mesh = mesh;
			mesh.vertices = new Vector3[0];
			mesh.uv = new Vector2[0];
			mesh.triangles = new int[0];

			MeshCollider CCgb = go.GetComponent<MeshCollider>();
			CCgb.convex = false;
			CCgb.sharedMesh = mesh;
		}
	}
}