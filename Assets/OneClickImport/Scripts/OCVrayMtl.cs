#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEditor.Progress;

public class OCVrayMtl 
{
     
    public enum MtlType { Vray, CoronaPhysical, CoronaLegacyMtl }
    public MtlType mtlType;
    [SerializeField] OneClickSceneManager OCSM;
    public Color Diffuse;
    public Color Reflection;
    public string Name;
    public float reflection_glossiness;
    public float reflection_IOR;
    public bool reflection_fresnel;
    public Color Refraction;
    public Texture2D texmap_diffuse;    
    public int texmap_diffuse_channel;    
    public Texture2D texmap_displacement;
    public int texmap_displacement_channel;
    public Texture2D texmap_bump;
    public int texmap_bump_channel;
    public Texture2D texmap_reflection;
    public int texmap_reflection_channel;
    public Texture2D texmap_refraction;
    public int texmap_refraction_channel;
    public Texture2D texmap_reflectionGlossiness;
    public int texmap_reflectionGlossiness_channel;
    public Color refraction_fogColor;
    public Color selfIllumination;
    public bool selfIllumination_gi;
    public Texture2D hDRPMask;
    public Texture2D texmap_metalness;
    public bool texmap_metalness_on;
    public float texmap_metalness_multiplier;
    public float reflection_metalness;
    public float selfIllumination_multiplier;
    public bool brdf_useRoughness;
    // Corona Mtl "CoronaLegacyMtl" 
    public Texture2D texmapDiffuse;
    public Texture2D baseIorTexmap;
    public Texture2D metalnessTexmap;
    public int texmapDiffuse_channel;
    public int texmapDiffuse_u_tiles;
    public int texmapDiffuse_v_tiles;
    public int roughnessMode;
    //public "texmapDiffuse_uniform" : null, 
    public Color colorDiffuse;
    public Color colorReflect;
    public float baseIor;
    public Color colorRefract;
    public Color ColorcolorOpacity;
    public Color colorTranslucency;
    public Color colorSelfIllum;
    public float levelDiffuse;
    public float baseLevel;
    public float levelReflect;
    public float levelRefract;
    public float levelOpacity;
    public float levelTranslucency;
    public float levelSelfIllum;
    public float reflectGlossiness;
    public float refractGlossiness;
    public float fresnelIor;
    public float _Base_Layer_Level;
    public float _Reflection_Level;
    #region VRAY
    public OCVrayMtl(OCObject o, OneClickSceneManager ocsm, float[] color, string name, float[] _reflection, float _reflection_glossiness, string texmap_diffuse_path, bool _reflection_fresnel,
        float[] refraction, string texmap_displacement_path, string texmap_bump_path, string texmap_reflection_path,
        string texmap_refraction_path, string texmap_reflectionGlossiness_path,
        float _reflection_IOR, int _texmap_diffuse_channel, int _texmap_displacement_channel, int _texmap_bump_channel, int _texmap_reflection_channel, int _texmap_refraction_channel, int _texmap_reflectionGlossiness_channel, float [] _refraction_fogColor,
        float[] _selfIllumination, bool _selfIllumination_gi, string texmap_metalness_path, float _reflection_metalness, float _selfIllumination_multiplier, bool _brdf_useRoughness)

    {
        mtlType=MtlType.Vray;
        selfIllumination_multiplier=_selfIllumination_multiplier;
        texmap_diffuse_channel = _texmap_diffuse_channel;
        texmap_displacement_channel = _texmap_displacement_channel;
        texmap_bump_channel = _texmap_bump_channel;
        texmap_reflection_channel = _texmap_reflection_channel;
        texmap_refraction_channel = _texmap_refraction_channel;
        texmap_reflectionGlossiness_channel = _texmap_reflectionGlossiness_channel;

        if (_selfIllumination != null) selfIllumination = new Color(_selfIllumination[0]/255f, _selfIllumination[1] / 255f, _selfIllumination[2] / 255f, _selfIllumination[3] / 255f);
        
        selfIllumination_gi = _selfIllumination_gi;
        
        if (_refraction_fogColor! != null)
            refraction_fogColor = new Color(_refraction_fogColor[0], _refraction_fogColor[1], _refraction_fogColor[2], _refraction_fogColor[3]);
        if (refraction!=null)  
            Refraction = new Color((refraction[0]/255f), (refraction[1] / 255f), (refraction[2] / 255f),(refraction[3] / 255f));
        
        OCSM = ocsm;

        if(texmap_metalness_path != null) texmap_metalness = AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(OCSM.TexturesPath, Path.GetFileName(texmap_metalness_path)));

        reflection_metalness = _reflection_metalness;

        reflection_glossiness=_reflection_glossiness;

        if (texmap_diffuse_path!=null)      texmap_diffuse = AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(OCSM.TexturesPath, Path.GetFileName(texmap_diffuse_path)));
        
        if(texmap_displacement_path!=null)      texmap_displacement = AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(OCSM.TexturesPath, Path.GetFileName(texmap_displacement_path)));
        
        if(texmap_bump_path!=null)   texmap_bump = AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(OCSM.TexturesPath, Path.GetFileName(texmap_bump_path)));
        
        if(texmap_reflection_path!=null) texmap_reflection = AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(OCSM.TexturesPath, Path.GetFileName(texmap_reflection_path)));
     
        if(texmap_reflectionGlossiness_path!= null) texmap_reflectionGlossiness = AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(OCSM.TexturesPath, Path.GetFileName(texmap_reflectionGlossiness_path)));
        if(texmap_reflectionGlossiness_path!= null) texmap_reflectionGlossiness = AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(OCSM.TexturesPath, Path.GetFileName(texmap_reflectionGlossiness_path)));

     
        if (texmap_refraction_path!=null)  
            texmap_refraction = AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(OCSM.TexturesPath, Path.GetFileName(texmap_refraction_path)));
        
        if(texmap_reflectionGlossiness_path!=null)              
            texmap_reflectionGlossiness = AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(OCSM.TexturesPath, Path.GetFileName(texmap_reflectionGlossiness_path)));
        if(color!=null)  
            Diffuse = new Color(color[0] / 255f, color[1] / 255f, color[2] / 255f, 1 - (refraction[0] / 255f));
        
        if(_reflection!=null)   
            Reflection = new Color(_reflection[0] / 255f, _reflection[1] / 255f, _reflection[2] / 255f);
        
        Name = name;
        brdf_useRoughness = _brdf_useRoughness;

        reflection_fresnel = _reflection_fresnel;
              
        if(refraction!=null)      Refraction = new Color(refraction[0], refraction[1], refraction[2]);
        reflection_IOR = _reflection_IOR;



        if (texmap_bump != null)
        {
            ChangeBumpTexturesImportType(texmap_bump);
            return;
        }
        if (texmap_displacement != null) ChangeBumpTexturesImportType(texmap_displacement);
       
    }
    #endregion
    public OCVrayMtl(OneClickSceneManager ocsm, OCObject o)// CORONA
    {
        if(o.octype== "_CoronaPhysicalMtl") mtlType = MtlType.CoronaPhysical;
            if (o.octype == "CoronaLegacyMtl")
            {
                mtlType = MtlType.CoronaLegacyMtl;

                levelReflect = o.levelReflect;
            }


        if ( o.ocname == "mat_122_5")
            {
                ;
            }

        selfIllumination_multiplier = o.levelSelfIllum; 

     if(o.texmapDiffuse_channel!=0)   texmap_diffuse_channel = o.texmapDiffuse_channel;
     if (o.texmap_diffuse_channel != 0) texmap_diffuse_channel = o.texmap_diffuse_channel;
     if (o.baseTexmap_channel != 0) texmap_diffuse_channel = o.baseTexmap_channel;
     if(o.baseRoughnessTexmap_channel!=0) texmap_reflectionGlossiness_channel = o.baseRoughnessTexmap_channel;
     if(o.texmapReflectGlossiness_channel != 0) texmap_reflectionGlossiness_channel = o.texmapReflectGlossiness_channel;
     if(o.baseBumpTexmap_channel != 0) texmap_bump_channel= o.baseBumpTexmap_channel;
         //texmap_displacement_channel = _texmap_displacement_channel;
       // if (o.texmapBump != 0) texmap_bump_channel= o.texmapBump;

      //  texmap_reflection_channel = _texmap_reflection_channel;
      //  texmap_refraction_channel = _texmap_refraction_channel;
      //  texmap_reflectionGlossiness_channel = _texmap_reflectionGlossiness_channel;

        if (o.colorSelfIllum != null) selfIllumination = new Color(o.colorSelfIllum[0] / 255f, o.colorSelfIllum[1] / 255f, o.colorSelfIllum[2] / 255f, o.colorSelfIllum[3] / 255f);

        //selfIllumination_gi = _selfIllumination_gi;

        //if (_refraction_fogColor! != null)
        //    refraction_fogColor = new Color(_refraction_fogColor[0], _refraction_fogColor[1], _refraction_fogColor[2], _refraction_fogColor[3]);
        if (o.refractionAmount != null)
            Refraction = new Color((o.refractionAmount*255 ), (o.refractionAmount*255 ), (o.refractionAmount*255 ), (o.refractionAmount*255 ));

        OCSM = ocsm;

        //if (texmap_metalness_path != null) texmap_metalness = AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(OCSM.TexturesPath, Path.GetFileName(texmap_metalness_path)));
      
        if(o.metalnessMode==0) reflection_metalness = 0;
        else
        reflection_metalness = 1;

        if (metalnessTexmap != null) reflection_metalness = 0;

        levelReflect = o.levelReflect;

        if (o.texmapDiffuse != null) texmap_diffuse = AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(OCSM.TexturesPath, Path.GetFileName(o.texmapDiffuse)));
        if (o.baseTexmap != null) texmap_diffuse = AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(OCSM.TexturesPath, Path.GetFileName(o.baseTexmap)));

        //if (texmap_displacement_path != null) texmap_displacement = AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(OCSM.TexturesPath, Path.GetFileName(texmap_displacement_path)));

        if (o.texmapBump != null) texmap_bump = AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(OCSM.TexturesPath, Path.GetFileName(o.texmapBump)));

        if (o.baseBumpTexmap != null) texmap_bump = AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(OCSM.TexturesPath, Path.GetFileName(o.baseBumpTexmap)));

        if (o.texmapReflect != null) texmap_reflection = AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(OCSM.TexturesPath, Path.GetFileName(o.texmapReflect)));

        if (o.texmapReflectGlossiness != null) texmap_reflectionGlossiness = AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(OCSM.TexturesPath, Path.GetFileName(o.texmapReflectGlossiness)));
        
        if (o.baseRoughnessTexmap != null) texmap_reflectionGlossiness = AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(OCSM.TexturesPath, Path.GetFileName(o.baseRoughnessTexmap)));
        
        if (o.baseIorTexmap != null) baseIorTexmap = AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(OCSM.TexturesPath, Path.GetFileName(o.baseIorTexmap)));
        
        if (o.metalnessTexmap != null) baseIorTexmap = AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(OCSM.TexturesPath, Path.GetFileName(o.metalnessTexmap)));


        if (o.texmapRefraction != null)
            texmap_refraction = AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(OCSM.TexturesPath, Path.GetFileName(o.texmapRefraction)));
        baseIor = o.baseIor;
        //if (texmap_reflectionGlossiness_path != null)
        //    texmap_reflectionGlossiness = AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(OCSM.TexturesPath, Path.GetFileName(texmap_reflectionGlossiness_path)));
         if ( o.basecolor != null)
            Diffuse = new Color(o.basecolor[0] / 255f, o.basecolor[1] / 255f, o.basecolor[2] / 255f, 1 - (o.basecolor[0] / 255f));

         if (o.colorReflect != null)
            Reflection = new Color(o.colorReflect[0] / 255f, o.colorReflect[1] / 255f, o.colorReflect[2] / 255f);

        if(o.baseLevel!=null) baseLevel= o.baseLevel;

        if(o.colorDiffuse!=null)
            Diffuse = new Color(o.colorDiffuse[0] / 255f, o.colorDiffuse[1] / 255f, o.colorDiffuse[2] / 255f, o.colorDiffuse[3]/255f);
       
        Name = o.ocname;
        //if (_brdf_useRoughness == false)
        //    reflection_glossiness = _reflection_glossiness;
        //else

        roughnessMode = o.roughnessMode;
        if(o.roughnessMode==0)
            reflection_glossiness = o.baseRoughness;
        else reflection_glossiness = 1-o.baseRoughness;


        if (texmap_reflectionGlossiness == null) roughnessMode = 0;
        //reflection_fresnel = _reflection_fresnel;

         //if (o.colorRefract != null) Refraction = new Color(o.colorRefract[0], o.colorRefract[1], o.colorRefract[2]);
        reflection_IOR = o.baseIor;

        if (texmap_bump != null)
        {
            ChangeBumpTexturesImportType(texmap_bump);
            return;
        }


        if (mtlType == MtlType.CoronaLegacyMtl)
        {
            reflection_glossiness = o.reflectGlossiness;
            reflection_IOR = o.fresnelIor;
            baseLevel = o.levelDiffuse;
            _Reflection_Level = o.levelReflect;
        }
        //if (texmap_displacement != null) ChangeBumpTexturesImportType(texmap_displacement);

    }
    void ChangeBumpTexturesImportType(Texture2D tex )
    { 
        //get assetpath from texture2d 
        string path = AssetDatabase.GetAssetPath(tex);
        AssetDatabase.ImportAsset(path);
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        //if (importer.textureType == TextureImporterType.NormalMap) return;
        importer.textureType = TextureImporterType.NormalMap;
        importer.isReadable = true;
        AssetDatabase.WriteImportSettingsIfDirty(path);
        importer.SaveAndReimport();

        int z = 0;
        for (int i = 0; i < tex.width; i++)
        {
            if (i > 24) break;
            Color pix = tex.GetPixel(Random.Range(0, tex.width), Random.Range(0, tex.height));
            float res = 4 * (0.5f - pix.r) * (0.5f - pix.r) + 4 * (0.5f - pix.g) * (0.5f - pix.g) + (0.5f - pix.b) * (0.5f - pix.b);

            if (res > 0.95f)
            {
                z++;
            }
        }
        //if (z > 20)
        {
            Debug.Log(tex.name + " Converted From Grayscale");
            TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
            textureImporter.textureType = TextureImporterType.NormalMap;
            textureImporter.convertToNormalmap = true;
            textureImporter.heightmapScale = OCSM.ConvertBumpToNormalDefault;
            //   textureImporter. = 0.025f;
            textureImporter.SaveAndReimport();
        //    return;
        //}
        //else
        //{
        //    Debug.Log(tex.name + " is normalMap");
        }

    }
}
#endif