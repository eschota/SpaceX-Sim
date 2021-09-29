using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketManager : MonoBehaviour
{

    [SerializeField] Slider slider;
    [SerializeField] TMPro.TextMeshProUGUI CountOfMans;


    
    private void Start()
    {
        slider.onValueChanged.AddListener(SetMans);
    }
    public void SetMans(float slider)
    {
        CountOfMans.text = ((int) Mathf.Lerp(5,30, slider )).ToString("0");
    }
}
