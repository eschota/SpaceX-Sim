using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Module", menuName = "ScriptableObjects/Module", order = 1)]
public class ModuleSO : ScriptableObject
{
    public string Name;
    public Sprite icon;
    public float CostProduction = 1.1f;
    public int[] ProductionByClass = {100,100,100};
    public Module Prefab;
   
}
