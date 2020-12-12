using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitResearchLab : UnitEco
{
    void Awake()
    {
        id = GameManager.UnitsResearchLab.Count;
        GameManager.UnitsResearchLab.Add(this);
    }
}
