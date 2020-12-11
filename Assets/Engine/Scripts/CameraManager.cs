using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private float Speed = 1;
    private float zoom;
    private Vector3 startPos,currentPos;

   Quaternion target;
    
    float tiltAngle = 60.0f;
    private Transform Pivot;
    private void Awake()
    {
        transform.SetParent( Pivot = new GameObject("Pivot").transform);
    }
    void Update()
    {
        if (GameManager.CurrentState != GameManager.State.Play) return;
         
        Zoom();
        NearEarth();
    }

    private void NearEarth()
    {
        if (Input.GetMouseButtonDown(1))
            {
                startPos = Input.mousePosition;
                currentPos = Pivot.rotation.eulerAngles;
            } else
        if (Input.GetMouseButton(1))
            {
                Vector3 temp = ((Input.mousePosition - startPos) / Screen.width) * 100;

                target = Quaternion.Euler(currentPos.x- temp.y, currentPos.y + temp.x, 0);

            }
        if (Input.GetMouseButtonUp(1))
            {

            }
        Pivot.rotation = Quaternion.Lerp(Pivot.rotation, target, 10 * Time.unscaledDeltaTime * Speed);

        
    }

    private void Zoom()
    {
        zoom += 5 * Input.mouseScrollDelta.y;
        if (zoom != 0) Pivot.localScale *= 1 - 0.1f * zoom * Time.unscaledDeltaTime;

        Pivot.localScale = Vector3.one * (Mathf.Clamp(Pivot.localScale.x, 0.45f, 4));
        zoom = Mathf.Lerp(zoom, 0, Time.unscaledDeltaTime * 3);
        if (Input.GetMouseButtonDown(2)) zoom = 0;
    }
    private void OnGUI()
    {
        if (Application.isEditor)
            
            {
                //GUI.Label(new Rect(Screen.width * 0.1f, Screen.height * 0.1f, Screen.width * 0.2f, Screen.height * 0.2f), (Input.mousePosition - startPos).ToString());
                //GUI.Label(new Rect(Screen.width * 0.1f, Screen.height * 0.2f, Screen.width * 0.2f, Screen.height * 0.2f), (target.eulerAngles).ToString());


                //      GUI.Label(new Rect(400, 200, 100, 100), Camera.main.WorldToScreenPoint(Vector3.up * maxPos).y.ToString());
                //      if (LastBlock != null) GUI.Label(new Rect(100, 300, 100, 100), LastBlock.transform.position.y.ToString());
            }
    }
}
