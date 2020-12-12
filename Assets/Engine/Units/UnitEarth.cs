using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitEarth : Unit
{


    private void Update()
    {
        if (GameManager.CurrentState == GameManager.State.MenuStartGame)
            transform.Rotate(Vector3.back, Time.deltaTime);

        if (GameManager.CurrentState == GameManager.State.Play)
        {
            var hours = TimeManager.Hours;
            var angle = Mathf.Lerp(360, 0f, hours / 24f);

            transform.rotation = Quaternion.Euler(-90f, 180f, angle);
        }
    }
}
