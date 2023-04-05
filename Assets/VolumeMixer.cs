using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
[ExecuteInEditMode]
public class VolumeMixer : MonoBehaviour
{
    [SerializeField] Camera Cam;
    [SerializeField] Volume Space;
    [SerializeField] Volume Ground;
    [SerializeField] Transform target;
    [SerializeField] AnimationCurve curve;
    [SerializeField] float startDist;


    [SerializeField] Vector2 clipStart;
    [Range (0,1)]
    [SerializeField] double clc;
    void Start()
    {

        startDist = Vector3.Distance(Cam.transform.position, Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        if(Application.isPlaying)
        clc = (Vector3.Distance(Cam.transform.position, Vector3.zero)) / startDist;
        //Cam.nearClipPlane = Mathf.Lerp(ClipStart.x, ClipEnd.x, Curve.Evaluate((float)clc));
        //Cam.farClipPlane = Mathf.Lerp(clipStart.x, clipStart.y, curve.Evaluate((float)clc));
        //Space.weight = Mathf.Clamp01( curve.Evaluate((float) clc));
        //Ground.weight =1- Space.weight;
    }
}
