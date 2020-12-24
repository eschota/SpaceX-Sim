using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[ExecuteInEditMode]
public class Module : Unit
{
    public enum ModuleType { None=-1, Engine=0, FirstStage=1, SecondStage=2 }
    public ModuleType type;
    
     
    public int moduleCost = 1;
    public Sprite moduleIcon;
    public int[] ProductionTime = new int[] { 10,10,10};
    public string Description="Description";
    [SerializeField] public Camera moduleCamera;
    [SerializeField] public GameObject modulePrefab;
   public string filename
    {
     get =>   Application.dataPath + "/Resources/Modules/Icons/" + gameObject.name ;
    }
        private void Reset()
    {
        gameObject.layer = 9;// Modules
        Name = gameObject.name;
        if (moduleCamera == null) moduleCamera = GetComponentInChildren<Camera>();
        moduleIcon = Resources.Load<Sprite>("Modules/Icons/" + gameObject.name);


    }
    public void RenderIcon()
    {
        
        ScreenCapture.CaptureScreenshot(filename + ".png");
        Debug.Log("ScreenShot Captured: " + filename);
    }

   
    [ContextMenu ("GetIcon")]
    public void GetIcon()
    {
        moduleIcon = Resources.Load<Sprite>("Modules/Icons/"+gameObject.name);
       
    }
}
