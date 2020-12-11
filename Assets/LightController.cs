using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    [SerializeField] List<Light> Lights;
    [SerializeField] float DayStart = 5.5f;
    [SerializeField] float DayEnd = 19.5f;

    float localTimer;
    private void Awake()
    {
        Time.timeScale = 1;
        if (TimeManager.Hours == null) localTimer = 5;
        else localTimer = TimeManager.Hours;
        localTimer += Time.deltaTime;
        
    }
    void Update()
    {
        localTimer += Time.deltaTime;
                    if (localTimer > 24) localTimer = 0;
        if (localTimer > DayStart && localTimer<DayEnd) foreach (var item in Lights) item.enabled = true;
        else foreach (var item in Lights) item.enabled = true;
            
    }
}
