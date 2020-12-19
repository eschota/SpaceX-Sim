using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof (Button))]
public class UIButtonScenario : MonoBehaviour
{
    [SerializeField] ScenarioManager.State thisState;
    [SerializeField][HideInInspector] Button but;

    private void Reset()
    {
        but = GetComponent<Button>();
    }
    void Awake()
    {if(but==null) but = GetComponent<Button>();
        but.onClick.AddListener( OnClick);
    }
    void OnClick()
    {
        ScenarioManager.CurrentState = thisState;
    }
}
