using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class WindowCreateNewEarthUnit : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Dropdown SelectSize;
    [SerializeField] TMPro.TextMeshProUGUI TotalCost;
    public static CountrySO CurrentLauchPlace;
    [SerializeField] GameObject CancelButton;
    [SerializeField] Button OkButton;
    [SerializeField] TMPro.TextMeshProUGUI DangerZone;
    private GameObject UnitLaunchPrefab;
    void Start()
    {

        OkButton.onClick.AddListener(OnClick);
        SelectSize.onValueChanged.AddListener(OnChangeValue);
        OnChangeValue(0);
    }
    
    void OnChangeValue(int id)
    {
        TotalCost.text = ((id + 1) * 100).ToString()+"K$";
    }
    private void OnDestroy()
    {
        OkButton.onClick.RemoveAllListeners();
        SelectSize.onValueChanged.RemoveAllListeners();
    }
    void OnClick()
    {

        if (WorldMapManager.instance.CurrenUnitPoint == null) { Alert.instance.AlertMessage = "Select Place First!!!";  return; }

            GameObject obj = Instantiate(Resources.Load<GameObject>("UnitPoint/UnitPoint"));

             
           
           

           
        
        obj.transform.parent = GameManager.UnitsAll.Find(X => X.GetType() == typeof(UnitEarth)).transform;
        obj.transform.position = WorldMapManager.instance.CurrenUnitPoint.transform.position;

        
        switch (GameManager.CurrentState)
        {
            case GameManager.State.CreateLaunchPlace:
                CreateUnitByType<UnitLaunchPlace>(obj.transform);
                break;
            case GameManager.State.CreateResearchLab:
                CreateUnitByType<UnitResearchLab>(obj.transform);
                break;
            case GameManager.State.CreateProductionFactory:
                CreateUnitByType<UnitProductionFactory>(obj.transform);
                break;
        }
        Destroy(obj.gameObject);


    }
    private void Update()
    {
        if(GameManager.instance.Creation)
        DangerZone.text = WorldMapManager.instance.MaxDamage.ToString();   
    }
    void CreateUnitByType<T> (Transform transform) where T : MonoBehaviour
    {
        var unit = new GameObject(typeof(T).ToString()).AddComponent<T>();
        (unit as UnitEco).EcoRentCost = (SelectSize.value + 1) * 100;
        unit.transform.SetParent(GameManager.Earth.transform);
        unit.transform.position = transform.position;
        GameManager.instance.OpenUnitScene(unit as Unit);
        
        
        CameraManager.FlyToUnit = unit.transform;
    }
}
