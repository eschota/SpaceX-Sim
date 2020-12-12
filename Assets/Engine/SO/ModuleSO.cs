using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Module", menuName = "ScriptableObjects/Module", order = 1)]
public class ModuleSO : ScriptableObject
{
    public string Name;
    public Sprite icon;
    public int CostProduction = 100;
    public int[] Production = {100,100,100};

   
}
