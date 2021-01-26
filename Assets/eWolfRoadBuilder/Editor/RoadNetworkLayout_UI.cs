using UnityEditor;
using UnityEngine;

namespace eWolfRoadBuilder
{
    [CustomEditor(typeof(RoadNetworkLayout))]
    public class RoadNetworkLayout_UI : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            RoadNetworkLayout roadScript = (RoadNetworkLayout)target;

            if (roadScript.CanRemoveAllNodes())
            {
                GUI.color = Color.red;
                if (GUILayout.Button("Remove all road Nodes"))
                {
                    roadScript.RemoveAllNodes();
                }
            }
            else
            {
                GUI.color = Color.yellow;
                if (GUILayout.Button("Start new road"))
                {
                    Selection.activeGameObject = roadScript.StartNewRoad();
                }
            }
        }
    }
}