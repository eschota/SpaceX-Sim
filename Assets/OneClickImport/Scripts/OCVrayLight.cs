using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

#if UNITY_PIPELINE_HDRP
using UnityEngine.Rendering.HighDefinition;
#endif
public class OCVrayLight : MonoBehaviour
{
    [HideInInspector] public string ocname;
    [HideInInspector] public int type;
    [HideInInspector] public bool on;
    [HideInInspector] public float color_temperature;
    [HideInInspector] public Color color;
    [HideInInspector] public float multiplier;
    [HideInInspector] public float size0;
    [HideInInspector] public float size1;
    [HideInInspector] public float size2;
    [HideInInspector] public float sizeLenght;
    [HideInInspector] public float sizeWidth;
    [HideInInspector] public bool castShadows;
    [HideInInspector] public bool DoubleSided;
    [HideInInspector] public bool invisible;
    [HideInInspector] public Texture2D texmap;
    [HideInInspector] public bool texmap_on;
    [HideInInspector] public Vector3 position;
    [HideInInspector] public Vector3 scale;
    [HideInInspector] public Vector3 rotation;
    [HideInInspector] public Light l;
    
    // Update is called once per frame
    void Update()
    {
       
    }
    [ContextMenu ("test Light")]
    void TestLight()
    {
#if UNITY_PIPELINE_HDRP
        HDAdditionalLightData lightData = gameObject.GetComponent<HDAdditionalLightData>();
        if (lightData != null)
        {
            lightData.intensity = 3;
            
        }
#endif
    }
}
