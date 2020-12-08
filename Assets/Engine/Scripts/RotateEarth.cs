using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEarth : MonoBehaviour
{
    [SerializeField] private GameObject Earth;
    [SerializeField] private float RotateSpeed=10;
    [SerializeField] private CanvasGroup canvasGroup;

    void Awake()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
    }
    void Update()
    {
        if (GameManager.CurrentState==GameManager.State.CreateLauchPlace)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                Earth.transform.Rotate(0, RotateSpeed * Time.deltaTime, 0);
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                Earth.transform.Rotate(0, -RotateSpeed * Time.deltaTime, 0);
            }
        }
    }
}
