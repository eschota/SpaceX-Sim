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
    }
    void Update()
    {
        localTimer += Time.deltaTime* Speed;
                    if (localTimer > 24) localTimer = 0;
        if (localTimer > DayStart && localTimer<DayEnd) foreach (var item in Lights) item.enabled = true;
        else foreach (var item in Lights) item.enabled = true;


        Sun.intensity = SunIntensity.Evaluate(localTimer / 24f);
        for (int i = 0; i < colors.Length; i++)
        {
            emissivMat[i].SetColor("_EmissionColor", colors[i] * EmissiveIntensity.Evaluate(localTimer));
        }
        

        
        Sun.transform.rotation =Quaternion.Euler(SunRotationX.Evaluate(localTimer / 24f)*360, SunRotationY.Evaluate(localTimer / 24f) * 360, SunRotationZ.Evaluate(localTimer / 24f) * 360);
        RenderSettings.ambientIntensity = AmbientLight.Evaluate(localTimer / 24f);
    }
}
