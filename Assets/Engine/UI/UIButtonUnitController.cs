using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        units=GetComponentsInChildren<UIButtonUnit>();
        foreach (var item in units)
        {
            item.gameObject.SetActive(false);
        }
        EnterCurrentUnit.onClick.AddListener(OnClickEnter);
        GameManager.EventCreatedNewUnit += OnCreateNewUnit;
        EnterCurrentUnit.gameObject.SetActive(false);

    }
    void OnCreateNewUnit(Unit unit)
    {
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
    void OnClickEnter()
    {
    
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
