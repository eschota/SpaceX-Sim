using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class PlanetComputeShaderToTexture : MonoBehaviour
{
    public Material ClimateZonesMat;
    public Material HeightPlusOceanMap;
    public Material SetToMaterial;
    
    public RenderTexture rt;
    // Use this for initialization
    [ContextMenu("Compute")]

    
    void Compute()
    {
        RenderTexture rt = new RenderTexture(512, 512, 24);
        Material bakemat = ClimateZonesMat;
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
        SetToMaterial.SetTexture("_BaseColorMap", frame);
    }
}
