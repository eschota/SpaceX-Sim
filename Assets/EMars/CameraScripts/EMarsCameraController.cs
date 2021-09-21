using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Rendering.PostProcessing;
 

[ExecuteInEditMode]
public class EMarsCameraController : MonoBehaviour
{
    public float isCollided = 0;

    [SerializeField] private Vector3[] StatePositions;
    [SerializeField] private Vector3[] StateRotations;
    public float Wheel,currentLerp;
   [SerializeField] public CameraState CurrentCameraState;
   public GameObject Pivot, PivotShift;
   //[SerializeField] PostProcessVolume ppv;
   // DepthOfField dof;
    float mistakeTimer;
    Vector2 mPos;
    [SerializeField] float DistanceMax = 50;
    
    [SerializeField] float CameraSpeed = 50;
    [SerializeField] AnimationCurve CameraSpeedMultByHeight=new AnimationCurve(new Keyframe(0, 0.1f), new Keyframe(1, 1));

    private float totalSpeed
    {
        get =>  CameraSpeed * Mathf.Clamp( CameraSpeedMultByHeight.Evaluate(Camera.main.transform.position.y / StatePositions[StatePositions.Length - 1].y),0.2f,1);
        
    }
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
            transform.position = Vector3.up * 200;
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
          

        if (!Application.isPlaying) return;
        if (Input.GetKeyDown(KeyCode.R)) UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        CameraInputInteraction();
        MoveCameraByHeightAndRotateInEditor();
        DragCamera();
        
    }
    void MoveCameraByKeboard()
    {
        Vector3 dir;
            if (Input.GetKey(KeyCode.W)|| Input.GetKey(KeyCode.UpArrow) || Input.mousePosition.y>Screen.height*0.95f)
        {
            dir = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
            
                PivotShift.transform.position+=(totalSpeed * dir);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.mousePosition.y < Screen.height * 0.05f)
        {
            dir = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
            
            PivotShift.transform.position-=(totalSpeed * dir);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.mousePosition.x < Screen.width * 0.05f)
        {
            dir = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z);
            
            PivotShift.transform.position-=(totalSpeed * dir);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.mousePosition.x > Screen.width * 0.95f)
        {
            dir = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z);
            
            PivotShift.transform.position+=(totalSpeed * dir);
        }
        Debug.DrawRay(PivotShift.transform.position, 10*CameraSpeed * new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z));
    }


    private Vector3 DragPosStart;
    void DragCamera()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float angle =Vector3.Angle(ray.direction, Vector3.down);

            float distToZeroPlane = ray.origin.y * Mathf.Tan(Mathf.Deg2Rad * angle);
        //Debug.Log("Angle = " + angle.ToString("00") + " Distance: " + distToZeroPlane.ToString("00000"));
            Vector3 planarDirection = new Vector3(ray.direction.x, 0, ray.direction.z);
            Vector3 planarOrigin = new Vector3(ray.origin.x, 0, ray.origin.z);
        DragPosStart = planarOrigin + (planarDirection.normalized * distToZeroPlane);
       // Debug.DrawLine(DragPosStart, DragPosStart+planarDirection*100, Color.green);


        }
        else
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float angle = Vector3.Angle(ray.direction, Vector3.down);

            float distToZeroPlane = ray.origin.y * Mathf.Tan(Mathf.Deg2Rad * angle);
            //Debug.Log("Angle = " + angle.ToString("00") + " Distance: " + distToZeroPlane.ToString("00000"));
            Vector3 planarDirection = new Vector3(ray.direction.x, 0, ray.direction.z);
            Vector3 planarOrigin = new Vector3(ray.origin.x, 0, ray.origin.z);
           Vector3 DragPos = planarOrigin + (planarDirection.normalized * distToZeroPlane);

             
                PivotShift.transform.position += -DragPos + DragPosStart;
                PivotShift.transform.position = new Vector3(PivotShift.transform.position.x, 0, PivotShift.transform.position.z);
                DragPosStart = (DragPos);
                Pivot.transform.position = Vector3.Lerp(Pivot.transform.position, PivotShift.transform.position, 25 * Time.deltaTime);
             
        }
    }

    

    void CameraInputInteraction()
    {
        if (Application.isPlaying)
        {
                if (Input.GetMouseButtonDown(2) || Input.GetMouseButtonDown(1))
            {
                
                mPos = Input.mousePosition;
            }
            if (Input.GetMouseButton(2))
            {
                PivotShift.transform.rotation = Quaternion.Euler(0, PivotShift.transform.rotation.eulerAngles.y + (((mPos.x - Input.mousePosition.x) / Screen.width) * Time.deltaTime * 500), 0);
            }
            else
            {
            
                 
                    Wheel += 1.4f * Input.GetAxis("Mouse ScrollWheel");
                    Wheel = Mathf.Clamp01(Wheel);

                    currentLerp = Mathf.Lerp(currentLerp, Wheel, 5 * Time.deltaTime);
                    currentLerp = Mathf.Clamp01(currentLerp);

                    Camera.main.transform.localPosition = Vector3.Lerp(StatePositions[2], StatePositions[0], currentLerp);
                    Camera.main.transform.localRotation = Quaternion.Euler(Vector3.Lerp(StateRotations[2], StateRotations[0], currentLerp));
                 
                    MoveCameraByKeboard();
                    
            }
           
            if(Vector3.Distance(PivotShift.transform.position, transform.position) > DistanceMax)
            {
                PivotShift.transform.Translate((transform.position - PivotShift.transform.position).normalized*10);
            }


            if (!Input.GetMouseButton(0))   Pivot.transform.position = Vector3.Lerp(Pivot.transform.position, PivotShift.transform.position,10* Time.deltaTime);
              Pivot.transform.rotation = Quaternion.Lerp(Pivot.transform.rotation, PivotShift.transform.rotation,10* Time.deltaTime);
        }
    }
    void MoveCameraByHeightAndRotateInEditor()
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, DistanceMax);
    }
}
