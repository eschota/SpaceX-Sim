using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace eWolfRoadBuilder
{
    [CustomEditor(typeof(RoadNetworkNode))]
    [CanEditMultipleObjects]
    public class RoadNetworkNode_UI : Editor
    {
        /// <summary>
        /// Draw the inspector with extra options
        /// </summary>
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            RoadNetworkNode roadScript = (RoadNetworkNode)target;

            if (Selection.gameObjects.Length == 1)
            {
                GameObject oa = Selection.gameObjects[0];

                RoadNetworkNode rnn2 = oa.GetComponent<RoadNetworkNode>();
                if (rnn2 != null)
                {
                    if (rnn2.Details.CanExtrudeRoad())
                    {
                        GUI.color = Color.yellow;
                        if (GUILayout.Button("Extrude road"))
                        {
                            Selection.activeGameObject = roadScript.ExtrudeRoad();
                            SetSceneDirty();
                            EditorGUIUtility.ExitGUI();
                            return;
                        }
                    }

                    GUI.color = Color.red;
                    if (GUILayout.Button("Delete Node"))
                    {
                        roadScript.DeleteNode();
                        SetSceneDirty();
                        EditorGUIUtility.ExitGUI();
                        return;
                    }
                }
            }
            else
            {
                GUI.color = Color.red;
                if (GUILayout.Button("Delete All Selected Nodes"))
                {
                    foreach (GameObject o in Selection.gameObjects)
                    {
                        RoadNetworkNode rnn = o.GetComponent<RoadNetworkNode>();
                        if (rnn != null)
                        {
                            rnn.DeleteNode();
                        }
                    }
                    SetSceneDirty();
                    EditorGUIUtility.ExitGUI();
                    return;
                }
            }

            if (Selection.gameObjects.Length == 2)
            {
                List<RoadNetworkNode> nodes = new List<RoadNetworkNode>();
                foreach (GameObject o in Selection.gameObjects)
                {
                    RoadNetworkNode rnn = o.GetComponent<RoadNetworkNode>();
                    if (rnn != null)
                        nodes.Add(rnn);
                }

                GUI.color = Color.yellow;
                if (GUILayout.Button("Connect Nodes"))
                {
                    roadScript.ConnectNodes(nodes);
                    Selection.activeGameObject = Selection.gameObjects[0];
                    SetSceneDirty();
                    return;
                }

                if (GUILayout.Button("Insert node between selected nodes"))
                {
                    Selection.activeGameObject = roadScript.InsertNewNode(nodes);
                    SetSceneDirty();
                    return;
                }
            }

            GUI.color = Color.green;
            if (GUILayout.Button("Create Mesh"))
            {
                roadScript.CreateMesh();
                SetSceneDirty();
            }

            if (!roadScript.HasOverridenCrossSection())
            {
                GUI.color = Color.green;
                if (GUILayout.Button("Override Cross Section"))
                {
                    foreach (GameObject o in Selection.gameObjects)
                    {
                        RoadNetworkNode rnn = o.GetComponent<RoadNetworkNode>();
                        if (rnn != null)
                        {
                            rnn.AddOverridableCrossSections();
                            SetSceneDirty();
                        }
                    }
                }
            }
            else
            {
                GUI.color = Color.red;
                if (GUILayout.Button("Remove Cross section"))
                {
                    foreach (GameObject o in Selection.gameObjects)
                    {
                        RoadNetworkNode rnn = o.GetComponent<RoadNetworkNode>();
                        if (rnn != null)
                        {
                            rnn.RemoveOverridableCrossSections();
                            SetSceneDirty();
                        }
                    }

                    EditorGUIUtility.ExitGUI();
                }
            }

            if (!roadScript.HasOverridenMaterialDetails())
            {
                GUI.color = Color.green;
                if (GUILayout.Button("Override Material Details"))
                {
                    foreach (GameObject o in Selection.gameObjects)
                    {
                        RoadNetworkNode rnn = o.GetComponent<RoadNetworkNode>();
                        if (rnn != null)
                        {
                            rnn.AddOverridableMaterialsDetails();
                            SetSceneDirty();
                        }
                    }
                }
            }
            else
            {
                GUI.color = Color.red;
                if (GUILayout.Button("Remove Material Details"))
                {
                    foreach (GameObject o in Selection.gameObjects)
                    {
                        RoadNetworkNode rnn = o.GetComponent<RoadNetworkNode>();
                        if (rnn != null)
                        {
                            rnn.RemoveOverridableMaterialDetails();
                            SetSceneDirty();
                        }
                    }

                    EditorGUIUtility.ExitGUI();
                }
            }
        }

        /// <summary>
        /// Make the scene as dirty so it will request the save scene option when you close the scene
        /// </summary>
        private void SetSceneDirty()
        {
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}