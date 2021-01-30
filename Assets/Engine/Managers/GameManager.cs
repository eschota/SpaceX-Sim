    #region Using
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;
using UnityEngine.SceneManagement;
using Object = System.Object;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
#endregion
#region Base Functions
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    void Start()
    {
    }
    public static GameObject Canvas;
    private void Awake()
    {
        if (FindObjectsOfType<GameManager>().Length > 1 || instance != null)
        {
            DestroyImmediate(this.gameObject);
            Debug.Log("<color: red> ДВА СКРИПТА ГЕЙМ МЕНЕДЖЕР!!! </color>");
            return;
        }
        instance = this;
        CreateRoots();
        DontDestroyOnLoad(gameObject.AddComponent<Eco>());
        DontDestroyOnLoad(gameObject.AddComponent<TimeManager>());
        Canvas= Instantiate(Resources.Load<GameObject>("Canvas"));
        DontDestroyOnLoad(Canvas);
        //GameObject WorldMap= Instantiate( Resources.Load("world_map"))as GameObject;
        //WorldMap.transform.SetParent(transform);


        SceneManager.sceneLoaded += OnSceneLoaded;
        UnitsAll.CollectionChanged += OnChangeUnits;
    }
    void Update()
    {
        AsyncLoadFunc();
        Hack();
    }
    #endregion
    #region Variables
    private static UnitEarth _earth;
    public static UnitEarth Earth
    {
        get
        {
            if (_earth == null) _earth = UnitsAll.Find(X => X.GetType() == typeof(UnitEarth)) as UnitEarth;
            return _earth;
        }
    }
    public static bool AboveEarth
    {
        get {
            if (CurrentState == State.CreateLaunchPlace || CurrentState == State.CreateProductionFactory || CurrentState == State.CreateResearchLab || CurrentState == State.PlaySpace || CurrentState == State.CreateSeaLaunch)
                return true;
            else return false;
            }
    }
    public static bool Creation => (CurrentState == State.CreateLaunchPlace || CurrentState == State.CreateProductionFactory || CurrentState == State.CreateResearchLab || CurrentState == State.CreateSeaLaunch);
    public static event Action EventChangeState;
    public static event Action<Unit> EventWithUnit;
    public enum State { MenuStartGame, Pause, MenuLoadGame, PlaySpace, CreateLaunchPlace, CreateResearchLab, CreateProductionFactory, PlayStation, PlayBase, ResearchGlobal, EarthResearchLab, EarthProductionFactory, EarthLauchPlace, ScenarioEditorSelection, Settings, Save, Load, PlayEarth, ScenarioEditorGlobal, StartGameSelectScenario,CreateSeaLaunch,Back }
    private static State _currentState;

    public static State LastState;
    public static State CurrentState
    {
        get => _currentState;
        set
        {
            if (value == State.Back)
            {
                _currentState = LastState;
                if (EventChangeState != null) EventChangeState();
                return;
            }
            if (_currentState!= LastState) LastState = _currentState;
            switch (value)
            {
                case State.PlaySpace:
                    if (CurrentState == State.MenuStartGame)
                    {
                        
                        StartNewGame();
                    }
                    else
                    if (CurrentState == State.PlayEarth)
                    {
                        SceneManager.LoadScene(0);
                        Earth.gameObject.SetActive(true);
                    }
                    break;
                case State.PlayStation:
                    CameraControllerInSpace.instance.TargetObject = UnitsAll.Find(X => X.GetType() == typeof(UnitStation)).transform;
                    return;

                case State.ResearchGlobal:

                    CameraControllerScenarioResearch.instance.CameraPivot.SetParent(ScenarioManager.instance.InGameResearchPanel);

                    break;
                /////////////////////////////////////////////////////
                ////
                ////            выбор сохранения загрузки редактирования сценариев
                ////

                case State.ScenarioEditorSelection:
                    
                    ScenarioManager.instance.EnterScenarioManager();
                    break;

                case State.Save:
                     
                    ScenarioManager.instance.EnterScenarioManager();
                    ScenarioManager.instance.SaveNameInputField.text = DateTime.Today + "_" + DateTime.Now.ToLongTimeString();
                    break;

                case State.Load:
                    
                    ScenarioManager.instance.EnterScenarioManager();
                    break;

                case State.StartGameSelectScenario:
                     
                    ScenarioManager.instance.EnterScenarioManager();
                    break;
                /////////////////////////////////////////////////////
                ////
                ////
                ////
                ///
                case State.PlayEarth:
                    
                    WorldMapManager.instance.CurrentState = WorldMapManager.State.Earth;
                    Earth?.gameObject.SetActive(false);
                    break;
            }

            Debug.Log(string.Format("<color=yellow> State changed " + _currentState + ":=" + value + "</color>"));
            _currentState = value;
            if (EventChangeState != null) EventChangeState();

        }
    }

    public Transform ResearchesTransform, ResearchModulesTransform, ModulesTransform, BuildingsTransform,EarthUnits;
    private void CreateRoots()
    {
        ResearchesTransform = new GameObject("Researches").transform;
        ResearchesTransform.SetAsFirstSibling();
        DontDestroyOnLoad(ResearchesTransform);
        ResearchModulesTransform = new GameObject("ResearchedModules").transform;
        ResearchModulesTransform.SetAsFirstSibling();
        DontDestroyOnLoad(ResearchModulesTransform);
        ModulesTransform = new GameObject("Modules").transform;
        ModulesTransform.SetAsFirstSibling();
        DontDestroyOnLoad(ModulesTransform);
        BuildingsTransform = new GameObject("Buildings").transform;
        BuildingsTransform.SetAsFirstSibling();
        DontDestroyOnLoad(BuildingsTransform);   
        EarthUnits= new GameObject("EarthUnits").transform;
        EarthUnits.SetAsFirstSibling();
        DontDestroyOnLoad(EarthUnits);
    }
    private static GameParameters _gameParam;
    public static GameParameters GameParam
    {
        get
        {
            if (_gameParam == null)
            {
                _gameParam = (Resources.LoadAll<GameParameters>("GameParametres")[0]) as GameParameters;
                Debug.Log("Loaded GameParams from Default: " + _gameParam.name);
            }
            return _gameParam;
        }
    }

    public enum level { Easy, Medium, Hard }
    #endregion
    #region Game Start Load Save etc
    private static void StartNewGame()
    {
        Eco.IniEco("");
        TimeManager.Init();
        SpeedManager.instance.CurrenSpeed = SpeedManager.Speed.Normal;

    }

    static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "SeaLaunch") CurrentState = State.PlayEarth;
        if (SceneManager.GetActiveScene().name == "Launch") CurrentState = State.PlayEarth;
        if (SceneManager.GetActiveScene().name == "Production") CurrentState = State.PlayEarth;
        if (SceneManager.GetActiveScene().name == "Research") CurrentState = State.PlayEarth;
        Debug.Log("Loaded Scene: " + SceneManager.GetActiveScene().name);
    }
    static public void LoadGame(string Name)
    {
        Eco.IniEco(Name);
    }
    #endregion
    #region Unit Events
    

    public static void OnChangeUnits(object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add: // если добавление                
                Console.WriteLine($"Добавлен новый объект: {(e.NewItems[0] as Unit).Name}");
                if(EventWithUnit!=null)EventWithUnit(e.NewItems[0] as Unit);
                break;
            case NotifyCollectionChangedAction.Remove: // если удаление
                 
                Console.WriteLine($"Удален объект: {e.OldItems[0]}");
                break;
            case NotifyCollectionChangedAction.Replace: // если замена
                //User replacedUser = e.OldItems[0] as User;
                //User replacingUser = e.NewItems[0] as User;
                //Console.WriteLine($"Объект {replacedUser.Name} заменен объектом {replacingUser.Name}");
                break;
        }
        
    }

    #endregion
    #region LauchPlace ResearchLab Production Factory
    public static ObservableCollection<Unit> UnitsAll = new ObservableCollection<Unit>();
  
    public static List<Unit> UnitsLaunchPlace = new List<Unit>();
    public static List<Unit> UnitsResearchLab = new List<Unit>();
    public static List<Unit> UnitsProductionFactory = new List<Unit>();
    public static List<BuildingUnit> Buildings = new List<BuildingUnit>();

   
    #endregion
    #region OnGui Hack Etc
    private bool F1 = false;
    void Hack()
    {
        if (Input.GetKeyDown(KeyCode.R)) UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        
        if (Input.GetKeyDown(KeyCode.Plus)|| Input.GetKeyDown(KeyCode.KeypadPlus)) Eco.Balance += Eco.Balance;
        //if (Input.GetKeyDown(KeyCode.Z)) SceneManager.LoadScene(1);
        //if (Input.GetKeyDown(KeyCode.X)) SceneManager.LoadScene(2);
        //if (Input.GetKeyDown(KeyCode.C)) SceneManager.LoadScene(3);
        //if (Input.GetKeyDown(KeyCode.V)) SceneManager.LoadScene(4);
    }
    private void OnGUI()
    {
        if (Application.isEditor)
            if (F1 == true)
            {
                GUI.Label(new Rect(Screen.width * 0.1f, Screen.height * 0.1f, Screen.width * 0.2f, Screen.height * 0.2f), CurrentState.ToString());


                //      GUI.Label(new Rect(400, 200, 100, 100), Camera.main.WorldToScreenPoint(Vector3.up * maxPos).y.ToString());
                //      if (LastBlock != null) GUI.Label(new Rect(100, 300, 100, 100), LastBlock.transform.position.y.ToString());
            }
    }
    public int sceneIndex;
    public void OpenUnitScene(Unit unit)
    {
        if(CameraControllerOnEarth.instance!=null) CameraControllerOnEarth.instance.SavePos();
        var position = unit.transform.position;
        var earth = UnitsAll.Find(u => u.GetType() == typeof(UnitEarth)).transform;
        var earthDirection = earth.right;
        position.y = 0f;
        var angle = Vector3.SignedAngle(earthDirection, position.normalized, Vector3.up);
        var localHoursOffset = -angle / 180f * 12f;
        TimeManager.LocalHoursOffset = localHoursOffset;
        
        
        
        
        if (unit.GetType() == typeof(UnitLaunchPlace)) sceneIndex = 1;
        if (unit.GetType() == typeof(UnitSeaLaunch)) sceneIndex = 2;

        else if (unit.GetType() == typeof(UnitResearchLab)) sceneIndex = 3;
        else if (unit.GetType() == typeof(UnitProductionFactory)) sceneIndex =4;

        //  SceneManager.LoadScene(sceneIndex);

        CameraControllerInSpace.instance.FlyToUnit = unit.transform;
      
        StartCoroutine(LoadScene());
    }
    public AsyncOperation asyncLoad ;
    private AsyncOperation async;
     
    IEnumerator LoadScene()
    {
        

        async = Application.LoadLevelAsync(sceneIndex);
        async.allowSceneActivation = false;

        Debug.Log("start loading");

        yield return async;
    }

   
    private void AsyncLoadFunc()
    {
        if (CameraControllerInSpace.instance.FlyToUnit == null)
        if (async != null && async.progress>=0.9f)
        {
            async.allowSceneActivation = true;
        }
    }

    public static Unit Create(string FilePath)
    {
        string[] strs = FilePath.Split('.');
        switch (strs[strs.Length-1])
        {   case "Unit":
            return JsonUtility.FromJson<Unit>(System.IO.File.ReadAllText(FilePath));
                break;

            case "Module":
                return JsonUtility.FromJson<Module>(System.IO.File.ReadAllText(FilePath));
                break;
            case "ModuleEngine":
                return JsonUtility.FromJson<ModuleEngine>(System.IO.File.ReadAllText(FilePath));
                break;
            case "ModuleISS":
                return JsonUtility.FromJson<ModuleISS>(System.IO.File.ReadAllText(FilePath));
                break;
            case "Research":
                return JsonUtility.FromJson<Research>(System.IO.File.ReadAllText(FilePath));

                break;

            default:
                return null;
                break;
        }
        return null;
    }
     
}


public static class ObservableCollectionExtension
{
    public static T Find<T>(this ObservableCollection<T> list, Func<T, bool> predicate)
    {
        int len = list.Count;
        for (int i = 0; i < len; i++) if (predicate(list[i])) return list[i];
        return default;
    }
}
#endregion
