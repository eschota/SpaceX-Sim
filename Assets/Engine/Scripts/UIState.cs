using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
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
    }

    void Start()
    {
        OnChange();
    }
    void Update()
    {
      if(Application.isEditor)  EditorUIInteract();
    }
    [HideInInspector] [SerializeField] CanvasGroup CG;
    [SerializeField] List <GameManager.State> thisState;

    void OnChange()
    {
        if(thisState.Exists(X=>X==GameManager.CurrentState))  Show();
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

    void EditorUIInteract()
    {
        if (Application.isPlaying) return;
        if (Selection.activeGameObject == gameObject)
        {
            foreach (var item in FindObjectsOfType<UIState>())
            {

                if (item.gameObject!=gameObject)  item.CG.alpha = 0;
                else item.CG.alpha = 1;
            }
            

        }
    }
}
