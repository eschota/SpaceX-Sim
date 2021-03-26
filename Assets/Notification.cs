using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Notification : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] public TMPro.TextMeshProUGUI Label;
    [SerializeField] public TMPro.TextMeshProUGUI Text;
    [SerializeField] Button btn;
    public Unit unit;
    
 
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            OnClick();
        //else if (eventData.button == PointerEventData.InputButton.Middle)
        //    Debug.Log("Middle click");
        else if (eventData.button == PointerEventData.InputButton.Right)
            OnRightClick();
    }    
    public void NotificationIni(Unit _unit)
    {
        unit = _unit;
        Label.text = "Construction Completed";
        Text.text = unit.name;
    }

    void OnClick()
    {
    Debug.Log("Открыть окно с построенным зданием"); 
            Destroy(gameObject,1);
    
    }void OnRightClick()
    {
    Debug.Log("Открыть окно с построенным зданием"); 
            Destroy(gameObject,0);
    
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
