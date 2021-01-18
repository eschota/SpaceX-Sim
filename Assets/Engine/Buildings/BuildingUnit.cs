﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingUnit : Module
{

    [SerializeField] public Vector2 Size;
    
   
    public enum BuildingClass{ Light=0, Medium=1,Heavy=2}
    public BuildingClass CurrentClass;
    public enum BuildinType { Launch, Research, Factory}
    [SerializeField] public List< BuildinType> Types;
    public float ConsctructionProcess = -1;
    public override void IniAfterJSONRead()
    {
        base.IniAfterJSONRead();
    }
    public override void Awake()
    {
        base.Awake();
        GameManager.Buildings.Add(this);
    }

  
}