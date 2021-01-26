using System.Collections.Generic;
#if DEBUG
using UnityEditor;
#endif
using UnityEngine;

namespace eWolfRoadBuilder
{
	public class RoadBuildData : IRoadBuildData
	{
		/// <summary>
		/// Default Constructor
		/// </summary>
		public RoadBuildData()
		{
			CreateMaterialsArray();
		}

		#region Public Properties
		/// <summary>
		/// Gets the list of vertices
		/// </summary>
		public List<Vector3> MeshVertices
		{
			get { return _meshVertices; }
		}

		/// <summary>
		/// Gets the list of Road UVs
		/// </summary>
		public List<Vector2> MeshUVs
		{
			get { return _meshUVs; }
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Get the triangels for the correct material
		/// </summary>
		/// <param name="materialName">The material name</param>
		/// <returns>The list of triangles</returns>
		public List<int> GetTriangles(string materialName)
		{
			if (string.IsNullOrEmpty(materialName))
			{
				if (RoadConstructorHelper.MaterialFrequencySet.GetDetails.Length == 0)
					return _meshMaterialsTriangles["base"];
				else
				{
					foreach (MaterialFrequency mf in RoadConstructorHelper.MaterialFrequencySet.GetDetails)
					{
						if (mf.Frequency == MaterialFrequency.FrequencyRate.MainTexture)
							materialName = mf.Material.name;
					}
				}
			}

			return _meshMaterialsTriangles[materialName];
		}

        /// <summary>
        /// Create the mesh materials
        /// </summary>
		public void CreateMaterialsArray()
		{
			if (RoadConstructorHelper.MaterialFrequencySet.GetDetails.Length == 0)
			{
				_meshMaterialsTriangles.Add("base", new List<int>());
				return;
			}

			foreach (MaterialFrequency mf in RoadConstructorHelper.MaterialFrequencySet.GetDetails)
			{
                if (!_meshMaterialsTriangles.ContainsKey(mf.Material.name))
                {
                    _meshMaterialsTriangles.Add(mf.Material.name, new List<int>());
                }
			}
		}

		/// <summary>
		/// Apply the collision details to the object
		/// </summary>
		/// <param name="baseobject">The object to add the mesh too</param>
		/// <param name="createCollision">True to add collsion</param>
		public void ApplyCollisionDetails(GameObject baseobject, bool createCollision)
		{
			MeshCollider CCgb = baseobject.GetComponent<MeshCollider>();
			if (createCollision)
			{
				CCgb.convex = false;
				CCgb.sharedMesh = baseobject.GetComponent<MeshFilter>().sharedMesh;
			}
			else
			{
				CCgb.sharedMesh = null;
			}
		}

		/// <summary>
		/// Apply all the mest details to the object
		/// </summary>
		/// <param name="baseobject">The object to add the mesh too</param>
		public void ApplyMeshDetails(GameObject baseobject)
		{
#if DEBUG
            CenterMesh(baseobject);

			Mesh mesh = new Mesh();
            mesh.name = baseobject.name + "Mesh";
			baseobject.GetComponent<MeshFilter>().mesh = mesh;
			mesh.vertices = MeshVertices.ToArray();
			mesh.uv = MeshUVs.ToArray();

			// Create the material and assign triangles
			Renderer r = baseobject.GetComponent<Renderer>();
			List<Material> materials = new List<Material>();
			int count = 0;
			mesh.subMeshCount = _meshMaterialsTriangles.Count;

			foreach (MaterialFrequency mf in RoadConstructorHelper.MaterialFrequencySet.GetDetails)
			{
                int[] tris = GetTriangles(mf.Material.name).ToArray();
                if (tris.Length == 0)
                    continue;

                materials.Add(mf.Material);
				mesh.SetTriangles(tris, count++);
			}

            mesh.subMeshCount = count; // just in case we didn't add all of them

            r.materials = materials.ToArray();

            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            mesh.RecalculateBounds();

            if (RoadConstructorHelper.Lighting.BakedLighting)
            {
                if (count != 0)
                {
                    UnwrapParam up = new UnwrapParam();
                    up.hardAngle = RoadConstructorHelper.Lighting.HardAngle;
                    up.packMargin = RoadConstructorHelper.Lighting.PackMargin;
                    up.angleError = RoadConstructorHelper.Lighting.AngleError;
                    up.areaError = RoadConstructorHelper.Lighting.AngleError;

                    Unwrapping.GenerateSecondaryUVSet(mesh, up);
                }
            }

#if UNITY_5_5_OR_NEWER
#if UNITY_EDITOR
            UnityEditor.MeshUtility.Optimize(mesh);
#endif
#else
            mesh.Optimize();
#endif
#endif
        }

        /// <summary>
        /// Center the mesh to the base position
        /// </summary>
        /// <param name="baseobject">the base object to center around</param>
		private void CenterMesh(GameObject baseobject)
		{
			Vector3 pos = baseobject.transform.position;

			for (int i = 0; i < MeshVertices.Count; i++)
			{
				MeshVertices[i] -= pos;
			}
		}
#endregion

        #region Private Fields
		/// <summary>
		/// The vertices of the road
		/// </summary>
		private List<Vector3> _meshVertices = new List<Vector3>();

		/// <summary>
		/// The UVs for the road
		/// </summary>
		private List<Vector2> _meshUVs = new List<Vector2>();

		/// <summary>
		/// The triangles draw order for the road for each materials used
		/// </summary>
		private Dictionary<string, List<int>> _meshMaterialsTriangles = new Dictionary<string, List<int>>();
        #endregion
	}
}
