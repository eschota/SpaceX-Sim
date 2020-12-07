using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameParametresLaunchPlace", menuName = "ScriptableObjects/GameParametresLaunchPlace", order = 1)]
public class GameParametersLaunchPlace : ScriptableObject
{
    public string Name;
    public int CostBuild = 300;
    public int CostLaunch = 50;
    public int CostPerMonth = 10;
    public Color ColorPlace;
   
}
