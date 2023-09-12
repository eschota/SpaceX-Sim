#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine; 
using System.IO;
using System.Linq; 
using UnityEditor.SceneManagement;
using UnityEditor.Experimental.GraphView;
using UnityEngine.Rendering;

#if UNITY_PIPELINE_HDRP
using UnityEngine.Rendering.HighDefinition;
#endif
using UnityEditor.VersionControl;

[ExecuteInEditMode]
public class OneClickSceneManager : MonoBehaviour
{
    #region menu
     
    [SerializeField] public float BumpPowerInMaterialDefault = 0.2f;
    [SerializeField] public float ConvertBumpToNormalDefault = 0.0065f;
    #endregion
    #region Variables

    public bool ConvertBumpToNormals = true;
    public bool BakeLightingAfterImport = true;
    public bool _ImportLights = true;
    public bool _ImportCameras = true;
    public bool _ImportEnvironment = true;

    [HideInInspector] public float Units = 1;
    [HideInInspector][SerializeField] public TextAsset SceneJSON;
    [HideInInspector][SerializeField] public List<OCVrayLightMaterial> VrayLightMaterials = new List<OCVrayLightMaterial>();
    [HideInInspector][SerializeField] public List<OCVrayMtl> VrayMtls = new List<OCVrayMtl>();
    [HideInInspector][SerializeField] public List<OCMesh> OCMeshes = new List<OCMesh>();
    [HideInInspector][SerializeField] public List<GameObject> GameObjects = new List<GameObject>();
    [HideInInspector][SerializeField] public List<string> fbxes = new List<string>();
    [HideInInspector][SerializeField] List<MeshRenderer> meshRenderers = new List<MeshRenderer>();
    [HideInInspector][SerializeField] public List<Material> materials = new List<Material>();
    [HideInInspector][SerializeField] List<OCVrayLight> OCVrayLights = new List<OCVrayLight>();
    [HideInInspector][SerializeField] List<string> ExtractedMaterials = new List<string>();

    [HideInInspector][SerializeField] List<OCObject> OCCamerasobjs = new List<OCObject>();
    [HideInInspector][SerializeField] List<OCObject> OCVrayLightsobjs = new List<OCObject>();
    Objs JSONItems;
    [HideInInspector] public string TexturesPath;
    [HideInInspector][SerializeField] public string AssetFullPath;
    [HideInInspector] List<OCFBX> GOS = new List<OCFBX>();
    string MeshesPath;
    string MaterialsPath;

    [HideInInspector] public static OneClickSceneManager instance;
    [HideInInspector][SerializeField] public bool _isReady = false;
    [HideInInspector] public Texture2D envMap;
    [HideInInspector] public Cubemap OcEnvMap;
    #endregion
    #region Fields
    private Shader _vrayMtl;
    public Shader VrayMtl
    {
        get
        {
            if (_vrayMtl == null)
            {
#if UNITY_PIPELINE_HDRP
                _vrayMtl = Shader.Find("Shader Graphs/OCVrayMtlHDRP");

#else
                _vrayMtl = Shader.Find("Shader Graphs/OCVrayMtlURP");
#endif
            }
            return _vrayMtl;
        }
    } private Shader _CoronaMtl;
    public Shader CoronaMtl
    {
        get
        {
            if (_CoronaMtl == null)
            {
#if UNITY_PIPELINE_HDRP
                _CoronaMtl = Shader.Find("Shader Graphs/OCCoronaMtlHDRP");

#else
                _CoronaMtl = Shader.Find("Shader Graphs/OCCoronaMtlURP");
#endif
            }
            return _CoronaMtl;
        }
    }
    private Shader _CoronaMtlLegacy;
    public Shader CoronaMtlLegacy
    {
        get
        {
            if (_CoronaMtlLegacy == null)
            {
#if UNITY_PIPELINE_HDRP
                _CoronaMtlLegacy = Shader.Find("Shader Graphs/OCCoronaLegacyMtlHDRP");

#else
                _CoronaMtlLegacy = Shader.Find("Shader Graphs/OCCoronaLegacyMtlURP");
#endif
            }
            return _CoronaMtlLegacy;
        }
    }

    private Shader _vrayMtlRefraction;
    public Shader VrayMtlRefraction
    {
        get
        {
            if (_vrayMtlRefraction == null)
            {
#if UNITY_PIPELINE_HDRP

                _vrayMtlRefraction = Shader.Find("Universal Render Pipeline/Lit");
#else
                _vrayMtlRefraction = Shader.Find("Universal Render Pipeline/Lit");

#endif
            }
            return _vrayMtlRefraction;
        }
    }
    #endregion
    #region IniScene
    void Update()
    {
        if (_isReady == false)
        {
            _isReady = true;
            ImportScene3dsmax();

        }
    }
    void Awake()
    {
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    #endregion
    #region ImportScene3dsmax
    [ContextMenu("ImportScene3dsmax")]
    public void ImportScene3dsmax()
    {
        SetHDRPASset();

        SetImportPaths();

        ResetAllData();


        ReadJSON();

        ExtractMaterials();

        PlaceMeshes();

        if (_ImportCameras) ImportVrayCameras();

        if (_ImportLights) ImportLights();


        collectMaterials();

        SetMaterialParams();

        ImportEnvironmentAndVolume();

        SetHighPolyIgnoreLM();

        SetIgnoreLMForRefractObjects();
        DeleteMDLRootObjects();

        if (BakeLightingAfterImport)
        {
            Lightmapping.BakeAsync();

        }

    }


    public void DeleteMDLRootObjects()
    {
        int ChildCount = transform.childCount;
        for (int i = 0; i < ChildCount; i++)
        {
            while (transform.GetChild(i).childCount>0 )
            {
                transform.GetChild(i).GetChild(0).SetParent(transform);
            }
        }
        GameObject.DestroyImmediate(transform.GetChild(0).gameObject);
        GameObject.DestroyImmediate(transform.GetChild(0).gameObject);
    }
    [ContextMenu("SetHDRPASSet")]
    public void SetHDRPASset()
    {
#if UNITY_PIPELINE_HDRP
        RenderPipelineAsset defaultRenderPipelineAsset ;

        string[] guids = AssetDatabase.FindAssets("OneClickHDRPDefaultSettings");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            RenderPipelineAsset material = AssetDatabase.LoadAssetAtPath<RenderPipelineAsset>(path);
            if (material != null && material.name == "OneClickHDRPDefaultSettings")
            {
                GraphicsSettings.defaultRenderPipeline = material;
                QualitySettings.renderPipeline = material;

            }

        }

    
#endif 
}

private void ImportEnvironmentAndVolume()
    {
        if (_ImportEnvironment)
        {
            envMap = AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(TexturesPath, "ocbake_env.hdr"));
            if (envMap != null)
            {
                Debug.Log("No Environment map, skip...");  
            
            string path = AssetDatabase.GetAssetPath(envMap);
            AssetDatabase.ImportAsset(path);
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            TextureImporter textureImporter = AssetImporter.GetAtPath(Path.Combine(TexturesPath, "ocbake_env.hdr")) as TextureImporter;
            textureImporter.textureShape = TextureImporterShape.TextureCube;
            textureImporter.SaveAndReimport();
            OcEnvMap = AssetDatabase.LoadAssetAtPath<Cubemap>(Path.Combine(TexturesPath, "ocbake_env.hdr"));
            }
        }
        if (FindObjectOfType<OCVolumeManager>() != null)
        {
            GameObject OCLM = FindObjectOfType<OCVolumeManager>().gameObject;
            if (OCLM != null) DestroyImmediate(OCLM.gameObject as GameObject);
        }
        if (OCVolumeManager.instance == null)
        {
            OCVolumeManager.instance = new GameObject("OCVolumeManager").AddComponent<OCVolumeManager>();

            OCVolumeManager.instance.CreateHDRPVolumeProfile();
            OCVolumeManager.instance.CreateReflectionProbes();

        }
    }
    #endregion

    void ExtractMaterials()
    {
        string dir = Path.GetDirectoryName(MaterialsPath);
         

        if (!AssetDatabase.IsValidFolder(MaterialsPath))
        AssetDatabase.CreateFolder(dir, Path.GetFileName(MaterialsPath));
       // AssetDatabase.CreateFolder("", MaterialsPath);


             foreach (string item in fbxes)
            if(item!="")
        {
                string p = Path.Combine(MeshesPath, Path.GetFileName(item + ".fbx"));

                ExtractMaterials(p, MaterialsPath);                         
        } 
            
         
    }
    void PlaceMeshes()
    {
        foreach (string item in fbxes) 
            if (item != "")
            {
            string p = Path.Combine(MeshesPath, Path.GetFileName(item + ".fbx"));

            GameObject GO = AssetDatabase.LoadAssetAtPath<GameObject>(p) as GameObject;
                if (GO != null)
                    {
                        GameObjects.Add(Instantiate(GO, transform));
                    if (GameObjects.Last().GetComponent<MeshFilter>() != null) GameObjects.Last().name= GameObjects.Last().GetComponent<MeshFilter>().sharedMesh.name;
                    }
            }
        GOS = new List<OCFBX>();
        foreach (GameObject item in GameObjects)
            GOS.AddRange(item.GetComponentsInChildren<OCFBX>());

            foreach (OCFBX g in GOS)
        {
            OCMesh o= OCMeshes.Find(X => X.idx == g.idx);
            if (o != null)
            {
               
                
                // Create Instances Objects
                if (g.GetComponent<MeshFilter>() != null)
                    foreach (var item in OCMeshes)
                {
                    if (item.meshidx == o.meshidx && o.idx != item.idx && item.meshidx != 0)
                    {
                            OCFBX Dummy = GOS.Find(X => X.idx == item.idx);
                            try
                            {
                                MeshFilter m = Dummy.gameObject.GetComponent < MeshFilter > ();
                                if (m==null)
                                m = Dummy.gameObject.AddComponent<MeshFilter>();
                                m.sharedMesh = g.GetComponent<MeshFilter>().sharedMesh;

                                MeshRenderer mr = Dummy.gameObject.AddComponent<MeshRenderer>();
                                mr.sharedMaterials = g.GetComponent<MeshRenderer>().sharedMaterials;
                            }
                            catch
                            {
                               // Debug.Log("ERROR MESH " + g.name);
                            }
                            
                        }
                }
                MeshRenderer mrg = g.gameObject.GetComponent<MeshRenderer>();
                if (mrg != null)
                {
                    if (o.castShadows)
                        mrg.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
                    else
                        mrg.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

                    mrg.receiveShadows = o.receiveShadows;
                }
            }
        }
    }

    
    void SetMaterialParams()
    {

      
        foreach (var m in materials)
        {
            OCVrayMtl item = VrayMtls.Find(X => X.Name == m.name);
            if (item != null)
            {
                if (item.mtlType == OCVrayMtl.MtlType.Vray)
                {
                    m.shader = VrayMtl;
                    if (item.reflection_fresnel == true) m.SetInt("_Fresnel", 1); else m.SetInt("_Fresnel", 0);
                }
                else 
                { 
                    m.shader = CoronaMtl;
                    if (item.mtlType == OCVrayMtl.MtlType.CoronaLegacyMtl) m.shader = CoronaMtlLegacy;
                    if (item.baseIorTexmap == null) m.SetInt("_Fresnel", 1);
                    else m.SetInt("_Fresnel", 0);

                    m.SetFloat("_Reflection_metalness", item.reflection_metalness); 
                    m.SetInt("_roughnessMode", item.roughnessMode); 
                    m.SetFloat("_Base_Layer_Level", item.baseLevel);
                    m.SetFloat("_Reflection_Level", item._Reflection_Level);
                } 
                SetMainMaps(item, m);

                SetEmission(item, m);

                SetRefraction(item, m);

                
 
            }
            OCVrayLightMaterial vrl = VrayLightMaterials.Find(X => X.Name == m.name);
            if (vrl != null)
            {
                if (vrl.texmap != null) 
                    {
                        m.SetTexture("_EmissiveTex", vrl.texmap);
                        m.SetColor("_EmissiveColor",   Color.white);
                        m.SetFloat("_EmissiveMult",10000);
                    }
                    else
                    {
                        m.SetFloat("_EmissiveMult",1);
                        m.SetColor("_EmissiveColor", vrl.multiplier * vrl.Diffuse);
                    }
                m.SetFloat("_EmissiveExposureWeight", 0.5f);
                m.EnableKeyword("_EMISSION");
                m.globalIlluminationFlags = MaterialGlobalIlluminationFlags.AnyEmissive;
                Canvas.ForceUpdateCanvases();
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                EditorUtility.SetDirty(m);
             }
        }
       
    }

    private void BakeHDRPLitMaskAndSetParams(OCVrayMtl item, Material m)
    {
        m.shader = Shader.Find("Vray2PBRMasks");
        BakeMaskRenderTexture(item, m);
        if (item.hDRPMask != null)
        {
            m.SetTexture("_MaskMap", item.hDRPMask);
        }
        m.shader = Shader.Find("HDRP/Lit");
    }

    private void SetEmission(OCVrayMtl item, Material m)
    {
        if (item.selfIllumination_gi == true)
        {  
             
            m.SetColor("_EmissiveColor",  item.selfIllumination * item.selfIllumination_multiplier);
            m.SetFloat("_EmissiveExposureWeight", 0.5f); 
            m.EnableKeyword("_EMISSION");  
            m.globalIlluminationFlags = MaterialGlobalIlluminationFlags.AnyEmissive; 
            Canvas.ForceUpdateCanvases();
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            EditorUtility.SetDirty(m);
           // Debug.Log("Set Emi for " + m.name + " to " + item.selfIllumination +  " color " + item.selfIllumination + " " );
        }
    }

    private void SetMainMaps(OCVrayMtl item, Material m)
    {
        if (item.Name == "mat_122_5")
        {
            ;
        }

        m.SetColor("_Color", item.Diffuse);
        m.SetColor("_BaseColor", item.Diffuse);
        m.SetColor("_SpecColor", item.Reflection);
        m.SetFloat("_Metallic", item.Reflection.r);
        m.SetFloat("_Smoothness", item.reflection_glossiness);
        m.SetFloat("_Glossiness", item.reflection_glossiness);
        m.SetFloat("_GlossyReflections", item.reflection_glossiness);
        m.SetFloat("_Reflection_metalness", item.reflection_metalness);
        m.SetFloat("_IOR", item.reflection_IOR);
        if(item.brdf_useRoughness==true)
        m.SetInt("_brdf_useRoughness", 1);
        else 
        m.SetInt("_brdf_useRoughness", 0);
        if (item.texmap_displacement != null)
        {
            m.SetTexture("_BumpMap", item.texmap_displacement);
            m.SetTexture("_NormalMap", item.texmap_displacement);
            m.SetInt("_BumpMapUV", item.texmap_displacement_channel - 1);

        }
        if (item.texmap_bump != null)
        {
            try
            {
                m.SetTexture("_BumpMap", item.texmap_bump);
            }
            catch { }
            m.SetTexture("_NormalMap", item.texmap_bump);
            m.SetInt("_BumpMapUV", item.texmap_bump_channel - 1);
            m.SetFloat("_BumpStrength", BumpPowerInMaterialDefault);
        }



        if (item.texmap_metalness != null)
        {
            m.SetTexture("_Reflection_metalness_tex", item.texmap_metalness);
        }
        if (item.texmap_diffuse != null)
        {
            m.SetTexture("_BaseColorMap", item.texmap_diffuse);
            m.SetTexture("_BaseMap", item.texmap_diffuse);
            m.SetInt("_BaseMapUV", item.texmap_diffuse_channel - 1);
            m.SetInt("_UVBase", item.texmap_diffuse_channel - 1);
            m.SetColor("_Color", Color.white);
            m.SetColor("_BaseColor", Color.white);
        }
        if (item.texmap_reflectionGlossiness != null)
        {
            m.SetTexture("_SmoothMap", item.texmap_reflectionGlossiness);
            m.SetInt("_SmoothMapUV", item.texmap_reflectionGlossiness_channel - 1);
            m.SetFloat("_Smoothness", 1);
            m.SetFloat("_Glossiness", 1);
        }
        if (item.texmap_reflection != null)
        {
            m.SetTexture("_SpecGlossMap", item.texmap_reflection);
            m.SetInt("_SpecGlossMapUV", item.texmap_reflection_channel - 1);
            m.SetColor("_SpecColor", Color.white);
            m.SetFloat("_Metallic", 1);
        }
    }

    public void BakeMaskRenderTexture(OCVrayMtl mtl, Material mat)
    {
        
        //RenderTexture rt = new RenderTexture(512, 512, 24);
        //Material bakemat = mat;
        //RenderTexture.active = rt; 
        //Graphics.Blit(null, rt, bakemat);

        ////RenderTexture.active = rtdest;
        //Texture2D frame = new Texture2D(512, 512);
        //frame.ReadPixels(new Rect(0, 0, 512, 512), 0, 0, false);
        //frame.Apply();
        //byte[] bytes = frame.EncodeToPNG();
        //string p = Path.Combine(TexturesPath, mat.name + "_HDRP_MASK.png");

        //FileStream file = File.Open(p, FileMode.Create);
        //BinaryWriter binary = new BinaryWriter(file);
        //binary.Write(bytes);
        //file.Close();

        //AssetDatabase.Refresh();
        //AssetDatabase.ImportAsset(p);
        //Texture2D hDRPMask = AssetDatabase.LoadAssetAtPath<Texture2D>(p);
        //Debug.Log("HDRP Mask Created: " + hDRPMask);
        //RenderTexture.active = null;
        //mat.SetTexture("_MaskMap", hDRPMask);


    } 
    public void ExtractMaterials(string assetPath, string destinationPath)
    {
        HashSet<string> hashSet = new HashSet<string>();
        IEnumerable<Object> enumerable = from x in AssetDatabase.LoadAllAssetsAtPath(assetPath)
                                         where x.GetType() == typeof(Material)
                                         select x;
        foreach (Object item in enumerable)
        {
            string path = System.IO.Path.Combine(destinationPath, item.name) + ".mat";

            ExtractedMaterials.Add(path);
            if (ExtractedMaterials.Contains(path))
            {
            //    continue;
            }
            //path = AssetDatabase.GenerateUniqueAssetPath(path);
           
            string value = AssetDatabase.ExtractAsset(item, path);
            if (string.IsNullOrEmpty(value))
            {
                hashSet.Add(assetPath);
            }
        }

        foreach (string item2 in hashSet)
        {
            AssetDatabase.WriteImportSettingsIfDirty(item2);
            AssetDatabase.ImportAsset(item2, ImportAssetOptions.ForceUpdate);
        }
    }
    void ImportLights()
    {
        foreach (var l in FindObjectsOfType<Light>())
        {

#if UNITY_PIPELINE_HDRP

            HDAdditionalLightData lightData = l.gameObject.GetComponent<HDAdditionalLightData>();
            if (lightData == null)

                lightData = l.gameObject.AddComponent<HDAdditionalLightData>();
            lightData.intensity *= 1000;
#endif
        }


        foreach (var item in OCVrayLightsobjs)
        {
            if (OCLightingManager.instance == null)
            {
                OCLightingManager.instance = new GameObject("OCLightingManager").AddComponent<OCLightingManager>();
            }
            Vector3 pos = Vector3.one * 1000000000;
            Quaternion rot = Quaternion.identity;
            if (item.octype == "VRaySun"|| item.octype== "CoronaSun")
            {
                 

                
                OCFBX target = GOS.Find(X => X.idx == item.idx);
                if (target != null)
                {
                    pos = new Vector3(target.transform.position[0], target.transform.position[1], target.transform.position[2]);
                    OCFBX targetRot = GOS.Find(X => X.idx == item.targetidx);

                    rot = Quaternion.LookRotation(-pos + targetRot.transform.position);
                    //rot = targetRot.transform.rotation;
                }
                else
                if (item.targetidx != 0)
                {
                    target = GOS.Find(X => X.idx == item.targetidx);
                    if (target != null)
                    {
                        Debug.Log(item.targetidx + ": " + item.ocname);
                        pos = new Vector3(target.transform.position[0], target.transform.position[1], target.transform.position[2]);
                        rot = target.transform.rotation;
                    }
                    else Debug.LogError("Error position of sun");
                }
            }
            if (item.octype == "VRayLight"|| item.octype== "CoronaLight")//&& item.targetidx!=0
            {
                OCFBX target = GOS.Find(X => X.idx == item.idx);
                     if(target!=null  )
                {
                        pos = new Vector3(target.transform.position[0], target.transform.position[1], target.transform.position[2]);
                        rot = target.transform.rotation;

                } else
                     if(item.targetidx != 0)
                {
                    target = GOS.Find(X => X.idx == item.targetidx);
                    pos = new Vector3(target.transform.position[0], target.transform.position[1], target.transform.position[2]);
                    rot = target.transform.rotation;
                }

            } 
            FindObjectOfType<OCLightingManager>().AddLight(item,rot, pos);

           

            
        }
    }
     
    
    void SetImportPaths()
    {
        string path = AssetFullPath;
        int k = path.IndexOf("Assets");
        path = path.Substring(k, path.Length-k); 

        //if (path.Substring(0, 6) != "Assets") path = "Assets\\" + path + ".fbx";
        TexturesPath = path.Substring(0, path.Length - 10) + "_textures";
        MeshesPath = path.Substring(0, path.Length - 10) + "_meshes";
        MaterialsPath = path.Substring(0, path.Length - 10) + "_materials";
        SceneJSON = AssetDatabase.LoadAssetAtPath<TextAsset>(path.Substring(0, path.Length - 10) + ".json");
    }
    void ResetAllData()
    {
        VrayLightMaterials.Clear();VrayMtls.Clear();OCMeshes.Clear();fbxes.Clear();GameObjects.Clear();meshRenderers.Clear();materials.Clear();
        OCVrayLights.Clear();OCVrayLightsobjs.Clear();OCCamerasobjs.Clear();ExtractedMaterials.Clear();GOS.Clear();
        try
        {
            while (transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }

        }
        catch { }
        if (FindObjectOfType<OCLightingManager>() != null)
        {
            GameObject OCLM = FindObjectOfType<OCLightingManager>().gameObject;
            if (OCLM != null) DestroyImmediate(OCLM.gameObject as GameObject);
        }
        if (FindObjectOfType<OCCamerasManager>() != null)
        {
            GameObject OCLM = FindObjectOfType<OCCamerasManager>().gameObject;
            if (OCLM != null) DestroyImmediate(OCLM.gameObject as GameObject);
        }
       
    }
    void ReadJSON()
    {
        JSONItems = JsonUtility.FromJson<Objs>(SceneJSON.text);

        foreach (OCObject item in JSONItems.objs)
        { 
                if (item.idx == 0)
            {
                if (item.octype == "VRayMtl" || item.octype == "SimpleMtl" || item.octype == "VRayBlendMtl" )
                {
                    //if (item.selfIllumination_gi == true && (item.selfIllumination[0] + item.selfIllumination[1] + item.selfIllumination[2]) > 1)
                    //{
                    //    VrayLightMaterials.Add(new OCVrayLightMaterial(TexturesPath, item.selfIllumination, item.ocname,
                    //    item.texmap_diffuse, ));
                    //    continue;
                    //}

                    VrayMtls.Add(new OCVrayMtl(item, this, item.Diffuse, item.ocname, item.Reflection, item.reflection_glossiness,
                        item.texmap_diffuse, item.reflection_fresnel
                        , item.Refraction,
                        item.texmap_displacement,
                        item.texmap_bump,
                        item.texmap_reflection,
                        item.texmap_refraction,
                        item.texmap_reflectionGlossiness,
                        item.reflection_IOR,
                        item.texmap_diffuse_channel,
                        item.texmap_bump_channel,
                        item.texmap_bump_channel,
                        item.texmap_reflection_channel,
                        item.texmap_texmap_refraction_channel,
                        item.texmap_reflectionGlossiness_channel,
                        item.refraction_fogColor,
                        item.selfIllumination,
                        item.selfIllumination_gi,
                        item.texmap_metalness,
                        item.reflection_metalness,
                        item.selfIllumination_multiplier,
                        item.brdf_useRoughness
                        )); ;
                }
                else if (item.octype == "_CoronaPhysicalMtl" || item.octype == "CoronaLegacyMtl")
                { 
                    VrayMtls.Add(new OCVrayMtl(this, item
                        )); ;
                }
                else
                if (item.octype == "VRayLightMtl" )
                {
                    VrayLightMaterials.Add(new OCVrayLightMaterial(TexturesPath, item.color, item.ocname,
                       item.texmap_diffuse, item.multiplier));
                }
                else
                    if (item.octype == "fbxes")
                {
                    fbxes.AddRange(item.fbxes);
                }
                else
                if (item.octype == "fake")
                {
                    if (item.units == "meters") Units = 1;
                    else
                    if (item.units == "centimeters") Units = 0.01f;
                    else
                    if (item.units == "millimeters") Units = 0.001f;

                }

            }
            else
                if (item.octype == "VRayPhysicalCamera") OCCamerasobjs.Add(item);
            
            else
            if (item.octype == "VRayLight" || item.octype == "VRaySun"|| item.octype== "CoronaLight"|| item.octype == "CoronaSun") OCVrayLightsobjs.Add(item);
             
            else
            {
                OCMeshes.Add(new OCMesh(
                        item.ocname,
                        item.castShadows,
                        item.receiveShadows,
                        item.renderable,
                        item.visibility,
                        item.idx,
                        item.paridx,
                        item.meshidx,
                        item.octype,
                        item.nodepos,
                        item.noderot,
                        item.nodescale,
                        item.position,
                        item.rotation,
                        item.dir,
                        item.last,
                        item.skiplm
                        
                    ));
            }
        }
       // VrayMtls.OrderBy(X => X.Name);
    }
    void SetHighPolyIgnoreLM()
    {
        foreach (OCMesh g in OCMeshes)
        {
            OCFBX o = GOS.Find(X => X.idx == g.idx);
            if (o != null)
            {
                if(g.skiplm==true)
                {
                    o.gameObject.isStatic = false;
                }
            }
        }
    }
            void ImportVrayCameras()
    {
        foreach (OCObject item in OCCamerasobjs)
        {
            if (OCCamerasManager.instance == null)
            {
                OCCamerasManager.instance = new GameObject("OCCamerasManager").AddComponent<OCCamerasManager>();
            }
            FindObjectOfType<OCCamerasManager>().AddCamera(item);
        }
    }
    void collectMaterials()
    {
        List<Material> exmats = new List<Material>();
        exmats.Clear();
        AssetDatabase.Refresh();
        meshRenderers.Clear();
        string[] aMaterialFiles = Directory.GetFiles(MaterialsPath, "*.mat", SearchOption.TopDirectoryOnly);
        foreach (string matFile in aMaterialFiles)
        {
            exmats.Add((Material)AssetDatabase.LoadAssetAtPath(matFile, typeof(Material)));
        }

        materials.Clear();
        materials.AddRange(exmats);

        for (int j = 0; j < GameObjects.Count; j++)
        {
            List<MeshRenderer> MRs = GameObjects[j].GetComponentsInChildren<MeshRenderer>().ToList();
            meshRenderers.AddRange(MRs);
            for (int i = 0; i < MRs.Count; i++)
            {
                Material[] temp = MRs[i].sharedMaterials;
                for (int k = 0; k < MRs[i].sharedMaterials.Length; k++)
                {
                    Material m = exmats.Find(X => X.name == MRs[i].sharedMaterials[k].name);
                    if (m == null) continue;

                    temp[k] = m;


                    MRs[i].gameObject.isStatic = true;
                    // EditorUtility.SetDirty(MRs[i].sharedMaterials[k]);
                }
                MRs[i].sharedMaterials = temp;

            }
        }
        materials.OrderBy(X => X.name);
    }


    private Material _refractMtl;
    public Material refractMtl
    {
        get
        {
            if(_refractMtl == null)
            {
                string[] guids = AssetDatabase.FindAssets("t:Material");

                foreach (string guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    Material material = AssetDatabase.LoadAssetAtPath<Material>(path);
#if UNITY_PIPELINE_HDRP
                    if (material != null && material.name == "GlassHDRP")
                    {
                        return material;
                    }
#endif
                    if (material != null && material.name == "GlassURP")
                    {
                        return material;
                    }
                }

            }
                return _refractMtl;
        }
    }
    private void SetRefraction(OCVrayMtl item, Material m)
    {
       

        if ((item.Refraction.r + item.Refraction.g + item.Refraction.b) / 3f > 150f || item.texmap_refraction != null)
        {   
            if (item.mtlType == OCVrayMtl.MtlType.CoronaPhysical)
            {
                m.SetColor("_Color", Color.white);
                m.SetColor("_BaseColor", Color.white);
            }

            //var getKeywordsMethod = typeof(ShaderUtil).GetMethod("GetShaderGlobalKeywords", BindingFlags.Static | BindingFlags.NonPublic);
            //string[] keywords = (string[])getKeywordsMethod.Invoke(null, new object[] { m.shader });
            ////Debug.Log(m.name + " set refraction: " + item.Refraction);
            m.SetFloat("_Metallic", item.reflection_metalness);
            //if (item.texmap_refraction != null)
            m.SetFloat("_Mode", 3);
            m.SetOverrideTag("Queue", "Transparent");
            m.SetOverrideTag("_SURFACE_TYPE_TRANSPARENT", "enabled");
            //m.SetOverrideTag("TRANSPARENT_COLOR_SHADOW");
            m.SetOverrideTag("RenderType", "Transparent");
            m.SetOverrideTag("SurfaceType", "Transparent");
            m.SetFloat("_Surface", 1);
            m.SetFloat("_Blend", 1);
            // Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
            m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            m.SetInt("_ZWrite", 0);
            m.SetInt("_ZWrite", 1);
            m.EnableKeyword("_ALPHATEST_ON");
            m.DisableKeyword("_ALPHABLEND_ON");
            m.DisableKeyword("_ALPHAPREMULTIPLY_ON"); 
            //m.renderQueue = 3000;
            m.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
            m.SetOverrideTag("RenderType", "TransparentCutout");

            m.EnableKeyword("_BLENDMODE_ALPHA");
            m.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            m.EnableKeyword("_ENABLE_FOG_ON_TRANSPARENT");
            m.DisableKeyword("_BLENDMODE_ADD");
            m.DisableKeyword("_BLENDMODE_PRE_MULTIPLY");

            m.SetFloat(Shader.PropertyToID("__surface"), 1.0f);

            m.SetFloat(Shader.PropertyToID("__blend"), 0.0f);
            m.SetFloat("_SurfaceType", 1);
            m.SetFloat("_RenderQueueType", 5);
            m.SetFloat("_BlendMode", 0);
            m.SetFloat("_AlphaCutoffEnable", 1);
            m.SetFloat("_AlphaCutoffShadow", 0.9f);
            m.SetFloat("_AlphaCutoff", 0);
            m.SetFloat("_UseShadowThreshold", 1);
            m.SetFloat("_SrcBlend", 1f);
            m.SetFloat("_DstBlend", 10f);
            m.SetFloat("_AlphaSrcBlend", 1f);
            m.SetFloat("_AlphaDstBlend", 10f);
            m.SetFloat("_ZTestDepthEqualForOpaque", 4f);

            m.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
            Canvas.ForceUpdateCanvases();
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            EditorUtility.SetDirty(m);
        }
        if (item.texmap_diffuse != null)
        {
            m.SetTexture("_BaseColorMap", item.texmap_diffuse);
            m.SetTexture("_BaseMap", item.texmap_diffuse);
            m.SetInt("_BaseMapUV", item.texmap_diffuse_channel - 1);
            m.SetInt("_UVBase", item.texmap_diffuse_channel - 1);
            m.SetColor("_Color", Color.white);
            m.SetColor("_BaseColor", Color.white);
        }
    
    }
    void SetIgnoreLMForRefractObjects(  )
    {
        foreach (var item in meshRenderers)
        {
            if (item.sharedMaterial.HasProperty("_Mode"))
            if (item.sharedMaterial.GetFloat("_Mode") == 3)
            {
                item.sharedMaterial = refractMtl;
                item.shadowCastingMode=UnityEngine.Rendering.ShadowCastingMode.Off;
                item.scaleInLightmap = 0;
                item.receiveGI = 0;
            }
        }  
    }
}
#endif