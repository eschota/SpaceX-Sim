using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;

public class GameManager : MonoBehaviour
{
    void Start()
    {

    }
    private void Awake()
    {
        gameObject.AddComponent<Eco>();
        gameObject.AddComponent<TimeManager>();
        Instantiate( Resources.Load("Canvas"));
    }
    void Update()
    {
        Hack();
    }
    #region Variables
    public static event Action EventChangeState;
    public static event Action EventCreatedNewUnit;
    public enum State { MenuStartGame, Pause, MenuLoadGame, Play, CreateLauchPlace,CreateResearchLab,CreateProductionFactory, PlayStation, PlayBase }
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
            }

            Debug.Log("State changed " + _currentState + "=>" + value);
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
    public static List<Unit> Units = new List<Unit>();
    public static void CreateLaunchPlace(CountrySO launchPlace ,GameObject launchPlaceTemp,Unit type)
    {
        if (!Eco.Buy(launchPlace.CostBuild, "Not Enough Money :(")) return; 
        type.transform.position = launchPlaceTemp.transform.position;
        type.transform.parent = GameObject.FindObjectOfType<UnitEarth>().transform;
        Units.Add(type);     
        Destroy(launchPlaceTemp);
        EventCreatedNewUnit();
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
    #endregion
}
