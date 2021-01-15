using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitResearchLab : UnitEco
{
    public override void Awake()
    {
        base.Awake();
        id = GameManager.UnitsResearchLab.Count;
        GameManager.UnitsResearchLab.Add(this);
    }
    public override void IniAfterJSONRead()
    {
        base.IniAfterJSONRead();
        UIButtonPlay.number.text = (GameManager.UnitsResearchLab.Count ).ToString();
    }
}
