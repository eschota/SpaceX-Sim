using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

public class ResearchAndProductionManager : MonoBehaviour
{
    
    public static ResearchAndProductionManager instance;

    private void Awake()
    {
        if (instance != null) DestroyImmediate(instance);
        instance = this;
        TimeManager.EventChangeDay += OnChangeDay;

        ResearchesAvailable = ScenarioManager.instance.Researches;

        foreach (var r in ResearchesAvailable)
            foreach (var m in r.Modules)
                if (m.GetType() == typeof(BuildingResearchLab)
                 || m.GetType() == typeof(BuildingRocketLaunch)
                 || m.GetType() == typeof(BuildingProductionFactory))                    
                                                                         BuildingsAvailableForBuild.Add(m as BuildingUnit);



        CalculateResearchesCompletion();
        CalculateConstructions();
    }
    void OnChangeDay()
    { 
        CalculateResearchesCompletion();
        CalculateConstructions();
        CalculateResearchingProgress();
        RefreshResearchesUI();
        RefreshProductionUI();
        CalculateProductionModules();
    }
    #region Researches

    public List<Research> ResearchesCompleted = new List<Research>();
    public List<Research> ResearchesAvailable = new List<Research>();
    public List<Research> ResearchesInProgress = new List<Research>();
 
    public List<Module> ModulesAvailableForProduction = new List<Module>();        
    public List<Module> ModulesProduced = new List<Module>();        
    public List<BuildingUnit> BuildingsAvailableForBuild = new List<BuildingUnit>();
    
  
    public void CalculateResearchingProgress()
    {
        foreach (var item in ResearchesAvailable)
        {
            foreach (var lab in item.LabsResearchingNow)
            {
                if(lab.ConstructionCompletedPercentage>=100)
                item.TimeCompleted[(int)lab.CurrentBuildingClass] += lab.Productivity;
                RefreshResearchesUI();
            }
        }
    }
    public void CalculateResearchesCompletion()
    {
        List<Research> Temp = new List<Research>();
        foreach (var item in ResearchesAvailable)
        {
            if (item.Completed)
            {
                Temp.Add(item);
                Debug.Log("Research Completed: " + item.Name);
                ModulesAvailableForProduction.AddRange(item.Modules);
            }
        }
        foreach (var item in Temp)
        {
            ResearchesAvailable.Remove(item);
            ResearchesCompleted.Add(item);
            NotificationsManager.instance.AddNotification(item);
            RefreshResearchesUI();
        }
        UIResearchManager.instance.RefreshGloballFill();
    }

    
    void CalculateConstructions()
    {
        foreach (var item in GameManager.Buildings.FindAll(X=>X.isResearch==false))
        {
            if (item.ConstructionCompletedPercentage < 100)
            {
                if (item.isResearch == false) item.ConsctructionProcess++;
                if (item.ConstructionCompletedPercentage == 100)
                {
                    NotificationsManager.instance.AddNotification(item);
                     
                }
            }
        }
    }

    void RefreshResearchesUI()
    {
        foreach (var item in ResearchesAvailable)
        {
            item.researchButton.Refresh();
        }
    }
    #endregion
    #region Productions

    void CalculateProductionModules()
    {
        
    }
 void RefreshProductionUI()
    {
        
    }

    #endregion
}