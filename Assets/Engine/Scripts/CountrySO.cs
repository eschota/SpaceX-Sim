using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CountrySO", menuName = "ScriptableObjects/CountrySO", order = 1)]
public class CountrySO : ScriptableObject
{
    public string Name;
    public int CostBuild = 300;
    public int CostLaunch = 50;
    public int CostPerMonth = 10;
   
}
