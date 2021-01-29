using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public abstract class LightControllerAbstract : MonoBehaviour
{
    [Range(0, 24)]
    [SerializeField] 
    protected float _localTimer;
    [Header("Скорость времени")]
    [SerializeField] 
    protected float Speed = 1;

    [Header("Яркость Эмбиент Отражений, Рефлекшн пробы")]
    [SerializeField]
    protected AnimationCurve Reflections;
    [Header("Рефлекшен пробы")]
    [SerializeField]
    protected List<ReflectionProbe> ReflectionProbes;

    public float localTimer => _localTimer;

    protected void Update()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying && Selection.activeGameObject == gameObject)// работа в эдиторе
        {
            List<LightControllerAbstract> LC = new List<LightControllerAbstract>(); LC.AddRange(FindObjectsOfType<LightControllerAbstract>());
            foreach (var item in LC) item._localTimer = _localTimer;
            (LC.Find(X => X is GlobalLightController) as GlobalLightController)?.ProcessSun();
        }
#endif
    }
    protected void IniTimer()
    {
        if (!Application.isPlaying) _localTimer = 12;
        if (TimeManager.Hours <= 0.1f)
            _localTimer = 12;
        else
            _localTimer = TimeManager.Hours;
    }
    protected void CorrectLocalTime()
    {
        var localHoursOffset = TimeManager.LocalHoursOffset;
        _localTimer += localHoursOffset;

        if (_localTimer > 24f)
            _localTimer -= 24f;
        else if (_localTimer < 0f)
            _localTimer = 24f - _localTimer;
    }
    protected void ProcessReflectionProbes()
    {
        if (Reflections != null)
        {
            foreach (var item in ReflectionProbes) if (item != null) item.intensity = Reflections.Evaluate(_localTimer / 24f);
        }
    }
    protected void ProcessTime()
    {
        if (Application.isPlaying)
            _localTimer += Time.deltaTime * Speed * TimeManager.TimeScale;
        if (_localTimer > 24f)
            _localTimer = 0;
    }
}
