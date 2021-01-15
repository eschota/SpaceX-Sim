using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager instance;
    [SerializeField] BuildingUnit.BuildinType ThisType;
    [SerializeField] private List <BuildingUnit> buildingUnitPrefabs;

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

    public List <BuildingUnit > BuildingUnitPrefabs => buildingUnitPrefabs;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            DestroyImmediate(gameObject);
            return;
        } 
        
        GetAvailableBuildingsIn();
        
        var canvas = Instantiate(Resources.Load("BuildUnitCanvas"));
        var buildUnitCanvas = FindObjectOfType<BuildUnitCanvas>();

        buildUnitCanvas.BuildButton.onClick.AddListener(() => BuildController.instance.OnBuildClick());
        buildUnitCanvas.DeleteUnitYesButton.onClick.AddListener(() => BuildController.instance.DeleteUnitAccept());
        buildUnitCanvas.DeleteUnitNoButton.onClick.AddListener(() => BuildController.instance.DeleteUnitCancel());
    }





    void GetAvailableBuildingsInTest()
    {
       
    }
    void GetAvailableBuildingsIn()
    {
        if (GameManager.instance == null) 
        {
            buildingUnitPrefabs.AddRange(Resources.LoadAll<BuildingUnit>(""));
            return;
        }
        buildingUnitPrefabs.AddRange(ResearchAndProductionManager.instance.BuildingsAvailable.FindAll(X => X.Types.Contains(ThisType)));
    }
}