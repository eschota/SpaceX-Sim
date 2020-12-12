using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonUnit : MonoBehaviour
{
    [SerializeField] object UnitClass;
    public Unit unit;
    [SerializeField] TMPro.TextMeshProUGUI number;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        CameraManager.instance.TargetObject = unit.transform;
    }
    
   
}
