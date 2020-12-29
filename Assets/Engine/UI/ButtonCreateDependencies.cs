using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCreateDependencies : MonoBehaviour
{
    [SerializeField] Button btn;
    [SerializeField] public UIResearchButton UIresearch;
    void Awake()
    {
        btn.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        UIresearch.DependenceNow = true;
        foreach (var it in ScenarioManager.instance.CurrentScenario.Researches)
        {
            it.researchButton.clearDependence.gameObject.SetActive(false);
        }
            foreach (var item in FindObjectsOfType<ButtonCreateDependencies>())
        {
            item.gameObject.SetActive(false);
        }
    }
    
}
