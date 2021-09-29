using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasState : MonoBehaviour
{
    CanvasGroup CG;
    [SerializeField] GM.State thisState;
    void Start()
    {
        CG = GetComponent<CanvasGroup>();

    }

    // Update is called once per frame
    void Update()
    {
        if (thisState == GM.CurrentState)
        {
            CG.alpha += GM.EventTimer;
        }
        else
            CG.alpha -= GM.EventTimer;
    }
}
