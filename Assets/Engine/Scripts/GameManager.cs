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
    }
    void Update()
    {

    }
    #region Variables
    public static event Action EventChangeState;
    public enum State { MenuStartGame, Pause, MenuLoadGame, Play, CreateLauchPlace, PlayStation, PlayBase }
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
                        return;
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
        CurrentState = State.CreateLauchPlace;
    }
    static public void LoadGame(string Name)
    {
        Eco.IniEco(Name);
    }
    #endregion
    #region LauchPlace
    public static List<Unit> LaunchPlaces = new List<Unit>();
    public static void CreateLaunchPlace(GameParametersLaunchPlace launchPlace)
    {
        GameObject LP = new GameObject();LP.AddComponent<Unit>();
        LaunchPlaces.Add(LP.GetComponent<Unit>());
        Eco.Balance -= launchPlace.CostBuild;
    }
    #endregion
}
