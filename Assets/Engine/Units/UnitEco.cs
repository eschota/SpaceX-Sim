using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnitEco : Unit
{
    public int id = -1;
    public UIButtonUnit UIbutton;
    public Country Country;
    public bool EcoBilledFirstMonth=false;
    public int EcoRentCost=1;


    public override void IniAfterJSONRead()
    {
        base.IniAfterJSONRead();
        transform.SetParent(GameManager.Earth.transform);
        transform.localPosition = localPosition;
        transform.localRotation= Quaternion.Euler( localRotation);
    }

     
}
