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

        var canvas = Instantiate(Resources.Load("BuildUnitCanvas"));
        var buildUnitCanvas = FindObjectOfType<BuildUnitCanvas>();

        buildUnitCanvas.BuildButton.onClick.AddListener(() => BuildController.instance.OnBuildClick());
        buildUnitCanvas.DeleteUnitYesButton.onClick.AddListener(() => BuildController.instance.DeleteUnitAccept());
        buildUnitCanvas.DeleteUnitNoButton.onClick.AddListener(() => BuildController.instance.DeleteUnitCancel());
    }
}