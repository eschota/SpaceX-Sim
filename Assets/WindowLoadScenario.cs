using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowLoadScenario : UIWindows
{
    [SerializeField] ButtonSelectScenario ScenarionButtons;
    [SerializeField] Transform ContentRoot;

    List<ButtonSelectScenario> butns = new List<ButtonSelectScenario>();
    public void LoadScenarios()
    {
        for (int i = 0; i < butns.Count; i++)
        {
            Destroy(butns[i].gameObject);

        }
        butns.Clear();
        for (int i = 0; i < ScenarioManager.instance.LoadedScenarios.Count; i++)
        {
            butns.Add(Instantiate(ScenarionButtons, ContentRoot));
            butns[butns.Count - 1].scenario = ScenarioManager.instance.LoadedScenarios[i];
            butns[butns.Count - 1].ScenarioName.text = ScenarioManager.instance.LoadedScenarios[i].Name;
        }    
    }

    public void DeleteScenartio()
    {
        LoadScenarios();
        ScenarioManager.instance.CurrentScenario?.DeleteFilesAndFoldersOfScenario();
        ScenarioManager.instance.LoadedScenarios.Remove(ScenarioManager.instance.CurrentScenario);
        ScenarioManager.instance.CurrentScenario = null;
        LoadScenarios();

    }

    public void EditScenario()
    {
        if (ScenarioManager.instance.CurrentScenario != null)
        {
            ScenarioManager.instance.LoadScenarioUnits();
            ScenarioManager.instance.CurrentState = ScenarioManager.State.StartConditions;
        }
    }

    public void CreateNewScenario()
    {
        ScenarioManager.instance.CreateNewCurrentScenario();
        ScenarioManager.instance.CurrentState = ScenarioManager.State.StartConditions;
    }
    public static WindowLoadScenario instance;
        void Start()
    {
        instance = this;
    }
}
