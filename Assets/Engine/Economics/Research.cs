using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class Research : Unit
{

    public int[] TimeCost = { 100, 100, 100 };

    public Vector2 position;
    public Vector2 pivotStart;
    public Vector2 pivotEnd;

    public int[] TimeCompleted = { 0, 0, 0 };

    private bool _completed;

    [System.NonSerialized]
    public List<Research> Dependances = new List<Research>();
    public List<int> DependencesID = new List<int>();
    [System.NonSerialized]
    public List<Module> Modules = new List<Module>();
    public List<int> ModulesID = new List<int>();
    [System.NonSerialized]
    public UIResearchButton researchButton;

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

    public override void Awake()// 
    {

    }
    public override void OnDestroy()
    {
        
        ScenarioManager.instance.Researches.Remove(this);
    }
    public override void Ini()
    {
        name = Name = "Research " + ScenarioManager.instance.Researches.Count.ToString();
        ScenarioManager.instance.Researches.Add(this);
        researchButton = Instantiate(Resources.Load<UIResearchButton>("UI/ScenarioManager/ResearchButton"), ScenarioManager.instance.CameraPivot);
        researchButton.research = this;
        researchButton.name = name + "_Button";
        researchButton.Rect.position = localPosition;
        researchButton.Refresh();
    }
    public void RestoreDependencies()
    {
        Dependances.Clear(); Modules.Clear();
        foreach (var item in DependencesID) Dependances.Add(ScenarioManager.instance.Researches.Find(X => X.ID == item));
        foreach (var item in ModulesID) Modules.Add(ScenarioManager.instance.Modules.Find(X => X.ID == item));
    }

    public override void SaveJSON()
    {
        ID = GetInstanceID();
        ModulesID.Clear();
        DependencesID.Clear();
        localPosition = researchButton.Rect.position;
        foreach (var item in Modules) ModulesID.Add(item.GetInstanceID());
        foreach (var item in Dependances) DependencesID.Add(item.GetInstanceID());


        string jsonData = JsonUtility.ToJson(this, true);
        File.WriteAllText(Path.Combine(ScenarioManager.instance.CurrentScenario.CurrentFolder, ID + "." + GetType().ToString()), jsonData);
        Debug.Log("File Saved at: " + Path.Combine(ScenarioManager.instance.CurrentScenario.CurrentFolder, ID + "." + GetType().ToString()));
    }

  
}