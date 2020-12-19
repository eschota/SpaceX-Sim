using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[ExecuteInEditMode]
public class UIResearchButton : MonoBehaviour
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
    void Start()
    {

    }
#if UNITY_EDITOR
    // Update is called once per frame
    void Update()
    {

        if (Selection.activeObject == gameObject)
        {

            research.position = new Vector2(_rect.position.x, _rect.position.y);
            FindObjectOfType<UIResearchManager>().RebuildLinks();
        }
        
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
