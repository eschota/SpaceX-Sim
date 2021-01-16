﻿using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class SECSEditor : EditorWindow
{
    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;

    // Add menu item named "My Window" to the Window menu
    [MenuItem("Window/SECS")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(SECSEditor));
    }

    void OnGUI()
    {
        Module mod;
        string modName = "";

        GUILayout.Label("Выделенный модуль", EditorStyles.boldLabel);

      
            if (GUILayout.Button("RenderIcon"))
        {

            mod = Selection.activeGameObject.GetComponent<Module>();
            if (mod == null) return;
            ScreenCapture.CaptureScreenshot(mod.IconFilePath);
            AssetDatabase.Refresh();
            AssetDatabase.ImportAsset(mod.IconFilePath);
            AssetDatabase.Refresh();


            ////string path = AssetDatabase.getasseta(mod.IconFilePath + ".png");
            //Texture2D t = AssetDatabase.LoadAssetAtPath(mod.IconFilePath + ".png", typeof(Texture2D)) as Texture2D;
            //string path = AssetDatabase.GetAssetPath(t);
            //TextureImporter importer= AssetImporter.GetAtPath(path) as TextureImporter;

            //importer.textureType = TextureImporterType.Sprite;
            //importer.filterMode = FilterMode.Trilinear;

            //EditorUtility.SetDirty(importer);
            //AssetDatabase.SaveAssets();
            //AssetDatabase.Refresh();
        

        }     if (GUILayout.Button("GetIconFromPreview"))
        {

            mod = Selection.activeGameObject.GetComponent<Module>();
            if (mod == null) return;

            Texture2D tex= AssetPreview.GetAssetPreview(mod.Prefab) as Texture2D;

            System.IO.File.WriteAllBytes(mod.IconFilePath, tex.EncodeToPNG());
            AssetDatabase.Refresh();
            AssetDatabase.ImportAsset(mod.IconFilePath);
        
        } 
        
        if (GUILayout.Button("GetIconFromPreviewALL"))
        {

            foreach (var item in Resources.LoadAll<Module>("Modules"))
            {
                if (item.Prefab == null) { Debug.LogError(item.name); return; }
                item.PrefabName = item.Prefab.name;
                
             
                Texture2D tex = AssetPreview.GetAssetPreview(item.Prefab) as Texture2D;


           //     if (System.IO.File.Exists(item.IconFilePath)) System.IO.File.Delete(item.IconFilePath);
                System.IO.File.WriteAllBytes(item.IconFilePath, tex.EncodeToPNG());
              
            }
            AssetDatabase.Refresh();

            Debug.Log("TotalFiles: " + Resources.LoadAll<Module>("Modules").Length);


        }










    }

    //EditorGUILayout.button
    //groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
    //myBool = EditorGUILayout.Toggle("Toggle", myBool);
    //myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
    //EditorGUILayout.EndToggleGroup();





}