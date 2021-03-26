using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICircleProgress : MonoBehaviour
{
    public Transform Root;
    [SerializeField] RectTransform rect;
    [SerializeField] public Image Progress;
    [SerializeField] public TMPro.TextMeshProUGUI percentage;
    private void Awake()
    {
        Progress.fillAmount = 0;
        percentage.text = "0%";
    }
    private void Update()
    {
        
            rect.position = Camera.main.WorldToScreenPoint(Root.position)+Vector3.up*Screen.height*0.05f;
    }
}
