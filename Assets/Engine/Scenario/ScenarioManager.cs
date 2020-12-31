using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
public class ScenarioManager : MonoBehaviour
{
    public string ScenariosFolder
    {
        get => Application.streamingAssetsPath + "/Scenarios/";
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
                    LoadAllExistenceScenarios();
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
    
    public Scenario CurrentScenario;
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

    public List<Scenario> LoadedScenarios = new List<Scenario>();
   

    
    private void Update()
    {
        if(Input.GetMouseButtonDown(1))  WindowEditResearch.instance.CurrentResearch = null; 
    }
    public void AddModule()
    {
        WindowSelectModule.instance.CurrentResearchSelected = WindowEditResearch.instance.CurrentResearch;
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

        
        WindowEditResearch.instance.CurrentResearch = CurrentScenario.Researches[CurrentScenario.Researches.Count - 1];
        WindowEditResearch.instance.OnEditResearch();
        WindowSelectModule.instance.Hide();
        WindowEditModule.instance.Hide();
    }
   
    [System.Serializable]
    public class Scenario
    {
        public int[] StartDate;
        public string Name;
        public string CurrentFolder => Path.Combine(instance.ScenariosFolder, Name);
        
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
            DeleteFilesAndFoldersOfScenario();

            string jsonData = JsonUtility.ToJson(this, true);

            if (!Directory.Exists(instance.CurrentScenario.CurrentFolder)) Directory.CreateDirectory(instance.CurrentScenario.CurrentFolder);
            File.WriteAllText(Path.Combine( CurrentFolder,"scenario.dat") , jsonData);
            foreach (var item in instance.CurrentScenario.Researches)
            {
                item.SaveJSON();
                foreach (var module in item.ModulesOpen)
                {
                    module.SaveJSON();
                }
            }
            Debug.Log("File Saved at: " + instance.ScenariosFolder);
        }

      
        public void DeleteFilesAndFoldersOfScenario()
        {

            if (Directory.Exists(CurrentFolder))
            {
                //DirectoryInfo dir = new DirectoryInfo(CurrentFolder);
                //FileInfo[] info = dir.GetFiles("*.unit");
                //foreach (FileInfo f in info)
                //{
                //    f.Delete();
                //}
                    Directory.Delete(CurrentFolder,true);
            }
            
        }
    }
    #endregion
    public void LoadAllExistenceScenarios()
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
        ClearResearchesAndModules(); 
        WindowLoadScenario.instance.LoadScenarios();
    }
    public void LoadScenarioResearchAndModules()
    {
        
        DirectoryInfo dir = new DirectoryInfo(CurrentScenario.CurrentFolder);
        FileInfo[] info = dir.GetFiles("*Research");
       
        foreach (FileInfo f in info)
        {
            // GameManager.Create(Path.Combine(CurrentScenario.CurrentFolder,f.Name));

            string jsondata = System.IO.File.ReadAllText(Path.Combine(CurrentScenario.CurrentFolder, f.Name));
            Research R= new GameObject().AddComponent<Research>();
             JsonUtility.FromJsonOverwrite(jsondata,R);
            //string temp = File.ReadAllText(Path.Combine(CurrentScenario.CurrentFolder, f.Name));

            //CurrentScenario.Researches.Add(Instantiate(Resources.Load("UI/ScenarioManager/ResearchButton") as GameObject, CameraPivot).GetComponent<Research>());
            //buttons.Add(CurrentScenario.Researches[CurrentScenario.Researches.Count - 1].researchButton);
            //CurrentScenario.Researches[CurrentScenario.Researches.Count - 1].JsonFilePath = Path.Combine(CurrentScenario.CurrentFolder, f.Name);

        }
    //    foreach (var item in CurrentScenario.Researches) item.RestoreDependencies();
        // загружаем модули
        LoadModules();


    }

    private void ClearResearchesAndModules()
    {
        foreach (var item in FindObjectsOfType<Research>()) if (item != null) Destroy(item.gameObject);        
        foreach (var item in FindObjectsOfType<Module>()) if (item != null) Destroy(item.gameObject);
        
        buttons.Clear();
        CurrentScenario.Researches.Clear();
    }
    public void LoadModules()
    {
        DirectoryInfo dir = new DirectoryInfo(CurrentScenario.CurrentFolder);
        FileInfo[] info = dir.GetFiles("*.Module");
        List<Module> modules = new List<Module>();
        modules.AddRange(Resources.LoadAll<Module>("Modules"));
        foreach (FileInfo f in info)
        {
           // Module.SaveData SD;
           //    string temp = File.ReadAllText(Path.Combine(CurrentScenario.CurrentFolder, f.Name));
           // SD = JsonUtility.FromJson<Module.SaveData>(temp);
           //if( modules.Exists(X => X.Prefab.name == SD.PrefabName))
           // {
           //     Module ins= new Module();                
           //     ins.JsonFilePath = Path.Combine(CurrentScenario.CurrentFolder, f.Name);
                
           // }
          // SaveData
        }
        
    }
}
