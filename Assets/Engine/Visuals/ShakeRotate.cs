using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeRotate : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private float angle = 10f;

    void Update()
    {
        transform.Rotate(Vector3.up, Mathf.Sin(90 + Time.time * speed) * angle, Space.Self);
    }
}
