using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointGroundZeroToShaderCoords : MonoBehaviour
{
    [SerializeField] Material mat; 
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float angle = Vector3.Angle(ray.direction, Vector3.down);

        float distToZeroPlane = ray.origin.y * Mathf.Tan(Mathf.Deg2Rad * angle);
        //Debug.Log("Angle = " + angle.ToString("00") + " Distance: " + distToZeroPlane.ToString("00000"));
        Vector3 planarDirection = new Vector3(ray.direction.x, 0, ray.direction.z);
        Vector3 planarOrigin = new Vector3(ray.origin.x, 0, ray.origin.z);
        Vector3 DragPos = planarOrigin + (planarDirection.normalized * distToZeroPlane);


        mat.SetVector("MouseGroundPoint", DragPos);
    }
}
