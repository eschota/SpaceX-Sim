using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonSelectModule : MonoBehaviour
{

    [SerializeField] public Image image;
    public Module module
    {
        get => _module;
        set
        {
            _module = value;
            image.sprite = value.Icon;
        }
    }
    private Module _module;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);

    }
    void OnClick()
    {
        WindowEditModule.instance.currentModule = module;
    }
}
