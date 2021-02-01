using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;
public class UIResearchManager : MonoBehaviour
{
    public static UIResearchManager instance;
    [SerializeField] public Transform Grid;

    public List<UiLabButton> ButtonsResearchLabs = new List<UiLabButton>();
    void Start()
    {
        instance = this;
        GameManager.EventChangeState += OnChangeState;
    }
   private UIResearchButton _CurrentResearchSelected;
   public UIResearchButton CurrentResearchSelected
    {
        get => _CurrentResearchSelected;
        set
        {
            if (value == null)
            {
                _CurrentResearchSelected = null;
                foreach (var item in ButtonsResearchLabs) item.ButtonAddThisLabToResearch. gameObject.SetActive(false);
                return;
            }
            foreach (var item in ButtonsResearchLabs) item.ButtonAddThisLabToResearch.gameObject.SetActive(true);
            
            _CurrentResearchSelected = value;
            
        }
    }

    void OnChangeState ()
    {
        if (GameManager.CurrentState != GameManager.State.ResearchGlobal) return;
        ClearResearchLabsButtons();
        AddResearchLabButtons();
        CurrentResearchSelected = null;
    }

    private void AddResearchLabButtons()
    {
        foreach (var item in GameManager.Buildings)
        {
            if (!item.isResearch)
                if (item.GetType() == typeof(BuildingResearchLab))
                {
                    (item as BuildingResearchLab).ButtonLab.name = (item as BuildingResearchLab).ButtonLab.name + ButtonsResearchLabs.Count.ToString();
                    ButtonsResearchLabs.Add((item as BuildingResearchLab).ButtonLab);
                }
        }
    }

    public void AddLabToResearch(BuildingResearchLab lab)
    {
        CurrentResearchSelected.research.LabsResearchingNow.Add(lab);
        lab.ButtonLab.ButtonAddThisLabToResearch.gameObject.SetActive(false);
    }
    private void ClearResearchLabsButtons()
    {
        if (GameManager.CurrentState != GameManager.State.ResearchGlobal) return;
        if (ButtonsResearchLabs.Count>0) for (int i = 0; i < ButtonsResearchLabs.Count; i++)
            {
            if(ButtonsResearchLabs[i]!=null)   Destroy(ButtonsResearchLabs[i].gameObject);
            }        
        ButtonsResearchLabs.Clear();

    }
    private void OnDestroy()
    {
        GameManager.EventChangeState -= OnChangeState;

    }

    private void Update()
    {
        if (GameManager.CurrentState != GameManager.State.ResearchGlobal) return;
        if (Input.GetMouseButtonDown(0)) SelectResearch();
        if (Input.GetMouseButtonDown(1)) CurrentResearchSelected = null;// deselect research

    }

    void SelectResearch()
    {


        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            pointerId = -1,
        };

        pointerData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);


        foreach (var item in results)
        {
            UIResearchButton tempButton = ScenarioManager.instance.buttons.Find(X => X.gameObject == item.gameObject);
            if (tempButton != null)
            {
                CurrentResearchSelected = tempButton;
                Debug.Log("Выбрано новое исследование: " + CurrentResearchSelected.name);
                //if (tempButton != this)
                //{

                //    if (!tempButton.research.Dependances.Contains(research))
                //        if (!research.Dependances.Contains(tempButton.research))// исключаем круговую зависимость
                //        {
                //            tempButton.research.Dependances.Add(this.research);
                //            RebuildLinks();
                //            tempButton.RebuildLinks();

                //        }

                //}
            }
        }
    }
            
}
