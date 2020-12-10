using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float zoom;
    public Vector3 v,mp0, mp1, dmp, offset;
   Quaternion target;
     float smooth = 10.0f;
    float tiltAngle = 60.0f;
    private Transform Pivot;
    private void Awake()
    {
        transform.SetParent( Pivot = new GameObject("Pivot").transform);
    }
    void Update()
    {
         if (GameManager.CurrentState == GameManager.State.Play)
         {
            if(Input.GetMouseButtonUp(1)) zoom=0;
            

            if(Input.GetMouseButtonDown(1))
            {
                mp0=Input.mousePosition+offset;
            }
            if(Input.GetMouseButton(1))
            {
                // rotation
                mp1=Input.mousePosition;
                dmp=mp1-mp0;
                target = Quaternion.Euler( dmp.y*v.y,dmp.x*v.x,0);
                Pivot.rotation = Quaternion.Slerp(Pivot.rotation, target,  Time.deltaTime * smooth);
                // zoom
               
               
            }
            zoom += 5*Input.mouseScrollDelta.y;
            if (zoom != 0) Pivot.localScale *= 1 - 0.1f * zoom * Time.deltaTime;
            if (Input.GetMouseButtonUp(1))
            {
                offset=-dmp;
            }
            zoom = Mathf.Lerp(zoom, 0, Time.deltaTime*5);
            if (Input.GetMouseButtonDown(2)) zoom = 0;
        }
    }
}
