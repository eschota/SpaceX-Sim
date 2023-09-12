#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

#if UNITY_PIPELINE_HDRP
using UnityEngine.Rendering.HighDefinition;
#endif
using UnityEngine.UIElements;
using UnityEngine.Events;
using System.Linq;
using LightType = UnityEngine.LightType;
using static UnityEditor.Progress;
using UnityEditorInternal;

[ExecuteInEditMode]
public class OCLightingManager : MonoBehaviour
{
    public static OCLightingManager instance;
    public List<OCVrayLight> Lights = new List<OCVrayLight>();
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
        name = "OCLightingManager";
        transform.SetAsFirstSibling();
    }
    OneClickSceneManager u;

#if UNITY_PIPELINE_HDRP
HDAdditionalLightData lightData ;
#endif
    [SerializeField] float AreaLightsMult = 1;
    [SerializeField] float SpotLightsMult = 1;
    [SerializeField] float DirectionalLightsMult = 1;
    [SerializeField] float PointLightsMult = 1;
    void OnValidate()
    {
        if(Lights.Count>0)
        foreach (var item in Lights)
        {
            if(item.l.type == LightType.Area) 
                    item.l.intensity = item.multiplier*AreaLightsMult;
            if(item.l.type == LightType.Spot)
                    item.l.intensity = item.multiplier*SpotLightsMult;
            if(item.l.type == LightType.Directional)
                    item.l.intensity = item.multiplier*DirectionalLightsMult;
            if(item.l.type == LightType.Point)
                    item.l.intensity = item.multiplier*PointLightsMult;

             
            
        }
    }
    public void AddLight(OCObject obj, Quaternion rot, Vector3 targetPos)
    {
        u = FindObjectOfType<OneClickSceneManager>();

        

            OCVrayLight O = new GameObject().AddComponent<OCVrayLight>();

        Lights.Add(O);

        O.gameObject.transform.SetParent(transform);
        O.gameObject.name = obj.ocname;
        O.transform.position = targetPos;
        O.transform.rotation = rot;
        O.type = obj.type;
        Light light = O.gameObject.AddComponent<Light>();
        O.l = light;
        O.l.renderMode = LightRenderMode.ForcePixel;
#if UNITY_PIPELINE_HDRP
        lightData = O.gameObject.GetComponent<HDAdditionalLightData>();
        if (lightData == null)         
            lightData = O.gameObject.AddComponent<HDAdditionalLightData>(); 
#endif

        if (obj.octype == "VRayLight" && O.type == 2) CreateVrayPointLight(O, obj, rot, targetPos);//point light

        if (obj.octype == "VRaySun") CreateVraySun(O, obj, rot, targetPos);

        if (obj.octype == "VRayLight" && O.type == 0) CreateVrayPlaneLight(O, obj, rot, targetPos);

        if (obj.octype == "VRayLight" && O.type == 4) CreateVrayDiscLight(O, obj, rot, targetPos);// || obj.shape==2)//disc light
     
        if (obj.octype == "CoronaLight" && obj.shape == 0) CreateCoronaPointLight(O, obj, rot, targetPos);//point light

        if (obj.octype == "CoronaSun") CreateCoronaSun(O, obj, rot, targetPos);

        if (obj.octype == "CoronaLight" && obj.shape == 1) CreateCoronaPlaneLight(O, obj, rot, targetPos);

        if (obj.octype == "CoronaLight" && obj.shape== 2) CreateCoronaDiscLight(O, obj, rot, targetPos);// || obj.shape==2)//disc light

        if (obj.enabled == null || obj.enabled == false) light.enabled = false;
        else light.enabled = true;

    }

    #region VRAY
    void CreateVraySun(OCVrayLight O, OCObject obj, Quaternion rot, Vector3 TargetPos)
    {

        O.l.type = UnityEngine.LightType.Directional;
        O.l.color = new UnityEngine.Color(obj.filter_Color[0] / 255f, obj.filter_Color[1] / 255f, obj.filter_Color[2] / 255f);
        O.multiplier = O.l.intensity = 1 * obj.intensity_multiplier;
        O.l.shadows = LightShadows.Soft;
        O.l.lightmapBakeType = LightmapBakeType.Mixed;
        UnityEngine.RenderSettings.sun = O.l;
#if UNITY_PIPELINE_HDRP
            lightData.intensity =1000* O.multiplier;// 100000 *obj.intensity_multiplier;
            lightData.rayTraceContactShadow = true;
            lightData.SetShadowResolutionLevel(3);
            lightData.EnableShadows(true); 
            lightData.SetShadowResolution(2048);
#endif 
    }
    void CreateVrayPointLight(OCVrayLight O, OCObject obj, Quaternion rot, Vector3 TargetPos)
    {
        O.l.type = UnityEngine.LightType.Point;
        O.l.color = new UnityEngine.Color(obj.color[0] / 255f, obj.color[1] / 255f, obj.color[2] / 255f);
        O.multiplier = O.l.intensity = obj.multiplier;
        O.l.lightmapBakeType = LightmapBakeType.Mixed;
        O.l.shadows = LightShadows.Soft;



#if UNITY_PIPELINE_HDRP
        lightData.intensity = 100 * O.multiplier;// 1000 * obj.multiplier;
            lightData.rayTraceContactShadow = true;
            lightData.SetShadowResolutionLevel(3);
            lightData.EnableShadows(true); 
            lightData.range=2; 
            lightData.SetShadowResolution(2048);
            lightData.lightmapBakeType = LightmapBakeType.Mixed;
#endif
    }
    void CreateVrayPlaneLight(OCVrayLight O, OCObject obj, Quaternion rot, Vector3 TargetPos)
    {
        O.l.type = UnityEngine.LightType.Area;
        O.l.color = new UnityEngine.Color(obj.color[0] / 255f, obj.color[1] / 255f, obj.color[2] / 255f);

        O.l.lightmapBakeType = LightmapBakeType.Mixed;
        O.l.shadows = LightShadows.Soft;
        O.l.transform.Rotate(Vector3.right, 180);

        O.l.areaSize = new Vector2(obj.sizeLength * u.Units, obj.sizeWidth * u.Units);
        O.multiplier=O.l.intensity = 0.0001f * obj.multiplier * 1.5f;// *obj.sizeLength*obj.sizeWidth*u.Units;
        O.l.lightmapBakeType = LightmapBakeType.Mixed;
        O.l.areaSize = new Vector2(obj.sizeLength * u.Units, obj.sizeWidth * u.Units);
        CreateMeshLight(PrimitiveType.Cube, O, obj);

#if UNITY_PIPELINE_HDRP
        lightData.intensity = 1000 * O.multiplier;// obj.multiplier * 1500 * obj.sizeLength * obj.sizeWidth;//*u.Units;
        lightData.lightmapBakeType = LightmapBakeType.Mixed;
        lightData.SetAreaLightSize(new Vector2(obj.sizeLength * u.Units, obj.sizeWidth * u.Units));
#endif
    }
    void CreateVrayDiscLight(OCVrayLight O, OCObject obj, Quaternion rot, Vector3 TargetPos)
    {
        O.l.transform.Rotate(Vector3.right, 180);
        O.l.type = UnityEngine.LightType.Spot;
        O.l.innerSpotAngle = (1 - obj.lightDistribution) * 179;
        O.l.spotAngle = O.l.innerSpotAngle * 2;
        O.l.color = new UnityEngine.Color(obj.color[0] / 255f, obj.color[1] / 255f, obj.color[2] / 255f);
        O.multiplier = O.l.intensity = 1500 * 0.00001f * obj.multiplier * obj.size0;
        O.l.lightmapBakeType = LightmapBakeType.Mixed;
        O.l.shadows = LightShadows.Soft;
        CreateMeshLight(PrimitiveType.Sphere, O, obj);

#if UNITY_PIPELINE_HDRP
        lightData.intensity = 1000 * O.multiplier;// obj.multiplier * 1500 * obj.sizeLength * obj.sizeWidth;//*u.Units;
        lightData.lightmapBakeType = LightmapBakeType.Mixed;
        lightData.SetAreaLightSize(new Vector2(obj.sizeLength * u.Units, obj.sizeWidth * u.Units));

#endif

    }
    #endregion
    #region Corona
    void CreateCoronaSun(OCVrayLight O, OCObject obj, Quaternion rot, Vector3 TargetPos)
    {

        O.l.type = UnityEngine.LightType.Directional;
        O.l.color = new UnityEngine.Color(obj.colorDirect[0] / 255f, obj.colorDirect[1] / 255f, obj.colorDirect[2] / 255f);
        O.multiplier = O.l.intensity = 15 * obj.intensity;
        O.l.shadows = LightShadows.Soft;
        O.l.lightmapBakeType = LightmapBakeType.Mixed;

        //.transform.Rotate(Vector3.right, 180);
        UnityEngine.RenderSettings.sun = O.l;

#if UNITY_PIPELINE_HDRP
            lightData.intensity = 100000*obj.intensity;
            lightData.rayTraceContactShadow = true;
            lightData.SetShadowResolutionLevel(3);
            lightData.EnableShadows(true); 
            lightData.SetShadowResolution(2048);
#endif 
    }
    void CreateCoronaPointLight(OCVrayLight O, OCObject obj, Quaternion rot, Vector3 TargetPos)
    {
        O.l.type = UnityEngine.LightType.Point;
        O.l.color = new UnityEngine.Color(obj.color[0] / 255f, obj.color[1] / 255f, obj.color[2] / 255f);
        O.multiplier = O.l.intensity = 1.5f*obj.intensity;
        O.l.lightmapBakeType = LightmapBakeType.Baked;
        O.l.shadows = LightShadows.Soft;



#if UNITY_PIPELINE_HDRP
            lightData.intensity = 1000* obj.multiplier;
            lightData.rayTraceContactShadow = true;
            lightData.SetShadowResolutionLevel(3);
            lightData.EnableShadows(true); 
            lightData.range=2; 
            lightData.SetShadowResolution(2048);
            lightData.lightmapBakeType = LightmapBakeType.Mixed;
#endif
    }
    void CreateCoronaPlaneLight(OCVrayLight O, OCObject obj, Quaternion rot, Vector3 TargetPos)
    {
        O.l.type = UnityEngine.LightType.Area;
        O.l.color = new UnityEngine.Color(obj.color[0] / 255f, obj.color[1] / 255f, obj.color[2] / 255f);
        O.multiplier = O.l.intensity =1.5f* obj.intensity;
        O.l.lightmapBakeType = LightmapBakeType.Baked;
        O.l.shadows = LightShadows.Soft;
        O.l.transform.Rotate(Vector3.right, 180);

        O.l.areaSize = new Vector2(obj.sizeLength * u.Units, obj.sizeWidth * u.Units);
        O.multiplier = O.l.intensity = obj.multiplier * 1.5f;// *obj.sizeLength*obj.sizeWidth*u.Units;
        O.l.lightmapBakeType = LightmapBakeType.Mixed;
        O.l.areaSize = new Vector2(obj.sizeLength * u.Units, obj.sizeWidth * u.Units);
        O.l.bounceIntensity = 1;

        O.l.areaSize = new Vector2(obj.width * u.Units, obj.height * u.Units);
        O.l.intensity = obj.intensity * 1.5f;// *obj.sizeLength*obj.sizeWidth*u.Units;
         


#if UNITY_PIPELINE_HDRP
        lightData.intensity = obj.intensity * 1500 * obj.sizeLength * obj.sizeWidth;//*u.Units;
        lightData.lightmapBakeType = LightmapBakeType.Mixed;
        lightData.SetAreaLightSize(new Vector2(obj.sizeLength * u.Units, obj.sizeWidth * u.Units));
#endif
    }
    void CreateCoronaDiscLight(OCVrayLight O, OCObject obj, Quaternion rot, Vector3 TargetPos)
    {
        O.l.transform.Rotate(Vector3.right, 180);
        O.l.type = UnityEngine.LightType.Spot;
        O.l.innerSpotAngle = (1 - obj.lightDistribution) * 179;
        O.l.spotAngle = O.l.innerSpotAngle*2;
        O.l.color = new UnityEngine.Color(obj.color[0] / 255f, obj.color[1] / 255f, obj.color[2] / 255f);
        O.multiplier = O.l.intensity = 2* obj.intensity ;
        O.l.lightmapBakeType = LightmapBakeType.Baked;
        O.l.bounceIntensity = 1;
        O.l.shadows = LightShadows.Soft;
#if UNITY_PIPELINE_HDRP
        lightData.intensity = obj.intensity * 1500 * obj.sizeLength * obj.sizeWidth;//*u.Units;
        lightData.lightmapBakeType = LightmapBakeType.Mixed;
        lightData.SetAreaLightSize(new Vector2(obj.sizeLength * u.Units, obj.sizeWidth * u.Units));
#endif

    }
    #endregion

   void CreateMeshLight(PrimitiveType pt,OCVrayLight O,OCObject obj)
    {
        GameObject plane = GameObject.CreatePrimitive(pt);
        plane.transform.SetParent(O.transform);
        plane.transform.localPosition = Vector3.zero;
        plane.transform.localScale = new Vector3(obj.sizeLength * u.Units, 0.01f, obj.sizeWidth * u.Units);
        plane.transform.rotation = O.transform.rotation;
        plane.transform.Rotate(Vector3.right, 90);
        MeshRenderer m = plane.GetComponent<MeshRenderer>();
        m.sharedMaterial = new Material(m.sharedMaterial);
        m.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        m.scaleInLightmap = 0;
        m.sharedMaterial.SetColor("_EmissiveColor", O.l.color * 1.3f);
        m.sharedMaterial.SetColor("_EmissionColor", O.l.color * 1.3f);
        m.sharedMaterial.SetFloat("_EmissiveExposureWeight", 0.5f);
        m.sharedMaterial.EnableKeyword("_EMISSION");
        m.sharedMaterial.globalIlluminationFlags = MaterialGlobalIlluminationFlags.AnyEmissive;
        m.sharedMaterial.SetFloat("_EmissiveMult", O.multiplier * 10000);
        plane.isStatic = true;
        if (pt == PrimitiveType.Cylinder)
        {
            plane.transform.localScale = new Vector3(obj.sizeLength * u.Units, 0.01f, 0.01f);
        }
#if UNITY_PIPELINE_HDRP
        m.sharedMaterial.SetColor("_EmissiveColor", O.l.color * 3.3f);
        m.sharedMaterial.SetFloat("_EmissiveExposureWeight", 0.5f);
        m.sharedMaterial.EnableKeyword("_EMISSION");
        m.sharedMaterial.globalIlluminationFlags = MaterialGlobalIlluminationFlags.AnyEmissive;
#endif



    }
}


#endif