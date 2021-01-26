using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingUnit : Module
{

    [SerializeField] public Vector2 Size;
    
   
    public enum BuildingClass{ Light=0, Medium=1,Heavy=2}
    public BuildingClass CurrentClass;
    public enum BuildinType { Launch, Research, Factory,SeaLaunch}
    [SerializeField] public List< BuildinType> Types;
    public float ConsctructionProcess = -10;
    public override void IniAfterJSONRead()
    {
        base.IniAfterJSONRead();
    }
    public override void Awake()
    {
        base.Awake();
        if (isResearch == false) transform.SetParent(GameManager.instance.BuildingsTransform); else transform.SetParent(GameManager.instance.ResearchModulesTransform);
        if (!GameManager.Buildings.Contains(this)) GameManager.Buildings.Add(this);
        
    }
    public int ConstructionCompleted => (Mathf.RoundToInt(ConsctructionProcess/ ProductionTime[0] ));


}