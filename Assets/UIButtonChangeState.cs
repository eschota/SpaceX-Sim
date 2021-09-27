using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonChangeState : MonoBehaviour
{
    [SerializeField] EState.UIState thisState;


    private void Start()
    {
        EState.EventChangeState += OnChangeState;
    }
    void OnChangeState()
    {
        EState.CurrentState = thisState;
    }
    private void OnDestroy()
    {
        EState.EventChangeState -= OnChangeState;
    }
}
