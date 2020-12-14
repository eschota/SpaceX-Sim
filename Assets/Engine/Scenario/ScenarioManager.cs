﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

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
    #region  Scenario
    [SerializeField] TMPro.TMP_InputField ScenarioName;
    [SerializeField] TMPro.TMP_InputField ScenarioStartDay;
    [SerializeField] TMPro.TMP_InputField ScenarioStartMonth;
    [SerializeField] TMPro.TMP_InputField ScenarioStartYear;
    [SerializeField] TMPro.TMP_InputField ScenarioStartBalance;
    Scenario scenario;
    public void CreateScenarioButtonClick()
    {
        scenario = new Scenario( ScenarioName.text, int.Parse(ScenarioStartDay.text), int.Parse(ScenarioStartMonth.text), int.Parse(ScenarioStartYear.text), int.Parse(ScenarioStartBalance.text));
        scenario.SaveScenario();
    }
    [System.Serializable]
    public class Scenario
    {
        public int[] StartDate;
        public string Name;
        public int StartBalance; 
        public Scenario ( string _Name, int  _StartDay, int _StartMonth, int _StartYear, int _StartBalance)
        {
            Name = _Name;
            StartBalance = _StartBalance;
            StartDate = new int[3];
            StartDate[0] = _StartDay;
            StartDate[1] = _StartMonth;
            StartDate[2] = _StartYear;
        }
        public void SaveScenario()
        {
            string jsonData = JsonUtility.ToJson(this, true);
            File.WriteAllText(Application.streamingAssetsPath+"/Scenarios/"+Name+".dat", jsonData);
            Debug.Log("File Saved at: " + Application.streamingAssetsPath + "/Scenarios/" + Name + ".dat");
        }
    }
    #endregion
}
