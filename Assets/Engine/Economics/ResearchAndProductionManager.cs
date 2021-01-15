using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class ResearchAndProductionManager : MonoBehaviour
{
    public static ResearchAndProductionManager instance;   
    private void Awake()
    {
        if (instance != null) DestroyImmediate(instance);
            instance = this;
        TimeManager.EventChangeDay += OnChangeDay;

        ResearchesAvailable = ScenarioManager.instance.Researches;
        CalculateResearchesProgress();
    }
    void OnChangeDay()
    { 
        CalculateResearchesProgress();
    }
    

    public List<Research> ResearchesCompleted = new List<Research>();
    public List<Research> ResearchesAvailable = new List<Research>();
    public List<Research> ResearchesInProgress = new List<Research>();
    public void CalculateResearchesProgress()
    {
        List<Research> Temp = new List<Research>();
        foreach (var item in ResearchesAvailable)
        {
            if (item.Completed)
            {
                Temp.Add(item);                
                Debug.Log("Research Completed: " + item.Name);
                ModulesAvailable.AddRange(item.Modules);
            }
        }
        foreach (var item in Temp)
        {
            ResearchesAvailable.Remove(item);
            ResearchesCompleted.Add(item);
        }
    }
    public List<Module> ModulesAvailable = new List<Module>();        


}