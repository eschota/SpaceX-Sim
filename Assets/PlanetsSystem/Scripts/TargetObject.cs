using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObject : MonoBehaviour
{
    [SerializeField] public Collider col;
    [SerializeField] public float zoomInSDifferencial = 0;
    void Awake()
    {
        col = GetComponent<Collider>();
        col.enabled = true;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
