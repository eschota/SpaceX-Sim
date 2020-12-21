using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Research : Unit
{
    public enum Type { None, Engine, Rockets, Modules, SpaceSuit, Equipment, Concept }
    
    public List<Research> Dependances;
    public int[] TimeCost = { 100, 100, 100 };

    public Vector2 position;
    public Vector2 pivotStart;
    public Vector2 pivotEnd; 
    public List<Module> ModulesOpen;    
    public UIResearchButton researchButton;

    public int[] TimeCompleted = { 0, 0, 0 };

    private bool _completed;
    public bool Completed
    {
        get
        {
            if (TimeCompleted[0] == TimeCost[0])
                if (TimeCompleted[1] == TimeCost[1])
                    if (TimeCompleted[2] == TimeCost[2]) 
                    {
                        _completed = true;
                    };
            return _completed;
                
        }
        set
        {
            if( value==true)
            {
                TimeCompleted = TimeCost;
                _completed = true;
            }
            else
            {
                TimeCompleted = new int []{0,0,0};
            }
            GameManager.EventUnit(this);
        }
    }
    
}
