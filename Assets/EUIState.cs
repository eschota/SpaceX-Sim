using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EUIState : MonoBehaviour
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIState : MonoBehaviour
    {
        private void Reset()
        {
            CG = GetComponent<CanvasGroup>();
        }
        private void Start()
        {
            EState.EventChangeState += OnChange;
            if (thisState == null) Debug.LogError("Ни одного стейта не выбрано у ГУЙ объекта : " + name);
            OnChange();

        }


        void Update()
        {

        }
        [SerializeField] public CanvasGroup CG;
        [SerializeField] List<EState.UIState> thisState;


        void OnChange()
        {
            if (thisState.Count < 1) return;
            if (thisState.Exists(X => X == EState.CurrentState)) Show();
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
           EState.EventChangeState -= OnChange;

        }



    }
}
