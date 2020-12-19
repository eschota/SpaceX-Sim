using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[RequireComponent (typeof(CanvasGroup))]
public class UIStateScenario : MonoBehaviour
{
    private void Reset()
    {
        CG = GetComponent<CanvasGroup>();
    }
    private void Awake()
    {
        ScenarioManager.EventChangeState += OnChange; 
        if (thisState  == null) Debug.LogError("Ни одного стейта не выбрано у ГУЙ объекта : " + name);
    }

    void Start()
    {
        OnChange();
    }
    void Update()
    {
      
    }
     [SerializeField] public CanvasGroup CG;
    [SerializeField] List <ScenarioManager.State> thisState;
    

    void OnChange()
    {
        if (thisState.Count < 1) return;
        if (thisState.Exists(X=>X==ScenarioManager.CurrentState))  Show();
        else Hide();
    }  
    void Show()
    {
        CG.alpha = 1;
        CG.interactable = true;
        CG.blocksRaycasts = true;
    }
    void Hide()
    {
        
        CG.alpha = 0;

        CG.interactable = false;
        CG.blocksRaycasts = false;
    }

   
    private void OnDestroy()
    {
        ScenarioManager.EventChangeState -= OnChange;
      
    }


   
}
#if UNITY_EDITOR

[CustomEditor(typeof(UIStateScenario))]
class UIStateButtonScenario : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("IsolateThisUI"))
        {
            List<UIStateScenario> statesParent = new List<UIStateScenario>();
            statesParent.AddRange(Selection.activeGameObject.GetComponentsInParent<UIStateScenario>());
            foreach (var item in statesParent[0].GetComponentsInChildren<UIStateScenario>())
            {
                
                if (item.gameObject == Selection.activeGameObject)
                    item.CG.alpha = 1;
                else
                    item.CG.alpha = 0;
                if (statesParent.Exists(X => X == item)) item.CG.alpha = 1;
            }
            foreach (var item in Selection.activeGameObject.GetComponentsInParent<UIStateScenario>())
            {
                item.CG.alpha = 1;
            }
        }

    }
}
#endif
