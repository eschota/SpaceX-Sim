using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleCameraController : MonoBehaviour
{
    [SerializeField] Transform pivot;
    

    // Update is called once per frame
    void Update()
    {
        pivot.transform.Rotate(Vector3.right *5* Time.unscaledDeltaTime);
    }
}
