using eWolfRoadBuilder.Terrains;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace eWolfRoadBuilder
{
    /// <summary>
    /// The Road Builder UI
    /// </summary>
    [CustomEditor(typeof(RoadBuilder))]
    public class RoadBuilder_UI : Editor
    {
        /// <summary>
        /// The inspector for the RoadBuilder
        /// </summary>
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            RoadBuilder roadBuilder = (RoadBuilder)target;
            GUI.color = Color.red;
            if (GUILayout.Button("Remove mesh"))
            {
                roadBuilder.RemoveRoadMesh(true);
                SetSceneDirty();
            }

            GUI.color = Color.green;
            if (GUILayout.Button("Create Mesh"))
            {
                roadBuilder.CreateMesh(true);
                SetSceneDirty();
            }

            GUI.color = Color.yellow;
            if (GUILayout.Button("Repair layout"))
            {
                for (int i = 0; i < 5; i++)
                {
                    roadBuilder.RepairAllNodes();
                }
                Debug.Log("Repaired all nodes in layout.");
                EditorGUIUtility.ExitGUI();
            }

            if (!roadBuilder.HasTerrainModifier())
            {
                GUI.color = Color.green;
                if (GUILayout.Button("Add terrain modifier"))
                {
                    roadBuilder.AddTerrainModifier();
                    SetSceneDirty();
                }
            }
            else
            {
                GUI.color = Color.green;
                if (GUILayout.Button("Modify Terrain to match road"))
                {
                    roadBuilder.ModifyTerrain();
                    SetSceneDirty();
                }

                TerrainModifier tm = roadBuilder.GetComponent<TerrainModifier>();
                if (tm != null && tm.HasStoredTerrain)
                {
                    if (GUILayout.Button("Restore previous terrain"))
                    {
                        tm.RestoreTerrain();
                        SetSceneDirty();
                    }
                }

                GUI.color = Color.red;
                if (GUILayout.Button("Remove terrain modifier"))
                {
                    roadBuilder.RemoveTerrainModifier();
                    SetSceneDirty();
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