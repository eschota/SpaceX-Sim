using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    public static event Action EventChangeState;
    public static float EventTimer = 1;
    public enum State { None, Play, Pause, Building }
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

    }

    // Update is called once per frame
    void Update()
    {
        EventTimer += Time.unscaledDeltaTime;

        if (EventTimer > 2 && CurrentState == State.None) CurrentState = State.Play;
        if (Input.GetKeyDown(KeyCode.F1)) CurrentState = State.None;
        if (Input.GetKeyDown(KeyCode.F2)) CurrentState = State.Building;
    }
}
