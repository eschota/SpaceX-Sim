#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(OCVolumeManager))]
public class OCInspectorVolumeManager : Editor
{ 
    public override void OnInspectorGUI()
    {
        //if (GUILayout.Button("Bake All Reflection Probes"))
        //{
        //    FindObjectOfType<OCVolumeManager>().BakeAllReflectionProbes();
        //}
        EditorGUILayout.LabelField("Reflection Probes: "+ FindObjectOfType<OCVolumeManager>().transform.childCount);
        DrawDefaultInspector();
        
    } 
}
#endif