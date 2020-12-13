using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScenarioManager : MonoBehaviour
{
    public static event Action EventChangeState; 
    public enum State {None, StartConditions,PoliticMap  }
    private static State _currentState;
    public static State CurrentState
    {
        get => _currentState;
        set
        {

            switch (value)
            {
                
            }

            Debug.Log(string.Format("<color=blue> State changed " + _currentState + ":=" + value + "</color>"));
            _currentState = value;
            if (EventChangeState != null) EventChangeState();

        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
