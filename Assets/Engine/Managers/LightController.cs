using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    [SerializeField] Light Sun;
    [SerializeField] AnimationCurve SunIntensity;
    [SerializeField] AnimationCurve SunRotationX;
    [SerializeField] AnimationCurve SunRotationY;
    [SerializeField] AnimationCurve SunRotationZ;
    [SerializeField] AnimationCurve AmbientLight;
    [SerializeField] float Speed=1;
    [SerializeField] List<Light> Lights;
    [SerializeField] float DayStart = 5.5f;
    [SerializeField] float DayEnd = 19.5f;
    [SerializeField] Material[] emissivMat;
    [SerializeField] AnimationCurve EmissiveIntensity;
    Color[] colors;
    float localTimer;

    
    List<float> RandomTimersForLights = new List<float>();
    private void Awake()
    {
        Time.timeScale = 1;
        if (TimeManager.Hours == null) localTimer = 5;
        else localTimer = TimeManager.Hours;
        localTimer += Time.deltaTime;
        colors = new Color[emissivMat.Length];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = emissivMat[i].GetColor("_EmissionColor");
        }
        for (int i = 0; i < Lights.Count; i++)
        {
            RandomTimersForLights.Add(Random.Range(0f, 1f));
        }
    }
    void Update()
    {
        localTimer += Time.deltaTime* Speed;
                    if (localTimer > 24) localTimer = 0;
        for (int i = 0; i < Lights.Count; i++)

            if (localTimer + RandomTimersForLights[i] > DayStart && localTimer + RandomTimersForLights[i] < DayEnd) 
            {
                Lights[i].enabled = true;
            } 
        else Lights[i].enabled = false;


        Sun.intensity = SunIntensity.Evaluate(localTimer / 24f);
        for (int i = 0; i < colors.Length; i++)
        {
            emissivMat[i].SetColor("_EmissionColor", colors[i] * EmissiveIntensity.Evaluate(localTimer/24f));
        }
        

        
        Sun.transform.rotation =Quaternion.Euler(SunRotationX.Evaluate(localTimer / 24f)*360, SunRotationY.Evaluate(localTimer / 24f) * 360, SunRotationZ.Evaluate(localTimer / 24f) * 360);
        RenderSettings.ambientIntensity = AmbientLight.Evaluate(localTimer / 24f);
    }
}
