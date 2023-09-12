#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using System;
using System.Reflection; 
using UnityEngine.Experimental.GlobalIllumination;
using System.Data.Common;

using Object = UnityEngine.Object;
#if UNITY_PIPELINE_HDRP
using UnityEngine.Rendering.HighDefinition;
#endif
[ExecuteInEditMode]
public class OCVolumeManager : MonoBehaviour
{
    [Range (2f,100)]
    [Header(" Rebake probes if Changed ")]
    [SerializeField] float StepSizeBetweenProbes = 8;
    [Range (0,10)]
    [SerializeField] float BlendDistance = 2;
    [Range (0,10)]
    [SerializeField] float intensity = 1f;
    [Range (0.5f,500)]
    [SerializeField] float Size = 24;

    [SerializeField] Vector3 PositionShift = Vector3.zero;
    public static OCVolumeManager instance;
    private OneClickSceneManager _sceneManager;
    public OneClickSceneManager sceneManager
    {
        get
        {
            if (_sceneManager == null)
                _sceneManager = FindObjectOfType<OneClickSceneManager>();
            return _sceneManager;
        }
    }
    List<ReflectionProbe> RPS = new List<ReflectionProbe>();

 
    private void Update()
    {
        calcHash();
    }
    float newhash;
    float hash = 0;
    float hashEdit = 0;
    float newHashEdit;
    void calcHash()
    {
        if (Selection.activeObject != this.gameObject) return;

        newhash = StepSizeBetweenProbes;

        if (Mathf.Abs(newhash - hash) > 0.1f)
        {


            CreateReflectionProbes();
            //BakeAllReflectionProbes();
            hash = newhash;
            return;
        }

        newHashEdit = Size + BlendDistance + intensity+PositionShift.magnitude;

        if(Mathf.Abs(newHashEdit - hashEdit)> 0.1f)
        {
            hashEdit = newHashEdit;
            EditProbes(); 
        }



      
    }
    public Volume volume;
    void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
        name = "OCVolumeAndReflectionProbesManager";
        transform.SetAsFirstSibling();

        newHashEdit = Size + BlendDistance + intensity + PositionShift.magnitude;
        newhash = StepSizeBetweenProbes;

    }
    [ContextMenu("Create HDRP Volume Profile")]
    public void CreateHDRPVolumeProfile()
    {
        if (gameObject.GetComponent<Volume>() != null)
        {
            DestroyImmediate(GetComponent<Volume>());
        }

       

         volume = gameObject.AddComponent<Volume>();
         VolumeProfile v = AssetDatabase.LoadAssetAtPath<VolumeProfile>("Assets/OneClickImport/VolumeProfiles/OCDefaultVolumeProfile.asset");
         volume.profile=v;

        // string volumeAsset = System.IO.Path.Combine(sceneManager.TexturesPath, sceneManager.name + " Volume Profile.asset");
        // AssetDatabase.CreateAsset(new VolumeProfile(), volumeAsset);
        // volume.sharedProfile = AssetDatabase.LoadAssetAtPath<VolumeProfile>(volumeAsset);
        // volume.sharedProfile.name = sceneManager.name + " Volume Profile";
        // VolumeProfile profile = volume.sharedProfile;

        //if (!volume.profile.TryGet<VisualEnvironment>(out var visEnv))
        //{
        //    visEnv = volume.profile.Add<VisualEnvironment>(true);

        //    visEnv.skyType = new NoInterpIntParameter(1);
        //    visEnv.skyType.overrideState = true;
        //    visEnv.skyAmbientMode = new SkyAmbientModeParameter(SkyAmbientMode.Dynamic);
        //    visEnv.skyAmbientMode.overrideState = true;
        //    visEnv.active = true;

        //}

#if UNITY_PIPELINE_HDRP
        if (!volume.profile.TryGet<HDRISky>(out var skyfog))
        {
            skyfog = volume.profile.Add<HDRISky>(true);
        }

        skyfog.active = true;
        skyfog.skyIntensityMode.value = SkyIntensityMode.Exposure;

        skyfog.skyIntensityMode.overrideState = true;
        skyfog.exposure = new FloatParameter(13f);
        skyfog.exposure.overrideState = true;
        skyfog.hdriSky.Override(FindObjectOfType<OneClickSceneManager>().OcEnvMap);
#endif
        // if (!profile.TryGet<Tonemapping>(out var tone))
        // {
        //     tone = profile.Add<Tonemapping>(true);

        //     tone.mode= new TonemappingModeParameter(TonemappingMode.ACES);
        //     tone.active = true;
        //     tone.mode.overrideState=true;

        // }
        // if (!profile.TryGet<Exposure>(out var exp))
        // {
        //     exp = profile.Add<Exposure>(true);
        //     exp.mode = new ExposureModeParameter(ExposureMode.Fixed, false);
        //     exp.fixedExposure = new FloatParameter(13);
        //     exp.fixedExposure.overrideState = true;
        //     //exp.limitMin = new FloatParameter(5);
        //     //exp.limitMax = new FloatParameter(13);
        //     //exp.limitMax.overrideState = true;
        //     //exp.limitMin.overrideState = true;
        //     //exp.histogramPercentages = new FloatRangeParameter(new Vector2(0, 100), 10, 90, true);
        //     exp.active = true;

        // }

        // if (!profile.TryGet<ScreenSpaceReflection>(out var SSR))
        // {
        //     SSR = profile.Add<ScreenSpaceReflection>(true);  

        //     SSR.active = true;

        // }
        //if (!profile.TryGet<Bloom>(out var bloom))
        // {
        //     bloom= profile.Add<Bloom>(true);
        //     bloom.threshold = new MinFloatParameter(1.2f,0,true);
        //     bloom.intensity = new ClampedFloatParameter(0.1f,0f,2,true);
        //     bloom.active = true;

        // }


    }
    public static void SetIndirectResolution(float val)
    {
        SetFloat("m_LightmapEditorSettings.m_Resolution", val);
    }

    public static void SetAmbientOcclusion(float val)
    {
        SetFloat("m_LightmapEditorSettings.m_CompAOExponent", val);
    }

    public static void SetBakedGiEnabled(bool enabled)
    {
        SetBool("m_GISettings.m_EnableBakedLightmaps", enabled);
    }

    public static void SetFinalGatherEnabled(bool enabled)
    {
        SetBool("m_LightmapEditorSettings.m_FinalGather", enabled);
    }

    public static void SetFinalGatherRayCount(int val)
    {
        SetInt("m_LightmapEditorSettings.m_FinalGatherRayCount", val);
    }

    public static void SetFloat(string name, float val)
    {
        ChangeProperty(name, property => property.floatValue = val);
    }

    public static void SetInt(string name, int val)
    {
        ChangeProperty(name, property => property.intValue = val);
    }

    public static void SetBool(string name, bool val)
    {
        ChangeProperty(name, property => property.boolValue = val);
    }

    public static void ChangeProperty(string name, Action<SerializedProperty> changer)
    {
        var lightmapSettings = getLighmapSettings();
        var prop = lightmapSettings.FindProperty(name);
        if (prop != null)
        {
            changer(prop);
            lightmapSettings.ApplyModifiedProperties();
        }
        else Debug.LogError("lighmap property not found: " + name);
    }

    static SerializedObject getLighmapSettings()
    {
        var getLightmapSettingsMethod = typeof(LightmapEditorSettings).GetMethod("GetLightmapSettings", BindingFlags.Static | BindingFlags.NonPublic);
        var lightmapSettings = getLightmapSettingsMethod.Invoke(null, null) as Object;
        return new SerializedObject(lightmapSettings);
    }

    public static void Test()
    {
#if UNITY_EDITOR
        SetBakedGiEnabled(true);
        SetIndirectResolution(1.337f);
        SetAmbientOcclusion(1.337f);
        SetFinalGatherEnabled(true);
        SetFinalGatherRayCount(1337);

        LightingSettings ls;

        UnityEditor.Lightmapping.TryGetLightingSettings(out ls);
        if (ls == null)
        {
            UnityEditor.Lightmapping.lightingSettings=new LightingSettings();
        }
        UnityEditor.Lightmapping.TryGetLightingSettings(out ls);

        // Configure the LightingSettings object
            ls.albedoBoost = 1.1f;
            ls.lightmapper = LightingSettings.Lightmapper.ProgressiveGPU;
            ls.ao = true;
            ls.aoMaxDistance = 0.33f;
            ls.aoExponentDirect = 0.33f;
            ls.aoExponentIndirect = 0.33f;
            if (ls == null) Debug.LogError("ls is null");
            else UnityEditor.Lightmapping.lightingSettings = ls;
        
#endif
    }
    void RenderSettingsSet()
    {
        Test();
    }
    [ContextMenu("CreateReflectionProbes")]
    public void CreateReflectionProbes()
    {
        RenderSettingsSet();
        RPS.Clear();
        while(transform.childCount>0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }


        Bounds bounds = GetLocalBoundsForObject(sceneManager.gameObject);

        float maxB=bounds.extents.x;
        if(maxB<bounds.max.z) maxB=bounds.max.z;
        //StepSizeBetweenProbes = maxB / 10;
        //StepSizeBetweenProbes = Mathf.Clamp(StepSizeBetweenProbes, 0.5f, 100);

        int maxpp = 0;
        for (float i = 1; i < bounds.max.x; i+=StepSizeBetweenProbes)
            for (float j = 1; j < bounds.max.z; j+=StepSizeBetweenProbes)
            {
                float x = Mathf.Lerp(bounds.min.x, bounds.max.x, i / bounds.max.x);
                float z = Mathf.Lerp(bounds.min.z, bounds.max.z, j / bounds.max.z);

                if (maxpp++ > 30) return;
                AddReflectionProbe(new Vector3(x, posy(), z)+ PositionShift, (i+j).ToString());
            }
    }

    float posy()
    {
        float res = 0;
        OCCamerasManager ocmr=FindObjectOfType<OCCamerasManager>();
        if (ocmr != null)
        {
            if (ocmr.transform.childCount > 0)
                res = ocmr.transform.GetChild(0).position.y;
        }
        return res;
    }

    void EditProbes()
    {

        RPS=transform.GetComponentsInChildren<ReflectionProbe>().ToList();



        foreach (var item in RPS)
        {
            item.blendDistance = BlendDistance;
            item.size= new Vector3(Size, 30, Size);
            item.intensity = intensity;


#if UNITY_PIPELINE_HDRP
            var probe = item.GetComponent<HDAdditionalReflectionData>();
            if (probe != null)
            {
                //probe.SetTexture(ProbeSettings.Mode.Custom, AssetDatabase.LoadAssetAtPath<Cubemap>(System.IO.Path.Combine(sceneManager.TexturesPath, "RefProbe_" + _name + ".exr")));
                probe.multiplier = intensity;
                probe.influenceVolume.boxSize = new Vector3( Size, 30, Size);
                probe.influenceVolume.boxBlendDistancePositive = new Vector3(BlendDistance, BlendDistance, BlendDistance);
                probe.influenceVolume.boxBlendDistanceNegative = new Vector3(BlendDistance, BlendDistance, BlendDistance);
            }
#endif
        }
        int s = 0;
        Bounds bounds = GetLocalBoundsForObject(sceneManager.gameObject);
        for (float i = 1; i < bounds.max.x; i += StepSizeBetweenProbes)
            for (float j = 1; j < bounds.max.z; j += StepSizeBetweenProbes)
            {
                float x = Mathf.Lerp(bounds.min.x, bounds.max.x, i / bounds.max.x);
                float z = Mathf.Lerp(bounds.min.z, bounds.max.z, j / bounds.max.z);

                transform.GetChild(s).transform.position=  new Vector3(x, posy(), z) + PositionShift;
                s++;
                if (s > 30) return;
            }
    }
    void AddReflectionProbe(Vector3 pos, string _name)
    {
        Size = (int) StepSizeBetweenProbes * 3;
        BlendDistance = StepSizeBetweenProbes / 2;

        ReflectionProbe RP = new GameObject().AddComponent<ReflectionProbe>();
        RP.transform.position = pos+Vector3.up;
        RP.transform.SetParent(transform);
        RP.name = "ReflectionProbe";
        RP.size = new Vector3(Size, 30, Size);
        RP.intensity = intensity;
        RP.blendDistance = BlendDistance;
        RP.timeSlicingMode = UnityEngine.Rendering.ReflectionProbeTimeSlicingMode.AllFacesAtOnce;
        RP.enabled = true;
        //RP.mode = ReflectionProbeMode.Custom;

//Lightmapping.BakeReflectionProbe(RP, System.IO.Path.Combine(sceneManager.TexturesPath, "RefProbe_" + _name + ".exr"));
//RP.customBakedTexture = AssetDatabase.LoadAssetAtPath<Cubemap>(System.IO.Path.Combine(sceneManager.TexturesPath, "RefProbe_" + _name + ".exr"));


#if UNITY_PIPELINE_HDRP
        
        RPS.Add(RP);

        var probe = RP.GetComponent<HDAdditionalReflectionData>();
        if (probe == null) return;
        //probe.SetTexture(ProbeSettings.Mode.Custom, AssetDatabase.LoadAssetAtPath<Cubemap>(System.IO.Path.Combine(sceneManager.TexturesPath, "RefProbe_" + _name + ".exr")));
        probe.multiplier = intensity;
        probe.influenceVolume.boxSize = new Vector3(Size, 30, Size);

        probe.influenceVolume.boxBlendDistancePositive = new Vector3(BlendDistance, BlendDistance, BlendDistance);
        probe.influenceVolume.boxBlendDistanceNegative = new Vector3(BlendDistance, BlendDistance, BlendDistance);
#endif
        }

#if UNITY_PIPELINE_HDRP
    public void BakeAllReflectionProbes()
    {
        foreach (var RP in RPS)
        {


            UnityEditor.Lightmapping.BakeReflectionProbe(RP,System.IO.Path.Combine(  sceneManager.TexturesPath,RP.name+".exr"));
            RP.customBakedTexture=AssetDatabase.LoadAssetAtPath<Cubemap>(System.IO.Path.Combine(sceneManager.TexturesPath, RP.name + ".exr"));

            var probe = RP.GetComponent<HDAdditionalReflectionData>();
            probe.SetTexture(ProbeSettings.Mode.Custom, AssetDatabase.LoadAssetAtPath<Cubemap>(System.IO.Path.Combine(sceneManager.TexturesPath, RP.name + ".exr")));
            probe.mode = ProbeSettings.Mode.Custom;
        }
    }
#endif
    public Bounds GetLocalBoundsForObject(GameObject go)
    {
        var referenceTransform = go.transform;
        var b = new Bounds(Vector3.zero, Vector3.zero);
        RecurseEncapsulate(referenceTransform, ref b);
        return b;

        void RecurseEncapsulate(Transform child, ref Bounds bounds)
        {
            var mesh = child.GetComponent<MeshFilter>();
            if (mesh)
            {
                if (mesh.sharedMesh == null) return;
                 
                var lsBounds = mesh.sharedMesh.bounds;
                var wsMin = child.TransformPoint(lsBounds.center - lsBounds.extents);
                var wsMax = child.TransformPoint(lsBounds.center + lsBounds.extents);
                bounds.Encapsulate(referenceTransform.InverseTransformPoint(wsMin));
                bounds.Encapsulate(referenceTransform.InverseTransformPoint(wsMax));
            }
            foreach (Transform grandChild in child.transform)
            {
                RecurseEncapsulate(grandChild, ref bounds);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
         
            var bounds = GetLocalBoundsForObject(sceneManager.gameObject);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        UnityEngine.Random.seed = 0;
        Gizmos.color = UnityEngine.Random.ColorHSV();
        Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, 0.33f);
        for (int i = 0; i < transform.childCount; i++)
        {
            Gizmos.DrawCube(transform.GetChild(i).position, Vector3.one * Size); 
        }
    }
}
#endif