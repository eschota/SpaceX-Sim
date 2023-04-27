using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class PlanetComputeShaderToTexture : MonoBehaviour
{

    public enum TextureSize
    {
        small = 512, large = 1024, big = 2048, huge=4096, max = 8192
    }
    [Header("Textures Size: 512->8192")]
    public TextureSize textureSize=TextureSize.small;
    [Header ("Atmosphere Color")]
    public Color AtmosphereColor = Color.blue;
    [Header ("Atmosphere Intensity")]
    [HideInInspector]
    public float AtmosphereIntensity = 1;
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
    [SerializeField] Material AtmosphereMaterial;
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
        AtmosphereMaterial.SetColor("_AtmosphereColor", AtmosphereColor);

    }
    
    void ComputeMap(int renderID,string TexturePrefix)
    {
        RenderTexture rt = new RenderTexture((int)textureSize, (int)textureSize / 2, 24);
        Material bakemat = ComputeShaderMat;
        ComputeShaderMat.SetInt("_RenderID",renderID);
        ComputeShaderMat.SetFloat("_NoiseClimat", ClimatZoneNoise);
        ComputeShaderMat.SetFloat("_Temperature", Temperature);
        ComputeShaderMat.SetFloat("_OceanLevel", OceanLevel);
        ComputeShaderMat.SetColor("_AtmosphereColor", AtmosphereColor);
        RenderTexture.active = rt;
        Graphics.Blit(null, rt, bakemat);

        //RenderTexture.active = rtdest;
        Texture2D frame = new Texture2D((int) textureSize, (int)textureSize/2);
        frame.ReadPixels(new Rect(0, 0, (int)textureSize, (int)textureSize/2), 0, 0, false);
        frame.Apply();
        //byte[] bytes = frame.EncodeToPNG();
        //string p = (Application.dataPath + "/p.png");

        //FileStream file = File.Open(p, FileMode.Create);
        //BinaryWriter binary = new BinaryWriter(file);
        //binary.Write(bytes);
        //file.Close();

        //AssetDatabase.Refresh();
        //AssetDatabase.ImportAsset(p);
        //Texture2D hDRPMask = AssetDatabase.LoadAssetAtPath<Texture2D>(p);
        //Debug.Log("HDRP Mask Created: " + hDRPMask);
        //RenderTexture.active = null;
        SetToMaterial.SetTexture(TexturePrefix, frame);
    }
}
