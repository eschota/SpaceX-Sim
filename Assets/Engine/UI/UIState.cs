using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

 
[RequireComponent (typeof(CanvasGroup))]
public class UIState : MonoBehaviour
{
    private void Reset()
    {
        CG = GetComponent<CanvasGroup>();
    }
    private void Awake()
    {
        GameManager.EventChangeState += OnChange; 
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
    [SerializeField] List <GameManager.State> thisState;
    

    void OnChange()
    {
        if (thisState.Count < 1) return;
        if (thisState.Exists(X=>X==GameManager.CurrentState))  Show();
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
        GameManager.EventChangeState -= OnChange;
      
    }


   
}
//#if UNITY_EDITOR

//[CustomEditor(typeof(UIState))]
//class UIStateButton : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();
//        if (GUILayout.Button("IsolateThisUI"))
//        {
//            List<UIState> statesParent = new List<UIState>();
//            statesParent.AddRange(Selection.activeGameObject.GetComponentsInParent<UIState>());
//            foreach (var item in statesParent[0].GetComponentsInChildren<UIState>())
//            {
                
//                if (item.gameObject == Selection.activeGameObject)
//                    item.CG.alpha = 1;
//                else
//                    item.CG.alpha = 0;
//                if (statesParent.Exists(X => X == item)) item.CG.alpha = 1;
//            }
//            foreach (var item in Selection.activeGameObject.GetComponentsInParent<UIState>())
//            {
//                item.CG.alpha = 1;
//            }
//        }

//    }
//}
//#endif
