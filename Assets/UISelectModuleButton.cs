using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelectModuleButton : MonoBehaviour
{
    [SerializeField] public Image icon;
    [SerializeField] public TMPro.TextMeshProUGUI ModuleName;
    [SerializeField] public Module thisModule;

    Button but;
    void Start()
    {
        (but=GetComponent<Button>()).onClick.AddListener(OnClick);   
    }

    void OnClick()
    {
        UISelectModule.instance.CurrentSelectedModule = thisModule;
    }
    private void OnDestroy()
    {
        but.onClick.RemoveAllListeners();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
