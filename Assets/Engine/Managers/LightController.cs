using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class LightController : MonoBehaviour
{
    [Range(0, 24)]
    [SerializeField] float localTimer;
    [SerializeField] Light Sun;
    [SerializeField] AnimationCurve SunIntensity;
    [SerializeField] AnimationCurve SunRotationX;
    [SerializeField] AnimationCurve SunRotationY;
    [SerializeField] AnimationCurve SunRotationZ;
    
    [SerializeField] AnimationCurve AllLight;
    [SerializeField] AnimationCurve Reflections;
    [SerializeField] AnimationCurve Ambients;
    [SerializeField] float Speed=1;
    [SerializeField] List<Light> Lights;
 
    [SerializeField] Material[] emissivMat;
    [SerializeField] AnimationCurve EmissiveIntensity;

    [SerializeField] List<ReflectionProbe> ReflectionProbes;
    Color[] colors;
    
    List<float> RandomTimersForLights = new List<float>();
    
    private void Awake()
    {
        Time.timeScale = 1;
        if (TimeManager.Hours == null)
            localTimer = 5;
        else
            localTimer = TimeManager.Hours;
        CorrectLocalTime();
        
        colors = new Color[emissivMat.Length];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = emissivMat[i].GetColor("_EmissionColor");
        }
        for (int i = 0; i < Lights.Count; i++)
        {
            RandomTimersForLights.Add(Random.Range(-0.5f,0.5f));
        }
    }
    
    void Update()
    {
        if (Application.isPlaying)
            localTimer += Time.deltaTime * Speed * TimeManager.TimeScale; 
        if (localTimer > 24)
            localTimer = 0;
        for (int i = 0; i < Lights.Count; i++)             
        {
            float tempIntensity = AllLight.Evaluate((localTimer + RandomTimersForLights[i])/24f);
            if (tempIntensity < 0.1f)
                Lights[i].enabled = true;
            else
                Lights[i].enabled = false;
        } 
        


        Sun.intensity = SunIntensity.Evaluate(localTimer / 24f);
        for (int i = 0; i < colors.Length; i++)
        {
            emissivMat[i].SetColor("_EmissionColor", colors[i] * EmissiveIntensity.Evaluate(localTimer/24f));
        }
        

        
        Sun.transform.rotation =Quaternion.Euler(SunRotationX.Evaluate(localTimer / 24f)*360 , SunRotationY.Evaluate(localTimer / 24f) * 360, SunRotationZ.Evaluate(localTimer / 24f) * 360);
        RenderSettings.ambientIntensity = Ambients.Evaluate(localTimer / 24f);
        RenderSettings.reflectionIntensity = Reflections.Evaluate(localTimer / 24f);
        foreach (var item in ReflectionProbes) item.intensity= Reflections.Evaluate(localTimer / 24f);

    }

    private void CorrectLocalTime()
    {
        var localHoursOffset = TimeManager.LocalHoursOffset;
        localTimer += localHoursOffset;

        if (localTimer > 24f)
            localTimer -= 24f;
        else if (localTimer < 0f)
            localTimer = 24f - localTimer;
    }
}
