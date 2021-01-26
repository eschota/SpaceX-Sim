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
    private int cost;
    void Start()
    {

        OkButton.onClick.AddListener(OnClick);
        SelectSize.onValueChanged.AddListener(OnSizeChange);
        WorldMapManager.instance.OnAllowedBuildChanged += OnAllowedBuildChanged;
        OnSizeChange(0);
    }

    private void OnAllowedBuildChanged(bool allowed)
    {
        OkButton.interactable = allowed;
    }

    private void UpdateCost()
    {
        cost = Mathf.FloorToInt((SelectSize.value + 1) * 50 * (1 + WorldMapManager.instance.currentPointValue * 2));
    }
    
    private void OnSizeChange(int id)
    {
        UpdateCost();
        OnChangeLabel();
    }

    private void OnValueChange()
    {
        UpdateCost();
        OnChangeLabel();
    }

    void OnChangeLabel()
    {
        TotalCost.text = cost + "K$";
    }
    private void OnDestroy()
    {
        OkButton.onClick.RemoveAllListeners();
        SelectSize.onValueChanged.RemoveAllListeners();
        if (WorldMapManager.instance) WorldMapManager.instance.OnAllowedBuildChanged -= OnAllowedBuildChanged;
    }
    void OnClick()
    {
        if (WorldMapManager.instance.CurrenUnitPoint == null) { Alert.instance.AlertMessage = "Select Place First!!!";  return; }

        GameObject obj = Instantiate(Resources.Load<GameObject>("UnitPoint/UnitPoint"));
        
        obj.transform.parent = GameManager.UnitsAll.Find(X => X.GetType() == typeof(UnitEarth)).transform;
        obj.transform.position = WorldMapManager.instance.CurrenUnitPoint.transform.position;

        if (WorldMapManager.instance.CurrentPointCountry.isOcean)// Если строим на океане, то создаем силонч несмотря ни на что
        {
            CreateUnitByType<UnitSeaLaunch>(obj.transform);
        }
        else
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
        if (GameManager.instance.Creation)
        {
            DangerZone.text = WorldMapManager.instance.MaxDamage.ToString();
            OnValueChange();
        }
    }
    void CreateUnitByType<T> (Transform transform) where T : MonoBehaviour
    {
        var unit = new GameObject(typeof(T).ToString()).AddComponent<T>();
        (unit as UnitEco).EcoRentCost = cost;
        unit.transform.SetParent(GameManager.Earth.transform);
        unit.transform.position = transform.position;
        GameManager.instance.OpenUnitScene(unit as Unit);
        
        
        CameraManager.FlyToUnit = unit.transform;
    }
}
