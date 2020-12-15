using UnityEngine;
using System;

public class TimeManager : MonoBehaviour
{
    public static event Action EventChangeDay;

    public static float LocalHoursOffset;
    public static float TimeScale = 1f;

    private static float _timer;
    private static int _days;
    private static int _years;
    private static int _months;
    private static float _hours;
    private static float _elapsedTime;

    public static int Days
    {
        get => _days;
        set
        {
            _days = value;
            EventChangeDay();
        }
    }

    public static int Years
    {
        get => _years;
        set
        {
            _years = value;
            EventChangeDay();
        }
    }

    public static int Months
    {
        get => _months;
        set
        {
            _months = value;
            EventChangeDay();
        }
    }

    public static float ElapsedTime
    {
        get => _elapsedTime;
    }

    public static float Hours => _hours;

    public static void Init()
    {
        Days = Mathf.RoundToInt(GameManager.GameParam.StartDate[0]);
        Years = Mathf.RoundToInt(GameManager.GameParam.StartDate[1]);
        Months = Mathf.RoundToInt(GameManager.GameParam.StartDate[2]);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.CurrentState == GameManager.State.Play)
                GameManager.CurrentState = GameManager.State.Pause;
            else
                GameManager.CurrentState = GameManager.State.Play;
        }

        if (GameManager.CurrentState != GameManager.State.Play)
            return;

        _timer += Time.deltaTime * TimeScale;
        CalendarControl();
    }

    private static void CalendarControl()
    {
        _hours += Time.deltaTime * TimeScale;
        if (_hours >= 24)
            _hours = 0;

        var days = _timer / 24f;

        if (days > 365)
        {
            Years++;
            _timer = 0;
        }

        if (days > 0 && days <= 31)
        {
            Days = (int) (days + 1);
            Months = 1;
        }
        else if (days > 31 && days <= 60)
        {
            Months = 2;
            Days = (int) (days - 31);
        }
        else if (days > 59 && days <= 90)
        {
            Months = 3;
            Days = (int) (days - 59);
        }
        else if (days > 90 && days <= 121)
        {
            Months = 4;
            Days = (int) (days - 90);
        }
        else if (days > 120 && days <= 150)
        {
            Months = 5;
            Days = (int) (days - 120);
        }
        else if (days > 151 && days <= 181)
        {
            Months = 6;
            Days = (int) (days - 151);
        }
        else if (days > 181 && days <= 212)
        {
            Months = 7;
            Days = (int) (days - 181);
        }
        else if (days > 212 && days <= 243)
        {
            Months = 8;
            Days = (int) (days - 212);
        }
        else if (days > 243 && days <= 273)
        {
            Months = 9;
            Days = (int) (days - 242);
        }
        else if (days > 273 && days <= 304)
        {
            Months = 10;
            Days = (int) (days - 273);
        }
        else if (days > 304 && days <= 334)
        {
            Months = 11;
            Days = (int) (days - 303);
        }
        else if (days > 334 && days <= 365)
        {
            Months = 12;
            Days = (int) (days - 333);
        }
    }
}