#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class OCVrayLightMaterial 
{
    string TexturesPath;
    public Color Diffuse;
    public string Name;
    public Texture2D texmap;
    public float multiplier;
    public float selfIllumination_multiplier;
   
    public OCVrayLightMaterial(string _TexturesPath, float[] color, string name,string texmap_path,float _multiplier)
    {
        TexturesPath = _TexturesPath;
         if (texmap_path!=null)   texmap = AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(TexturesPath, Path.GetFileName(texmap_path)));
            Diffuse = new Color(color[0] / 255f, color[1] / 255f, color[2] / 255f);
            Name = name;
            multiplier = _multiplier;
    }
    public OCVrayLightMaterial(string _TexturesPath, OCObject o)
    {
        TexturesPath = _TexturesPath;
         if (o.texmap_path!=null)   texmap = AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(TexturesPath, Path.GetFileName(o.texmap_path)));
            Diffuse = new Color(o.color[0] / 255f, o.color[1] / 255f, o.color[2] / 255f);
            Name = o.ocname;
            multiplier = o.multiplier;
    }
}
#endif
