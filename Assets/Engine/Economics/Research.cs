using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
 
[System.Serializable]
public class Research : Unit
{


    public List<Research> Dependances;
    public int[] TimeCost = { 100, 100, 100 };

    public Vector2 position;
    public Vector2 pivotStart;
    public Vector2 pivotEnd;
    public List<Module> ModulesOpen;
    public UIResearchButton researchButton;

    public int[] TimeCompleted = { 0, 0, 0 };

    private bool _completed;
    public bool Completed
    {
        get
        {
            if (TimeCompleted[0] != TimeCost[0]) _completed = false;
            else
            if (TimeCompleted[1] != TimeCost[1]) _completed = false;
            else
            if (TimeCompleted[2] != TimeCost[2]) _completed = false;
            else
                _completed = true;

            return _completed;

        }
        set
        {
            if (value == true)
            {
                TimeCompleted = TimeCost;
                _completed = true;
            }
            else
            {
                TimeCompleted = new int[] { 0, 0, 0 };
            }
            GameManager.EventUnit(this);
        }
    }
    public override void Awake()
    {
        Name = "Research " + ScenarioManager.instance.CurrentScenario.Researches.Count.ToString();

    }
    private void OnDestroy()
    {
        Destroy(researchButton.gameObject);
    }
    public void RestoreDependencies()
    {
        for (int i = 0; i < SD.DependesID.Count; i++)
        {
            Dependances.Add(ScenarioManager.instance.CurrentScenario.Researches.Find(X=>X.ID==SD.DependesID[i]));
        }
        researchButton.RebuildLinks();
        researchButton.Refresh();
    }

    public override void SaveJSON()
    {
        ID = GetInstanceID();
        position = researchButton.Rect.position;
        SaveDataResearch SD= new SaveDataResearch(GetInstanceID(), Name, researchButton.Rect.position, Dependances, ModulesOpen,TimeCost,Completed);
        string jsonData = JsonUtility.ToJson(SD, true);
        File.WriteAllText(Path.Combine(ScenarioManager.instance.CurrentScenario.CurrentFolder, GetInstanceID() + ".unit"), jsonData);
        Debug.Log("File Saved at: " + Path.Combine(ScenarioManager.instance.CurrentScenario.CurrentFolder, GetInstanceID() + ".unit"));
    }
    public SaveDataResearch SD;
    public override void LoadJSON()
    {
        SD= JsonUtility.FromJson<SaveDataResearch>( File.ReadAllText(JsonFilePath));
        ID = SD.ID;
        Name = SD.Name;
        TimeCost = SD.TimeCost;
        Completed = SD.Completed;
        researchButton.Rect.position = SD.RectPosition;
        Dependances = new List<Research>();
        Dependances.Clear();
        ModulesOpen= new List<Module>();
        ModulesOpen.Clear();
    }

    [System.Serializable]
    public class SaveDataResearch:SaveData
    {
        public Vector3 RectPosition;
        public List<int> DependesID;
        public List<int> ModulesID;
        public int[] TimeCost;
        public bool Completed;
        public SaveDataResearch(int id, string name, Vector3 rect, List<Research> dependences, List<Module> modules, int[] timeCost, bool completed)
        {
            ID = id; RectPosition = rect;
            DependesID = new List<int>();
            foreach (var item in dependences)
            {
                DependesID.Add(item.GetInstanceID());
            }
            ModulesID = new List<int>();
            foreach (var item in modules)
            {
                ModulesID.Add(item.GetInstanceID());
            }
            TimeCost = timeCost;
            Name = name;
            Completed = completed;
        }
    }
}
