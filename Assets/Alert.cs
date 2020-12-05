using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Alert : MonoBehaviour
{
    [SerializeField]  CanvasGroup CG;
    private   string _alertMessage;
    public   string AlertMessage
    {
        get => _alertMessage;
        set
        {
            Time.timeScale = 0;
            _alertMessage = value;
            AlertMessageText.text = value;
            CG.alpha = 1;
            CG.interactable = true;
            CG.blocksRaycasts = true;
        }
    }
     [SerializeField] TMPro.TextMeshProUGUI AlertMessageText;

    public static Alert instance;
  
    private void Awake()
    {
        if (instance == null) instance = this; else DestroyImmediate(this);
        GetComponent<Button>().onClick.AddListener(OnClick);
       
        OnClick();
    }
    
void OnClick()
{
    Time.timeScale = 1;
        CG.alpha = 0;
        CG.interactable = false;
        CG.blocksRaycasts = false;
    }

     
}
