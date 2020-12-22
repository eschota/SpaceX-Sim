using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonCreateDependenciesResearch : MonoBehaviour
{
    [SerializeField] Button btn;
    [SerializeField] public Research research;
    void Awake()
    {
        btn.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        if (Input.GetMouseButtonDown(1))
        {
            research.Dependances.Clear();            
        }
        else
        {

        }

    }
    void Update()
    {
        
    }
}
