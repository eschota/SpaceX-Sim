using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateSolar : MonoBehaviour
{
    [SerializeField] private Vector3 axis = Vector3.up; // ��� ��������, ������� ����� ������� � ����������
    [SerializeField] private float speed = 10.0f; // �������� �������� �������

    void Update()
    {
        transform.Rotate(axis, speed * Time.deltaTime); // ������� ������ ������ ��������� ���
    }
}
