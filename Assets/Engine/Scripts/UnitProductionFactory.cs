using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitProductionFactory : Unit
{
    public override void Start()
    {
        base.Start();

        unitType = UnitType.ProductionFactory;
    }
}
