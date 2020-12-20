using System;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager instance;

    [SerializeField] private BuildingUnit[] buildingUnitPrefabs;

    public static event Action EventChangeState;
    public static event Action<Unit> EventCreatedNewUnit;

    public enum State
    {
    }

    private State _currentState;

    public State CurrentState
    {
        get => _currentState;
        set
        {
            switch (value)
            {
            }
            _currentState = value;
            EventChangeState();
        }
    }

    public BuildingUnit[] BuildingUnitPrefabs => buildingUnitPrefabs;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            DestroyImmediate(gameObject);
            return;
        }
    }
}