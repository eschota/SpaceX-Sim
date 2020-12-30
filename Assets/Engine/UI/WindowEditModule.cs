using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowEditModule : UIWindows
{
    
 
    [SerializeField] public UIModuleParam ModuleParam;

    private Module _currentModule;
    public  Module currentModule
    {
        get => _currentModule;
        set
        {
            _currentModule = value;
            if (value == null)
            {
                CurrentMode = Mode.hide;
            }
            else
            {
                CurrentMode = Mode.show;
                LoadModule();
            }
            
        }
    }
    public static WindowEditModule instance;
    private void Awake()
    {
        instance = this;
        Hide();
    }

    public void LoadModule()
    {
        WindowSelectModule.instance.Hide();
        Name.text = currentModule.Name;
    }
    public void CloseWindow()
    {
        currentModule = null;
    }

    public void DeleteModuleFromResearch()
    {
        WindowEditResearch.instance.CurrentResearch.ModulesOpen.Remove(currentModule);
        WindowEditResearch.instance.CurrentResearch.researchButton.Refresh();
        WindowEditResearch.instance.RefreshWindow();
        Hide();
    }




    [SerializeField] TMPro.TextMeshProUGUI Name;
}
