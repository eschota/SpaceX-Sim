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

        terra = FindObjectOfType<Terrain>();
        PlanarPlane.GetComponent<MeshRenderer>().enabled = false;
        PlanarPlane.layer = 2;
        LoadBuildings();
    }



    private void Update()
    {
      
     
    }

   public void PlaceBuilding(BuildingUnit unit,GameObject prefab, Vector3 pos, Vector3 rot)
    {
        Selectable S = prefab.GetComponent<Selectable>();
        S.transform.position = pos;
        S.RootUnit = Instantiate( unit); 
        //
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
            Instantiate(Resources.Load<GameObject>(item.PrefabPath));
        }
    }
}