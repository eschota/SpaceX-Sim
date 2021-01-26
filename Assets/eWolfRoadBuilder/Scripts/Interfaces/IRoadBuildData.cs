using System.Collections.Generic;
using UnityEngine;

namespace eWolfRoadBuilder
{
    /// <summary>
    /// The road build data interface
    /// </summary>
	public interface IRoadBuildData
	{
		/// <summary>
		/// Gets the list of vertices
		/// </summary>
		List<Vector3> MeshVertices { get; }

		/// <summary>
		/// Get the list of uvs
		/// </summary>
		List<Vector2> MeshUVs { get; }

		/// <summary>
		/// Get the list of triangles for the given material names
		/// </summary>
		/// <param name="materialName">The name of the material set to get</param>
		/// <returns>The list of triangles</returns>
		List<int> GetTriangles(string materialName);

		/// <summary>
		/// Apply all the mest details to the object
		/// </summary>
		/// <param name="baseobject">The object to add the mesh too</param>
		void ApplyMeshDetails(GameObject baseobject);

		/// <summary>
		/// Apply the collision details to the object
		/// </summary>
		/// <param name="baseobject">The object to add the mesh too</param>
		/// <param name="createCollision">True to add collsion</param>
		void ApplyCollisionDetails(GameObject baseobject, bool createCollision);
	}
}