using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnitEco : Unit
{
    public int id = -1;    
    public Country Country;
    public bool EcoBilledFirstMonth=false;
    public int EcoRentCost=1;
    private UIButtonUnit _UIButtonPlay;
    public UIButtonUnit UIButtonPlay
    {
        get
        {
            if (_UIButtonPlay == null)
            {

                _UIButtonPlay = Instantiate(Resources.Load<UIButtonUnit>("UI/ButtonUnits/" + GetType().ToString()));
                UIButtonUnitOnEarth alter= Instantiate(Resources.Load<UIButtonUnitOnEarth>("UI/ButtonUnits/" + GetType().ToString()+"Earth"));
                alter.transform.SetParent(UIButtonUnitController.instance.transform);
                alter.mainNutton = _UIButtonPlay.btn;
                alter.unit = this;
                _UIButtonPlay.transform.SetParent(UIButtonUnitController.instance.UnitsGrid);
            }
            return _UIButtonPlay;
        }
    }
    public override void IniAfterJSONRead()
    {
        base.IniAfterJSONRead();
        transform.SetParent(GameManager.Earth.transform);
        transform.localPosition = localPosition;
        transform.localRotation= Quaternion.Euler( localRotation);
        UIButtonPlay.name = "UIButton" + Name;
        UIButtonPlay.unit = this;
    }
    public override void Awake()
    {
        base.Awake();
        UIButtonPlay.name = "UIButton" + Name;
        UIButtonPlay.unit = this;
    }
}
