#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Globalization;
using UnityEditor.AssetImporters;
using UnityEditor.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEditor.VersionControl;

using UnityEngine.Rendering;
public class OneClickPreAssetImporterFBX : AssetPostprocessor
{ 

    public void OnPreprocessMaterialDescription(MaterialDescription description, Material material, AnimationClip[] materialAnimation)
    {
      //  Debug.Log(description.materialName);
    }
    void OnPostprocessGameObjectWithUserProperties(GameObject go, string[] names, System.Object[] values)
    {

        if (Application.isPlaying) return;
        ModelImporter importer = (ModelImporter)assetImporter;
        var asset_name = Path.GetFileName(importer.assetPath);
        var asset_path = Path.GetFullPath(importer.assetPath);


        // Debug.LogFormat("OnPostprocessGameObjectWithUserProperties(go = {0}) asset = {1}", go.name, asset_name);

      



        for (int i = 0; i < names.Length; i++)
        {
            string propertyName = names[i];
            if (propertyName != "UDP3DSMAX") continue;



            object propertyValue = values[i];
            string[] str = propertyValue.ToString().Split(new string[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);
            try
            {
                foreach (var item in str)
                {
                    string p = item.Split('=')[0].ToLower();

                    string v = item.Split('=')[1].ToLower();
                    if (p.Length > 8)
                        if (p.Substring(0, 8) == "oneclick")
                        {

                            go.gameObject.name = assetPath;

                            if (go.gameObject.GetComponent<OneClickSceneManager>() == null)
                                go.gameObject.AddComponent<OneClickSceneManager>();
                            go.gameObject.GetComponent<OneClickSceneManager>().AssetFullPath = asset_path;
                        }
                    if (p == "idx ")
                    {
                        int idx = 0;
                        int.TryParse(item.Split('=')[1].ToLower(), out idx);
                        OCFBX ICI = go.gameObject.GetComponent<OCFBX>();
                        if (ICI == null)
                        {
                            ICI = go.gameObject.AddComponent<OCFBX>();
                            ICI.idx = idx;
                        }
                    }


                }
            }
            catch { };

        }

    }
}
#endif

