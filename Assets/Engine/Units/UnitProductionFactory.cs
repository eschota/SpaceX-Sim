using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitProductionFactory : UnitEco
{
    public override void Awake()
    {
        base.Awake();
        id = GameManager.UnitsProductionFactory.Count;
        GameManager.UnitsProductionFactory.Add(this);
    }
    public override void IniAfterJSONRead()
    {
        base.IniAfterJSONRead();
        UIButtonPlay.number.text = (GameManager.UnitsProductionFactory.Count ).ToString();
    }
}
