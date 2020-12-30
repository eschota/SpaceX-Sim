using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
[ExecuteInEditMode]
public class ModuleEngine : Module
{
    public int Power = 100;
    public int FuelTaking = 1;







    public SaveDataEngine SD;
    public override void SaveJSON()
    {
        ID = GetInstanceID();

        SD = new SaveDataEngine( Name, GetInstanceID(), Prefab.name , Cost, ProductionTime, FuelTaking, Power);
        string jsonData = JsonUtility.ToJson(SD, true);
        File.WriteAllText(Path.Combine(ScenarioManager.instance.CurrentScenario.CurrentFolder, GetInstanceID() + ".Module"), jsonData);
        Debug.Log("File Saved at: " + Path.Combine(ScenarioManager.instance.CurrentScenario.CurrentFolder, GetInstanceID() + ".Module"));
    }

    public override void LoadJSON()
    {
        SD = JsonUtility.FromJson<SaveDataEngine>(File.ReadAllText(FilePath));
        ID = SD.ID;
        Name = SD.Name;
        ProductionTime = SD.ProductionTime;
        Power = SD.Power;
        FuelTaking = SD.FuelTaking;
        Cost = SD.Cost;
        Prefab = Resources.Load<GameObject>("Modules/" + SD.PrefabName);
        foreach (var item in ScenarioManager.instance.CurrentScenario.Researches)
        {
            if (item.SD.ModulesID.Exists(X => X == SD.ID))
            {
                item.ModulesOpen.Add(this);
                item.researchButton.Refresh();
                transform.SetParent(item.transform);
            }
        }
    }


    [System.Serializable]
    public class SaveDataEngine: SaveData
    {
        
        public int Cost;
        public int[] ProductionTime;
        public int Power;
        public int FuelTaking;
        public SaveDataEngine(string name, int id, string prefabName, int cost, int []productionTime, int fuelTaking, int power)
        {
            ID = id; Name = name; PrefabName = prefabName; Cost = cost; ProductionTime = productionTime; Power = power; FuelTaking = fuelTaking;
        }
    }







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
                //foreach (var item in FindObjectsOfType<Module>())
                //{
                //    item.Prefab?.SetActive(false);
                //    item.Camera.enabled = false;
                //}
                Selection.activeGameObject.GetComponent<ModuleEngine>().Prefab.SetActive(true);
                Selection.activeGameObject.GetComponent<ModuleEngine>().Camera.enabled = true;
                Selection.activeGameObject.GetComponent<ModuleEngine>().RenderIcon();
                AssetDatabase.Refresh();
                Selection.activeGameObject.GetComponent<ModuleEngine>().Icon = Resources.Load("Modules/Icons/" + Selection.activeGameObject.GetComponent<ModuleEngine>().name) as Sprite;
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
