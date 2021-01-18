using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Rendering.PostProcessing;

public class CameraControllerOnEarth : MonoBehaviour
{

    private float zoom;
    public Vector3 startPos = new Vector3(0, 300, -200);
    public Vector3 currentPos = Vector3.zero;
    public Vector3 target = Vector3.zero;
    public Vector3 startRot = new Vector3(45, 0, 0);
    Vector3 startDrag, CurrentDrag, targetDrag;
    Transform Pivot;
    //  DepthOfField dof;
    GameParameters GP;
    public static CameraControllerOnEarth instance;
    void Start()
    {
        instance = this;
        GP = Resources.Load<GameParameters>("GameParametres/GameParametresBase");
        //    FindObjectOfType<PostProcessVolume>().profile.TryGetSettings(out dof);
        Pivot = new GameObject("Pivot").transform;
        Camera.main.transform.position = GP.CameraEarthstartPosition;
        Camera.main.transform.rotation = Quaternion.Euler(GP.CameraEarthstartRotation);

        Camera.main.transform.SetParent(Pivot);
        LoadPos();
    }

    // Update is called once per frame
    void Update()
    {
        Zoom();

        Rotate();

    }
    void DOF()
    {
        //if (dof == null) return;

        //Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;
        //if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit))
        //{
        //    dof.focusDistance.value = Vector3.Distance(Camera.main.transform.position, hit.point);
        //}
    }
    void Rotate()
    {
        if (Input.GetMouseButtonDown(1))
        {

            startPos = Input.mousePosition;
            currentPos = Camera.main.transform.rotation.eulerAngles;
        }
        else
      if (Input.GetMouseButton(1))
        {
            Vector3 temp = ((Input.mousePosition - startPos) / Screen.width) * 500;

            target = new Vector3(currentPos.x - temp.y, currentPos.y + temp.x, 0);
       //     target = new Vector3(Mathf.Clamp( currentPos.x - temp.y,-90,40), currentPos.y + temp.x, 0);

        } if (Input.GetMouseButtonUp(1))
        {
            startPos = Input.mousePosition;
            currentPos = Camera.main.transform.rotation.eulerAngles;

        }
        
        {
            Move();
          //  Drag();
        }
        //if (TargetObject != null)
        //{
        //    // Pivot.transform.LookAt(-TargetObject.transform.position);
        //    target = Quaternion.LookRotation(-TargetObject.transform.position).eulerAngles;
        //    //return;
        //}
//        Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, Quaternion.Euler(target), 5 * Time.unscaledDeltaTime * GP.CameraEarthSpeed);
    }
    void Drag()
    {
        if (Input.GetMouseButtonDown(0))
        {

            startDrag = Input.mousePosition;
            CurrentDrag = Pivot.transform.position;
        }
        else
    if (Input.GetMouseButton(0))
        {
            Vector3 temp = ((Input.mousePosition - startDrag) / Screen.width) * 200;

            targetDrag = new Vector3(temp.x, 0, temp.y);

        }
        Pivot.transform.position = Vector3.Lerp(Pivot.transform.position, Pivot.transform.position - targetDrag, Time.deltaTime * GP.CameraEarthSpeed * 0.3f);
        if (Input.GetMouseButtonUp(0))
        {
            targetDrag = Vector3.zero;
        }
    }
    private void Move( )
    {
        if (Input.mousePosition.x > Screen.width || Input.mousePosition.x < 0) return;
        if (Input.mousePosition.y > Screen.height || Input.mousePosition.y < 0) return;
        Vector3 forward = Camera.main.transform.forward;
        Vector3 left = -Camera.main.transform.right;
        left = new Vector3(left.x, 0, left.z);
        left = left.normalized * GP.CameraEarthSpeed;
        forward = new Vector3(forward.x, 0, forward.z);
        forward = forward.normalized * GP.CameraEarthSpeed;
        
        if (Input.mousePosition.x < Screen.width * 0.05f || Input.GetKey(KeyCode.A)) if(Bound(Pivot.position+left))Pivot.transform.position = Vector3.Lerp(Pivot.transform.position, Pivot.transform.position + left, Time.deltaTime * GP.CameraEarthSpeed);
        if (Input.mousePosition.x > Screen.width * 0.95f || Input.GetKey(KeyCode.D)) if (Bound(Pivot.position - left))Pivot.transform.position = Vector3.Lerp(Pivot.transform.position, Pivot.transform.position - left, Time.deltaTime * GP.CameraEarthSpeed);
        if (Input.mousePosition.y > Screen.height * 0.95f || Input.GetKey(KeyCode.W)) if (Bound(Pivot.position +forward)) Pivot.transform.position = Vector3.Lerp(Pivot.transform.position, Pivot.transform.position + forward, Time.deltaTime * GP.CameraEarthSpeed);
        if (Input.mousePosition.y < Screen.height * 0.05f || Input.GetKey(KeyCode.S)) if (Bound(Pivot.position -forward)) Pivot.transform.position = Vector3.Lerp(Pivot.transform.position, Pivot.transform.position - forward, Time.deltaTime * GP.CameraEarthSpeed);


    }

    
    
    private bool Bound(Vector3 pos1)
{
        if (Mathf.Abs( pos1.x) > GP.CameraEarthBoundings.x) return false;
        if (Mathf.Abs( pos1.z) > GP.CameraEarthBoundings.z) return false;
        return true;
}
     
    private void Zoom()
    {
        zoom += 3 * Input.mouseScrollDelta.y;
        if (zoom != 0) Pivot.localScale *= 1 - 0.1f * zoom * Time.unscaledDeltaTime;

        Pivot.localScale = Vector3.one * (Mathf.Clamp(Pivot.localScale.x, 0.25f, 5));
        zoom = Mathf.Lerp(zoom, 0, Time.unscaledDeltaTime * 3);
        if (Input.GetMouseButtonDown(2)) zoom = 0;
    }

    public void SavePos()
    {
        string SceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        PlayerPrefs.SetFloat(SceneName+"x", Pivot.transform.position.x);
        PlayerPrefs.SetFloat(SceneName+"y", Pivot.transform.position.y);
        PlayerPrefs.SetFloat(SceneName+"z", Pivot.transform.position.z);
        PlayerPrefs.SetFloat(SceneName+"s", Pivot.transform.localScale.z);
        PlayerPrefs.Save();
    }
    void LoadPos()
    {
        string SceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (PlayerPrefs.HasKey(SceneName + "x"))
        {
            Pivot.transform.position = new Vector3(PlayerPrefs.GetFloat(SceneName + "x"), PlayerPrefs.GetFloat(SceneName + "y"), PlayerPrefs.GetFloat(SceneName + "z"));
            Pivot.transform.localScale =Vector3.one* PlayerPrefs.GetFloat(SceneName + "s");
        }
    }
    
}
