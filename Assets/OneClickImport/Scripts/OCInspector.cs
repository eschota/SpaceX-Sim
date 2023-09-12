#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(OneClickSceneManager))]
public class OCInspector : Editor
{
#if UNITY_EDITOR
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Re/ImportScene"))
        {
            FindObjectOfType<OneClickSceneManager>().ImportScene3dsmax();
        }
        DrawDefaultInspector();
        OneClickSceneManager oc = FindObjectOfType<OneClickSceneManager>();
        if (oc == null) return;
        string str = "Meshes: " + oc.OCMeshes.Count + " Mats: " + oc.materials.Count + " Environment: ";
        EditorGUILayout.HelpBox(str, MessageType.Info);
    }
#endif
}
#endif