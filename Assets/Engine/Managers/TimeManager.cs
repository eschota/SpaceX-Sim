using UnityEngine;
using System;

public class TimeManager : MonoBehaviour
{
    public static event Action EventChangeDay;

    public static float LocalHoursOffset;
    public static float TimeScale = .5f;
    public static float Timer =0;

    
    private static int _days;
    private static int _years;
    private static int _months;
    private static float _hours;
    private static float _elapsedTime;

    public static int TotalDays;

    public static int Days
    {
        get => _days;
        set
        {
            _days = value;
            TotalDays++;
            EventChangeDay();
        }
    }

    public static int Years
    {
        get => _years;
        set
        {
            _years = value;
            
        }
    }

    public static int Months
    {
        get => _months;
        set
        {
            _months = value;
            
        }
    }

    public static float Hours => _hours;

    public static float ElapsedTime
    {
        get => _elapsedTime;
    }

    public static void Init()
    {
        Days = Mathf.RoundToInt(ScenarioManager.instance.CurrentScenario.StartDate[0]);
        Years = Mathf.RoundToInt(ScenarioManager.instance.CurrentScenario.StartDate[1]);
        Months = Mathf.RoundToInt(ScenarioManager.instance.CurrentScenario.StartDate[2]);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.CurrentState == GameManager.State.PlaySpace)
                GameManager.CurrentState = GameManager.State.Pause;
            else
                GameManager.CurrentState = GameManager.State.PlaySpace;
        }
        
        if (SpeedManager.instance.CurrenSpeed == SpeedManager.Speed.Stop) return;
        CalendarControl();
    }
    private static int days_of_year;
    private static void CalendarControl()
    {
        _hours += Time.deltaTime * TimeScale;
         
        if (_hours >= 24)
        {
            _hours = 0;
            Days++;
            days_of_year++;
        }
        if (Months > 12)
        {
            Months = 1;
            Years++;
        }

        if (days_of_year > 365)
        {
            Years++;
            Timer = 0;
            days_of_year = 1;
            Days = 1;
        }

        if (days_of_year > 0 && days_of_year <= 31)
        { 
            Months = 1;
        }
        else if (days_of_year > 31 && days_of_year <= 60)
        {
            if (Days > 31) Days = 1;
            Months = 2;
        }
        else if (days_of_year > 59 && days_of_year <= 90)
        {
            if (Days > 31) Days = 1;
            Months = 3;
        }
        else if (days_of_year > 90 && days_of_year <= 121)
        {
            if (Days > 31) Days = 1;
            Months = 4;
        }
        else if (days_of_year > 120 && days_of_year <= 150)
        {
            if (Days > 30) Days = 1;
            Months = 5;
        }
        else if (days_of_year > 151 && days_of_year <= 181)
        {
            if (Days > 30) Days = 1;
            Months = 6;
        }
        else if (days_of_year > 181 && days_of_year <= 212)
        {
            if (Days > 31) Days = 1;
            Months = 7;
        }
        else if (days_of_year > 212 && days_of_year <= 243)
        {
            if (Days > 31) Days = 1;
            Months = 8;
        }
        else if (days_of_year > 243 && days_of_year <= 273)
        {
            if (Days > 28) Days = 1;
            Months = 9;
        }
        else if (days_of_year > 273 && days_of_year <= 304)
        {
            if (Days > 31) Days = 1;
            Months = 10;
        }
        else if (days_of_year > 304 && days_of_year <= 334)
        {
            if (Days > 30) Days = 1;
            Months = 11;
        }
        else if (days_of_year > 334 && days_of_year <= 365)
        {
            if (Days > 31) Days = 1;
            Months = 12;
        }
    }
}