using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIResearchButton : MonoBehaviour
{
    [ExecuteInEditMode]
    public ResearchSO research;
    [SerializeField] private TMPro.TextMeshProUGUI _text;
    [SerializeField] private RectTransform _rect;
    [SerializeField] public RectTransform pivotStart;
    [SerializeField] public RectTransform pivotEnd;
    [SerializeField] public TMPro.TextMeshProUGUI CostText;
     public RectTransform Rect
    {
        get
        {
            if (_rect == null) _rect = GetComponent<RectTransform>();
            return _rect;
        }
    }
     public TMPro.TextMeshProUGUI text
    {
        get
        {
            if (_text == null) _text = GetComponentInChildren<TMPro.TextMeshProUGUI>();

            return _text;
        }
         
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Application.isEditor) return;
    }
}
