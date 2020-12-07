using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameParametres", menuName = "ScriptableObjects/GameParametres", order = 1)]
public class GameParameters : ScriptableObject
{
    public int StartBalance = 100;
    public Vector3 StartDate = new Vector3(1, 16, 1985);
    
}
