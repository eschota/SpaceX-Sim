using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[ExecuteInEditMode]
public class Module : Unit // главное это префаб модуля, по нему мы ищем и создаем необходимые ГО в игре 
{
    public enum ModuleType { None=-1, Engine=0, FirstStage=1, SecondStage=2, IssModule=3 }
    public ModuleType type;
    
     
    public int Cost = 1;
    private Sprite _icon;
    public Sprite Icon
    {
        get
        {
            if (_icon == null) _icon=GetIcon();
            return _icon;
        }
        set
        {
            _icon = value;
        }
    }

    public int[] ProductionTime = new int[] { 10,10,10};
    public string Description="Description";
    [SerializeField] public Camera Camera;
    
   public string filename
    {
     get =>   Application.dataPath + "/Resources/Modules/Icons/" + gameObject.name ;
    }
        private void Reset()
    {
        gameObject.layer = 9;// Modules
        Name = gameObject.name;
        if (Camera == null) Camera = GetComponentInChildren<Camera>();
        Icon = Resources.Load<Sprite>("Modules/Icons/" + gameObject.name);
        Camera.targetTexture = Resources.Load<RenderTexture>("Modules/RT");

    }
    public void RenderIcon()
    {
        RenderTexture rt = Camera.targetTexture;

        byte[] bytes = toTexture2D(rt).EncodeToPNG();
        System.IO.File.WriteAllBytes(filename+".png", bytes);
        Debug.Log("ScreenShot Captured: " + filename);
    }
    Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(512, 512, TextureFormat.RGB24, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }
    private void Awake()
    {
        RenderIcon();
        GetIcon();
    }
    //ScreenCapture.CaptureScreenshot(filename + ".png");
    //Debug.Log("ScreenShot Captured: " + filename);



    [ContextMenu ("GetIcon")]
    public Sprite GetIcon()
    {
        return Resources.Load<Sprite>("Modules/Icons/"+gameObject.name);
       
    }
}
