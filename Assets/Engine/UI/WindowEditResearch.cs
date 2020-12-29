using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
public class WindowEditResearch : UIWindows
{
    [SerializeField] public TMPro.TMP_InputField ResearchName;
    [SerializeField] public TMPro.TMP_InputField Light;
    [SerializeField] public TMPro.TMP_InputField Medium;
    [SerializeField] public TMPro.TMP_InputField Heavy;
     
    [SerializeField] public Toggle Completed;
    [SerializeField] public UIButtonSelectModule ButtonSelectModule;
    [SerializeField] public Transform SelectModulesTransform;

    
    private Research _currentResearch;
    public Research CurrentResearch
    {
        get => _currentResearch;
        set
        {

            

            if (_currentResearch != value)
            {
                WindowSelectModule.instance.CurrentResearchSelected = null;
                WindowSelectModule.instance.CurrentSelectedModule = null;

            }

            _currentResearch = value;

            if (value == null)
            {
                Hide();
                WindowSelectModule.instance.CurrentResearchSelected = null;
                WindowSelectModule.instance.CurrentSelectedModule = null;
            }
            else
            {
                RefreshWindow();
                CurrentResearch.researchButton.RebuildLinks();

               
                Show();
            }
        }
    }
    public void RefreshWindow()
    {
        ClearModulesButton();
        foreach (var item in CurrentResearch.ModulesOpen)
        {
            AddModuleButton(item);
        }
        ResearchName.SetTextWithoutNotify( CurrentResearch.Name);
        Light.SetTextWithoutNotify(CurrentResearch.TimeCost[0].ToString());
        Medium.SetTextWithoutNotify(CurrentResearch.TimeCost[1].ToString());
        Heavy.SetTextWithoutNotify(  CurrentResearch.TimeCost[2].ToString());
        Completed.SetIsOnWithoutNotify( CurrentResearch.Completed);
    }
    public List<UIButtonSelectModule> UIModuleButtons = new List<UIButtonSelectModule>();
    public void AddModuleButton(Module module)
    {
        UIModuleButtons.Add(   Instantiate(ButtonSelectModule, SelectModulesTransform));
        UIModuleButtons[UIModuleButtons.Count - 1].module = module;
    }
    public void ClearModulesButton()
    {
        for (int i = 0; i < UIModuleButtons.Count; i++)
        {
            Destroy(UIModuleButtons[i].gameObject);
        }
        UIModuleButtons.Clear();
    }

    public void OnEditResearch()
    {
        if (CurrentResearch == null) return;

        Debug.Log("Ebal Koney");
        CurrentResearch.Name = ResearchName.text;
        CurrentResearch.gameObject.name = CurrentResearch.Name;
        int light = 0;
        int.TryParse(Light.text,out light);
        int medium = 0;
        int.TryParse(Medium.text,out medium);
        int heavy = 0;
        int.TryParse(Heavy.text,out heavy);
        
        CurrentResearch.TimeCost = new[] {light,medium,heavy};
        CurrentResearch.Completed = Completed.isOn;
        CurrentResearch.researchButton.Refresh();
    }
    public static WindowEditResearch instance;
    void Awake()
    {
        instance = this;
        Hide();
    }
    public void DeleteResearch()
    {
        Research temp = CurrentResearch;
        foreach (var item in ScenarioManager.instance.CurrentScenario.Researches.FindAll(X => X.Dependances.Contains(temp)))//удаляем зависимомсти от этого рисерча
        {
            item.Dependances.Remove(temp);
            item.researchButton.RebuildLinks();
        }

            
            ScenarioManager.instance.buttons.Remove(CurrentResearch.researchButton);//удаляем кнопки
        ScenarioManager.instance.CurrentScenario.Researches.Remove(CurrentResearch);//удаляем рисерчи
        Destroy(CurrentResearch.researchButton.gameObject);
        Destroy(CurrentResearch.gameObject);
        CurrentResearch = null;
        
        Hide();
    }
}
