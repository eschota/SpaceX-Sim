using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private float Speed = 1;
    private float zoom;
    private Vector3 startPos,currentPos;
    public static CameraManager instance;
    public Vector3 target;
    public Transform TargetObject;
    private Transform Pivot;
    private static Unit _flyToUnit;
    public static Unit FlyToUnit
    {
        get => _flyToUnit;
        set
        {
          
                Camera.main.transform.SetParent(null);
            _flyToUnit= value;
        }
    }
    
    private void Awake()
    {
        if (instance == null) instance = this; else DestroyImmediate(this.gameObject);
        transform.SetParent( Pivot = new GameObject("Pivot").transform);
        target = transform.rotation.eulerAngles ;
    }
    void Update()
    {
         //if (GameManager.CurrentState != GameManager.State.Play || GameManager.CurrentState != GameManager.State.CreateLauchPlace || GameManager.CurrentState != GameManager.State.CreateResearchLab||
         //   GameManager.CurrentState != GameManager.State.CreateProductionFactory) return;
        if (FlyToUnit != null)
        {
            FlyTo();
            return;
        }
        Zoom();
        NearEarth();
    }

    private void FlyTo()
    {
        if (Vector3.Distance(transform.position, FlyToUnit.transform.position) > 1)
        {
            transform.position = Vector3.Slerp(transform.position, FlyToUnit.transform.position, Time.unscaledDeltaTime * 1.5f);
            transform.LookAt(FlyToUnit.transform.position);
        }
        else FlyToUnit = null;
        

    }
    private void NearEarth()
    {
        
        
        if (Input.GetMouseButtonDown(1))
            {
            TargetObject = null;
                startPos = Input.mousePosition;
                currentPos = Pivot.rotation.eulerAngles;
            } else
        if (Input.GetMouseButton(1))
            {
                Vector3 temp = ((Input.mousePosition - startPos) / Screen.width) * 100;

                target =new Vector3( currentPos.x- temp.y, currentPos.y + temp.x, 0 );

            }
        if (TargetObject != null)
        {
           // Pivot.transform.LookAt(-TargetObject.transform.position);
            target = Quaternion.LookRotation(-TargetObject.transform.position).eulerAngles;
            //return;
        }
        Pivot.rotation = Quaternion.Lerp(Pivot.rotation, Quaternion.Euler( target), 10 * Time.unscaledDeltaTime * Speed);

        
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
