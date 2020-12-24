using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEditModule : UIWindows
{
    private Module _currentModule;
 
    [SerializeField] public UIModuleParam ModuleParam;
    public  Module currentModule
    {
        get => _currentModule;
        set
        {
            if (value == null)
            {
                Hide();
            }
            else
            {
                CG.alpha = 1;
                LoadModule();
            }
        }
    }
    // Update is called once per frame
    private void Awake()
    {
        currentModule = null;
    }

    public static void LoadModule()
    {

    }
    public void CloseWindow()
    {
        currentModule = null;
    }
}
