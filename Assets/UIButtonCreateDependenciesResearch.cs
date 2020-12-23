using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonCreateDependenciesResearch : MonoBehaviour
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
        foreach (var it in ScenarioManager.instance.Researches)
        {
            it.researchButton.clearDependence.gameObject.SetActive(false);
        }
            foreach (var item in FindObjectsOfType<UIButtonCreateDependenciesResearch>())
        {
            item.gameObject.SetActive(false);
        }
    }
    
}
