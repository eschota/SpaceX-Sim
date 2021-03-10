using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
    public Transform target;
    public int orbitSpeed;
    private void Update()
    
    {
        transform.RotateAround(target.transform.position, target.transform.up, orbitSpeed * Time.deltaTime);//ебучая хуета
    }
}
