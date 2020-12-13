using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Research", menuName = "ScriptableObjects/Research", order = 1)]
public class ResearchSO : ScriptableObject
{
    public enum Type { None, Engine, Rockets, Modules, SpaceSuit, Equipment, Concept}
    public string Name;
    public List< ResearchSO > Dependances;
    public int[] Cost = {100,100,100 };

    public Vector2 position;
    public Vector2 pivotStart;
    public Vector2 pivotEnd;
    public GameManager.level Level;
    public List<ModuleSO> ModulesOpen;
}
