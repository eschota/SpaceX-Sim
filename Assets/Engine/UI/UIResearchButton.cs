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

    Vector3 startPos;
    Vector3 currentPos;
   
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }
#if UNITY_EDITOR
    // Update is called once per frame
    void Update()
    {

       
        
    }
    void OnClick()
    {
        ScenarioManager.instance.CurrentResearch.ResearchSelected = research;
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.pointerCurrentRaycast.screenPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    [CustomEditor(typeof(UIResearchButton))]
    class DecalMeshHelperEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Rebuild"))
                FindObjectOfType<UIResearchManager>().Rebuild();
            if (GUILayout.Button("RebuildLinks"))
                FindObjectOfType<UIResearchManager>().RebuildLinks();
        }
    }
#endif
}
