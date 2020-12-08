using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICalendar : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI days;
    [SerializeField] TMPro.TextMeshProUGUI months;
    [SerializeField] TMPro.TextMeshProUGUI years;
    void Start()
    {
        TimeManager.EventChangeDay += OnChange;
        OnChange();
    }
    void OnChange()
    {
        days.text = TimeManager.Days.ToString();
        months.text = TimeManager.Months.ToString();
        years.text = TimeManager.Years.ToString();
    } 
}
