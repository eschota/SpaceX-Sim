﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[ExecuteInEditMode]
public class Module : Unit
{
    public enum moduleType { None, Engine, Rockets, Modules, SpaceSuit, Equipment, Concept }
    public moduleType type;
     
    public int moduleCost = 1;
    public Sprite moduleIcon;
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
        if (moduleCamera == null) moduleCamera = Instantiate(Resources.Load("ModuleCamera") as GameObject, transform).GetComponent<Camera>() ;
    }
    public void RenderIcon()
    {
        
        ScreenCapture.CaptureScreenshot(filename + ".png");
        Debug.Log("ScreenShot Captured: " + filename);
    }

    private void Awake()
    {
        
    }
#if UNITY_EDITOR
    [ExecuteInEditMode]
    [CustomEditor(typeof(Module))]
    class RenderCam : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Render Icon"))
            {
                foreach (var item in FindObjectsOfType<Module>())
                {
                    item.modulePrefab.SetActive(false);
                    item.moduleCamera.enabled = false;
                }
                Selection.activeGameObject.GetComponent<Module>().modulePrefab.SetActive(true);
                Selection.activeGameObject.GetComponent<Module>().moduleCamera.enabled = true;
                Selection.activeGameObject.GetComponent<Module>().RenderIcon();
                AssetDatabase.Refresh();
                Selection.activeGameObject.GetComponent<Module>().moduleIcon = Resources.Load("Modules/Icons/" + Selection.activeGameObject.GetComponent<Module>().name) as Sprite;
            }
            
            // if (GUILayout.Button("IsolateThisModule"))
            // {
            //    foreach (var item in FindObjectsOfType<Module>())
            //    {
            //        item.Prefab.SetActive(false);
            //        item.IconCamera.enabled = false;
            //    }
            //    Selection.activeGameObject.GetComponent<Module>().Prefab.SetActive(true);
            //    Selection.activeGameObject.GetComponent<Module>().IconCamera.enabled = true;

            //} if (GUILayout.Button("Get Icon Resource"))
            // {
            //     Selection.activeGameObject.GetComponent<Module>().IconSprite=Resources.Load ("Modules/Icons/"+Selection.activeGameObject.GetComponent<Module>().name) as Texture2D;
            // }

        }
    }
#endif
}
