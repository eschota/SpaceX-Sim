using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class ResearchManager : MonoBehaviour
{
    public static ResearchManager instance;
    bool ini = false;
    private void Awake()
    {
        TimeManager.EventChangeDay += OnChangeDay;
    }
    void OnChangeDay()
    {
        if (ini == false) Ini();


        CalculateResearchesProgress();
    }
    public void CalculateResearchesProgress()
    {

    }


    public void Ini()
    {
        ini = true;
    }
}