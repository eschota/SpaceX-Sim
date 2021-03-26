using UnityEditor;
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
        //Module mod;
        //string modName = "";

        //GUILayout.Label("Выделенный модуль", EditorStyles.boldLabel);

      
            if (GUILayout.Button("RenderIcon"))
        {
            foreach (var item in Resources.LoadAll<Module>("Modules"))
                {
                   if (item.PrefabName == Selection.activeGameObject.name)
                {
                    ScreenCapture.CaptureScreenshot(item.IconFilePath);
                    Debug.Log("Captured Screenshot :" + item.IconFilePath);
                }
            }
                

        }
            //if (GUILayout.Button("GetIconFromPreview"))
        //{

        //    mod = Selection.activeGameObject.GetComponent<Module>();
        //    if (mod == null) return;

        //    Texture2D tex= AssetPreview.GetAssetPreview(mod.Prefab) as Texture2D;

        //    System.IO.File.WriteAllBytes(mod.IconFilePath, tex.EncodeToPNG());
        //    AssetDatabase.Refresh();
        //    AssetDatabase.ImportAsset(mod.IconFilePath);
        
        //} 
        
        //if (GUILayout.Button("GetIconFromPreviewALL"))
        //{

        //    foreach (var item in Resources.LoadAll<Module>("Modules"))
        //    {
        //        item.OnValidate();
        //    }

        //}










    }

    //EditorGUILayout.button
    //groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
    //myBool = EditorGUILayout.Toggle("Toggle", myBool);
    //myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
    //EditorGUILayout.EndToggleGroup();





}