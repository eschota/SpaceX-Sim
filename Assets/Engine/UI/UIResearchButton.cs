using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;

[ExecuteInEditMode]
public class UIResearchButton : MonoBehaviour, IDragHandler, IEndDragHandler//, IPointerClickHandler
{

    public Research research;
    [SerializeField] private TMPro.TextMeshProUGUI ResearchName;
   
    [SerializeField] private RectTransform _rect;
    [SerializeField] public RectTransform pivotStart;
    [SerializeField] public RectTransform pivotEnd;
    [SerializeField] public TMPro.TextMeshProUGUI CostText;
    [SerializeField] public Image ProgressLight;
    [SerializeField] public Image ProgressMedium;
    [SerializeField] public Image ProgressHeavy;
    [SerializeField] public TMPro.TextMeshProUGUI LightMax;
    [SerializeField] public TMPro.TextMeshProUGUI MediumMax;
    [SerializeField] public TMPro.TextMeshProUGUI HeavyMax;
    [SerializeField] public UIModule ModulesIcons;
    [SerializeField] public Transform ModulesIconsRootTransform;
    [SerializeField] GameObject[] progressesGO;
    [SerializeField] Arrow LinkPoint;
    List<Arrow> Arrows  = new List<Arrow>();
    [SerializeField] public ButtonCreateDependencies createrDependence;
    [SerializeField] public  RectTransform clearDependence;
    public bool DependenceNow=false;
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
        
        research = GetComponent<Research>();
        research.researchButton = this;
        GetComponent<Button>().onClick.AddListener(OnClick);
        Refresh();
        //        research.researchButton.transform.position = new Vector3(200, 200);

    }
    void OnDestroy()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
    }

    List<UIModule> modules = new List<UIModule>();
   public void Refresh()
    {
        for (int i = 0; i < modules.Count; i++)
        {
            Destroy(modules[i].gameObject);
        }
        modules.Clear();
        for (int i = 0; i < research.ModulesOpen.Count; i++)
        {
            modules.Add(Instantiate(ModulesIcons, ModulesIconsRootTransform));
            modules[modules.Count - 1].imgSprite.sprite = research.ModulesOpen[i].moduleIcon;
        }
        
        

        ResearchName.text = research.Name;

        if (research.TimeCost[0] > 0)
        {
            progressesGO[0].SetActive(true);
            LightMax.text = research.TimeCompleted[0].ToString() + "/" + research.TimeCost[0].ToString();
            ProgressLight.fillAmount = research.TimeCompleted[0] / research.TimeCost[0];
        }
        else progressesGO[0].SetActive(false);
        if (research.TimeCost[1] > 0)
        {
            progressesGO[1].SetActive(true);
            MediumMax.text = research.TimeCompleted[1].ToString() + "/" + research.TimeCost[1].ToString();
            ProgressMedium.fillAmount = research.TimeCompleted[1] / research.TimeCost[1];
        }
        else progressesGO[1].SetActive(false);
        if (research.TimeCost[2] > 0)
        {
            progressesGO[2].SetActive(true);
            HeavyMax.text = research.TimeCompleted[2].ToString() + "/" + research.TimeCost[2].ToString();
            ProgressHeavy.fillAmount = research.TimeCompleted[2] / research.TimeCost[2];
        }
        else progressesGO[2].SetActive(false);

        RebuildLinks();
    }
    
   
    // Update is called once per frame
     
    List<Arrow> addArrows = new List<Arrow>();
    public void ShowArrowsWhenCreateDependance()
    {
        if (DependenceNow == false) return;
      
        for (int i = 0; i < addArrows.Count; i++)
        {
            DestroyImmediate(addArrows[i].gameObject);
        }
        addArrows.Clear();
        CreateLinkOnCreate( Input.mousePosition, Rect.position); 
    }
   
    public void ClearDependencies()
    {
        research.Dependances.Clear();
        RebuildLinks();
    }

    void OnClick()
    {
       if(research!= WindowEditResearch.instance.CurrentResearch) WindowEditResearch.instance.CurrentResearch = research;


     //Research res= ScenarioManager.instance.Researches.Find(X => X.researchButton.CreateDependence == true);
     //   if (res == null) return;
     //   if (research != res) res.Dependances.Add(research);
        
     //   res.researchButton.CreateDependence = false;
     //   CreateDependence = false;
     //   res.researchButton.RebuildLinks();
     //   RebuildLinks();
    }
    Vector2 delta= Vector2.zero;
    public void OnDrag(PointerEventData eventData)
    {   if(delta.sqrMagnitude<1)
        delta = eventData.pointerCurrentRaycast.screenPosition - new Vector2(Rect.position.x,Rect.position.y);
      //  Debug.Log(delta);
        Rect.position = -delta+ eventData.pointerCurrentRaycast.screenPosition;
        foreach (var item in ScenarioManager.instance.CurrentScenario.Researches)
        {
            item.researchButton.RebuildLinks();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
        research.position = Rect.position;
        delta = Vector2.zero;
        foreach (var item in ScenarioManager.instance.CurrentScenario.Researches)
        {
            item.researchButton.RebuildLinks();
        }
    }
   
    public List<RaycastResult> RaycastMouse()// здесь обрабатывается клик и ищется зависимость рисерча
    {

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            pointerId = -1,
        };

        pointerData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        if (DependenceNow)
        {
            DependenceNow = false;
            RebuildLinks();
            foreach (var item in results)
            {
                UIResearchButton tempButton = ScenarioManager.instance.buttons.Find(X => X.gameObject == item.gameObject);
                if (tempButton != null)
                {
                    if (tempButton != this)
                    {
                        DependenceNow = false;
                        if (!tempButton.research.Dependances.Contains(research))
                        if(!research.Dependances.Contains(tempButton.research))// исключаем круговую зависимость
                        {
                            tempButton.research.Dependances.Add(this.research);
                            RebuildLinks();
                            tempButton.RebuildLinks();

                        }

                    }
                }
            }
        }

     //   Debug.Log(results.Count);
        //if(results.Find(X=>X.GetType()==typeof(UIResearchButton)))

        return results;
    }
    void Update()
    {
        ShowArrowsWhenCreateDependance();
        if (Input.GetMouseButtonDown(0)) RaycastMouse();
    }
        public void RebuildLinks()
    {
        for (int i = 0; i < addArrows.Count; i++)
        {
            DestroyImmediate(addArrows[i].gameObject);
        }
        addArrows.Clear();
        if (research.Dependances.Count > 0) clearDependence.gameObject.SetActive(true); else clearDependence.gameObject.SetActive(false);
        createrDependence.gameObject.SetActive(true);
        for (int i = 0; i < Arrows.Count; i++)
        {
            DestroyImmediate(Arrows[i].gameObject);
        }
        Arrows.Clear();
        for (int i = 0; i < research.Dependances.Count; i++)
        {
           
                if (research.Dependances.Count > 0)
                {
                     
                        CreateLink(Rect.position, research.Dependances[i].researchButton.Rect.position);                    
                }
        }
    }
    void CreateLink(Vector2 start, Vector2 end)
    {
        float dis = Vector2.Distance(start, end);
        for (int i = 2; i < dis / 66 - 2; i++)
        {
            Arrows.Add(Instantiate(LinkPoint, transform));
            Arrows[Arrows.Count - 1].Rect.position = Vector2.Lerp(start, end, (float)i / (dis / 66));

            float targetRotation = Mathf.Atan((end.y - start.y) / (end.x - start.x));
            if(end.x - start.x >0)
            Arrows[Arrows.Count - 1].Rect.rotation = Quaternion.Euler(0, 0, 180 + targetRotation *Mathf.Rad2Deg);
            else
                Arrows[Arrows.Count - 1].Rect.rotation = Quaternion.Euler(0, 0, targetRotation * Mathf.Rad2Deg);

        }
    } 
    void CreateLinkOnCreate(Vector2 start, Vector2 end)
    {
        float dis = Vector2.Distance(start, end);
        for (int i = 2; i < dis / 66 ; i++)
        {
            addArrows.Add(Instantiate(LinkPoint, transform));
            addArrows[addArrows.Count - 1].Rect.position = Vector2.Lerp(start, end, (float)i / (dis / 66));

            float targetRotation = Mathf.Atan((end.y - start.y) / (end.x - start.x));
            if(end.x - start.x >0)
                addArrows[addArrows.Count - 1].Rect.rotation = Quaternion.Euler(0, 0, 180 + targetRotation *Mathf.Rad2Deg);
            else
                addArrows[addArrows.Count - 1].Rect.rotation = Quaternion.Euler(0, 0, targetRotation * Mathf.Rad2Deg);

        }
    }

}
