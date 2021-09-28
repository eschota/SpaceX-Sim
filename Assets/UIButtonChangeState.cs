using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UIButtonChangeState : UIState
{

    [SerializeField] EState.UIState changeToState;
    
    private void Start()
    {
        EState.EventChangeState += OnChangeState;
    }
    void OnChangeState()
    {
        EState.CurrentState = changeToState;
    }
    private void OnDestroy()
    {
        EState.EventChangeState -= OnChangeState;
    }


    

}
