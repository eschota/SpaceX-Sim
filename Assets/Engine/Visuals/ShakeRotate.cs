using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeRotate : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private float angle = 10f;

    private float startRotation;
    private float currentRotation;

    private void Start()
    {
        startRotation = transform.localEulerAngles.y;
    }
    void Update()
    {
        currentRotation = angle / 2f * Mathf.Sin(Time.time * speed);
        Vector3 euler = transform.localEulerAngles;
        euler.y = startRotation + currentRotation;
        transform.localEulerAngles = euler;
    }
}
