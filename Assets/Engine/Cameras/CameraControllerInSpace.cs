using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 [DefaultExecutionOrder(-100)]
public class CameraControllerInSpace : MonoBehaviour
{
    [HideInInspector] [SerializeField] public Camera thisCamera;
    [Range(0.1f, 2)] [SerializeField] private float Speed = 1;

    [Range(0.0f, 1)] [SerializeField] float distanceToEarthFly = 0.8f;

    private float FlyToTimer;
    [Header("Fly Time to Target")]
    [Range(0, 10)] [SerializeField] public float FlyToTime = 2;
    [Header("Fly Speed Curve")]
    [SerializeField] public AnimationCurve FlyToCurve;
    [SerializeField] ParticleSystem VFXFlyToEffect;

     
    private float zoom;
    private Vector3 startPos, currentPos;
    public static CameraControllerInSpace instance;
    private Vector3 targetRotation;
    [HideInInspector]public Transform TargetObject;
    private Transform Pivot;

    private Vector3 targetPositionOverUnit;
    private Vector3 StartPositionOverUnit;
    private Quaternion targetRotationOverUnit;
    private Quaternion StartRotationOverUnit;
    
    bool flyBack = false;
    private Transform _flyToUnit;
    public  Transform FlyToUnit
    {
        get => _flyToUnit;
        set
        {

            if (value != null) SetTargets(value);
            else if (VFXFlyToEffect != null) VFXFlyToEffect.Stop();
            FlyToTimer = 0;
            _flyToUnit= value;
        }
    }

    public void SetTargets(Transform thisValue)
    {
        if (VFXFlyToEffect != null) VFXFlyToEffect.Stop();
        if (VFXFlyToEffect != null) VFXFlyToEffect.Play();
        
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
        if (VFXFlyToEffect != null) VFXFlyToEffect.Stop();
        transform.SetParent(Pivot = new GameObject("Pivot").transform);
        targetRotation = transform.rotation.eulerAngles;
        DontDestroyOnLoad(Pivot);
         
        GameManager.EventChangeState += OnChangeState;
    }
    void Update()
    {   if (flyBack) FlyBack();

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

                targetRotation =new Vector3( currentPos.x- temp.y, currentPos.y + temp.x, 0 );

            }
        if (TargetObject != null)
        {
           // Pivot.transform.LookAt(-TargetObject.transform.position);
            targetRotation = Quaternion.LookRotation(-TargetObject.transform.position).eulerAngles;
            //return;
        }
        Pivot.rotation = Quaternion.Lerp(Pivot.rotation, Quaternion.Euler( targetRotation), 10 * Time.unscaledDeltaTime * Speed);

        
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
void OnChangeState()
{
        if (GameManager.CurrentState==GameManager.State.PlaySpace)
        {
            if (GameManager.LastState == GameManager.State.PlayEarth)
            {
                thisCamera.enabled = true;
                flyBack = true;
               

            }

        }
        if(GameManager.CurrentState == GameManager.State.PlayEarth)
        {
           thisCamera.enabled = false;
          
        }
}
    private void Zoom()
    {
        zoom += Speed * Input.mouseScrollDelta.y;
        if (zoom != 0) Pivot.localScale *= 1 - 0.1f * zoom * Time.unscaledDeltaTime * 5;

        Pivot.localScale = Vector3.one * (Mathf.Clamp(Pivot.localScale.x, 0.45f, 4));
        zoom = Mathf.Lerp(zoom, 0, Time.unscaledDeltaTime * 5);
        if (Input.GetMouseButtonDown(2)) zoom = 0;
    }
    private void Reset()
    {
        thisCamera = GetComponent<Camera>();
        FlyToCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    }
}


