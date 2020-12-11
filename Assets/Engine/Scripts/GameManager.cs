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

    void Start()
    {

    }
    private void Awake()
    {   if (FindObjectsOfType<GameManager>().Length > 1) { DestroyImmediate(this); Debug.Log("<color: red> ДВА СКРИПТА ГЕЙМ МЕНЕДЖЕР!!! </color>"); }
        gameObject.AddComponent<Eco>();
        gameObject.AddComponent<TimeManager>();
     if(FindObjectOfType<Canvas>()==null)    Instantiate( Resources.Load("Canvas")) ;
     SceneManager.LoadScene("UIResearch",LoadSceneMode.Additive);
     
    }
    void Update()
    {
        Hack();
    }
    #endregion
    #region Variables
    public static event Action EventChangeState;
    public static event Action <Unit> EventCreatedNewUnit;
    public enum State { MenuStartGame, Pause, MenuLoadGame, Play, CreateLauchPlace,CreateResearchLab,CreateProductionFactory, PlayStation, PlayBase,ResearchGlobal }
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

                    break;
                case State.PlayStation:
                    CameraManager.instance.TargetObject = UnitsAll.Find(X => X.GetType() == typeof(UnitStation)).transform;
                    return;
                    break;
            }

            Debug.Log(string.Format("<color=blue> State changed " + _currentState + ":=" + value + "</color>"));
            _currentState = value;
            if(EventChangeState!=null) EventChangeState();

        }
    }
    private static GameParameters _gameParam;
    public static GameParameters GameParam
    {
        get
        {
            if (_gameParam == null)
            {
                _gameParam=(Resources.LoadAll<GameParameters>("GameParametres")[0]) as GameParameters;
                Debug.Log("Loaded GameParams from Default: " + _gameParam.name);
            }
            return _gameParam;
        }
    }

    public enum level { Easy, Medium, Hard}
    #endregion
    #region Game Start Load Save etc
    private static void StartNewGame()
    {
        Eco.IniEco("");
        TimeManager.Ini();
       
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
    public static void CreateLaunchPlace(CountrySO launchPlace ,GameObject launchPlaceTemp,Unit currentUnit)
    {
        if (!Eco.Buy(launchPlace.CostBuild, "Not Enough Money :(")) return; 
        currentUnit.transform.position = launchPlaceTemp.transform.position;
        currentUnit.transform.parent = UnitsAll.Find(X =>   X.GetType() == typeof( UnitEarth)).transform;
        Destroy(launchPlaceTemp);
        EventCreatedNewUnit(currentUnit);
        CurrentState = State.Play;
    }

    public static void CreateResearchLab(CountrySO launchPlace, GameObject launchPlaceTemp)
    {

    }
    #endregion
    #region OnGui Hack Etc
    private bool F1 = false;
    void Hack()
    {
        if (Input.GetKeyDown(KeyCode.R)) UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        if (Input.GetKeyDown(KeyCode.Escape)) CurrentState = State.Play;
        if (Input.GetKeyDown(KeyCode.Plus)|| Input.GetKeyDown(KeyCode.KeypadPlus)) Eco.Balance += Eco.Balance;
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

}
#endregion
