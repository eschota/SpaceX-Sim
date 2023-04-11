using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateSolar : MonoBehaviour
{
    [SerializeField] private Vector3 axis = Vector3.up; // ось вращения, которую можно выбрать в инспекторе
    [SerializeField] private float speed = 10.0f; // скорость вращения объекта

    void Update()
    {
        transform.Rotate(axis, speed * Time.deltaTime); // вращаем объект вокруг выбранной оси
    }
}
