using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingUnit : Module
{


    
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
                    rootRenderer.material.SetColor("_Color", Color.green);
                    break;
                case EBuildingState.NotAcceptForBuilding:
                    rootRenderer.material.SetColor("_Color", Color.red);
                    break;
                case EBuildingState.BuildingNow:
                    rootRenderer.material.SetColor("_Color", Color.white);
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
    
    [SerializeField] private int size;
    [SerializeField] private string title;    
    [SerializeField] private Renderer rootRenderer;
    
    private BuildCell[] _cells;
    
    public int Size => size;
    public BuildCell[] Cells
    {
        get => _cells;
        set => _cells = value;
    }

    public string Title => title;


    public override void IniAfterJSONRead()
    {
        base.IniAfterJSONRead();
    }


    
}