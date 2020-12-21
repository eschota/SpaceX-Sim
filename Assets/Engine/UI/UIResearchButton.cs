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
    [SerializeField] GameObject[] progressesGO;
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
        research.position = Rect.position;
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
