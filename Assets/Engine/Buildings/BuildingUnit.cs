using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingUnit : Module
{

    [SerializeField] public Vector2 Size;
    
    public enum EBuildingState
    {
        AcceptForBuilding,
        NotAcceptForBuilding,
        BuildingNow,
        Working,
        NotWorking
    } 
    public enum BuildingClass{ Light=0, Medium=1,Heavy=2}
    public BuildingClass CurrentClass;
    public enum BuildinType { Launch, Research, Factory}
    [SerializeField] public List< BuildinType> Types;
    private EBuildingState _currentState;
    public static event Action EventChangeState;

    public EBuildingState CurrentState
    {
        get => _currentState;
        set
        {
            switch (value)
            {
                case EBuildingState.AcceptForBuilding:
                    
                    break;
                case EBuildingState.NotAcceptForBuilding:
                   
                    break;
                case EBuildingState.BuildingNow:
                  
                    break;
                case EBuildingState.Working:
                    break;
                case EBuildingState.NotWorking:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
            _currentState = value;
            EventChangeState?.Invoke();
        }
    }
    
 
     

    public override void IniAfterJSONRead()
    {
        base.IniAfterJSONRead();
    }
    public override void Awake()
    {
        base.Awake();
        GameManager.Buildings.Add(this);
    }


}