using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UIButtonChangeState : MonoBehaviour
{
    [SerializeField] EState.UIState thisState;
    [SerializeField] RectTransform rect;
    [SerializeField] Vector3 Pos1 = Vector3.one * 0.5f;
    [SerializeField] Vector3 Pos2 = Vector3.one * 0.5f; 
    [SerializeField] Vector3 Rot1 = Vector3.zero;
    [SerializeField] Vector3 Rot2 = Vector3.zero;
    [Range (0,1)]
    public float CurrentLerp = 0;

    private void OnValidate()
    {
        if (rect == null) rect = GetComponent<RectTransform>();
        rect.anchoredPosition = Vector3.Lerp(Pos1, Pos2, CurrentLerp);
        rect.rotation = Quaternion.Euler(Vector3.Lerp(Rot1, Rot2, CurrentLerp));
    }
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


    private void Update()
    {
        if (!Application.isPlaying) return;
        if(thisState==EState.CurrentState)
        {
            rect.anchoredPosition = Vector3.Lerp(Pos1, Pos2, EState.EventTimer);
            rect.rotation = Quaternion.Euler(Vector3.Lerp(Rot1, Rot2, EState.EventTimer));
        }
        else
        {
            rect.anchoredPosition = Vector3.Lerp(Pos2, Pos1, 5 * Time.deltaTime);
            rect.rotation = Quaternion.Euler(Vector3.Lerp(Rot2, Rot1, EState.EventTimer));
        }
    }

}
