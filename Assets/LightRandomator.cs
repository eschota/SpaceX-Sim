using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRandomator : MonoBehaviour
{
    [SerializeField] Vector2 MinMaxIntensity;
    [SerializeField] float Chastota=1;
    Light l;
    float target;
    float freq;
    void Start()
    {
        l = GetComponent<Light>();
        freq = Chastota;
    }

    // Update is called once per frame
    void Update()
    {
        freq -= Time.deltaTime;
        if (freq < 0)
        {
            freq = Chastota;
            target = Random.Range(MinMaxIntensity.x, MinMaxIntensity.y);
        }

        l.intensity = Mathf.Lerp(l.intensity, target, 10*Time.deltaTime);
    }
}
