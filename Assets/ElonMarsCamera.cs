using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElonMarsCamera : MonoBehaviour
{
    [SerializeField] Camera ManageCamera;
    [SerializeField] float speed = 0.1f;
    Vector3 starPos;
    Quaternion startRot;
    void Start()
    {
        starPos = transform.position;
        startRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (GM.CurrentState == GM.State.ManageRocket)
        {
            transform.position = Vector3.Lerp(transform.position, ManageCamera.transform.position, speed * GM.EventTimer);
            transform.rotation= Quaternion.Lerp(transform.rotation, ManageCamera.transform.rotation, speed * GM.EventTimer);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, starPos,speed* GM.EventTimer);
            transform.rotation = Quaternion.Lerp(transform.rotation, startRot, speed * GM.EventTimer);
        }
    }
}
