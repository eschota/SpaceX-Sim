using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitLaunchPlace : UnitEco
{
    void Awake()
    {
        id = GameManager.UnitsLaunchPlace.Count;
        GameManager.UnitsLaunchPlace.Add(this);
    }
    private void OnDestroy()
    {
        GameManager.UnitsLaunchPlace.Remove(this);

    }
}
