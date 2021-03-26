using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalLightController : LightControllerBase
{
    [Header("ШИ С ДЭ САН")]
    [SerializeField]
    private Light sun;
    [Header("Яркость солнца в течении дня")]
    [SerializeField]
    private AnimationCurve SunIntensity;
    [Header("Ротейшен солнца в течении дня, значение домножается на 360")]

    [SerializeField]
    private AnimationCurve SunRotationX;
    [SerializeField]
    private AnimationCurve SunRotationY;
    [SerializeField]
    private AnimationCurve SunRotationZ;

    [Header("Яркость Эмбиент подсветки")]
    [SerializeField]
    private AnimationCurve Ambients;

    private void Awake()
    {
        IniTimer();
    }

    protected new void Update()
    {
        base.Update();

        ProcessSun();
        ProcessAmbientAndReflectionAmbient();
    }
    public void ProcessSun()
    {
        if (sun != null)
        {
            sun.intensity = SunIntensity.Evaluate(_localTimer / 24f);
            sun.transform.rotation = Quaternion.Euler(SunRotationX.Evaluate(_localTimer / 24f) * 360, SunRotationY.Evaluate(_localTimer / 24f) * 360, SunRotationZ.Evaluate(_localTimer / 24f) * 360);
        }
    }

    void ProcessAmbientAndReflectionAmbient()
    {
        if (Ambients != null)
            RenderSettings.ambientIntensity = Ambients.Evaluate(_localTimer / 24f);
        if (Reflections != null)
            RenderSettings.reflectionIntensity = Reflections.Evaluate(_localTimer / 24f);
    }
}
