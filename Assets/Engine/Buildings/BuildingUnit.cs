using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingUnit : Module
{
    public enum BuildinType { Launch, Research, Factory, SeaLaunch }

    public enum BuildingClass { Light = 0, Medium = 1, Heavy = 2 }

    public int Productivity = 1;// скорость исследований, производства и т п

    [SerializeField] public Vector2 Size;
    
   
    public BuildingClass CurrentBuildingClass;
    [SerializeField] public List< BuildinType> Types;
    public float ConsctructionProcess = -10;
    public override void IniAfterJSONRead()
    {
        base.IniAfterJSONRead();
        SplitByIsResearch();
    }
    public override void Awake()
    {
        base.Awake();        
        if (!GameManager.Buildings.Contains(this)) GameManager.Buildings.Add(this);
        SplitByIsResearch();

    }
    public virtual void SplitByIsResearch()
    {
        if (isResearch) transform.SetParent(GameManager.instance.BuildingResearchTransform);
        else transform.SetParent(GameManager.instance.BuildingsTransform);
    }

    public int ConstructionCompletedPercentage => Mathf.Clamp( Mathf.RoundToInt(100 * ((float)ConsctructionProcess / (float)ProductionTime[0])),0,100);


}