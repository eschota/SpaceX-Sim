using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class OCReflectionProbesManager : MonoBehaviour
{
    [SerializeField] Vector3 BBOXSizeMin = new Vector3(100, 100, 100);
    [SerializeField] Vector3 BBOXSizeMax = new Vector3(100, 100, 100);
    Vector3 size;
    Vector3 center;
    void Awake()
    {
        
    }
    
    [ContextMenu("Create Reflection Probes")]
    void CreateReflectionProbes()
    {
        List<Renderer> Mrs = new List<Renderer>();
        foreach (var item in FindObjectsOfType<OCFBX>())
        {
            Renderer r = item.GetComponent<Renderer>();
            if(r!=null)Mrs.Add(r);
        }

        BBOXSizeMin = new Vector3(0, 0, 0);
        BBOXSizeMax = new Vector3(0, 0, 0);

        foreach (var item in Mrs)
        { 
            if (BBOXSizeMin.x > item.bounds.min.x) BBOXSizeMin.x = item.bounds.min.x;
            if (BBOXSizeMin.y > item.bounds.min.y) BBOXSizeMin.y = item.bounds.min.y;
            if (BBOXSizeMin.z > item.bounds.min.z) BBOXSizeMin.z = item.bounds.min.z;
            if (BBOXSizeMax.x > item.bounds.max.x) BBOXSizeMax.x = item.bounds.max.x;
            if (BBOXSizeMax.y > item.bounds.max.y) BBOXSizeMax.y = item.bounds.max.y;
            if (BBOXSizeMax.z > item.bounds.max.z) BBOXSizeMax.z = item.bounds.max.z;
        }
        size = BBOXSizeMax - BBOXSizeMin;
        center =  size / 2;
    }
    void Update()
    {
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        Gizmos.DrawCube(transform.position, size);
    }
}
