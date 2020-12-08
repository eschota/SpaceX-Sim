using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonUnit : MonoBehaviour
{
    public enum UnitType { RocketLaunch, ResearchLab, ProductionFactory }
    [SerializeField] public UnitType unitType;
    [SerializeField] TMPro.TextMeshProUGUI number;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
