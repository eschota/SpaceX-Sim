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
            SwitchLabButtonsOnResearchSelection(value);

            _CurrentResearchSelected = value;
            
        }
    }

    void SwitchLabButtonsOnResearchSelection(UIResearchButton lab)
    {
        foreach (var item in ButtonsResearchLabs) if (!lab.research.LabsResearchingNow.Contains(item.Lab)) item.ButtonAddThisLabToResearch.gameObject.SetActive(true);
            else item.ButtonAddThisLabToResearch.gameObject.SetActive(false);
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
                    if(item.ConstructionCompletedPercentage>=100)
                {
                    (item as BuildingResearchLab).ButtonLab.name = (item as BuildingResearchLab).ButtonLab.name + ButtonsResearchLabs.Count.ToString();
                    ButtonsResearchLabs.Add((item as BuildingResearchLab).ButtonLab);
                }
        }
    }

    public void AddLabToResearch(BuildingResearchLab lab)
    {
        if (!CurrentResearchSelected.research.LabsResearchingNow.Contains(lab))
        {
            CurrentResearchSelected.research.LabsResearchingNow.Add(lab);
            Debug.Log("Added Lab To Research: " + lab + " => " + CurrentResearchSelected);
            lab.ButtonLab.ButtonAddThisLabToResearch.gameObject.SetActive(false);
        }
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
        DrawLinks();
    }
    void DrawLinks()
    {
        for (int i = 0; i <Arrows.Count ; i++)
        {
            Destroy(Arrows[i].gameObject);
        }
        Arrows.Clear();
        foreach (var item in ScenarioManager.instance.Researches)
        {
            foreach (var buttons in item.LabsResearchingNow)
            {
                CreateLink( item.researchButton.transform.position, buttons.ButtonLab.transform.position);
            } 
        }
    }
    [SerializeField] public Arrow Arrow;
    List<Arrow> Arrows = new List<Arrow>();
    void CreateLink(Vector2 start, Vector2 end)
    {
        float dis = Vector2.Distance(start, end);
        for (int i = 2; i < dis / 30 - 2; i++)
        {
            Arrows.Add(Instantiate(Arrow, CameraControllerScenarioResearch.instance.CameraPivot));
            Arrows[Arrows.Count - 1].Rect.position = Vector2.Lerp(start, end, (float)i / (dis / 30));
            Arrows[Arrows.Count - 1].transform.SetAsFirstSibling();

            float targetRotation = Mathf.Atan((end.y - start.y) / (end.x - start.x));
            if (end.x - start.x > 0)
                Arrows[Arrows.Count - 1].Rect.rotation = Quaternion.Euler(0, 0, 180 + targetRotation * Mathf.Rad2Deg);
            else
                Arrows[Arrows.Count - 1].Rect.rotation = Quaternion.Euler(0, 0, targetRotation * Mathf.Rad2Deg);

        }
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
