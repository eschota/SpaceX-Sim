﻿using System.Collections;
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
                                                                         BuildingsAvailable.Add(m as BuildingUnit);



        CalculateResearchesProgress();
        CalculateConstructions();
    }
    void OnChangeDay()
    { 
        CalculateResearchesProgress();
        CalculateConstructions();
    }
    

    public List<Research> ResearchesCompleted = new List<Research>();
    public List<Research> ResearchesAvailable = new List<Research>();
    public List<Research> ResearchesInProgress = new List<Research>();
 
    public List<Module> ModulesAvailable = new List<Module>();        
    public List<BuildingUnit> BuildingsAvailable = new List<BuildingUnit>();
    
    public List<BuildingResearchLab> ResearchLabs = new List<BuildingResearchLab>();
    public List<BuildingRocketLaunch> RocketLauches = new List<BuildingRocketLaunch>();
    public List<BuildingProductionFactory> ProductionFactories = new List<BuildingProductionFactory>();
    

    public void AddBuilding( BuildingUnit unit)
    {
        if (unit as BuildingResearchLab != null) ResearchLabs.Add(unit as BuildingResearchLab);
        if (unit as BuildingRocketLaunch != null) RocketLauches.Add(unit as BuildingRocketLaunch);
        if (unit as BuildingProductionFactory != null) ProductionFactories.Add(unit as BuildingProductionFactory);
    }
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


    void CalculateConstructions()
    {
        foreach (var item in GameManager.Buildings)
        {
         if(item.isResearch==false)   item.ConsctructionProcess++;
        }
    }
}