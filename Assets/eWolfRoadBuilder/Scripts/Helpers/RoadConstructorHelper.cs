using System;
using System.Collections.Generic;
using eWolfRoadBuilder;
using eWolfRoadBuilder.Terrains;
using eWolfRoadBuilderHelpers;
using UnityEngine;

public class RoadConstructorHelper
{
    #region Fields
    public static CrossSection CrossSectionDetails;
    public static IMaterialFrequency MaterialFrequencySet;
    public static RoadNetworkLayout BaseNodeLayoutNode;
    public static Dictionary<string, float> UVSET = new Dictionary<string, float>();
    public static UV_SET RoadUVSet = UV_SET.RoadPavement;
    public static LightingOptions Lighting;    
    #endregion

    #region Public Methods
    /// <summary>
    /// Set the materials array
    /// </summary>
    /// <param name="materialFrequency">The material frequency</param>
    /// <param name="materials">The names of the all the materials to set</param>
    public static void SetMaterialsArray(string[] materials, IMaterialFrequency materialFrequency)
    {
        if (materialFrequency.GetDetails.Length < 0)
            return;

        foreach (MaterialFrequency mf in materialFrequency.GetDetails)
        {
            if (mf.Frequency == MaterialFrequency.FrequencyRate.MainTexture)
            {
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i] = mf.Material.name;
                }
            }
        }

        foreach (MaterialFrequency mf in materialFrequency.GetDetails)
        {
            if (mf.Frequency == MaterialFrequency.FrequencyRate.Randon50Percent)
            {
                List<int> diffArray = GetPercentOfIndexs(50, materials.Length);
                foreach (int number in diffArray)
                    materials[number] = mf.Material.name;
            }

            if (mf.Frequency == MaterialFrequency.FrequencyRate.Randon25Percent)
            {
                List<int> diffArray = GetPercentOfIndexs(25, materials.Length);
                foreach (int number in diffArray)
                    materials[number] = mf.Material.name;
            }

            if (mf.Frequency == MaterialFrequency.FrequencyRate.OncePerRoad)
            {
                int r = UnityEngine.Random.Range(0, materials.Length - 1);
                materials[r] = mf.Material.name;
            }

            if (mf.Frequency == MaterialFrequency.FrequencyRate.MiddleOfRoad)
            {
                int r = (materials.Length - 1) / 2;
                materials[r] = mf.Material.name;
            }
        }
    }

    /// <summary>
    /// Gets the main material from the frequency 
    /// </summary>
    /// <param name="materialFrequency">The material frequency</param>
    /// <returns>The name of the main material</returns>
    public static string GetMainMaterial(IMaterialFrequency materialFrequency)
    {
        foreach (MaterialFrequency mf in materialFrequency.GetDetails)
        {
            if (mf.Frequency == MaterialFrequency.FrequencyRate.MainTexture)
            {
                return mf.Material.name;
            }
        }

        return string.Empty;
    }

    /// <summary>
    /// Get the unique name for a road
    /// </summary>
    /// <returns>The unique name for the road</returns>
    internal static string GetUniqueRoadName(GameObject parent)
    {
        RoadNetworkLayout roadNetworkLayout = parent.GetComponentInParent<RoadNetworkLayout>();

        List<string> roadNames = new List<string>();
        foreach (Transform child in roadNetworkLayout.transform)
        {
            roadNames.Add(child.name);
        }

        int roadNumber = roadNames.Count + 1;

        bool valid = false;

        string roadTestName = string.Empty;
        while (!valid)
        {
            roadTestName = string.Format("R_" + roadNumber.ToString("0000"));
            valid = !roadNames.Contains(roadTestName);
            roadNumber++;
        }

        return roadTestName;
    }

    /// <summary>
    /// Set the mesh and collsion components if not already set
    /// </summary>
    /// <param name="obj">The object to add the components to</param>
    internal static void CreateMeshAndCollisionComponents(GameObject obj)
    {
        MeshFilter mf = obj.GetComponent<MeshFilter>();
        if (mf == null)
        {
            obj.AddComponent<MeshFilter>();
            mf = obj.GetComponent<MeshFilter>();
        }

        if (mf.sharedMesh == null)
        {
            Mesh mesh = new Mesh();
            obj.GetComponent<MeshFilter>().mesh = mesh;
        }

        Renderer r = obj.GetComponent<Renderer>();
        if (r == null)
            obj.AddComponent<MeshRenderer>();

        MeshCollider mc = obj.GetComponent<MeshCollider>();
        if (mc == null)
            obj.AddComponent<MeshCollider>();
    }

    /// <summary>
    /// The the UV set to use
    /// </summary>
    /// <param name="uvSet">The uv set to use</param>
    public static void SetUVValues(UV_SET uvSet)
    {
        RoadUVSet = uvSet;
        UVSET = UVSetBuilder.CreateUVSet(uvSet);
    }

    /// <summary>
    /// Gets a vaild cross section from the node
    /// </summary>
    /// <param name="node">The node</param>
    /// <returns>The cross section</returns>
    public static ICrossSection CrossSection(RoadNetworkNode node)
    {
        ICrossSection sc = node.gameObject.GetComponent<ICrossSection>();
        if (sc == null)
            sc = RoadConstructorHelper.CrossSectionDetails;

        return sc;
    }

    /// <summary>
    /// Get materials frequency for this node
    /// </summary>
    /// <param name="node">The node</param>
    /// <returns>The materials frequency</returns>
    public static IMaterialFrequency Materials(RoadNetworkNode node)
    {
        IMaterialFrequency mf = node.gameObject.GetComponent<IMaterialFrequency>();
        if (mf == null)
            mf = RoadConstructorHelper.MaterialFrequencySet;

        return mf;
    }

    /// <summary>
    /// Gets the angle between two vectors
    /// </summary>
    /// <param name="startPosition">The first vector</param>
    /// <param name="endPosition">The second vectot</param>
    /// <returns>The angle between the two vectors</returns>
    public static float GetAngle(Vector3 startPosition, Vector3 endPosition)
    {
        Vector3 diff = startPosition - endPosition;
        return MathsHelper.GetAngleFrom(diff.x, diff.z);
    }
    
    /// <summary>
    /// Apply the strights to the terrain
    /// </summary>
    /// <param name="_roadNetworkNode">The main road network node</param>
    /// <param name="tm">The Terrain modifier</param>
    /// <param name="index">The index of the road to use</param>
    public static void ApplyLeadingStrights(RoadNetworkNode _roadNetworkNode, TerrainModifier tm, int index)
    {
        string[] stringArray = new string[2] { _roadNetworkNode.name, _roadNetworkNode.Details.Roads[index].name };
        Array.Sort(stringArray);

        string streetFullName = string.Join("-", stringArray);

        StreetData street = StreetManager.Instance[streetFullName];
        RoadCrossSection rsc = street.GetFirst;
        if (rsc == null)
            return;

        RoadCrossSection rsca = street.GetSecond;
        if (rsca == null)
            return;

        tm.ApplyToTerrain(rsc, rsca);
    }
    
    #endregion

    #region Private Methods
    /// <summary>
    /// Get an array of indexes for the percentage given
    /// </summary>
    /// <param name="percentage">The percentage to use</param>
    /// <param name="length">The length of the data</param>
    /// <returns>The list of ints of the length at the percentage given</returns>
    private static List<int> GetPercentOfIndexs(int percentage, int length)
    {
        List<int> diffArray = new List<int>();

        for (int i = 0; i < length; i++)
        {
            int r = UnityEngine.Random.Range(0, 100);
            if (r < percentage)
                diffArray.Add(i);
        }

        return diffArray;
    }
    #endregion
}