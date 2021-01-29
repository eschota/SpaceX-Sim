using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalLightController : LightControllerAbstract
{
    [Header("Яркость локальных ИС")]
    [SerializeField] AnimationCurve AllLight;
    [Header("Локальные лайты")]
    [SerializeField] List<Light> Lights;
    [Header("Самосветящиеся материалы с Emissive")]
    [SerializeField] Material[] emissivMat;
    [Header("Их Цвета: количество должно соответствовать количеству материалов")]
    [SerializeField] Color[] emissivMatColors;
    [Header("Их яркость в течении дня ")]

    [SerializeField]
    AnimationCurve EmissiveIntensity;
    Color[] colors;
    private bool isLocalLightController = false;
    List<float> RandomTimersForLights = new List<float>();
    private void Awake()
    {
        IniTimer();
        CorrectLocalTime();
        CorrectPrefabsBuildingLocalTime();
        StartRandomizeLights();
    }

    protected new void Update()
    {
        base.Update();

        ProcessTime();
        ProcessLights();
        ProcessEmissive();
        ProcessReflectionProbes();
    }

    public void StartRandomizeLights()
    {
        if (emissivMat != null)
        {
            colors = new Color[emissivMat.Length];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = emissivMat[i].GetColor("_EmissionColor");
            }
        }
        if (Lights != null)
            for (int i = 0; i < Lights.Count; i++)
            {
                RandomTimersForLights.Add(Random.Range(-0.5f, 0.5f));
            }
    }

    void ProcessLights()
    {
        if (Lights != null)
            for (int i = 0; i < Lights.Count; i++)
            {
                float tempIntensity = AllLight.Evaluate((_localTimer + RandomTimersForLights[i]) / 24f);
                if (tempIntensity < 0.1f)
                    if (Lights[i] != null) Lights[i].enabled = true;
                    else
                    if (Lights[i] != null) Lights[i].enabled = false;
            }
    }
    void ProcessEmissive()
    {
        if (emissivMat != null)
            if (emissivMatColors != null)
                if (EmissiveIntensity != null)
                    for (int i = 0; i < emissivMatColors.Length; i++)
                    {
                        if (emissivMat.Length > i)
                            if (emissivMatColors[i] != null) emissivMat[i].SetColor("_EmissiveColor", emissivMatColors[i] * EmissiveIntensity.Evaluate(_localTimer / 24f));
                        // это для URP рендера 
                        //  if (emissivMatColors[i]!=null)   emissivMat[i].SetColor("_EmissionColor", emissivMatColors[i]* EmissiveIntensity.Evaluate(localTimer / 24f));
                    }
    }
    private void CorrectPrefabsBuildingLocalTime()
    {
        GlobalLightController global = FindObjectOfType<GlobalLightController>();
        if (global != null)
        {
            isLocalLightController = true;
            _localTimer = global.localTimer;
            if (global == this) isLocalLightController = false;
            else isLocalLightController = true;
        }
    }

    [ContextMenu("Добавить все лайты внутри этого префаба")]
    void AddInnerLights()
    {
        Lights.Clear();
        Lights.AddRange(GetComponentsInChildren<Light>());
    }
}
