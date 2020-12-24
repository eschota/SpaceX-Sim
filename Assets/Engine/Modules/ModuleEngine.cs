using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[ExecuteInEditMode]
public class ModuleEngine : Module
{
    public int Power = 100;
    public int FuelTaking = 1;



    


















#if UNITY_EDITOR
    [ExecuteInEditMode]
    [CustomEditor(typeof(ModuleEngine))]
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
                Selection.activeGameObject.GetComponent<ModuleEngine>().modulePrefab.SetActive(true);
                Selection.activeGameObject.GetComponent<ModuleEngine>().moduleCamera.enabled = true;
                Selection.activeGameObject.GetComponent<ModuleEngine>().RenderIcon();
                AssetDatabase.Refresh();
                Selection.activeGameObject.GetComponent<ModuleEngine>().moduleIcon = Resources.Load("Modules/Icons/" + Selection.activeGameObject.GetComponent<ModuleEngine>().name) as Sprite;
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
