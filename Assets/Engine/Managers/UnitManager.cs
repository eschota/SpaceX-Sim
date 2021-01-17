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