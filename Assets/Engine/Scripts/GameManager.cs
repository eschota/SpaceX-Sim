using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    void Start()
    {

    }
    private void Awake()
    {
        gameObject.AddComponent<Eco>();
        gameObject.AddComponent<TimeManager>();
    }
    void Update()
    {

    }
    #region Variables
    public static event Action EventChangeState;
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
    public static List<Unit> LaunchPlaces = new List<Unit>();
    public static void CreateLaunchPlace(CountrySO launchPlace)
    {
        if (!Eco.Buy(launchPlace.CostBuild, "Not Enough Money :(")) return; 
        GameObject LP = new GameObject();
        LP.AddComponent<Unit>();
        LaunchPlaces.Add(LP.GetComponent<Unit>());
         
        CurrentState = State.Play;
    }
    #endregion
    #region OnGui Hack Etc
    private bool F1 = false;
    void Hack()
    {
        if (Input.GetKeyDown(KeyCode.R)) UnityEngine.SceneManagement.SceneManager.LoadScene(0);
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
