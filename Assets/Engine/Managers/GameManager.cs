    #region Using
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;
using UnityEngine.SceneManagement;
using Object = System.Object;

#endregion
#region Base Functions
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    void Start()
    {
    }

    private void Awake()
    {
        if (FindObjectsOfType<GameManager>().Length > 1 || instance != null)
        {
            DestroyImmediate(this.gameObject);
            Debug.Log("<color: red> ДВА СКРИПТА ГЕЙМ МЕНЕДЖЕР!!! </color>");
            return;
        }
        instance = this;

        DontDestroyOnLoad(gameObject.AddComponent<Eco>());
        DontDestroyOnLoad(gameObject.AddComponent<TimeManager>());
        if (FindObjectOfType<Canvas>() == null)
            DontDestroyOnLoad(Instantiate(Resources.Load("Canvas")));
        //GameObject WorldMap= Instantiate( Resources.Load("world_map"))as GameObject;
        //WorldMap.transform.SetParent(transform);
        SceneManager.LoadScene("UIResearch", LoadSceneMode.Additive);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void Update()
    {
        Hack();
    }
    #endregion
    #region Variables 
    private static UnitEarth _Earth;
    public static UnitEarth Earth
    {
        get
        {
            if (_Earth == null) _Earth = FindObjectOfType<UnitEarth>();
            return _Earth;
        }
    }
    public static event Action EventChangeState;
    public static event Action<Unit> EventCreatedNewUnit;
    public enum State { MenuStartGame, Pause, MenuLoadGame, Play, CreateLauchPlace, CreateResearchLab, CreateProductionFactory, PlayStation, PlayBase, ResearchGlobal, EarthResearchLab, EarthProductionFactory, EarthLauchPlace, ScenarioEditor, Settings, Save, Load }
    private static State _currentState;
    public static State CurrentState
    {
        get => _currentState;
        set
        {
            switch (value)
            {
                case State.Play:
                    if (CurrentState == State.MenuStartGame)
                    {
                        StartNewGame();
                    }
                    else
                    if (CurrentState == State.EarthLauchPlace || CurrentState == State.EarthProductionFactory || CurrentState == State.EarthResearchLab)
                    {
                        TimeManager.TimeScale = 1f;
                        var earth = UnitsAll.Find(u => u.GetType() == typeof(UnitEarth)).gameObject;
                        earth.SetActive(true);
                        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                    }
                    break;
                case State.PlayStation:
                    CameraManager.instance.TargetObject = UnitsAll.Find(X => X.GetType() == typeof(UnitStation)).transform;
                    return;
                case State.ScenarioEditor:
                    ScenarioManager.CurrentState = ScenarioManager.State.StartConditions;
                    break;
            }

            Debug.Log(string.Format("<color=blue> State changed " + _currentState + ":=" + value + "</color>"));
            _currentState = value;
            if (EventChangeState != null) EventChangeState();

        }
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

    }

    static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "SeaLaunch") CurrentState = State.EarthLauchPlace;
        if (SceneManager.GetActiveScene().name == "Launch") CurrentState = State.EarthLauchPlace;
        if (SceneManager.GetActiveScene().name == "Production") CurrentState = State.EarthProductionFactory;
        if (SceneManager.GetActiveScene().name == "Research") CurrentState = State.EarthResearchLab;
        if (SceneManager.GetActiveScene().name == "Game") Earth?.gameObject.SetActive(true);else Earth?.gameObject.SetActive(false);

        Debug.Log("Loaded Scene: " + SceneManager.GetActiveScene().name);
    }
    static public void LoadGame(string Name)
    {
        Eco.IniEco(Name);
    }
    #endregion
    #region LauchPlace ResearchLab Production Factory
    public static List<Unit> UnitsAll = new List<Unit>();
    public static List<Unit> UnitsLaunchPlace = new List<Unit>();
    public static List<Unit> UnitsResearchLab = new List<Unit>();
    public static List<Unit> UnitsProductionFactory = new List<Unit>();
    public static void CreateUnit(Unit currentUnit)
    {
        EventCreatedNewUnit(currentUnit);
        CurrentState = State.Play;
    }

   
    #endregion
    #region OnGui Hack Etc
    private bool F1 = false;
    void Hack()
    {
        if (Input.GetKeyDown(KeyCode.R)) UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        if (Input.GetKeyDown(KeyCode.Escape)) CurrentState = State.Play;
        if (Input.GetKeyDown(KeyCode.Plus)|| Input.GetKeyDown(KeyCode.KeypadPlus)) Eco.Balance += Eco.Balance;
        if (Input.GetKeyDown(KeyCode.Z)) SceneManager.LoadScene(1);
        if (Input.GetKeyDown(KeyCode.X)) SceneManager.LoadScene(2);
        if (Input.GetKeyDown(KeyCode.C)) SceneManager.LoadScene(3);
        if (Input.GetKeyDown(KeyCode.V)) SceneManager.LoadScene(4);
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
    
    public void OpenUnitScene(Unit unit)
    {
        var position = unit.transform.position;
        var earth = UnitsAll.Find(u => u.GetType() == typeof(UnitEarth)).transform;
        var earthDirection = earth.right;
        position.y = 0f;
        var angle = Vector3.SignedAngle(earthDirection, position.normalized, Vector3.up);
        var localHoursOffset = -angle / 180f * 12f;
        TimeManager.LocalHoursOffset = localHoursOffset;
        TimeManager.TimeScale = 1 / 60f;
        
        
        
        var sceneIndex = 0;
        switch (unit.name)
        {
            case "ProductionFactory":
                sceneIndex = 2;
                break;
            case "LaunchPlace":
                sceneIndex = 3;
                break;
            case "ResearchLab":
                sceneIndex = 4;
                break;
            default:
                throw new Exception("Unit name not found");
        }

        StartCoroutine(LoadAsyncScene(sceneIndex));
    }

    private const float MinTimeBeforeLoadScene = 1.1f;
    
    private static IEnumerator LoadAsyncScene(int sceneIndex)
    {
        var asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        var timer = 0f;
        
        asyncLoad.allowSceneActivation = false;
        while (!asyncLoad.isDone)
        {
            timer += Time.deltaTime;

            if (asyncLoad.progress >= 0.9f && timer >= MinTimeBeforeLoadScene)
            {
                CameraManager.FlyToUnit = null;
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
#endregion
