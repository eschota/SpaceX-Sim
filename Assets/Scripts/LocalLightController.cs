using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LocalLightController : LightControllerBase
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
    List<float> RandomTimersForLights = new List<float>();
    private void Awake()
    {
        IniTimer();
        CorrectPrefabsBuildingLocalTime();
        StartRandomizeLights();
    }

    protected new void Update()
    {
        base.Update();
#if UNITY_EDITOR
        if (!Application.isPlaying && Selection.activeGameObject == gameObject)// работа в эдиторе
        {
            StartRandomizeLights();
        }
#endif

        ProcessLights();
        ProcessEmissive();
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
        {
            RandomTimersForLights.Clear();
            for (int i = 0; i < Lights.Count; i++)
            {
                RandomTimersForLights.Add(Random.Range(-0.5f, 0.5f));
            }
        }
    }

    void ProcessLights()
    {
        if (Lights != null)
            for (int i = 0; i < Lights.Count; i++)
            {
                float tempIntensity = AllLight.Evaluate((_localTimer + RandomTimersForLights[i]) / 24f);
                bool isToTurnOn = tempIntensity >= 0.1f;
                if (Lights[i])
                {
                    Lights[i].intensity = tempIntensity;
                    if (Lights[i].enabled != isToTurnOn)
                    {
                        Lights[i].enabled = isToTurnOn;
                    }
                }
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
            _localTimer = global.localTimer;
        }
    }

    [ContextMenu("Добавить все лайты внутри этого префаба")]
    void AddInnerLights()
    {
        Lights.Clear();
        Lights.AddRange(GetComponentsInChildren<Light>());
    }
}
