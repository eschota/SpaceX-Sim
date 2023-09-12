
using UnityEngine;

[System.Serializable] 
public class OCObject
{
    public string units;
    public bool enabled;
    // Visibility to Camera ToDO
    public bool renderable;
    public bool castShadows;
    public bool receiveShadows; public  int idx;
    public string ocname;
    public string octype;
    public float[] Diffuse;
    public float[] color;// VrayLightMtl
    public float diffuse_roughness;
    public float[] selfIllumination;
    public bool selfIllumination_gi;
    public float selfIllumination_multiplier;
    public float[] Reflection;
    public float reflection_glossiness;
    public float hilight_glossiness;
    public bool reflection_fresnel;
    public float reflection_IOR;
    public bool reflection_lockIOR;
    public float[] Refraction;
    public float refraction_glossiness;
    public float refraction_ior;
    public float[] refraction_fogColor;
    public float refraction_fogMult;
    public float refraction_fogDepth;
    public float refraction_fogBias;
    public int translucency_on;
    public float translucency_amount;
    public float translucency_thickness;
    public float translucency_scatterCoeff;
    public string texmap_diffuse;
    public bool texmap_diffuse_on;
    public string texmap_reflection;
    public bool texmap_reflection_on;
    public float texmap_reflection_multiplier;
    public string texmap_refraction;
    public bool texmap_refraction_on;
    public float texmap_refraction_multiplier;
    public string texmap_bump;
    public bool texmap_bump_on;
    public float texmap_bump_multiplier;
    public string texmap_reflectionGlossiness;
    public bool texmap_reflectionGlossiness_on;
    public float texmap_reflectionGlossiness_multiplier;
    public string texmap_refractionGlossiness;
    public string texmap_refractionGlossiness_on;
    public float texmap_refractionGlossiness_multiplier;
    public string texmap_refractionIOR;
    public bool texmap_refractionIOR_on;
    public float texmap_refractionIOR_multiplier;
    public string texmap_displacement;
    public bool texmap_displacement_on;
    public float texmap_displacement_multiplier;
    public string texmap_opacity;
    public bool texmap_opacity_on;
    public float texmap_opacity_multiplier;
    public string texmap_self_illumination;
    public bool texmap_self_illumination_on;
    public float texmap_self_illumination_multiplier;
    public string texmap;
    public bool brdf_useRoughness;
    public bool visibility;
    public string[] fbxes;
    //metallness
    public string texmap_metalness;
    public string texmap_metalness_on;
    public string texmap_metalness_multiplier;
    public float reflection_metalness;
    // channels
    public int texmap_diffuse_channel;
    public int texmap_bump_channel;
    public int texmap_reflection_channel;
    public int texmap_reflectionGlossiness_channel;    
    public int texmap_texmap_refraction_channel;    
    public int texmap_texmap_refractionGlossiness_channel;    

    // emissive vraylightmtl 
    public float multiplier;
    // vraylight
    public int type;
    public bool on;
    public float color_temperature;
    public float[] position;
    public float size0;
    public float size1;
    public float size2;
 
    public bool DoubleSided;
    public bool invisible;
    public string texmap_path;
    public bool texmap_on;
 

    //vraySun
    public float intensity_multiplier;
    public float[] filter_Color;
    //VrayAreaLight
    public float sizeLength;
    public float sizeWidth;
    public float[] rotation;
    public int targetidx;
    // Cameras
    public bool targeted;
    public float target_distance;
    public float focus_distance;
    public float[] dir;
    public float ISO;
    public float f_number;
    public float shutter_speed;
    public float exposure;
    public float fov;

    //mesh
    public int paridx;
    public int meshidx;
    public int last;
    public float[] nodepos;
    public float[] noderot;
    public float[] nodescale;

    //LM High Poly
    public bool skiplm;

    public string texmapDiffuse;
    public string metalnessTexmap;
    public string baseTexmap;
    public string texmapBump;
    public string baseBumpTexmap;
    public string texmapReflect;
    public string texmapReflectGlossiness;
    public string baseRoughnessTexmap;
    public string texmapRefraction;
    public string texmapRefractGlossiness;
    public int texmapDiffuse_channel;
    public int texmapDiffuse_u_tiles;
    public int texmapDiffuse_v_tiles;
    public int texmapReflectGlossiness_channel;
    //public "texmapDiffuse_uniform" : null, 
    public float[] colorDiffuse;
    public float[] colorReflect;

    public float[] colorRefract;
    public float[] ColorcolorOpacity;
    public float[] colorTranslucency;
    public float [] colorSelfIllum;
    public float levelDiffuse;

    public float levelReflect;
    public float levelRefract;
    public float levelOpacity;
    public float levelTranslucency;
    public float levelSelfIllum;
    public float reflectGlossiness;
    public float refractGlossiness;
    public float fresnelIor;
    // corona
    // #baseColor, --colors
    public string baseIorTexmap;
    public float[] basecolor;
    public float[] opacitycolor;
    public float[] selfIllumcolor;
    public float baseLevel;
    public float baseMapAmount;
    public float baseBumpMapAmount;
    public float opacityLevel;
    public float opacityMapAmount;
    public float refractionAmount;
    public float refractionAmountMapAmount;
    public float selfIllumLevel;
    public float selfIllumMapAmount;

    public float baseRoughness;
    public float baseRoughnessMapAmount;
    public float baseIor; 
    public float ior; 

    public float metalnessMode;
    public int roughnessMode;
    public int alphaMode;
    public int preset;
    public int iorMode;
    public int opacityCutout;

    public int baseTexmap_channel;
    public int baseRoughnessTexmap_channel;
    public int baseBumpTexmap_channel;
    // lights 
    public int shape;
    public float intensity;
    public int colorMode;
    public int intensityUnits;
    //"blackbodyTemp":6500.0,
    public float height;
    public float width;
    public float directionality;
    public float[] colorDirect; 
//"iesOn":true,
//"iesFile":null,
//"twosidedEmission":false,
//"target":null,
//"targeted":false,
    public float lightDistribution;
    public OCObject(int idx, string ocname, string octype)
    {
        this.idx = idx;
        this.octype = ocname;
        this.octype = octype;                   
        
    }
}
