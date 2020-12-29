using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEditor;
public class ScenarioManager : MonoBehaviour
{
    public string ScenariosFolder
    {
        get => Application.streamingAssetsPath + "/Scenarios/";
    }
    public string CurrentScenarioFolder
    {
        get => ScenariosFolder + CurrentScenario.Name    ;
    }
    public static event Action EventChangeState; 
    public enum State {None, StartConditions,Researches,PoliticMap, LoadScenario  }
    private   State _currentState;
    [SerializeField] Transform CameraPivot;
    public   State CurrentState
    {
        get => _currentState;
        set
        {

            switch (value)
            {
                case State.None:
                    break;
                case State.StartConditions:
                    break;
                case State.Researches:
                    break;
                case State.PoliticMap:
                    break;
                case State.LoadScenario:
                    LoadScenarios();
                    break;
                default:
                    break;
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
    [SerializeField] public WindowEditResearch CurrentResearcLink;
    public Scenario CurrentScenario;
    public void CreateNewCurrentScenario()
    {
        CurrentScenario = new Scenario( ScenarioName.text, int.Parse(ScenarioStartDay.text), int.Parse(ScenarioStartMonth.text), int.Parse(ScenarioStartYear.text), int.Parse(ScenarioStartBalance.text));
        CurrentScenario.SaveNewScenario();
        
        
    }
     public void SaveCurrentScenario()
    {
        CurrentScenario.SaveNewScenario();
      
        
    }

    public List<Scenario> LoadedScenarios = new List<Scenario>();
    public void LoadScenarios()
    {
        LoadedScenarios.Clear();
        DirectoryInfo dir = new DirectoryInfo(ScenariosFolder);
        FileInfo[] info = dir.GetFiles("*.scenario");
       
        foreach (FileInfo f in info)
        {
            string temp = File.ReadAllText(ScenariosFolder+"/"+(f.Name));
          //  Debug.Log("Scenario Loaded: " + f.Name+"  "+ temp);
            LoadedScenarios.Add(JsonUtility.FromJson<Scenario>(temp));
     } 
         Debug.Log("Scenarios Loaded: " + LoadedScenarios.Count);
        WindowLoadScenario.instance.LoadScenarios();
    }
    

    private void Update()
    {
        if(Input.GetMouseButtonDown(1)) ScenarioManager.instance.CurrentResearcLink.CurrentResearchSelected = null; 
    }
    public void AddModule()
    {
        WindowSelectModule.instance.CurrentResearchSelected = CurrentResearcLink.CurrentResearchSelected;
    }
     
    public List<Module> Modules= new List<Module>();
    public List<UIResearchButton> buttons = new List<UIResearchButton>();
    public void AddResearch()
    {
       CurrentScenario.Researches.Add( Instantiate( Resources.Load("UI/ScenarioManager/ResearchButton") as GameObject,CameraPivot).GetComponent<Research>());
        //  Researches[Researches.Count - 1].name = CurrentResearch.ResearchName.text;
        buttons.Add(CurrentScenario.Researches[CurrentScenario.Researches.Count - 1].researchButton);
        if (CurrentScenario.Researches.Count > 1)
        {
            CurrentScenario.Researches[CurrentScenario.Researches.Count - 1].Dependances.Add(CurrentScenario.Researches[CurrentScenario.Researches.Count - 2]);
            CurrentScenario.Researches[CurrentScenario.Researches.Count - 1].researchButton.Rect.position = CurrentScenario.Researches[CurrentScenario.Researches.Count - 2].researchButton.Rect.position+ new Vector3(450,0,0);
        }
        CurrentResearcLink.CurrentResearchSelected = CurrentScenario.Researches[CurrentScenario.Researches.Count - 1];
        WindowSelectModule.instance.Hide();
        WindowEditModule.instance.Hide();
    }
   
    [System.Serializable]
    public class Scenario
    {
        public int[] StartDate;
        public string Name;
        public string FilePath => instance.ScenariosFolder   + Name + ".scenario";
        public string FolderPath => instance.ScenariosFolder +   Name  ;
        
        public int StartBalance;
        public List<Research> Researches;
        public Scenario ( string _Name, int  _StartDay, int _StartMonth, int _StartYear, int _StartBalance)
        {
            Name = _Name;
            StartBalance = _StartBalance;
            StartDate = new int[3];
            StartDate[0] = _StartDay;
            StartDate[1] = _StartMonth;
            StartDate[2] = _StartYear;
            Researches = new List<Research>();
        }
        public void SaveNewScenario()
        {
            string jsonData = JsonUtility.ToJson(this, true);
            
           
            File.WriteAllText(FilePath , jsonData);
            foreach (var item in instance.CurrentScenario.Researches)
            {
                item.Save();
            }
            Debug.Log("File Saved at: " + instance.ScenariosFolder);
        }
        public void DeleteFilesAndFoldersOfScenario()
        {
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
                Debug.Log("Scenario " + Name + " Deleted");
            }
            if(Directory.Exists(FolderPath))
            FileUtil.DeleteFileOrDirectory(FolderPath);
            
        }
    }
    #endregion

    public void LoadScenarioUnits()
    {

        DirectoryInfo dir = new DirectoryInfo(CurrentScenario.FilePath);
        FileInfo[] info = dir.GetFiles("*.unit");
        CurrentScenario.Researches.Clear();
        foreach (FileInfo f in info)
        {
            string temp = File.ReadAllText(CurrentScenarioFolder + f.Name);
            Debug.Log("research Loaded: " + f.Name + "  " + temp);
            CurrentScenario.Researches.Add(JsonUtility.FromJson<Research>(temp));

        } 
    }
}
