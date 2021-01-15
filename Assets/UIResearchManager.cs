using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIResearchManager : MonoBehaviour
{
    public static UIResearchManager instance;
    [SerializeField] public Transform Grid;

    void Start()
    {
        instance = this;
    }   
}
