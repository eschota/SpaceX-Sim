using System;
using UnityEngine;

public class BuildingUnit : MonoBehaviour
{
    public enum EBuildingState
    {
        AcceptForBuilding,
        NotAcceptForBuilding,
        BuildingNow,
        Working,
        NotWorking
    }
    
    [SerializeField] private EBuildingState _currentState;
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
    [SerializeField] private Sprite icon;
    [SerializeField] private Renderer rootRenderer;
    
    private BuildCell[] _cells;
    
    public int Size => size;
    public BuildCell[] Cells
    {
        get => _cells;
        set => _cells = value;
    }

    public string Title => title;
    public Sprite Icon => icon;
}