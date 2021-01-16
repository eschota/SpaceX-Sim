using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[ExecuteInEditMode]
public class EarthCollider : MonoBehaviour
{ 
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red * 0.5f;
        Gizmos.DrawCube(transform.position, Vector3.one*100);
    }

    private void Update()
    {
        if (Selection.activeGameObject != this.gameObject) transform.position = new Vector3(100 * (Mathf.RoundToInt(transform.position.x / 100f)), transform.position.y, 100 * (Mathf.RoundToInt(transform.position.z / 100f)));
    }
}
