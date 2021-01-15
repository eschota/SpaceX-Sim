using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class BuildingResearchLab : BuildingUnit 
{
    private UiLabButton _ButtonLab;
    public UiLabButton ButtonLab
    {
        get
        {
            if (_ButtonLab == null)
            {
                _ButtonLab = Instantiate(Resources.Load<UiLabButton>("UI/ButtonUnits/ButtonResearchLab"+CurrentClass.ToString()));
                _ButtonLab.Lab = this;
                _ButtonLab.transform.SetParent(UIResearchManager.instance.Grid);
            }
            return _ButtonLab;
        }
    }
    public override void Awake()
    {
        base.Awake();
        ButtonLab.name = "ButtonLab" + Name;
    }
}
