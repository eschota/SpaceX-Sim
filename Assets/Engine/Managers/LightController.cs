using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[ExecuteInEditMode]
public class LightController : MonoBehaviour
{
    [Range(0, 24)]
    [SerializeField] float localTimer;
    [Header ("ШИ С ДЭ САН")]
    [SerializeField] Light Sun;
    [Header("Яркость солнца в течении дня")]
    [SerializeField] AnimationCurve SunIntensity;
    [Header("Ротейшен солнца в течении дня, значение домножается на 360")]

    [SerializeField] AnimationCurve SunRotationX;
    [SerializeField] AnimationCurve SunRotationY;
    [SerializeField] AnimationCurve SunRotationZ;
    [Header ("Яркость локальных ИС")]
    [SerializeField] AnimationCurve AllLight;
    [Header("Яркость Эмбиент Отражений, Рефлекшн пробы" )]
    [SerializeField] AnimationCurve Reflections;
    [Header("Яркость Эмбиент подсветки")]

    [SerializeField] AnimationCurve Ambients;
    [Header("Скорость времени")]

    [SerializeField] float Speed=1;
    [Header("Локальные лайты")]
    [SerializeField] List<Light> Lights;
    [Header("Самосветящиеся материалы с Emissive")]
    [SerializeField] Material[] emissivMat;
    [Header("Их Цвета: количество должно соответствовать количеству материалов")]
    [SerializeField] Color[] emissivMatColors;
    [Header("Их яркость в течении дня ")]

    [SerializeField] AnimationCurve EmissiveIntensity;
    [Header("Рефлекшен пробы")]
    [SerializeField] List<ReflectionProbe> ReflectionProbes;
    Color[] colors;
    private bool isLocalLightController=false;
    List<float> RandomTimersForLights = new List<float>();
    
    private void Awake()
    {
        IniTimer();
        CorrectLocalTime();
        CorrectPrefabsBuildingLocalTime();
        StartRandomizeLights(); 
    }
    void Update()
    {
        if (isLocalLightController)// если это не глобальный контроллер света
        {
            ProcessTime();
            ProcessLights();
            ProcessEmissive();
            ProcessReflectionProbes();
        }
        else
        {
            ProcessSun();
            ProcessAmbientAndReflectionAmbient();
            ProcessTime();
            ProcessLights();
            ProcessEmissive();
            ProcessReflectionProbes();
        }
#if UNITY_EDITOR
        
        if (!Application.isPlaying&& Selection.activeGameObject==gameObject)// работа в эдиторе
        {
            List < LightController > LC = new List<LightController>(); LC.AddRange(FindObjectsOfType<LightController>());
            foreach (var item in LC) item.localTimer = localTimer;
            LC.Find(X => X.Sun != null)?.ProcessSun();
            ProcessLights();
            ProcessEmissive();
            ProcessReflectionProbes();
            ProcessAmbientAndReflectionAmbient();
        }
#endif
    }
    private void IniTimer()
    {
         if (!Application.isPlaying) localTimer = 12;

                if (Application.isPlaying) if (FindObjectOfType<CameraControllerOnEarth>() == null) gameObject.AddComponent<CameraControllerOnEarth>();
        Time.timeScale = 1;
        if (TimeManager.Hours <= 0.1f)
            localTimer = 12;
        else
            localTimer = TimeManager.Hours;
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
                float tempIntensity = AllLight.Evaluate((localTimer + RandomTimersForLights[i]) / 24f);
                if (tempIntensity < 0.1f)
                    Lights[i].enabled = true;
                else
                    Lights[i].enabled = false;
            }
    }
    void ProcessEmissive()
    {
        if (emissivMat != null)
            if (emissivMatColors != null)
                if(EmissiveIntensity!=null)
                for (int i = 0; i < emissivMatColors.Length; i++)
                {
                        if(emissivMat.Length>i)
                     if (emissivMatColors[i] != null) emissivMat[i].SetColor("_EmissiveColor", emissivMatColors[i] * EmissiveIntensity.Evaluate(localTimer / 24f));
                     // это для URP рендера 
                  //  if (emissivMatColors[i]!=null)   emissivMat[i].SetColor("_EmissionColor", emissivMatColors[i]* EmissiveIntensity.Evaluate(localTimer / 24f));
                }
    }
    void ProcessSun()
    {
        if (Sun != null)
        {
            Sun.intensity = SunIntensity.Evaluate(localTimer / 24f);
            Sun.transform.rotation = Quaternion.Euler(SunRotationX.Evaluate(localTimer / 24f) * 360, SunRotationY.Evaluate(localTimer / 24f) * 360, SunRotationZ.Evaluate(localTimer / 24f) * 360);
        }
    }

    void ProcessAmbientAndReflectionAmbient()
    {
        if (Ambients != null)
            RenderSettings.ambientIntensity = Ambients.Evaluate(localTimer / 24f);
        if (Reflections != null)
            RenderSettings.reflectionIntensity = Reflections.Evaluate(localTimer / 24f);
    }
    void ProcessReflectionProbes()
    {
        if (Reflections != null)
        {
           
            foreach (var item in ReflectionProbes) if (item != null) item.intensity = Reflections.Evaluate(localTimer / 24f);
        }
    }
   
    void ProcessTime()
    {
        if (Application.isPlaying)
            localTimer += Time.deltaTime * Speed * TimeManager.TimeScale;
        if (localTimer > 24f)
            localTimer = 0;
    }
    private void CorrectPrefabsBuildingLocalTime()
    {
        List <LightController> LC = new List<LightController>();
        LC.AddRange(FindObjectsOfType<LightController>()) ;
        LightController global = LC.Find(X => X.Sun != null);
        if (global != null)
        {
            isLocalLightController = true;
            localTimer = global.localTimer;
            if (global == this) isLocalLightController = false;
            else isLocalLightController = true;
        }
        else
        {
            isLocalLightController = false;
        }

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

    [ContextMenu ("Добавить все лайты внутри этого префаба")]
    void AddInnerLights()
    {
        Lights.Clear();
        Lights.AddRange(GetComponentsInChildren<Light>());
    }
}
