using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEarth : MonoBehaviour
{
    [SerializeField] private GameObject Earth;
    [SerializeField] private float RotateSpeed=10;
    

    void Awake()
    {
       
    }
    void Update()
    {
        if (GameManager.CurrentState == GameManager.State.CreateLauchPlace ||
            GameManager.CurrentState == GameManager.State.CreateProductionFactory ||
            GameManager.CurrentState == GameManager.State.CreateResearchLab)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                Earth.transform.Rotate(Vector3.back, RotateSpeed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                Earth.transform.Rotate(Vector3.forward, RotateSpeed * Time.deltaTime);

            }
        }
    }
}
