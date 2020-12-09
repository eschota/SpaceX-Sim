using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonUnitController : MonoBehaviour
{
     UIButtonUnit[] units;
    void Start()
    {
        units=GetComponentsInChildren<UIButtonUnit>();
        foreach (var item in units)
        {
            item.gameObject.SetActive(false);
        }

        GameManager.EventCreatedNewUnit += OnCreateNewUnit;
    }    
    void OnCreateNewUnit()
    {
        int sum = FindObjectsOfType<UnitLaunchPlace>().Length;
        for (int i = 0; i <units.Length; i++)
        {
            if(sum>0)
            if (units[i].unitType == UIButtonUnit.UnitType.RocketLaunch)
            {
                    units[i].gameObject.SetActive(true);
                    sum--;
                }
        }
         sum = FindObjectsOfType<UnitResearchLab>().Length;
        for (int i = 0; i < units.Length; i++)
        {
            if (sum > 0)
                if (units[i].unitType == UIButtonUnit.UnitType.ResearchLab)
                {
                    units[i].gameObject.SetActive(true);
                    sum--;
                }
        }
        sum = FindObjectsOfType<UnitProductionFactory>().Length;
        for (int i = 0; i < units.Length; i++)
        {
            if (sum > 0)
                if (units[i].unitType == UIButtonUnit.UnitType.ProductionFactory)
                {
                    units[i].gameObject.SetActive(true);
                    sum--;
                }
        }
    }
}
