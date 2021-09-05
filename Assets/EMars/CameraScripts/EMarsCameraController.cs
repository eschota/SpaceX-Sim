using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Rendering.PostProcessing;
 

[ExecuteInEditMode]
public class EMarsCameraController : MonoBehaviour
{
   
    [SerializeField] private Vector3[] StatePositions;
    [SerializeField] private Vector3[] StateRotations;
    public float Wheel,currentLerp;
   [SerializeField] public CameraState CurrentCameraState;
    GameObject Pivot, PivotShift;
   //[SerializeField] PostProcessVolume ppv;
   // DepthOfField dof;
    float mistakeTimer;
    Vector2 mPos;
    [SerializeField] float CameraSpeed = 50;
    [SerializeField] float DistanceMax = 50;
    public enum CameraState
    {
        
        
        NearView=0, 
        MiddleView = 1,
        TopView = 2
    }

    private void Awake()
    {
        if (Application.isPlaying)
        {
            Pivot = new GameObject(); Pivot.name = "PivotCamera"; Pivot.transform.position = Vector3.zero; Pivot.transform.rotation = Quaternion.identity;
            PivotShift = new GameObject(); PivotShift.name = "PivotShift"; PivotShift.transform.position = Vector3.zero; PivotShift.transform.rotation = Quaternion.identity;
            Camera.main.transform.SetParent(Pivot.transform);
            Wheel = 0;
            currentLerp = 0;
            CurrentCameraState = CameraState.TopView;

            //PostProcessProfile ppf= ppv.profile;
            //ppf.TryGetSettings<DepthOfField>(out dof);
        }
    }
   
    void Update()
    {

       
         
        MoveCamera();
        //if (Application.isPlaying) dof.focusDistance.value = Vector3.Distance(Camera.main.transform.position, Camera.main.transform.parent.position);
        //else
        //{
        //    PostProcessProfile ppf = ppv.profile;
        //    ppf.TryGetSettings<DepthOfField>(out dof);
        //    dof.focusDistance.value = Vector3.Distance(Camera.main.transform.position, Vector3.zero);
        //}

        ChangeCamera();
        
    }
    void MoveCamera()
    {
        if (Application.isPlaying)
        {
            Wheel += 1.8f * Input.GetAxis("Mouse ScrollWheel");
            Wheel = Mathf.Clamp01(Wheel);

            currentLerp = Mathf.Lerp(currentLerp, Wheel, 5 * Time.deltaTime);
            currentLerp = Mathf.Clamp01(currentLerp);

            {

                Camera.main.transform.localPosition = Vector3.Lerp(StatePositions[2], StatePositions[0], currentLerp);
                Camera.main.transform.localRotation = Quaternion.Euler(Vector3.Lerp(StateRotations[2], StateRotations[0], currentLerp));
            }

            if (Input.mousePosition.x < 0 || Input.mousePosition.x > Screen.width || Input.mousePosition.y < 0 || Input.mousePosition.y > Screen.height)
            {

            }
            else
            {

                if (Input.mousePosition.x < 0.05f * Screen.width) mistakeTimer += Time.deltaTime;
                else
                 if (Input.mousePosition.x > 0.95f * Screen.width) mistakeTimer += Time.deltaTime;
                else
                 if (Input.mousePosition.y > 0.95f * Screen.height) mistakeTimer += Time.deltaTime;
                else
                 if (Input.mousePosition.y < 0.05f * Screen.height) mistakeTimer += Time.deltaTime;
                else mistakeTimer = 0;

                if (mistakeTimer > 0.17f)
                {
                    Vector3 Direction = Input.mousePosition - new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
                    Direction = new Vector3(Direction.x, 0, Direction.y);
                    Direction = (200*CameraSpeed * Direction.normalized * Time.deltaTime * mistakeTimer);
                    if (Vector3.Distance(PivotShift.transform.position + PivotShift.transform.TransformDirection(Direction), Vector3.zero) <= DistanceMax)
                        PivotShift.transform.position += PivotShift.transform.TransformDirection( Direction);
                }
            }
            if (Input.GetMouseButtonDown(2) || Input.GetMouseButtonDown(1))
            {
                
                mPos = Input.mousePosition;
            }
                if (Input.GetMouseButton(2))
            {
                PivotShift.transform.rotation=Quaternion.Euler(0,PivotShift.transform.rotation.eulerAngles.y+(((mPos.x-Input.mousePosition.x)/Screen.width)*Time.deltaTime*500),0);
            }
            if (Input.GetMouseButton(1))
            {
                Vector3 Direction = Input.mousePosition - new Vector3(mPos.x,mPos.y,0);
                Direction = new Vector3(Direction.x, 0, Direction.y);
                Direction =-1* (CameraSpeed * Direction * Time.deltaTime  );
                if (Vector3.Distance(PivotShift.transform.position + PivotShift.transform.TransformDirection(Direction), Vector3.zero) <= DistanceMax)
                    PivotShift.transform.position += PivotShift.transform.TransformDirection(Direction);
            }
            Pivot.transform.position = Vector3.Lerp(Pivot.transform.position, PivotShift.transform.position, 50*Time.deltaTime);
            Pivot.transform.rotation = Quaternion.Lerp(Pivot.transform.rotation, PivotShift.transform.rotation, 50*Time.deltaTime);
        }
    }
    void ChangeCamera()
    {
        if (!Application.isPlaying)
        {
            Camera.main.transform.position = StatePositions[(int)CurrentCameraState];
            Camera.main.transform.localRotation = Quaternion.Euler(StateRotations[(int)CurrentCameraState]);
        }
    }
    public void ChangeCameraStateNext()
    {
        if ((int) CurrentCameraState < 2) CurrentCameraState++;
         else CurrentCameraState=0;

    }
}
