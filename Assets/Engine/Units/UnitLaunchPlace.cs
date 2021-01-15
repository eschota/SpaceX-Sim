using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitLaunchPlace : UnitEco
{

    public override void Awake()
    {
        base.Awake();
        id = GameManager.UnitsLaunchPlace.Count+1;
        GameManager.UnitsLaunchPlace.Add(this);
    }
    private void OnDestroy()
    {
        GameManager.UnitsLaunchPlace.Remove(this);

    }
    public override void IniAfterJSONRead()
    {
        base.IniAfterJSONRead();
        UIButtonPlay.number.text = (GameManager.UnitsLaunchPlace.Count).ToString();
    }
}
