using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof (Button))]
public class UIButton : MonoBehaviour
{
    [SerializeField] GameManager.State thisState;
    [SerializeField][HideInInspector] Button but;

    private void Reset()
    {
        but = GetComponent<Button>();
    }
    void Awake()
    {if(but==null) but = GetComponent<Button>();
        but.onClick.AddListener( OnClick);
    }
    void OnClick()
    {
        GameManager.CurrentState = thisState;
    }
}
