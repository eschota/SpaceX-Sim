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
            if (isResearch) return null;
            if (_ButtonLab == null)
            {
                _ButtonLab = Instantiate(Resources.Load<UiLabButton>("UI/ButtonUnits/ButtonResearchLab"+CurrentClass.ToString()));
                _ButtonLab.Lab = this;
                _ButtonLab.transform.SetParent(UIResearchManager.instance.Grid);
                _ButtonLab.name = "ButtonLab" + Name;
            }
            return _ButtonLab;
        }
    }
    public override void Awake()
    {
        base.Awake();        
    }
}
