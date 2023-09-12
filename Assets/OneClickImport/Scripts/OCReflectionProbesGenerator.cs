using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class OCReflectionProbesGenerator : MonoBehaviour
{
    [SerializeField] Vector3 BBOXSize = new Vector3(100, 100, 100);
    void Awake()
    {
        
    }

    void CalcBBOX()
    {
        Bounds b = new Bounds(Vector3.zero, Vector3.zero);
        foreach (Renderer r in FindObjectsOfType(typeof(Renderer)))
            {
         //  b=  b.Encapsulate(r.bounds);
        } 
    }
    void Update()
    {
        
    }
}
