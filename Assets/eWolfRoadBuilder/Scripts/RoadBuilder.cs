using System;
using System.Collections.Generic;
using eWolfRoadBuilder.Terrains;
using eWolfRoadBuilderHelpers;
#if DEBUG
using UnityEditor;
#endif
using UnityEngine;

namespace eWolfRoadBuilder
{
    /// <summary>
    /// Handles the building the mesh part of the road
    /// </summary>
    public class RoadBuilder : MonoBehaviour, IRoadBuildData, IMaterialFrequency
    {
        #region Public Fields
        /// <summary>
        /// How the road will look
        /// </summary>
        public MaterialFrequency[] Details;

        /// <summary>
        /// The cross section details
        /// </summary>
        public CrossSection CrossSectionDetails;

        [Header("Options")]
        /// <summary>
        /// Create the collision for this road.
        /// </summary>
        [Tooltip("Applys the mesh as a collider to give the road collision")]
        public bool CreateCollision = true;

        /// <summary>
        /// Create mesh section for each node
        /// </summary>
        [Tooltip("Create mesh section for each node")]
        public bool MeshPerNode = false;

        /// <summary>
        /// Drop the mesh to the ground
        /// </summary>
        [Tooltip("Drop the mesh to the ground")]
        public bool DropToGround = true;

        /// <summary>
        /// Auto builder the road when any thing changes
        /// </summary>
        [Header("Advanced Options")]
        [Tooltip("Auto builder the road when you move a node. Can only be used when using MeshPerNode and not using DropToGround")]
        public bool AutoBuild = false;

        /// <summary>
        /// The UV set to use
        /// </summary>
        public UV_SET UvSet = UV_SET.RoadPavement;

        /// <summary>
        /// The Lighting options for the road
        /// </summary>
        public LightingOptions Lighting;
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the list of vertices
        /// </summary>
        public List<Vector3> MeshVertices
        {
            get
            {
                return _meshVertices;
            }
        }

        /// <summary>
        /// Gets the list of Road UVs
        /// </summary>
        public List<Vector2> MeshUVs
        {
            get
            {
                return _meshUVs;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Repair all the nodes in the layout
        /// </summary>
        public void RepairAllNodes()
        {
            Transform[] allChildren = GetComponentsInChildren<Transform>(true);
            foreach (Transform child in allChildren)
            {
                RoadNetworkNode rnn = child.GetComponent<RoadNetworkNode>();
                if (rnn != null)
                {
                    rnn.CheckNodeAreValid();
                    rnn.Details.CompressRoads();

                    for (int r = 0; r < rnn.Details.Roads.Count; r++)
                    {
                        RoadNetworkNode rnnInner = rnn.Details.Roads[r];
                        if (!rnnInner.HasLinksToNode(rnn))
                        {
                            Debug.Log("Node " + rnn.name + " links to " + rnnInner.name + " Linking them");
                            RoadNetworkLayout.AddRoadToNode(rnn, rnnInner);
                            rnn.Details.CompressRoads();
                            rnnInner.Details.CompressRoads();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Connect two nodes together
        /// </summary>
        /// <param name="firstRoad">The first road</param>
        /// <param name="secondRoad">The second road</param>
        public void ConnectNodes(RoadNetworkNode firstRoad, RoadNetworkNode secondRoad)
        {
            if (firstRoad != secondRoad)
            {
                RoadNetworkLayout.AddRoadToNode(firstRoad, secondRoad);
                RoadNetworkLayout.AddRoadToNode(secondRoad, firstRoad);
            }
        }

        /// <summary>
        /// Remove the mesh from all the road
        /// </summary>
        /// <param name="forceRemove">Force remove even if it has not changed</param>
        public void RemoveRoadMesh(bool forceRemove)
        {
            // remove all mesh from each node
            Transform[] allChildren = GetComponentsInChildren<Transform>(true);
            foreach (Transform child in allChildren)
            {
                RoadNetworkNode rub = child.GetComponent<RoadNetworkNode>();
                if (rub != null)
                {
                    if (rub.Details.IsNeighbourModified || forceRemove)
                    {
                        MeshFilter mf = child.GetComponent<MeshFilter>();
                        if (mf != null)
                        {
                            RoadBuilderHelper.RemoveMeshData(child.gameObject);
                            rub.Details.Modified = true;
                        }
                    }
                }
            }

            // Remove from the main object
            MeshFilter mf2 = GetComponent<MeshFilter>();
            if (mf2 != null)
            {
                RoadBuilderHelper.RemoveMeshData(gameObject);
            }

            IntersectionManager.Instance.Clear();
            StreetManager.Instance.Clear();
        }

        /// <summary>
        /// Create the road mesh
        /// </summary>
        public void CreateMesh(bool createMesh)
        {
            CheckRoadLinksMatch();

            RemoveRoadMesh(false);

            _meshVertices = new List<Vector3>();
            _meshUVs = new List<Vector2>();
            _meshMaterialsTriangles = new Dictionary<string, List<int>>();
            IntersectionManager.Instance.Clear();
            StreetManager.Instance.Clear();

            CreateMaterialsArray();
            CreateRoadLayout(gameObject, createMesh);
        }

        /// <summary>
        /// Drop the road to the floor
        /// </summary>
        public void DropRoad()
        {
            DropToGroundHelper.StepDropRoad();
        }

        /// <summary>
        /// Get the triangels for the correct material
        /// </summary>
        /// <param name="materialName">The material name</param>
        /// <returns>The list of triangles</returns>
        public List<int> GetTriangles(string materialName)
        {
            if (string.IsNullOrEmpty(materialName))
            {
                if (Details.Length == 0)
                    return _meshMaterialsTriangles["base"];
                else
                {
                    foreach (MaterialFrequency mf in Details)
                    {
                        if (mf.Frequency == MaterialFrequency.FrequencyRate.MainTexture)
                            materialName = mf.Material.name;
                    }
                }
            }

            return _meshMaterialsTriangles[materialName];
        }

        /// <summary>
        /// Draw the intersection array
        /// </summary>
        public void OnDrawGizmosSelected()
        {
            // re-add the below code for more debug info in the editor
            /*
            Color[] colors = new Color[] { Color.red, Color.blue, Color.green, Color.yellow, Color.magenta, Color.cyan };

			int indexer = 0;

			int count = IntersectionManager.Instance.LinksCount;
            for (int i = 0; i < count; i++)
            {
                RoadCrossSection rsca = null;

                Gizmos.color = colors[indexer++];
                if (indexer > colors.Length - 1)
                    indexer = 0;

                List<Guid> list = IntersectionManager.Instance[i];
                if (list.Count == 2)
                {
                    foreach (Guid item in list)
                    {
                        RoadCrossSection rsc = IntersectionManager.Instance[item];
                        
                        // Draw the inter section
                        Gizmos.DrawLine(rsc.CurbRightEndDrop, rsc.CurbRightEnd);
                        Gizmos.DrawLine(rsc.CurbRightEnd, rsc.CurbRightLip);
                        Gizmos.DrawLine(rsc.CurbRightLip, rsc.Right);

                        Gizmos.DrawLine(rsc.Right, rsc.Left);
                        Gizmos.DrawLine(rsc.Left, rsc.CurbLeftLip);
                        Gizmos.DrawLine(rsc.CurbLeftLip, rsc.CurbLeftEnd);
                        Gizmos.DrawLine(rsc.CurbLeftEnd, rsc.CurbLeftEndDrop);

                        if (rsca != null)
                        {
                            if (!rsca.IsRoadTwisted(rsc))
                            {
                                Gizmos.DrawLine(rsc.Right, rsca.Left);
                                Gizmos.DrawLine(rsc.Left, rsca.Right);
                            }
                            else
                            {
                                Gizmos.DrawLine(rsc.Left, rsca.Left);
                                Gizmos.DrawLine(rsc.Right, rsca.Right);
                            }
                        }
                        rsca = rsc;
                    }
                }
                if (list.Count == 3)
                {
                    RoadCrossSection a = IntersectionManager.Instance[list[0]];
                    RoadCrossSection b = IntersectionManager.Instance[list[1]];
                    RoadCrossSection c = IntersectionManager.Instance[list[2]];

                    Gizmos.DrawLine(a.Left, b.Right);
                    Gizmos.DrawLine(a.Right, b.Left);

                    Gizmos.DrawLine(a.Left, c.Right);
                    Gizmos.DrawLine(a.Right, c.Left);

                    Gizmos.DrawLine(b.Left, c.Right);
                    Gizmos.DrawLine(b.Right, c.Left);
                }
            }*/
        }

        /// <summary>
        /// Create the material array
        /// </summary>
        public void CreateMaterialsArray()
        {
            if (Details.Length == 0)
            {
                _meshMaterialsTriangles.Add("base", new List<int>());
            }

            foreach (MaterialFrequency mf in Details)
            {
                if (!_meshMaterialsTriangles.ContainsKey(mf.Material.name))
                {
                    _meshMaterialsTriangles.Add(mf.Material.name, new List<int>());
                }
            }

            if (!MeshPerNode)
                AddAllChildrenMaterials(gameObject);
        }

        /// <summary>
        /// Apply the mesh details to the object
        /// </summary>
        /// <param name="baseobject">The object to add the mesh too</param>
        public void ApplyMeshDetails(GameObject baseobject)
        {
#if DEBUG
            Mesh mesh = new Mesh();
            mesh.name = "RoadMesh";
            baseobject.GetComponent<MeshFilter>().mesh = mesh;

            ApplyObjectOffSet(baseobject.transform.position);

            mesh.vertices = MeshVertices.ToArray();
            mesh.uv = MeshUVs.ToArray();

            // Create the material and assign triangles
            Renderer r = baseobject.GetComponent<Renderer>();
            List<Material> materials = new List<Material>();
            int count = 0;
            mesh.subMeshCount = _meshMaterialsTriangles.Count;

            List<Material> AllMaterials = FindAllMaterials(baseobject);

            foreach (KeyValuePair<string, List<int>> meshTris in _meshMaterialsTriangles)
            {
                if (meshTris.Value.Count == 0)
                    continue;

                Material mat = AllMaterials.Find((m) => m.name == meshTris.Key);
                materials.Add(mat);
                mesh.SetTriangles(meshTris.Value.ToArray(), count++);
            }

            mesh.subMeshCount = count; // just in case we didn't add all of them

            r.materials = materials.ToArray();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            mesh.RecalculateBounds();

            if (RoadConstructorHelper.Lighting.BakedLighting)
            {
                Debug.Log("Appling unwrapping for Baked Lighting");

                UnwrapParam up = new UnwrapParam();
                up.hardAngle = RoadConstructorHelper.Lighting.HardAngle;
                up.packMargin = RoadConstructorHelper.Lighting.PackMargin;
                up.angleError = RoadConstructorHelper.Lighting.AngleError;
                up.areaError = RoadConstructorHelper.Lighting.AngleError;

                Unwrapping.GenerateSecondaryUVSet(mesh, up);
            }

#if UNITY_5_5_OR_NEWER
#if UNITY_EDITOR
            // TODO: Added option UnityEditor.MeshUtility.SetMeshCompression(mesh, UnityEditor.ModelImporterMeshCompression.);
            UnityEditor.MeshUtility.Optimize(mesh);
#endif
#else
            mesh.Optimize();
#endif
#endif
        }

        /// <summary>
        /// Apply the collision details to the object
        /// </summary>
        /// <param name="baseobject">The base object to add the collision too</param>
        /// <param name="createCollision">If true, create the collision meash.</param>
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
        /// Get the materials details
        /// </summary>
        public MaterialFrequency[] GetDetails
        {
            get
            {
                return Details;
            }
        }

        /// <summary>
        /// Whether or we have a terrain modifier or not
        /// </summary>
        /// <returns>True if we are using a terrain modifier</returns>
        public bool HasTerrainModifier()
        {
            TerrainModifier ocs = gameObject.GetComponent<TerrainModifier>();
            return ocs != null;
        }

        /// <summary>
        /// Add a terrain modifier
        /// </summary>
        public void AddTerrainModifier()
        {
            gameObject.AddComponent<TerrainModifier>();
            TerrainModifier omf = gameObject.GetComponent<TerrainModifier>();
        }

        /// <summary>
        /// Remove the terrain modifier
        /// </summary>
        public void RemoveTerrainModifier()
        {
            if (HasTerrainModifier())
            {
                gameObject.GetComponent<TerrainModifier>().enabled = false;
                DestroyImmediate(gameObject.GetComponent<TerrainModifier>());
            }
        }

        public void ModifyTerrain()
        {
            CreateMesh(true);

            TerrainModifier tm = gameObject.GetComponent<TerrainModifier>();

            tm.ApplyTerrainHeight(gameObject);
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Do all of the road links match
        /// </summary>
        private void CheckRoadLinksMatch()
        {
            Transform[] allChildren = GetComponentsInChildren<Transform>(true);
            foreach (Transform child in allChildren)
            {
                RoadNetworkNode rnn = child.GetComponent<RoadNetworkNode>();
                if (rnn != null)
                {
                    rnn.Details.CompressRoads();
                    foreach (RoadNetworkNode rnnInner in rnn.Details.Roads)
                    {
                        if (rnnInner != null)
                        {
                            rnnInner.Details.CompressRoads();
                            if (!rnnInner.HasLinksToNode(rnn))
                            {
                                Debug.Log("Node " + rnn.name + " links to " + rnnInner.name + " but not the other way around. Adding missing link");
                                RoadNetworkLayout.AddRoadToNode(rnnInner, rnn);
                                RoadNetworkLayout.AddRoadToNode(rnn, rnnInner);
                                rnn.Details.CompressRoads();
                                rnnInner.Details.CompressRoads();
                            }
                        }
                        else
                        {
                            Debug.Log("Node " + rnn.name + " Has a missing link.");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Add all of the materials from the children
        /// </summary>
        /// <param name="baseobject">The parent object</param>
        private void AddAllChildrenMaterials(GameObject baseobject)
        {
            Transform[] allChildren = baseobject.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in allChildren)
            {
                OverridableMaterialFrequency omf = child.GetComponent<OverridableMaterialFrequency>();
                if (omf != null)
                {
                    foreach (MaterialFrequency mf in omf.Details)
                    {
                        if (!_meshMaterialsTriangles.ContainsKey(mf.Material.name))
                        {
                            _meshMaterialsTriangles.Add(mf.Material.name, new List<int>());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get the all the materials from the road network nodes
        /// </summary>
        /// <param name="baseobject">The parent object</param>
        /// <returns>The array of all the materials</returns>
        private List<Material> FindAllMaterials(GameObject baseobject)
        {
            List<Material> materials = new List<Material>();

            RoadBuilder rb = baseobject.GetComponent<RoadBuilder>();

            foreach (MaterialFrequency mf in rb.Details)
            {
                if (!materials.Contains(mf.Material))
                    materials.Add(mf.Material);
            }

            Transform[] allChildren = baseobject.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in allChildren)
            {
                OverridableMaterialFrequency omf = child.GetComponent<OverridableMaterialFrequency>();
                if (omf != null)
                {
                    foreach (MaterialFrequency mf in omf.Details)
                    {
                        if (!materials.Contains(mf.Material))
                            materials.Add(mf.Material);
                    }
                }
            }

            return materials;
        }

        /// <summary>
        /// Create the road mesh for this object.
        /// </summary>
        /// <param name="baseObject">The base (main) object</param>
        private void CreateRoadLayout(GameObject baseObject, bool createMesh)
        {
            RoadConstructorHelper.CrossSectionDetails = CrossSectionDetails;
            RoadConstructorHelper.Lighting = Lighting;
            RoadConstructorHelper.MaterialFrequencySet = this;
            RoadConstructorHelper.BaseNodeLayoutNode = baseObject.GetComponent<RoadNetworkLayout>();
            RoadConstructorHelper.SetUVValues(UvSet);

            Transform[] allChildren = baseObject.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in allChildren)
            {
                RoadNetworkNode rub = child.GetComponent<RoadNetworkNode>();
                if (rub != null)
                {
                    if (rub.Details.Union == RoadNetworkNode.UNION_TYPE.NONE)
                        continue;

                    foreach (RoadNetworkNode rnn in rub.Details.Roads)
                    {
                        if (rnn.Details.Modified)
                            rub.Details.ModifiedSecondHand = true;
                    }

                    rub.CreateUnion();
                    if (rub.RoadUnion != null)
                    {
                        rub.RoadUnion.CreateLayout(this);
                        if (MeshPerNode)
                            RoadConstructorHelper.CreateMeshAndCollisionComponents(rub.gameObject);
                        else
                            RoadConstructorHelper.CreateMeshAndCollisionComponents(baseObject);
                    }
                }
            }

            foreach (Transform child in allChildren)
            {
                RoadNetworkNode rub = child.GetComponent<RoadNetworkNode>();
                if (rub != null)
                {
                    if (rub.Details.Union == RoadNetworkNode.UNION_TYPE.NONE)
                        continue;

                    rub.RoadUnion.CreateStreetLayout(this);
                }
            }

            if (createMesh)
            {
                if (DropToGround)
                    DropToGroundHelper.StepDropRoad();

                foreach (Transform child in allChildren)
                {
                    RoadNetworkNode rub = child.GetComponent<RoadNetworkNode>();
                    if (rub != null)
                    {
                        if (rub.Details.Union == RoadNetworkNode.UNION_TYPE.NONE)
                            continue;

                        if (MeshPerNode)
                        {
                            if (rub.Details.IsNeighbourModified)
                            {
                                rub.RoadUnion.CreateMesh(rub.BuildData);
                            }
                        }
                        else
                            rub.RoadUnion.CreateMesh(this);
                    }
                }
            }

            if (createMesh)
            {
                foreach (Transform child in allChildren)
                {
                    RoadNetworkNode rub = child.GetComponent<RoadNetworkNode>();
                    if (rub != null)
                    {
                        if (rub.Details.Union == RoadNetworkNode.UNION_TYPE.NONE)
                            continue;

                        if (MeshPerNode)
                        {
                            if (rub.Details.IsNeighbourModified)
                            {
                                rub.BuildData.ApplyMeshDetails(rub.gameObject);
                                rub.BuildData.ApplyCollisionDetails(rub.gameObject, CreateCollision);
                            }
                        }
                    }
                }
            }

            if (!MeshPerNode)
            {
                ApplyMeshDetails(baseObject);
                ApplyCollisionDetails(baseObject, CreateCollision);
            }
            foreach (Transform child in allChildren)
            {
                RoadNetworkNode rub = child.GetComponent<RoadNetworkNode>();
                if (rub != null)
                {
                    rub.Details.Modified = false;
                    rub.Details.ModifiedSecondHand = false;
                }
            }
        }

        /// <summary>
        /// Apply the off set of the base object to the vertices
        /// </summary>
        /// <param name="offSet">The off set to apply</param>
        private void ApplyObjectOffSet(Vector3 offSet)
        {
            for (int i = 0; i < MeshVertices.Count; i++)
            {
                MeshVertices[i] -= offSet;
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