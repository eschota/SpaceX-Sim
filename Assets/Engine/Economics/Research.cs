using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Research : Unit
{
    public enum Type { None, Engine, Rockets, Modules, SpaceSuit, Equipment, Concept }
    public string Name;
    public List<Research> Dependances;
    public int[] TimeCost = { 100, 100, 100 };

    public Vector2 position;
    public Vector2 pivotStart;
    public Vector2 pivotEnd; 
    public List<Module> ModulesOpen;    
    public UIResearchButton researchButton;   
}
