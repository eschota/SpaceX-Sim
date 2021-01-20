using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowSelectBuilding : UIWindows
{

    [SerializeField] TMPro.TextMeshProUGUI Name;
    
    private Unit _unit;
    public Unit unit
    {
        get => _unit;
        set
        {
            _unit = value;
            Name.text = value.Name;
        }
    }


    public override void Show()
    {
        base.Show();
        unit = UnitManager.instance.CurrentSelected;
    }
}
