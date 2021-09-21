using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[InitializeOnLoad]
[ExecuteInEditMode]
public class ReferenceHelper : MonoBehaviour
{
    private static Color TransparencyColor=Color.white;
    [SerializeField]  List<Image> References = new List<Image>();
    [SerializeField]  RawImage history ;

    private static Image CurrentImage;
    private static int CurrentImageID=0;
    private void Awake()
    {
        if(Application.isPlaying) Destroy(this);
    }
    private void OnValidate()
    {
        References.Clear();
        References.AddRange(transform.GetComponentsInChildren<Image>());
        foreach (var item in References) item.fillMethod = Image.FillMethod.Horizontal;
        foreach (var item in References) item.fillOrigin= 1;
      //  if(history==null) history = GameObject.Find("RenderHistoryRawImage").GetComponent<RawImage>();
    }

   
    private void Update()
    {
        if (true) ;// workaround for editor running
    }

    void SetCurrentImage(Image img)
    {
        foreach (var item in References)
        {
            item.color = new Color(0, 0, 0, 0);
        }
        img.color = TransparencyColor;
        CurrentImageID = References.IndexOf(img);
        CurrentImage = References[CurrentImageID];
    } 
    void SetCurrentImage(int id)
    {
        foreach (var item in References)
        {
            item.color = new Color(0, 0, 0, 0);
        }
        if (id == References.Count) CurrentImageID = 0;
        else CurrentImageID = id;
        References[CurrentImageID].color = TransparencyColor;
        history.color = new Color(0, 0, 0, 0);
    }
     void DisableAllReferences()
    {
        foreach (var item in References)
        {
            item.color = new Color(0,0,0,0);
        }
      if(CurrentImageID!=-1)  CurrentImage = References[CurrentImageID];
        CurrentImageID = -1;
        history.color = new Color(0, 0, 0, 0); 
    }

    [MenuItem("My Commands/Special Command %e")]
    void SpecialCommand()
    {
        Debug.Log("You used the shortcut Cmd+G (Mac)  Ctrl+G (Win)");
        Capture();
    }
     void EnableHistory()
    {
        DisableAllReferences();
        history.color = TransparencyColor;
    }
    public  void Capture()
    {
            DisableAllReferences();
            ScreenCapture.CaptureScreenshot(Path.Combine(Application.streamingAssetsPath, "history.png"));
        Texture2D tex =new Texture2D(Screen.width,Screen.height);
        tex.LoadImage(File.ReadAllBytes(Path.Combine(Application.streamingAssetsPath, "history.png")));
        tex.Apply();
        history.texture = tex;
        EnableHistory();
    }
    void OnGUI()
        {

            

            Event e = new Event();
            while (Event.PopEvent(e))
            {
            Vector3 lastPos = Camera.main.transform.position;

            if (e.rawType == EventType.KeyDown) //' && e.button == KeyCode.R)// left button 
            {
                Capture();
            }
                if (e.rawType == EventType.MouseDown && e.button == 0)// left button 
                {

                if (CurrentImageID == -1)
                {
                    if (CurrentImage == null)
                        SetCurrentImage(References[0]);
                    else SetCurrentImage(CurrentImage);
                }
                else
                {
                    DisableAllReferences();
                }
                
                //Ray ray = Camera.main.ScreenPointToRay(new Vector2(e.mousePosition.x, Screen.height - e.mousePosition.y));
                //RaycastHit hit;
                //if (Physics.Raycast(ray, out hit))
                //{

                //    Selection.activeGameObject = hit.collider.gameObject;
                //    Color C = Color.red;
                //    C.a = 0.33f;
                //    Gizmos.color = C;

                //    // Gizmos.DrawCube(Selection.activeGameObject.transform.position, Selection.activeGameObject.GetComponent<MeshRenderer>().bounds.size);

                //}
            }
            if (e.rawType == EventType.MouseDrag && e.button == 1)// right button
            {

                foreach (var item in References)
                {
                    item.fillAmount = 1 - (e.mousePosition.x / Screen.width);
                    TransparencyColor= new Color(255, 255, 255, 2 * (1 - (e.mousePosition.y / Screen.height)));
                }
              if(CurrentImageID!=-1)  References[CurrentImageID].color = TransparencyColor;


            }

            if (e.rawType == EventType.Used && e.button ==2)/// wheel ,puse
            {
                SetCurrentImage(++CurrentImageID);
            }  
            Camera.main.transform.Translate(Vector3.up, Space.World);
            Camera.main.transform.position = lastPos;
            e.Use();
            if (Input.GetKey(KeyCode.RightControl)) Capture();

        }
    }

    
   

}
