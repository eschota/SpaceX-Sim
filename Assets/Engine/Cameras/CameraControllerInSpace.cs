using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerInSpace : MonoBehaviour
{
    private float Speed = 1;
    [SerializeField] float distanceToEarthFly = 0.8f;
    private float zoom;
    private Vector3 startPos,currentPos;
    public static CameraControllerInSpace instance;
    public Vector3 target;
    public Transform TargetObject;
    private Transform Pivot;
    private Transform _flyToUnit;
    private Vector3 targetPositionOverUnit;
    private Vector3 StartPositionOverUnit;
    private Quaternion targetRotationOverUnit;
    private Quaternion StartRotationOverUnit;
    public  Transform FlyToUnit
    {
        get => _flyToUnit;
        set
        {

         if(value!=null)   SetTargets(value);
            FlyToTimer = 0;
            _flyToUnit= value;
        }
    }

    public void SetTargets(Transform thisValue)
    {
        Camera.main.transform.SetParent(null);
        targetPositionOverUnit = Vector3.Lerp(transform.position, thisValue.transform.position, distanceToEarthFly);
        StartPositionOverUnit = Camera.main.transform.position;
        Transform temp = new GameObject().transform;
        temp.position = Camera.main.transform.position;
        temp.LookAt(thisValue.transform.position);
        targetRotationOverUnit = temp.rotation;
        StartRotationOverUnit = Camera.main.transform.rotation;
    }

    public float FlyToTimer;
    [SerializeField] public float FlyToTime;
    [SerializeField] public AnimationCurve FlyToCurve;
    private void Awake()
    {
        if (instance == null) instance = this; else DestroyImmediate(this.gameObject);
        transform.SetParent( Pivot = new GameObject("Pivot").transform);
        target = transform.rotation.eulerAngles ;
    }
    void Update()
    {
        if (FlyToUnit != null)
        {
            FlyTo();
            return;
        }
        if (GameManager.CurrentState != GameManager.State.PlaySpace && GameManager.CurrentState != GameManager.State.CreateLaunchPlace && GameManager.CurrentState != GameManager.State.CreateResearchLab&&
            GameManager.CurrentState != GameManager.State.CreateProductionFactory) return;
        
        Zoom();
        NearEarth();
    }

    private void FlyTo()
    {
        FlyToTimer += Time.unscaledDeltaTime/FlyToTime;
        if (FlyToTimer<1)
        {

            Camera.main.transform.position = Vector3.Lerp(StartPositionOverUnit, targetPositionOverUnit, FlyToCurve.Evaluate( FlyToTimer));
            Camera.main.transform.rotation= Quaternion.Lerp(StartRotationOverUnit, targetRotationOverUnit, FlyToCurve.Evaluate( FlyToTimer));
               
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
