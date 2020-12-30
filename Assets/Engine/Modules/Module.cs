﻿using System.Collections;
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
    [SerializeField] public Camera _cam;
    
   public string filename
    {
     get =>   Application.dataPath + "/Resources/Modules/Icons/" + gameObject.name ;
    }
        private void Reset()
    {
        gameObject.layer = 9;// Modules
        Name = gameObject.name;
        if (_cam == null) _cam = GetComponentInChildren<Camera>();
        Icon = Resources.Load<Sprite>("Modules/Icons/" + gameObject.name);
        _cam.targetTexture = Resources.Load<RenderTexture>("Modules/RT");

    }
    public void RenderIcon()
    {

       

        ScreenCapture.CaptureScreenshot(filename + ".png");
        AssetDatabase.Refresh();
        AssetDatabase.ImportAsset(filename);
        AssetDatabase.Refresh(); 
    }
    //Texture2D toTexture2D(RenderTexture rTex)
    //{
    //    Texture2D tex = new Texture2D(512, 512, TextureFormat.RGB24, false);
    //    RenderTexture.active = rTex;
    //    tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
    //    tex.Apply();
    //    return tex;
    //}
    private void Awake()
    {
        RenderIcon();
        GetIcon();
    }

    public override void Update()
    {
        if (!Application.isPlaying)
            if (Selection.activeGameObject == gameObject)
            {
                foreach (var item in FindObjectsOfType<Module>())
                {
                    item._cam.gameObject.SetActive(false);
                }
                _cam.gameObject.SetActive(true);
                _cam.gameObject.SetActive(true);
                _cam.gameObject.SetActive(true);
            }
    }

    [ContextMenu("ChangeToSprite")]
    public void ChangeToSprite()
    {
        TextureImporter importer = AssetImporter.GetAtPath(filename) as TextureImporter;
        importer.textureType = TextureImporterType.Sprite;
        AssetDatabase.WriteImportSettingsIfDirty(filename);
    }
    [ContextMenu ("GetIcon")]
    public Sprite GetIcon()
    {
        return Resources.Load<Sprite>("Modules/Icons/"+gameObject.name);
       
    }



#if UNITY_EDITOR
    [ExecuteInEditMode]
    [CustomEditor(typeof(Module))]
   public class RenderCam : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Render Icon"))
            {
                Selection.activeGameObject.GetComponent<Module>().RenderIcon();
            }
        }
    }
#endif
}
