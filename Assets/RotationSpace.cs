using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class RotationSpace : MonoBehaviour
{
    [SerializeField] Volume volume;
    public HDRISky Sky;
    [SerializeField] float speed = 1;
    void Start()
    {
        volume.profile.TryGet(out Sky);

    }

    // Update is called once per frame
    void Update()
    {
        Sky.rotation.value +=(float)Time.deltaTime*speed;
    }
}
