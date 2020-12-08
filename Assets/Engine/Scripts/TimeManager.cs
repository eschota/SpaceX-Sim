using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeManager : MonoBehaviour
{
    public static float Timer;
    public static event Action EventChangeDay;
    private static int _days;
    private static float _elapsedTime;
    public static float ElapsedTime { get => _elapsedTime; }

    public static int Days
    {
        get => _days;
        set
        {
            _days = value;
            EventChangeDay();
        }
    }
    static int _years;
    public static int Years
    {
        get => _years;
        set
        {
            _years = value;
            EventChangeDay();
        }
    }
    static int _months;
    public static int Months
    {
        get => _months;
        set
        {
            _months = value;
            EventChangeDay(); 
        }
    }
    public static void Ini()
    {
        Days = Mathf.RoundToInt( GameManager.GameParam.StartDate[0]);
        Years = Mathf.RoundToInt(GameManager.GameParam.StartDate[1]);
        Months = Mathf.RoundToInt(GameManager.GameParam.StartDate[2]);

    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
            if(GameManager.CurrentState==GameManager.State.Play) GameManager.CurrentState = GameManager.State.Pause;
            else GameManager.CurrentState = GameManager.State.Play;

            if (GameManager.CurrentState != GameManager.State.Play) return;
         
            Timer += Time.deltaTime;
            CalendarControl();
    }
    void CalendarControl()
    {
        if (Timer > 365)
        {
            Years++;
            Timer = 0;

        }
        if (Timer > 0 && Timer <= 31)
        {
            Days = Mathf.RoundToInt(Timer + 1);
            Months = 1;
        }
        else
        if (Timer > 31 && Timer <= 60)
        {
            Months = 2;
            Days = Mathf.RoundToInt(Timer - 31);
        }
        else
        if (Timer > 59 && Timer <= 90)
        {
            Months = 3;
            Days = Mathf.RoundToInt(Timer - 59);
        }
        else
        if (Timer > 90 && Timer <= 121)
        {

            Months = 4;
            Days = Mathf.RoundToInt(Timer - 90);
        }
        else
        if (Timer > 120 && Timer <= 150)
        {
            Months = 5;
            Days = Mathf.RoundToInt(Timer - 120);
        }
        else
        if (Timer > 151 && Timer <= 181)
        {

            Months = 6;
            Days = Mathf.RoundToInt(Timer - 151);
        }
        else
        if (Timer > 181 && Timer <= 212)
        {

            Months = 7;
            Days = Mathf.RoundToInt(Timer - 181);
        }
        else
        if (Timer > 212 && Timer <= 243)
        {

            Months = 8;
            Days = Mathf.RoundToInt(Timer - 212);
        }
        else
        if (Timer > 243 && Timer <= 273)
        {

            Months = 9;
            Days = Mathf.RoundToInt(Timer - 242);
        }
        else
        if (Timer > 273 && Timer <= 304)
        {

            Months = 10;
            Days = Mathf.RoundToInt(Timer - 273);
        }
        else
        if (Timer > 304 && Timer <= 334)
        {

            Months = 11;
            Days = Mathf.RoundToInt(Timer - 303);
        }
        else
        if (Timer > 334 && Timer <= 365)
        {

            Months = 12;
            Days = Mathf.RoundToInt(Timer - 333);
        }
    }
}
