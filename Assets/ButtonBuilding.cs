using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBuilding : MonoBehaviour
{
    [SerializeField] public Image icon;
    [SerializeField] public TMPro.TextMeshProUGUI Name;
    [SerializeField] Button btn;
    public Unit building;

    private void Awake()
    {
        btn.onClick.AddListener(OnClick);
    }

    void OnClick()
    {

    }


    private void OnDestroy()
    {
        btn.onClick.RemoveAllListeners();
    }
    
}
