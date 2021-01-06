﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStation : Unit
{
    [Header ("Вращается только по оси Z")]
    [SerializeField] float SpeedPerDay=1;
    [SerializeField] public Transform ObjectToRotate;
    public override void Update()
    {
        

        if (GameManager.CurrentState == GameManager.State.PlaySpace)
        {
            var hours = TimeManager.Hours;
            var angle = Mathf.Lerp(360, 0f, hours / 24f);

            ObjectToRotate.localRotation = Quaternion.Euler(ObjectToRotate.localRotation.eulerAngles.x, ObjectToRotate.localRotation.eulerAngles.y, angle*SpeedPerDay);
        }
    }
}

