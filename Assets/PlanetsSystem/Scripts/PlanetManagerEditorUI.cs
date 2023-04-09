using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
[CustomEditor(typeof(PlanetManager))]
public class PlanetManagerEditorUI : Editor
{

        private PlanetManager _manager;
        public PlanetManager manager
    {

        get
        {
            if(_manager==null) _manager =FindObjectOfType<PlanetManager>();
            return _manager;
        }
    }
        public override void OnInspectorGUI()
        {

            GUILayout.Label("Total Planets: "+ manager.PlanetList.Count);
            GUILayout.Label("Current Planet: "+ (manager.CurrentPlanet+1).ToString());
        base.DrawDefaultInspector();
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Planet"))
        {
            manager.AddPlanet();
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Remove Planet"))
               manager.RemovePlanet();

        EditorGUILayout.Space();
        if (GUILayout.Button("Edit Planet"))
                Debug.Log("It's alive: " + target.name);
        GUILayout.EndHorizontal();
    }
    
}
