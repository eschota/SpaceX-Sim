﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;

[ExecuteInEditMode]
public class UIResearchButton : MonoBehaviour, IDragHandler, IEndDragHandler
{

    public Research research;
    [SerializeField] private TMPro.TextMeshProUGUI ResearchName;
   
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
            if (ResearchName == null) ResearchName = GetComponentInChildren<TMPro.TextMeshProUGUI>();

            return ResearchName;
        }

    }

    Vector3 startPos;
    Vector3 currentPos;
   void Awake()
    {
        UIEditResearch.EventChangeResearch += OnChangeResearch;
        research = GetComponent<Research>();
        research.researchButton = this;
        research.researchButton.transform.position = new Vector3(200, 200);
                
    }
    void OnChangeResearch()
    {
        ResearchName.text = research.Name;

    }
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }
   
    // Update is called once per frame
    void Update()
    {

       
        
    }

    void OnClick()
    {
      if(research!=  ScenarioManager.instance.CurrentResearcLink.CurrentResearchSelected) ScenarioManager.instance.CurrentResearcLink.CurrentResearchSelected = research;
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.pointerCurrentRaycast.screenPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
         
    }
    void OnDestroy()
    {

    }
#if UNITY_EDITOR
    //[CustomEditor(typeof(UIResearchButton))]
    //class DecalMeshHelperEditor : Editor
    //{
    //    public override void OnInspectorGUI()
    //    {
    //        base.OnInspectorGUI();
    //        if (GUILayout.Button("Rebuild"))
    //            FindObjectOfType<UIResearchManager>().Rebuild();
    //        if (GUILayout.Button("RebuildLinks"))
    //            FindObjectOfType<UIResearchManager>().RebuildLinks();
    //    }
    //}
#endif
}
