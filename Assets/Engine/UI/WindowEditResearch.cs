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

    
    private Research _research;
    public Research CurrentResearchSelected
    {
        get => _research;
        set
        {

            

            if (_research != value)
            {
                WindowSelectModule.instance.CurrentResearchSelected = null;
                WindowSelectModule.instance.CurrentSelectedModule = null;

            }

            _research = value;

            if (value == null)
            {
                Hide();
                WindowSelectModule.instance.CurrentResearchSelected = null;
                WindowSelectModule.instance.CurrentSelectedModule = null;
            }
            else
            {

                Refresh();


                value.researchButton.Refresh();
                Show();
            }
        }
    }
    public void Refresh()
    {
        ClearModulesButton();
        foreach (var item in CurrentResearchSelected.ModulesOpen)
        {
            AddModuleButton(item);
        }
        ResearchName.text = CurrentResearchSelected.Name;
        Light.text = CurrentResearchSelected.TimeCost[0].ToString();
        Medium.text = CurrentResearchSelected.TimeCost[1].ToString();
        Heavy.text = CurrentResearchSelected.TimeCost[2].ToString();
        Completed.isOn = CurrentResearchSelected.Completed;
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
        if (CurrentResearchSelected == null) return;
        CurrentResearchSelected.Name = ResearchName.text;

        int light = 0;
        int.TryParse(Light.text,out light);
        int medium = 0;
        int.TryParse(Medium.text,out medium);
        int heavy = 0;
        int.TryParse(Heavy.text,out heavy);

        CurrentResearchSelected.TimeCost = new[] {light,medium,heavy};
        CurrentResearchSelected.Completed = Completed.isOn;
        CurrentResearchSelected.researchButton.Refresh();
    }
    public static WindowEditResearch instance;
    void Awake()
    {
        instance = this;
        Hide();
    }
    public void DeleteResearch()
    {
        Research temp = ScenarioManager.instance.CurrentResearcLink.CurrentResearchSelected;
        foreach (var item in ScenarioManager.instance.CurrentScenario.Researches.FindAll(X => X.Dependances.Contains(temp)))//удаляем зависимомсти от этого рисерча
        {
            item.Dependances.Remove(temp);
            item.researchButton.RebuildLinks();
        }

            
            ScenarioManager.instance.buttons.Remove(ScenarioManager.instance.CurrentResearcLink.CurrentResearchSelected.researchButton);//удаляем кнопки
        ScenarioManager.instance.CurrentScenario.Researches.Remove(ScenarioManager.instance.CurrentResearcLink.CurrentResearchSelected);//удаляем рисерчи
        Destroy(ScenarioManager.instance.CurrentResearcLink.CurrentResearchSelected.researchButton.gameObject);
        Destroy(ScenarioManager.instance.CurrentResearcLink.CurrentResearchSelected.gameObject);
        ScenarioManager.instance.CurrentResearcLink.CurrentResearchSelected = null;
        
        Hide();
    }
}
