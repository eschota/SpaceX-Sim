using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEditModule : MonoBehaviour
{
    private Module _currentModule;
    [SerializeField] public CanvasGroup CG;
    [SerializeField] public UIModuleParam ModuleParam;
    public  Module currentModule
    {
        get => _currentModule;
        set
        {
            if (value == null)
            {
                CG.alpha = 0;
            }
            else
            {
                CG.alpha = 1;
                LoadModule();
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

    }

    public static void LoadModule()
    {

    }
    public void CloseWindow()
    {
        currentModule = null;
    }
}
