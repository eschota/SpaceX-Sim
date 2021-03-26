using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager instance;
    [SerializeField] BuildingUnit.BuildinType ThisType;
    [SerializeField] public List <BuildingUnit> buildingUnitPrefabs;
    [SerializeField] GameObject ConstructionGrid;
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
  
                    ConstructionGrid.SetActive(false);
                    terra.gameObject.SetActive(true);
                    UIUnitManager.instance. BuildingsPanel.CurrentMode = UIWindows.Mode.hide;
                    break;
                case State.SelectBuilding:
                    UIUnitManager.instance.BuildingsPanel.CurrentMode = UIWindows.Mode.hide;
                    ConstructionGrid.SetActive(false);
                    terra.gameObject.SetActive(true);
                    break;
                case State.PlaceBuilding:
 
                    UIUnitManager.instance.CurrentBuilding = null;
                   
                    ConstructionGrid.SetActive(true);
                    terra.gameObject.SetActive(false);
                    UIUnitManager.instance.WindowSelectUnit.CurrentMode = UIWindows.Mode.hide;
                    break;
                default:
                    break;
            }
            Debug.Log("State Changed: => " + value);
            _CurrentState = value;
        }
        
    }
    public List<Selectable> Selectables = new List<Selectable>();
  [SerializeField]  Transform terra;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            DestroyImmediate(gameObject);
            return;
        }
        ConstructionGrid.transform.position = new Vector3(ConstructionGrid.transform.position.x, 0, ConstructionGrid.transform.position.z);
        ConstructionGrid.SetActive(false);
        GetAvailableBuildingsIn();
       

        if (terra == null) terra = FindObjectOfType<Terrain>().transform.parent;
        
        LoadBuildings();
    }



    private void Update()
    {

        if (Input.GetMouseButtonDown(1)) CurrentState = State.None;
    }

   public void PlaceBuilding(BuildingUnit unit,GameObject prefab, Vector3 pos, Vector3 rot)
    {
        
        Selectable S = prefab.GetComponent<Selectable>();
        S.transform.position = pos;
        S.RootUnit = Instantiate( unit);
        S.IniSelectable(); 
        S.RootUnit.SetParentInHierarchyByType(S.RootUnit.isResearch=false);
        S.RootUnit.transform.localPosition = pos;
        S.RootUnit.localPosition = pos;
        S.RootUnit.ConsctructionProcess = 1;

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
        buildingUnitPrefabs.AddRange(ResearchAndProductionManager.instance.BuildingsAvailableForBuild.FindAll(X => X.Types.Contains(ThisType)));
    }

    void SliceTerrain()
    {
        TreeInstance[] originalTreeInstances;
        originalTreeInstances = Terrain.activeTerrain.terrainData.treeInstances;
    }



    void LoadBuildings()
    {
        if (GameManager.instance == null) return;
        foreach (var item in GameManager.Buildings.FindAll(X=>X.isResearch==false))
        {
            if(item.Types.Contains(ThisType))         
            {
                Selectable temp=   Instantiate(Resources.Load<GameObject>(item.PrefabPath)).GetComponent<Selectable>();
                temp.RootUnit = item;
                temp.transform.localPosition = item.localPosition;
                temp.IniSelectable();
              
            }
        }
    }
}