using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStation : Unit
{
    
    [SerializeField] float SpeedPerDay=1;
    [SerializeField] public Transform ObjectToRotate;
    private void Update()
    {
        

        if (GameManager.CurrentState == GameManager.State.Play)
        {
            var hours = TimeManager.Hours;
            var angle = Mathf.Lerp(360, 0f, hours / 24f);

            ObjectToRotate.localRotation = Quaternion.Euler(-90f, 180f, angle*SpeedPerDay);
        }
    }
}

