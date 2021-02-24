using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBUttonSelectModuleForBuild : MonoBehaviour
{
     public Module unit;
    [SerializeField] Image Icon;
    [SerializeField] TMPro.TextMeshProUGUI NameText;
    [SerializeField] Button btn;


    private void Awake()
    {
        btn.onClick.AddListener(OnClick);
    }
    public void Ini(Module unit)
    {
        NameText.text = unit.Name;
        Icon.sprite = unit.Icon;
    }
    void OnClick()
    {
        WindowSelectModule.instance.CurrentSelectedModule = unit;
    }
    private void OnDestroy()
    {
        btn.onClick.RemoveListener(OnClick);
    }

}
