using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Camera))]
public class CameraControllerInSpace : MonoBehaviour
{
    [SerializeField] Transform PlanetTransform;
    [SerializeField] List< TargetObject> TargetObjects = new List<TargetObject>();
    [HideInInspector][SerializeField] public Camera thisCamera;
    [Range(0.1f,2)] [SerializeField] private float ZoomSpeed = 1;

    [Range (0.0f,1)][SerializeField] float distanceToEarthFly = 0.8f;
    [SerializeField] Vector2 ZoomMinMax = new Vector2 (0.45f,1.6f);
    [SerializeField] Vector2 ZoomMinMaxClipCameraPlane = new Vector2 (10f,600000);
    [SerializeField] AnimationCurve ZoomFarClipPlaneAnimateCurve = new AnimationCurve (new Keyframe (0, 0), new Keyframe (1, 1));
    [SerializeField] AnimationCurve ZoomSpeedAnimateCurve = new AnimationCurve (new Keyframe (0, 0), new Keyframe (1, 1));
    
    private float FlyToTimer;
    [Header("Fly Time to Target")]
    [Range(0,10)][SerializeField] public float FlyToTime=2;
    [Header("Fly Speed Curve")]
    [SerializeField] public AnimationCurve FlyToCurve;
    [SerializeField] ParticleSystem FlyToEffect;

    bool flyBack = false;
    public float zoom;
    private Vector3 startPos, currentPos;
    public static CameraControllerInSpace instance;
    private Vector3 TargetObjectRotation;
    private Transform TargetObjectTransform;
    private Transform Pivot;

    private Vector3 targetPositionOverUnit;
    private Vector3 StartPositionOverUnit;
    private Quaternion targetRotationOverUnit;
    private Quaternion StartRotationOverUnit;

    private Transform _flyToUnit;
    public  Transform FlyToUnit
    {
        get => _flyToUnit;
        set
        {

            if (value != null) SetTargets(value);
            else if (FlyToEffect != null) FlyToEffect.Stop();
            FlyToTimer = 0;
            _flyToUnit= value;
        }
    }

    public void SetTargets(Transform thisValue)
    {
        if (FlyToEffect != null) FlyToEffect.Stop();
        if (FlyToEffect != null) FlyToEffect.Play();
        
        targetPositionOverUnit = Vector3.Lerp(transform.position, thisValue.transform.position, distanceToEarthFly);
        StartPositionOverUnit = thisCamera.transform.position;
        Transform temp = new GameObject().transform;
        temp.position = thisCamera.transform.position;
        temp.LookAt(thisValue.transform.position);
        targetRotationOverUnit = temp.rotation;
        StartRotationOverUnit = thisCamera.transform.rotation;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            IniCamera();
        }
        else
        {
            DestroyImmediate(this.gameObject);
            return;
        }
      
    }
    public void IniCamera()
    {
        instance = this;
        if (thisCamera==null) thisCamera=GetComponent<Camera>();
        if (FlyToEffect != null) FlyToEffect.Stop();
        transform.SetParent(Pivot = new GameObject("Pivot").transform);
        TargetObjectRotation = transform.rotation.eulerAngles;
        TargetObjectTransform = TargetObjects[0].transform; 
        TargetObjectTransform.SetParent(PlanetTransform);

    }
    void Update()
    {

        TurnCameraRotate();
        LockToPlanet();

        if (flyBack)
        {
            FlyBack();
            return;
        }

        if (FlyToUnit != null)
        {
            FlyTo();
            return;
        }
        
        
        Zoom();

        if (isrotate == true)
        {
            Pivot.rotation = Quaternion.Lerp(Pivot.rotation, Quaternion.Euler(Pivot.rotation.eulerAngles + Vector3.up * SpeedRotate), 10 * Time.unscaledDeltaTime);
            return;
        }


        NearEarth();
    }
    private void FlyBack()
    {
        FlyToTimer += Time.unscaledDeltaTime / FlyToTime;
        if (FlyToTimer < 1)
        {

            thisCamera.transform.position = Vector3.Lerp(targetPositionOverUnit,StartPositionOverUnit, FlyToCurve.Evaluate(FlyToTimer));
            thisCamera.transform.rotation = Quaternion.Lerp(targetRotationOverUnit,StartRotationOverUnit, FlyToCurve.Evaluate(FlyToTimer));

        }
        else flyBack=false;

    }
    private void FlyTo()
    {
        FlyToTimer += Time.unscaledDeltaTime/FlyToTime;
        if (FlyToTimer<1)
        {

            thisCamera.transform.position = Vector3.Lerp(StartPositionOverUnit, targetPositionOverUnit, FlyToCurve.Evaluate( FlyToTimer));
            thisCamera.transform.rotation= Quaternion.Lerp(StartRotationOverUnit, targetRotationOverUnit, FlyToCurve.Evaluate( FlyToTimer));
               
        }
        else FlyToUnit = null;
        

    }

    TargetObject currentTarget;
    
    public void LockToPlanet()
    {
        
            if (!Input.GetMouseButtonDown(0)) return;
        //    RaycastHit hit;
        //if (Physics.Raycast(thisCamera.transform.position, thisCamera.transform.forward, out hit, Mathf.Infinity))
        //{
        //    Debug.Log("TargetLocked: " + hit.collider.name );
        //    currentTarget = TargetObjects.FindLast(X => X.col.gameObject == hit.collider.gameObject);

        //}
        // foreach TargetObjects find minimum distance to get closest one in screen space to mouse pointer
        // then lock to it


            foreach(TargetObject target in TargetObjects)
        {
            Vector2 screenPoint = thisCamera.WorldToScreenPoint(target.transform.position);
            {
                float distance = Vector2.Distance(Input.mousePosition, screenPoint);
                Debug.Log("distance: " + distance);
                if (distance < Screen.width/15)
                {
                    currentTarget = target;
                    
                    break;
                }
            }
               
        }

        
        
        

            
    }
    bool locked = false;
    private void NearEarth()
    {
        
        
        if (Input.GetMouseButtonDown(1))
            {
               currentTarget=null;
                startPos = Input.mousePosition;
                currentPos = Pivot.rotation.eulerAngles;
            } else
        if (Input.GetMouseButton(1))
            {
                Vector3 temp = ((Input.mousePosition - startPos) / Screen.width) * 100;

                TargetObjectRotation = new Vector3( currentPos.x- temp.y, currentPos.y + temp.x, 0 )  ;
                TargetObjectRotation = new Vector3(ClampedAngle( TargetObjectRotation .x), ClampedAngle(TargetObjectRotation.y), ClampedAngle(TargetObjectRotation.z) );

            }
        if (currentTarget!=null)
        {
           // Pivot.transform.rotation=Quaternion.Lerp(Pivot.transform.rotation, Pivot.transform.LookAt(-currentTarget.transform.position),Time.unscaledDeltaTime*10);
            TargetObjectRotation = Quaternion.LookRotation(-TargetObjectTransform.transform.position).eulerAngles;
          //  return;
        }

         
        Pivot.rotation = Quaternion.Lerp(Pivot.rotation, Quaternion.Euler( TargetObjectRotation), 10 * Time.unscaledDeltaTime );
        

    }

    [SerializeField] float SpeedRotate = 1;

    private float ClampedAngle(float angleInDegrees)
    {
        if (angleInDegrees >= 360f)
        {
            return angleInDegrees - (360f * (int)(angleInDegrees / 360f));
        }
        else if (angleInDegrees >= 0f)
        {
            return angleInDegrees;
        }
        else
        {
            float f = angleInDegrees / -360f;
            int i = (int)f;
            if (f != i)
                ++i;
            return angleInDegrees + (360f * i);
        }
    }
    public float  zoomMult=1000;
    private void Zoom()
    {
        zoom -= ZoomSpeed * Input.mouseScrollDelta.y;
        zoom = Mathf.Clamp(zoom, -1, 1);
        if (zoom != 0) Pivot.localScale *= 1 - 0.1f * zoom * Time.unscaledDeltaTime*5;

        //Pivot.localScale = Vector3.one * Mathf.Clamp(Pivot.localScale.x*zoomMult*ZoomInSpeedCurve.Evaluate(Pivot.localScale.x - ZoomMinMax.x), ZoomMinMax.x,ZoomMinMax.y);


        float scale = ZoomSpeedAnimateCurve.Evaluate((zoom + 1.00001f) / 2);
        Pivot.localScale = Vector3.one * scale* scale* scale;
        Pivot.localScale = Vector3.one * Mathf.Clamp(Pivot.localScale.x, ZoomMinMax.x,ZoomMinMax.y);
        //zoom = Mathf.Lerp(zoom, 0, Time.unscaledDeltaTime*0.1f  );
        thisCamera.farClipPlane = 60000* ZoomFarClipPlaneAnimateCurve.Evaluate(Pivot.localScale.x -ZoomMinMax.x);
        //thisCamera.nearClipPlane = Mathf.Lerp(ZoomMinMaxClipCameraPlane.x, ZoomMinMaxClipCameraPlane.x, (1 - ZoomMinMax.x) / (ZoomMinMax.y));
        if (Input.GetMouseButtonDown(2)) zoom = 0;
    }
    private void Reset()
    {
        thisCamera = GetComponent<Camera>();
        FlyToCurve =  AnimationCurve.EaseInOut(0,0,1,1);
    }


    bool isrotate = false;
    void TurnCameraRotate()
    {
        if (Input.GetKeyUp(KeyCode.F11))
        {
            if (isrotate == false)
            {
                isrotate = true;
            }
            else
                isrotate = false;


        }
    }
}


