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
    } public string SaveFolder
    {
        get => Application.streamingAssetsPath + "/Saves/";
    }
    public List<Scenario> LoadedScenarios = new List<Scenario>();
    public List<Module> Modules = new List<Module>();
    public List<UIResearchButton> buttons = new List<UIResearchButton>();
    public List<Research> Researches = new List<Research>();
    public static event Action EventChangeState; 
    public enum State {None, StartConditions,Researches,WorldMap }
    private   State _currentState;
    [SerializeField] public Transform CameraPivot;
    public   State CurrentState
    {
        get => _currentState;
        set
        {
            Debug.Log(string.Format("<color=blue> State changed " + _currentState + ":=" + value + "</color>"));
            _currentState = value;
            switch (value)
            {
                case State.None:
                    break;
                case State.StartConditions:
                    break;
                case State.Researches:
                   // foreach (var item in buttons) item.transform.SetParent(CameraPivot.transform);
                    CameraControllerScenarioResearch.instance.CameraPivot.SetParent(CameraPivot.transform);
                    break;
               
                    
                  
                default:
                    break;
            }


            
            if (EventChangeState != null) EventChangeState();

        }
    }
    public static ScenarioManager instance;
    [SerializeField] public TMPro.TMP_InputField SaveNameInputField;
    [SerializeField] public Transform InGameResearchPanel; 
    [SerializeField] public Transform ResearchPanel; 
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
        if (Input.GetKeyDown(KeyCode.F8)) { SaveNameInputField.text = "QuickSave"; SaveGame(); }
       // if (Input.GetKeyDown(KeyCode.F4)) { CurrentScenario.Name = "QuickSave";LoadGame();
    }
    #endregion
    #region Работа со сценарием
    public void CreateNewCurrentScenario()
    {
        CurrentScenario = new Scenario( ScenarioName.text+LoadedScenarios.Count, int.Parse(ScenarioStartDay.text), int.Parse(ScenarioStartMonth.text), int.Parse(ScenarioStartYear.text), int.Parse(ScenarioStartBalance.text));
        
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
        CurrentScenario.SaveNewScenario( );       
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
        
        new GameObject().AddComponent<Research>().IniAfterJSONRead();

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
        public string CurrentFolder 
        {
            get
            {
                if (GameManager.CurrentState == GameManager.State.StartGameSelectScenario || GameManager.CurrentState == GameManager.State.ScenarioEditorSelection|| GameManager.CurrentState == GameManager.State.ScenarioEditorGlobal)
                    return Path.Combine(instance.ScenariosFolder, Name);
            else
                    return Path.Combine(instance.SaveFolder, Name);
            }
        }
        public int AreaCostBase = 1;
        public float ResearchBonusMult = 1; 
        public float ProductionBonusMult = 1;
        
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
            foreach (var item in GameManager.UnitsAll)
            {
             if(item!=null)   item.SaveJSON();
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
        DirectoryInfo dir;
        LoadedScenarios.Clear();
        string TempFolder;
        if (GameManager.CurrentState == GameManager.State.StartGameSelectScenario || GameManager.CurrentState == GameManager.State.ScenarioEditorSelection)
        {
            TempFolder = ScenariosFolder;            
        }
        else
        {
            TempFolder = SaveFolder;
        }
        dir = new DirectoryInfo(TempFolder);
        DirectoryInfo[] info = dir.GetDirectories("**");

        foreach (DirectoryInfo f in info)
        {
            string temp = "";
            if (File.Exists(Path.Combine(TempFolder, f.Name, "scenario.dat")))
            {
                temp = File.ReadAllText(Path.Combine(TempFolder, f.Name, "scenario.dat"));
                //  Debug.Log("Scenario Loaded: " + f.Name+"  "+ temp);
                LoadedScenarios.Add(JsonUtility.FromJson<Scenario>(temp));
            }
        }
        Debug.Log("Scenarios Loaded: " + LoadedScenarios.Count);
    //  if(CurrentScenario!=null) if(GameManager.CurrentState!=GameManager.State.Save) ClearResearchesAndModules(); 
        WindowLoadScenario.instance.LoadScenarios();
    }
    public void LoadScenarioResearchAndModules()// не забывать добавлять новые модули
    {
        LoadUnitsByType<ModuleEngine>();
        LoadUnitsByType<ModuleISS>();
        LoadUnitsByType<Research>();
        LoadUnitsByType<UnitLaunchPlace>();
        LoadUnitsByType<UnitResearchLab>();
        LoadUnitsByType<UnitProductionFactory>();
        LoadUnitsByType<UnitSeaLaunch>();
        LoadUnitsByType<BuildingUnit>();        
        LoadUnitsByType<BuildingProductionFactory>();        
        LoadUnitsByType<BuildingResearchLab>();        
        LoadUnitsByType<BuildingRocketLaunch>();        
        foreach (var item in Researches) item.RestoreDependencies();
        foreach (var item in Researches) 
        {
            item.researchButton.RebuildLinks(); 
            item.researchButton.Refresh();
        }
        SplitResearchedModulesAndReal();
        gameObject.AddComponent<ResearchAndProductionManager>();// инициация системы рисерчей
    }

    private void ClearResearchesAndModules()
    {
        WindowEditResearch.instance.CurrentResearch = null;
        foreach (var item in FindObjectsOfType<Research>()) if (item != null) Destroy(item.gameObject);        
        foreach (var item in FindObjectsOfType<UIResearchButton>()) if (item != null) Destroy(item.gameObject);        
        foreach (var item in FindObjectsOfType<UnitLaunchPlace>()) if (item != null) Destroy(item.gameObject);
        foreach (var item in FindObjectsOfType<UnitResearchLab>()) if (item != null) Destroy(item.gameObject);
        foreach (var item in FindObjectsOfType<UnitSeaLaunch>()) if (item != null) Destroy(item.gameObject);
        foreach (var item in FindObjectsOfType<UnitProductionFactory>()) if (item != null) Destroy(item.gameObject);
        foreach (var item in FindObjectsOfType<BuildingUnit>()) if (item != null) Destroy(item.gameObject);
        foreach (var item in FindObjectsOfType<BuildingProductionFactory>()) if (item != null) Destroy(item.gameObject);
        foreach (var item in FindObjectsOfType<BuildingResearchLab>()) if (item != null) Destroy(item.gameObject);
        foreach (var item in FindObjectsOfType<BuildingRocketLaunch>()) if (item != null) Destroy(item.gameObject);
        
        
        
        buttons.Clear();
        Researches.Clear();
    } 
    void LoadUnitsByType<T>() where T : MonoBehaviour
    {
        DirectoryInfo dir = new DirectoryInfo(CurrentScenario.CurrentFolder);
        string type = typeof(T).ToString();
        FileInfo[] info = dir.GetFiles( "*"+ type,SearchOption.AllDirectories);
        foreach (FileInfo f in info)
        {
            string jsondata = System.IO.File.ReadAllText(Path.Combine(CurrentScenario.CurrentFolder, f.Name));
            T R = new GameObject().AddComponent<T>();
            JsonUtility.FromJsonOverwrite(jsondata, R);
            (R as Unit).IniAfterJSONRead();
        }
    }




    private void SplitResearchedModulesAndReal()
    {
        foreach (var item in FindObjectsOfType<BuildingUnit>())
        {
          if(item.isResearch==false)  item.transform.SetParent(GameManager.instance.BuildingsTransform); else item.transform.SetParent(GameManager.instance.ResearchModulesTransform);
        }
        foreach (var item in FindObjectsOfType<Module>())
        {
            item.transform.SetParent(GameManager.instance.ModulesTransform);
        }
        foreach (var item in Researches)
        {
            item.transform.SetParent(GameManager.instance.ResearchesTransform);
            foreach (var modules in item.Modules)
            { 
                modules.name = modules.Name;
                modules.transform.SetParent(GameManager.instance.ResearchModulesTransform);
            }
        }
        
     
    }

    public void StartNewGame()
    { 
        if (CurrentScenario == null) return;
        LoadScenarioResearchAndModules();
        GameManager.CurrentState = GameManager.State.PlaySpace;
        Debug.LogWarning("Game Started:" + CurrentScenario);

    }
    public void SaveGame()
    {
        CurrentScenario.Name = SaveNameInputField.text;
        CurrentScenario.SaveNewScenario( );
        GameManager.CurrentState = GameManager.State.PlaySpace;
        Debug.LogWarning("Game Saved:" + SaveNameInputField.text);
    }
     
     public void LoadGame()
    {
        if (CurrentScenario == null) return;
        ClearResearchesAndModules();
        LoadScenarioResearchAndModules();
        GameManager.CurrentState = GameManager.State.PlaySpace;
        Debug.LogWarning("Game Loaded:" + CurrentScenario);
    }
    #endregion
}
