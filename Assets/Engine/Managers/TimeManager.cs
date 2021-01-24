using UnityEngine;
using System;

public class TimeManager : MonoBehaviour
{
    public static event Action EventChangeDay;

    public static float LocalHoursOffset;
    public static float TimeScale = 1f;
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

    public static float ElapsedTime
    {
        get => _elapsedTime;
    }

    public static float Hours;

    public static void Init()
    {
        Days = Mathf.RoundToInt(1);
        Years = Mathf.RoundToInt(1);
        Months = Mathf.RoundToInt(1);
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
    private static int days;
    private static void CalendarControl()
    {
        Hours += Time.deltaTime * Time.timeScale/24;
         
        if (Hours >= 24)
        {
            Hours = 0;
            Days++;
            days++;
        }
        if (Months > 12 || Months==0)
        {
            Months = 1;
            Years++;
        }

        if (days > 365)
        {
            Years++;
            Timer = 0;
            days = 1;
            Days = 1;
        }

        if (days > 0 && days <= 31)
        { 
            Months = 1;
        }
        else if (days > 31 && days <= 60)
        {
            if (Days > 31) Days = 1;
            Months = 2;
             
        }
        else if (days > 59 && days <= 90)
        {
            if (Days > 31) Days = 1;
            Months = 3;
           
        }
        else if (days > 90 && days <= 121)
        {
            if (Days > 31) Days = 1;
            Months = 4;
         
        }
        else if (days > 120 && days <= 150)
        {
            if (Days > 30) Days = 1;
            Months = 5;
            
        }
        else if (days > 151 && days <= 181)
        {
            if (Days > 30) Days = 1;
            Months = 6;
           
        }
        else if (days > 181 && days <= 212)
        {
            if (Days > 31) Days = 1;
            Months = 7;
          
        }
        else if (days > 212 && days <= 243)
        {
            if (Days > 31) Days = 1;
            Months = 8;
           
        }
        else if (days > 243 && days <= 273)
        {
            if (Days > 28) Days = 1;
            Months = 9;
          
        }
        else if (days > 273 && days <= 304)
        {
            if (Days > 31) Days = 1;
            Months = 10;
          
        }
        else if (days > 304 && days <= 334)
        {
            if (Days > 30) Days = 1;
            Months = 11;
           
        }
        else if (days > 334 && days <= 365)
        {
            if (Days > 31) Days = 1;
            Months = 12;
            
        }
    }
}