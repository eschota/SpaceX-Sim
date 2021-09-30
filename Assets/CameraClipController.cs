using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraClipController : MonoBehaviour
{
    [SerializeField] AnimationCurve Curve;
    [SerializeField] Camera Cam;
    [SerializeField] Transform TargetObject;
    [SerializeField] Vector2 ClipStart;
    [SerializeField] Vector2 ClipEnd;
    float startDist;
    void Start()
    {
        if (Cam == null) Cam = GetComponent<Camera>();
        startDist = Vector3.Distance(Cam.transform.position, TargetObject.position);
    }

    // Update is called once per frame
    void Update()
    {
        Cam.nearClipPlane = Mathf.Lerp(ClipStart.x, ClipEnd.x, Curve.Evaluate( Vector3.Distance(Cam.transform.position, TargetObject.position)/ startDist ));
        Cam.farClipPlane= Mathf.Lerp(ClipStart.y, ClipEnd.y, Curve.Evaluate( Vector3.Distance(Cam.transform.position, TargetObject.position)/ startDist ));
    }
}
