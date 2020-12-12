using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonUnit : MonoBehaviour
{
    [SerializeField] object UnitClass;
    public Unit unit;
    Button btn;
    [SerializeField] TMPro.TextMeshProUGUI number;
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        CameraManager.instance.TargetObject = unit.transform;
        FindObjectOfType<UIButtonUnitController>().ShowEnterButton(unit,btn.transform.position);
    }
    
   
}
