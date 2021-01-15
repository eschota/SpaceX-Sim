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



    private void Update()
    {
        //удалить после тестов
        if (Input.GetKeyDown(KeyCode.Z)) PlaceBuilding(buildingUnitPrefabs[0], Vector3.zero, Vector3.zero);
    }

    void PlaceBuilding(BuildingUnit unit, Vector3 pos, Vector3 rot)
    {
        BuildingUnit newUnit= Instantiate(unit);
        newUnit.transform.position = pos;
        newUnit.transform.rotation= Quaternion.Euler(rot);
        ResearchAndProductionManager.instance?.AddBuilding(newUnit);
    }
    void GetAvailableBuildingsIn()
    {
        
        if (GameManager.instance == null) 
        {
            if (buildingUnitPrefabs.Count > 0) if (buildingUnitPrefabs[0] != null) return;
            buildingUnitPrefabs.Clear();
            buildingUnitPrefabs.AddRange(Resources.LoadAll<BuildingUnit>(""));
            return;
        }
        buildingUnitPrefabs.Clear();
        buildingUnitPrefabs.AddRange(ResearchAndProductionManager.instance.BuildingsAvailable.FindAll(X => X.Types.Contains(ThisType)));
    }

   
}