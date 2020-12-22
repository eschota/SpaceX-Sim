using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class ScenarioManager : MonoBehaviour
{
    public static event Action EventChangeState; 
    public enum State {None, StartConditions,Researches,PoliticMap  }
    private static State _currentState;
    [SerializeField] Transform CameraPivot;
    public static State CurrentState
    {
        get => _currentState;
        set
        {

            switch (value)
            {
                
            }

            Debug.Log(string.Format("<color=blue> State changed " + _currentState + ":=" + value + "</color>"));
            _currentState = value;
            if (EventChangeState != null) EventChangeState();

        }
    }
    public static ScenarioManager instance;
    void Start()
    {
        instance = this;
    }

   
    #region  Scenario
    [SerializeField] TMPro.TMP_InputField ScenarioName;
    [SerializeField] TMPro.TMP_InputField ScenarioStartDay;
    [SerializeField] TMPro.TMP_InputField ScenarioStartMonth;
    [SerializeField] TMPro.TMP_InputField ScenarioStartYear;
    [SerializeField] TMPro.TMP_InputField ScenarioStartBalance;
    [SerializeField] public UIEditResearch CurrentResearcLink;
    public Scenario CurrentScenario;
    public void CreateScenarioButtonClick()
    {
        CurrentScenario = new Scenario( ScenarioName.text, int.Parse(ScenarioStartDay.text), int.Parse(ScenarioStartMonth.text), int.Parse(ScenarioStartYear.text), int.Parse(ScenarioStartBalance.text));
        CurrentScenario.SaveNewScenario();
        foreach (var item in Researches) item.Save();
        foreach (var item in Modules) item.Save();
        
    }

     public void DeselectResearch()
    {
        ScenarioManager.instance.CurrentResearcLink.CurrentResearchSelected = null;
    }
    public void AddModule()
    {
        UISelectModule.instance.CurrentResearchSelected = CurrentResearcLink.CurrentResearchSelected;
    }
    public List<Research> Researches = new List<Research>();
    public List<Module> Modules= new List<Module>();
    public void AddResearch()
    {
        Researches.Add( Instantiate( Resources.Load("UI/UIResearchButton") as GameObject,CameraPivot).GetComponent<Research>());
        //  Researches[Researches.Count - 1].name = CurrentResearch.ResearchName.text;
        if (Researches.Count > 1)
        {
            Researches[Researches.Count - 1].Dependances.Add(Researches[Researches.Count - 2]);
            Researches[Researches.Count - 1].researchButton.Rect.position = Researches[Researches.Count - 2].researchButton.Rect.position+ new Vector3(450,0,0);
        }
        CurrentResearcLink.CurrentResearchSelected = Researches[Researches.Count - 1];
    }
    public void DeleteResearch()
    {
        Research temp = CurrentResearcLink.CurrentResearchSelected;
        Researches.Remove(CurrentResearcLink.CurrentResearchSelected);
        Destroy(CurrentResearcLink.CurrentResearchSelected.researchButton.gameObject);
        Destroy(CurrentResearcLink.CurrentResearchSelected.gameObject);
        CurrentResearcLink.CurrentResearchSelected = null;
    }
    [System.Serializable]
    public class Scenario
    {
        public int[] StartDate;
        public string Name;
        public int StartBalance; 
        public Scenario ( string _Name, int  _StartDay, int _StartMonth, int _StartYear, int _StartBalance)
        {
            Name = _Name;
            StartBalance = _StartBalance;
            StartDate = new int[3];
            StartDate[0] = _StartDay;
            StartDate[1] = _StartMonth;
            StartDate[2] = _StartYear;
        }
        public void SaveNewScenario()
        {
            string jsonData = JsonUtility.ToJson(this, true);
            string Folder = Application.streamingAssetsPath + "/Scenarios/";
           
            File.WriteAllText(Folder+ "/" + Name + ".scenario", jsonData);
            Debug.Log("File Saved at: " +Folder);
        }
    }
    #endregion
}
