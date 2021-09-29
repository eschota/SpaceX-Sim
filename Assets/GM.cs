using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{
    public static event Action EventChangeState;
    public static float EventTimer = 1;
    public enum State { None,ManageRocket, Play, Pause, Win,Lose}
    private static State _currentState;
    public static State CurrentState
    {
        get
        {

            return _currentState;
        }

        set
        {
            _currentState = value;
            if (EventChangeState != null) EventChangeState();
            Debug.Log("Changing State: " + value);
            EventTimer = 0;
        }
    }
    void Start()
    {
        EventTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        EventTimer += Time.unscaledDeltaTime;

        if (EventTimer > 5 && CurrentState == State.None) CurrentState = State.ManageRocket;
        if (Input.GetKeyDown(KeyCode.F1)) CurrentState = State.None;
        if (Input.GetKeyDown(KeyCode.F2)) CurrentState = State.ManageRocket;
        if (Input.GetKeyDown(KeyCode.F3)) CurrentState = State.Play;
        if (Input.GetKeyDown(KeyCode.F4)) CurrentState = State.Pause;
        if (Input.GetKeyDown(KeyCode.F5)) CurrentState = State.Win;
        if (Input.GetKeyDown(KeyCode.F6)) CurrentState = State.Lose;
    }
}
