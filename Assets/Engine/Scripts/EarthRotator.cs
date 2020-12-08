using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthRotator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.CurrentState == GameManager.State.MenuStartGame) transform.Rotate(Vector3.back, Time.deltaTime);

        if (GameManager.CurrentState == GameManager.State.Play) transform.Rotate(Vector3.back, Time.deltaTime);
    }
}
