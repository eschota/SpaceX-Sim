using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EState : MonoBehaviour
{
    public static event Action EventChangeState;
    public static float EventTimer=1;
    public enum UIState { None, Play, Pause, Building}
    private static UIState _currentState;
    public static UIState CurrentState
    {
        get
        {

            return _currentState;
        }

        set
        {
            _currentState = value;
            if(EventChangeState!=null) EventChangeState();
            Debug.Log("Changing State: " + value);
            EventTimer = 0;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        EventTimer += Time.unscaledDeltaTime;
        if (Input.GetKeyDown(KeyCode.F1)) CurrentState = UIState.None;
        if (Input.GetKeyDown(KeyCode.F2)) CurrentState = UIState.Building;
    }
}
