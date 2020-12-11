using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitProductionFactory : UnitEco
{
    void Awake()
    {
        id = GameManager.UnitsProductionFactory.Count;
        GameManager.UnitsProductionFactory.Add(this);
    }
}
