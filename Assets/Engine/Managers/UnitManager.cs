using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager instance;
    [SerializeField] BuildingUnit.BuildinType ThisType;
    [SerializeField] public List <BuildingUnit> buildingUnitPrefabs;
    [SerializeField] GameObject PlanarPlane;
    private Unit _CurrentSelected;
    public Unit CurrentSelected
    {
        get => _CurrentSelected;
        set
        {
            if (value == null)
            {
                _CurrentSelected = value;
                UIUnitManager.instance.WindowSelectUnit.CurrentMode = UIWindows.Mode.hide;

                return;
            }
            _CurrentSelected = value;            
            UIUnitManager.instance.WindowSelectUnit.CurrentMode = UIWindows.Mode.show;
            
        }
    }
    public enum State {None, SelectBuilding, PlaceBuilding }
    private State _CurrentState;
    public State CurrentState
    {
        get => _CurrentState;
        set
        {
            switch (value)
            {
                case State.None: 
                    CurrentSelected = null;
                    break;
                case State.SelectBuilding:
                    UIUnitManager.instance.BuildingsPanel.CurrentMode = UIWindows.Mode.hide;
                    break;
                case State.PlaceBuilding:
                    UIUnitManager.instance.CurrentBuilding = null;
                    UIUnitManager.instance.WindowSelectUnit.CurrentMode = UIWindows.Mode.hide;
                    break;
                default:
                    break;
            }
            _CurrentState = value;
        }
        
    }
    public List<Selectable> Selectables = new List<Selectable>();
    Terrain terra;

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
        if (GameManager.instance == null) gameObject.AddComponent<GameManager>();
            terra = FindObjectOfType<Terrain>();
       
        LoadBuildings();
    }



    private void Update()
    {

        if (Input.GetMouseButtonDown(1)) CurrentState = State.None;
    }

   public void PlaceBuilding(BuildingUnit unit,GameObject prefab, Vector3 pos, Vector3 rot)
    {
        unit.ConsctructionProcess = 0;
        Selectable S = prefab.GetComponent<Selectable>();
        S.transform.position = pos;
        S.RootUnit = Instantiate( unit);
        S.IniSelectable();
        CurrentSelected = S.RootUnit;
        
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

    void SliceTerrain()
    {
        TreeInstance[] originalTreeInstances;
        originalTreeInstances = Terrain.activeTerrain.terrainData.treeInstances;
    }



    void LoadBuildings()
    {
        if (GameManager.instance == null) return;
        foreach (var item in GameManager.Buildings)
        {
            if (item.ConsctructionProcess > -1)
            {
                Selectable temp=   Instantiate(Resources.Load<GameObject>(item.PrefabPath)).GetComponent<Selectable>();
                temp.RootUnit = item;
                temp.transform.position = item.transform.position;
            }
        }
    }
}