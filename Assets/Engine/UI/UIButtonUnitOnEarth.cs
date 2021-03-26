using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonUnitOnEarth : MonoBehaviour
{
    [SerializeField] object UnitClass;
    public Unit unit;    
    public Button btn;
   public Button mainNutton;
    [SerializeField] TMPro.TextMeshProUGUI number; 
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick); 
    }

    void OnClick()
    {
        CameraControllerInSpace.instance.TargetObject = unit.transform;
    //    FindObjectOfType<UIButtonUnitController>().ShowEnterButton(unit,mainNutton.transform.position);
    }

    private void OnDestroy()
    { 
        btn.onClick.RemoveAllListeners();
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, Vector3.Distance(CameraControllerInSpace.instance.thisCamera.transform.position, unit.transform.position) / CameraControllerInSpace.instance.thisCamera.transform.position.magnitude);
        transform.position = CameraControllerInSpace.instance.thisCamera.WorldToScreenPoint(unit.transform.position);
    }
}
