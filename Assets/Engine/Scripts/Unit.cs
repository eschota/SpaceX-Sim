using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Unit : MonoBehaviour
{
    public enum UnitType { None, RocketLaunch, ResearchLab, ProductionFactory }
    public UnitType unitType = UnitType.None;
    public string Name;
    public int Days = 0;
    public int[] CreationDate;
    public virtual void Start()
    {
        
        CreationDate = new int [3];
        CreationDate[0] = TimeManager.Days; CreationDate[1] = TimeManager.Months; CreationDate[2]= TimeManager.Years;
        
        Debug.Log(string.Format("<color=blue> Created new Unit:" + name + "</color>"));
    }


    public virtual void Update()
    {
        
    }
}
