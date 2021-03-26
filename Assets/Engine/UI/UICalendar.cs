using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICalendar : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI hours;
    [SerializeField] TMPro.TextMeshProUGUI days;
    [SerializeField] TMPro.TextMeshProUGUI months;
    [SerializeField] TMPro.TextMeshProUGUI years;
    void Start()
    {
        TimeManager.EventChangeDay += OnChange;
        OnChange();
    }
    private void Update()
    {
        hours.text = Mathf.RoundToInt(TimeManager.Hours).ToString();
    }
    void OnChange()
    {
        days.text = TimeManager.Days.ToString();
        
        months.text = TimeManager.Months.ToString();
        years.text = TimeManager.Years.ToString();
    } 
}
