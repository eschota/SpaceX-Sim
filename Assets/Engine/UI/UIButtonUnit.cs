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
        UIButtonUnitController.instance.buttons.Add(this);
    }

    void OnClick()
    {
        CameraManager.instance.TargetObject = unit.transform;
        FindObjectOfType<UIButtonUnitController>().ShowEnterButton(unit,btn.transform.position);
    }

    private void OnDestroy()
    {
        UIButtonUnitController.instance.buttons.Remove(this);
        btn.onClick.RemoveAllListeners();
    }
}
