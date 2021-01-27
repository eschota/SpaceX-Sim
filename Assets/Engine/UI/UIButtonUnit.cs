using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonUnit : MonoBehaviour
{
    [SerializeField] object UnitClass;
    public Unit unit;    
   public Button btn;
    [SerializeField] public TMPro.TextMeshProUGUI number; 
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
        UIButtonUnitController.instance.buttons.Add(this);
    }

    void OnClick()
    {
        CameraControllerInSpace.instance.TargetObject = unit.transform;
        FindObjectOfType<UIButtonUnitController>().ShowEnterButton(unit,btn.transform.position);
    }

    private void OnDestroy()
    {
        UIButtonUnitController.instance.buttons.Remove(this);
        btn.onClick.RemoveAllListeners();
    }
}
