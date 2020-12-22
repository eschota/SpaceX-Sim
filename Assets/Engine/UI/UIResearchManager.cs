﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class UIResearchManager : MonoBehaviour
{
    
    [SerializeField] UIResearchButton button;
    [SerializeField] Arrow arrow;
    [SerializeField] Transform PivotButtons;
    [SerializeField] Transform PivotArrows;
    [SerializeField] List<UIResearchButton> buttons;
    [SerializeField] private RectTransform CameraPivot;
    [SerializeField] float DistanceArrows=100;
    
    List<Arrow> Arrows;
    private float zoom = 0;
    private Vector3 startPos, target;
    Vector3 maxpos;
    List<Research> Researches = new List<Research>();
    void Update()
    {
 
        MouseControl();
 
    }
    public static UIResearchManager instance;
    void Awake()
    {
        instance = this;
        maxpos = -Vector3.one * 10000;
        foreach (var item in FindObjectsOfType<UIResearchButton>())
        {
            if (item.Rect.position.x > maxpos.x) maxpos = new Vector3(item.Rect.position.x, maxpos.y, 0);
            if (item.Rect.position.y > maxpos.y) maxpos = new Vector3(maxpos.x, item.Rect.position.y, 0);
        }
    }
    void MouseControl()
    {
        if(GameManager.CurrentState!=GameManager.State.ResearchGlobal&& GameManager.CurrentState != GameManager.State.ScenarioEditor) return;
        if (Input.GetMouseButtonDown(1)|| Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            target = CameraPivot.position;
        }
        if (Input.GetMouseButton(1)|| Input.GetMouseButtonDown(0)) CameraPivot.position = target+(Input.mousePosition-startPos);
        if (Input.GetMouseButtonUp(1)) ;

       // CameraPivot.position = new Vector3(Mathf.Clamp(CameraPivot.position.x, 0, maxpos.x), Mathf.Clamp(CameraPivot.position.y, 0, maxpos.y), 0);
        zoom += 0.1f*Input.mouseScrollDelta.y;
        zoom= Mathf.Clamp(zoom, -0.5f, 0.5f);
        CameraPivot.localScale = Vector3.one *( 1 + zoom) ;
    }
   
 
}
