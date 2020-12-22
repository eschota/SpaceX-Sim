using System.Collections;
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
        research.researchButton.transform.position = new Vector3(200, 200);
                
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
            modules[modules.Count - 1].imgSprite.sprite = research.ModulesOpen[i].IconSprite;
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
        foreach (var item in ScenarioManager.instance.Researches)
        {
            item.researchButton.RebuildLinks();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        research.position = Rect.position;
    }
    void OnDestroy()
    {

    }
    
    public void RebuildLinks()
    { 
         
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
        for (int i = 2; i < dis / 100 - 2; i++)
        {
            Arrows.Add(Instantiate(LinkPoint, transform));
            Arrows[Arrows.Count - 1].Rect.position = Vector2.Lerp(start, end, (float)i / (dis / 100));
        }
    }

}
