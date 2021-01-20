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
      
     
    }

   public void PlaceBuilding(BuildingUnit unit,GameObject prefab, Vector3 pos, Vector3 rot)
    {
        unit.ConsctructionProcess = 0;
        Selectable S = prefab.GetComponent<Selectable>();
        S.transform.position = pos;
        S.RootUnit = Instantiate( unit);
        S.IniSelectable();
         
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