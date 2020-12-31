using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameParametres", menuName = "ScriptableObjects/GameParametres", order = 1)]
public class GameParameters : ScriptableObject
{
    [SerializeField] public Vector3 CameraEarthstartPosition = Vector3.one;
    [SerializeField] public Vector3 CameraEarthBoundings =100* Vector3.one;
    [SerializeField] public Vector3 CameraEarthstartRotation = 100* Vector3.one;
    [SerializeField] public float CameraEarthSpeed = 25;
    
}
