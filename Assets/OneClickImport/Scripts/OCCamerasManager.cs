#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class OCCamerasManager : MonoBehaviour
{
    public static OCCamerasManager instance;
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
        transform.SetSiblingIndex(1);
    }

    public void AddCamera(OCObject obj)
    {
        Camera Cam = new GameObject().AddComponent<Camera>();
        float u = FindObjectOfType< OneClickSceneManager>().Units;

        Cam.gameObject.transform.SetParent(transform);
        Cam.gameObject.name = obj.ocname;
#if UNITY_PIPELINE_URP
        UnityEngine.Rendering.Universal.UniversalAdditionalCameraData uac = Cam.GetComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>();
        if (uac == null)
        {
            uac = Cam.gameObject. AddComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>();
        }
        uac.renderPostProcessing = true;

#endif

        Cam.transform.localPosition = new Vector3(-obj.position[0] *u, obj.position[2] * u, -obj.position[1] * u);
        Cam.transform.LookAt(Cam.transform.position + new Vector3(obj.dir[0], obj.dir[2], obj.dir[1]));
        Cam.fieldOfView = obj.fov;
    }
}
#endif
