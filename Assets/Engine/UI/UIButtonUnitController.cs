using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonUnitController : MonoBehaviour
{
     UIButtonUnit[] units;

    [SerializeField] List<UIButtonUnit> UIButtonsUnitLaunchPlace;
    [SerializeField] List<UIButtonUnit> UIButtonsUnitResearch;
    [SerializeField] List<UIButtonUnit> UIButtonsUnitProduction;
    void Start()
    {
        units=GetComponentsInChildren<UIButtonUnit>();
        foreach (var item in units)
        {
            item.gameObject.SetActive(false);
        }

        GameManager.EventCreatedNewUnit += OnCreateNewUnit;
    }    
    void OnCreateNewUnit(Unit unit)
    {
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
}
