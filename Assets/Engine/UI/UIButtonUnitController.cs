﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
public class UIButtonUnitController : MonoBehaviour
{
    UIButtonUnit[] units;

    private Unit SelectedUnit;

    [SerializeField] Button EnterCurrentUnit;
    [SerializeField] List<UIButtonUnit> UIButtonsUnitLaunchPlace;
    [SerializeField] List<UIButtonUnit> UIButtonsUnitResearch;
    [SerializeField] List<UIButtonUnit> UIButtonsUnitProduction;

    void Start()
    {
        units = GetComponentsInChildren<UIButtonUnit>();
        foreach (var item in units)
        {
            item.gameObject.SetActive(false);
        }

        EnterCurrentUnit.onClick.AddListener(OnClickEnter);
        GameManager.UnitsAll.CollectionChanged += OnCreateNewUnit;
        EnterCurrentUnit.gameObject.SetActive(false);
    }

    void OnCreateNewUnit(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems == null) return;
        Unit unit = e.NewItems[0] as Unit;
        HideEnterButton();
        if (unit.GetType() == typeof(UnitLaunchPlace))
        {
            UIButtonsUnitLaunchPlace[(unit as UnitEco).id].gameObject.SetActive(true);
            UIButtonsUnitLaunchPlace[(unit as UnitEco).id].unit = unit;
        }

        if (unit.GetType() == typeof(UnitResearchLab))
        {
            UIButtonsUnitResearch[(unit as UnitEco).id].gameObject.SetActive(true);
            UIButtonsUnitResearch[(unit as UnitEco).id].unit = unit;
        }

        if (unit.GetType() == typeof(UnitProductionFactory))
        {
            UIButtonsUnitProduction[(unit as UnitEco).id].gameObject.SetActive(true);
            UIButtonsUnitProduction[(unit as UnitEco).id].unit = unit;
        }
    }

    public void OnClickEnter()
    {
        GameManager.instance.OpenUnitScene(SelectedUnit);
        CameraManager.FlyToUnit = SelectedUnit.transform;
    }

    public void ShowEnterButton(Unit unit, Vector3 pos)
    {
        SelectedUnit = unit;
        EnterCurrentUnit.transform.position = pos + Vector3.up * 80;
        EnterCurrentUnit.gameObject.SetActive(true);
    }

    public void HideEnterButton()
    {
        SelectedUnit = null;
        EnterCurrentUnit.gameObject.SetActive(false);
    }
}