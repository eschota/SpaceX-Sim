using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthRotator : MonoBehaviour
{
    [SerializeField] float speed = 1;
    [SerializeField] Vector3 Axis = Vector3.back;
    // Update is called once per frame
    void Update()
    {
        if (GameManager.CurrentState == GameManager.State.MenuStartGame) transform.Rotate(Axis, Time.deltaTime*speed);

        if (GameManager.CurrentState == GameManager.State.Play) transform.Rotate(Axis, Time.deltaTime*speed);
    }
}
