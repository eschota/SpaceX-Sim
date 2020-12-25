﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class WindowSelectModule : UIWindows
{
    [SerializeField] UISelectModuleButton ButtonSelectModule;
    [SerializeField] Transform SelectModuleButtons;
    [SerializeField] TMPro.TMP_Dropdown DropdownByTypes;
     
    [SerializeField] Button AddButton;
    private Research _research;
    private List<UISelectModuleButton> activeButtons = new List<UISelectModuleButton>();
    public Research CurrentResearchSelected
    {
        get => _research;
        set
        {
            _research = value;
            if (value == null)
            {
                CurrentMode = Mode.hide;
            }
            else
            {
                CurrentMode = Mode.show;
                OnChange(0);
            }
        }
    }

    
    private Module _currentSelectedModule;
    public Module CurrentSelectedModule
    {
        get => _currentSelectedModule;
        set
        {
            if (value == null) AddButton.gameObject.SetActive(false);
            else AddButton.gameObject.SetActive(true);
            _currentSelectedModule = value;
        }
    }
    public static WindowSelectModule instance;
    public List<Module> DefaultModules = new List<Module>();
    public List<Module> CurrentShow = new List<Module>();
    void Awake()
    {
        DropdownByTypes.onValueChanged.AddListener(OnChange);
        instance = this;
        CurrentResearchSelected = null;
        DefaultModules.AddRange(Resources.LoadAll("Modules/", typeof(Module)).Cast<Module>());
        Debug.Log("Modules Loaded: " + DefaultModules.Count);
    }
   public void AddModule(int id)
    {
        CurrentResearchSelected.ModulesOpen.Add(CurrentSelectedModule);
        CurrentResearchSelected.researchButton.Refresh();
        WindowEditResearch.instance.AddModuleButton();
    }

    void OnChange(int id)
    {
        
        for (int i = 0; i < activeButtons.Count; i++)
        {
            Destroy(activeButtons[i].gameObject);
        }
        CurrentSelectedModule = null;
        activeButtons.Clear();
        CurrentShow.Clear();
        CurrentShow= DefaultModules.FindAll(X => (int)X.type == id);
        
        for (int i = 0; i < CurrentShow.Count; i++)
        {
            activeButtons.Add( Instantiate(ButtonSelectModule, SelectModuleButtons));


            activeButtons[activeButtons.Count - 1].icon.sprite=CurrentShow[i].moduleIcon;
            

            activeButtons[activeButtons.Count - 1].ModuleName.text= CurrentShow[i].Name;

            activeButtons[activeButtons.Count - 1].thisModule= CurrentShow[i];


        }

    }
    public void Close()
    {
        CurrentResearchSelected = null;
        CurrentSelectedModule = null;

    }
}
