using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEditor; 
public class ScenarioManager : MonoBehaviour
{
    #region VARS
    public string ScenariosFolder
    {
        get => Application.streamingAssetsPath + "/Scenarios/";
    }
    public List<Scenario> LoadedScenarios = new List<Scenario>();
    public List<Module> Modules = new List<Module>();
    public List<UIResearchButton> buttons = new List<UIResearchButton>();
    public List<Research> Researches = new List<Research>();
    public static event Action EventChangeState; 
    public enum State {None, StartConditions,Researches,PoliticMap, LoadScenario  }
    private   State _currentState;
    [SerializeField] public Transform CameraPivot;
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
                    EnterScenarioManager();
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
   
    [SerializeField] TMPro.TMP_InputField ScenarioName;
    [SerializeField] TMPro.TMP_InputField ScenarioStartDay;
    [SerializeField] TMPro.TMP_InputField ScenarioStartMonth;
    [SerializeField] TMPro.TMP_InputField ScenarioStartYear;
    [SerializeField] TMPro.TMP_InputField ScenarioStartBalance;

    private Scenario _currentScenario;
    public Scenario CurrentScenario
    {
        get => _currentScenario;
        set
        {
            _currentScenario = value;
        }
    }
    #endregion

    #region Start & Update // Отмена выделения ресёрча
    void Start()
    {
        instance = this;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) WindowEditResearch.instance.CurrentResearch = null;
    }
    #endregion
    #region Работа со сценарием
    public void CreateNewCurrentScenario()
    {
        CurrentScenario = new Scenario( ScenarioName.text+LoadedScenarios.Count, int.Parse(ScenarioStartDay.text), int.Parse(ScenarioStartMonth.text), int.Parse(ScenarioStartYear.text), int.Parse(ScenarioStartBalance.text));
        CurrentScenario.SaveNewScenario();
        RefreshScenario();
        
    }
    public void RefreshScenario()
    {
        if (CurrentScenario == null) return;
        ScenarioName.SetTextWithoutNotify(CurrentScenario.Name);
        ScenarioStartBalance.SetTextWithoutNotify(CurrentScenario.StartBalance.ToString());
        ScenarioStartDay.SetTextWithoutNotify(CurrentScenario.StartDate[0].ToString());
        ScenarioStartMonth.SetTextWithoutNotify(CurrentScenario.StartDate[1].ToString());
        ScenarioStartYear.SetTextWithoutNotify(CurrentScenario.StartDate[2].ToString());
    }
    public void OnScenarioEdit()
    {
        CurrentScenario.Name = ScenarioName.text;
        int.TryParse(ScenarioStartBalance.text, out CurrentScenario.StartBalance  );
        int.TryParse(ScenarioStartDay.text, out CurrentScenario.StartDate[0]  );
        int.TryParse(ScenarioStartMonth.text, out CurrentScenario.StartDate[1]  );
        int.TryParse(ScenarioStartYear.text, out CurrentScenario.StartDate[2]  );
    }
     public void SaveCurrentScenario()
    {
        CurrentScenario.SaveNewScenario();       
    }

    
    #endregion



    public void AddModule()
    {
        WindowSelectModule.instance.CurrentResearchSelected = WindowEditResearch.instance.CurrentResearch;
    }
 /// <summary>
 /// 
 /// </summary>
    public void AddResearch()
    {
        
        new GameObject().AddComponent<Research>().Ini();

        if (Researches.Count > 1)
        {
            Researches[Researches.Count - 1].Dependances.Add(Researches[Researches.Count - 2]);
            Researches[Researches.Count - 1].researchButton.Rect.position = Researches[Researches.Count - 2].researchButton.Rect.position + new Vector3(450, 0, 0);
        }
        else Researches[Researches.Count - 1].researchButton.Rect.position = new Vector3(Screen.width / 5, +Screen.height / 5, 0);// позиция от  угла экрана 

        buttons.Add( Researches[ Researches.Count - 1].researchButton);
        WindowEditResearch.instance.CurrentResearch =  Researches[ Researches.Count - 1];
        WindowEditResearch.instance.OnEditResearch();
        WindowSelectModule.instance.Hide();
        WindowEditModule.instance.Hide();
    }

    #region Scenario Serialize


   
    [System.Serializable]
    public class Scenario
    {
        public int[] StartDate;
        public string Name;
        public string CurrentFolder => Path.Combine(instance.ScenariosFolder, Name);
        
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
            DeleteFilesAndFoldersOfScenario();

            string jsonData = JsonUtility.ToJson(this, true);

            if (!Directory.Exists(instance.CurrentScenario.CurrentFolder)) Directory.CreateDirectory(instance.CurrentScenario.CurrentFolder);
            File.WriteAllText(Path.Combine( CurrentFolder,"scenario.dat") , jsonData);
            foreach (var item in instance.Researches)
            {
                item.SaveJSON();
                foreach (var module in item.Modules)
                {
                    module.SaveJSON();
                }
            }
            Debug.Log("File Saved at: " + instance.ScenariosFolder);

            AssetDatabase.Refresh();
        }

      
        public void DeleteFilesAndFoldersOfScenario()
        {
            if (Directory.Exists(CurrentFolder))
            {
                    Directory.Delete(CurrentFolder,true);
            }
            
        }
    }
    #endregion

    #region ManageScenario
    public void EnterScenarioManager()
    {
        LoadedScenarios.Clear();
        DirectoryInfo dir = new DirectoryInfo(ScenariosFolder);
        DirectoryInfo[] info = dir.GetDirectories("**");

        foreach (DirectoryInfo f in info)
        {
            string temp = "";
            if (File.Exists(Path.Combine(ScenariosFolder, f.Name, "scenario.dat")))
            {
                temp = File.ReadAllText(Path.Combine(ScenariosFolder, f.Name, "scenario.dat"));
                //  Debug.Log("Scenario Loaded: " + f.Name+"  "+ temp);
                LoadedScenarios.Add(JsonUtility.FromJson<Scenario>(temp));
            }
        }
        Debug.Log("Scenarios Loaded: " + LoadedScenarios.Count);
      if(CurrentScenario!=null)  ClearResearchesAndModules(); 
        WindowLoadScenario.instance.LoadScenarios();
    }
    public void LoadScenarioResearchAndModules()
    {
        
        DirectoryInfo dir = new DirectoryInfo(CurrentScenario.CurrentFolder);
        FileInfo[] info = dir.GetFiles("*Research");
       
        foreach (FileInfo f in info)
        {
            string jsondata = System.IO.File.ReadAllText(Path.Combine(CurrentScenario.CurrentFolder, f.Name));
            Research R= new GameObject().AddComponent<Research>();
            JsonUtility.FromJsonOverwrite(jsondata,R);
            R.Ini();     

        }
        LoadModules();
        foreach (var item in Researches) item.RestoreDependencies();
        foreach (var item in Researches) { item.researchButton.RebuildLinks(); item.researchButton.Refresh(); }
        
        


    }

    private void ClearResearchesAndModules()
    {
        foreach (var item in FindObjectsOfType<Research>()) if (item != null) Destroy(item.gameObject);        
        foreach (var item in FindObjectsOfType<UIResearchButton>()) if (item != null) Destroy(item.gameObject);        
        foreach (var item in FindObjectsOfType<Module>()) if (item != null) Destroy(item.gameObject);
        
        buttons.Clear();
        Researches.Clear();
    }
    public void LoadModules()
    {
        DirectoryInfo dir = new DirectoryInfo(CurrentScenario.CurrentFolder);
        FileInfo[] info = dir.GetFiles("*.ModuleEngine"); 
        foreach (FileInfo f in info)
        {
            string jsondata = System.IO.File.ReadAllText(Path.Combine(CurrentScenario.CurrentFolder, f.Name));
            ModuleEngine R = new GameObject().AddComponent<ModuleEngine>();
            JsonUtility.FromJsonOverwrite(jsondata, R);
            R.Ini();
        }
        dir = new DirectoryInfo(CurrentScenario.CurrentFolder);
        info = dir.GetFiles("*.ModuleISS");
        foreach (FileInfo f in info)
        {
            string jsondata = System.IO.File.ReadAllText(Path.Combine(CurrentScenario.CurrentFolder, f.Name));
            ModuleISS R = new GameObject().AddComponent<ModuleISS>();
            JsonUtility.FromJsonOverwrite(jsondata, R);
            R.Ini();
        }
    }
    #endregion
}
