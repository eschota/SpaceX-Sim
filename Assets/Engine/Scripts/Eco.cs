﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Eco : MonoBehaviour
{

    public static void IniEco(string LoadName)
    {
        if (LoadName == "")
        {
            Balance = GameManager.GameParam.StartBalance;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static event Action EventChangeBalance;
    private static int _balance;
    public static int Balance
    {
        get => _balance;
        set
        {
            
            Debug.Log("Balance changed " + _balance + " => " + value);
            _balance = value;
            if (EventChangeBalance != null) EventChangeBalance();
        }
    }
    public static bool BalanceTest(int cost)
    {
        if (Balance - cost > 0) return true;
        else
        {
            Alert.instance.AlertMessage = "Not Enough Minerals";
            return false;
        }
    }

}
