using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class PlanetComputeShaderToTexture : MonoBehaviour
{
    [Header ("ClimateZonesNoise")]
    [Range (0,100)]
    public float ClimatZoneNoise = 22;
    [Header ("Ocean Level")]
    [Range (0,1)]
    public float OceanLevel = 0.21f;
    [Header ("Temperature")]
    [Range (0,1)]
    public float Temperature = 0.5f;
    public Material ComputeShaderMat;
    public Material SetToMaterial;
    [HideInInspector]
    public RenderTexture rt;
    // Use this for initialization
    [ContextMenu("Compute")]
    void ComputeMaps()
    {

        ComputeMap(0, "_BaseColorMap");
        ComputeMap(1, "_MaskMap");
        ComputeMap(2, "_NormalMap");
        ComputeMap(3, "_HeightMap");
        ComputeMap(4, "_EmissiveColorMap");
    }
    
    void ComputeMap(int renderID,string TexturePrefix)
    {
        RenderTexture rt = new RenderTexture(512, 512, 24);
        Material bakemat = ComputeShaderMat;
        ComputeShaderMat.SetInt("_RenderID",renderID);
        ComputeShaderMat.SetFloat("_NoiseClimat", ClimatZoneNoise);
        ComputeShaderMat.SetFloat("_Temperature", Temperature);
        ComputeShaderMat.SetFloat("_OceanLevel", OceanLevel);
        RenderTexture.active = rt;
        Graphics.Blit(null, rt, bakemat);

        //RenderTexture.active = rtdest;
        Texture2D frame = new Texture2D(512, 512);
        frame.ReadPixels(new Rect(0, 0, 512, 512), 0, 0, false);
        frame.Apply();
        byte[] bytes = frame.EncodeToPNG();
        string p = (Application.dataPath + "/p.png");

        FileStream file = File.Open(p, FileMode.Create);
        BinaryWriter binary = new BinaryWriter(file);
        binary.Write(bytes);
        file.Close();

        AssetDatabase.Refresh();
        AssetDatabase.ImportAsset(p);
        Texture2D hDRPMask = AssetDatabase.LoadAssetAtPath<Texture2D>(p);
        Debug.Log("HDRP Mask Created: " + hDRPMask);
        RenderTexture.active = null;
        SetToMaterial.SetTexture(TexturePrefix, frame);
    }
}
