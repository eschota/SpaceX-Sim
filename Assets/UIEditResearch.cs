using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIEditResearch : MonoBehaviour
{
    [SerializeField] public TMPro.TMP_InputField ResearchName;
    [SerializeField] public TMPro.TMP_InputField Light;
    [SerializeField] public TMPro.TMP_InputField Medium;
    [SerializeField] public TMPro.TMP_InputField Heavy;
    [SerializeField] public CanvasGroup CG;
    public static event Action EventChangeResearch;
    private Research _research;
    public Research CurrentResearchSelected
    {
        get => _research;
        set
        {
            _research = value;
            if (value == null)
            {
                CG.alpha = 0;
            }
            else
            {
                CG.alpha = 1;
                ResearchName.text = value.Name;

                EventChangeResearch();
            }
        }
    }
    
     

    public void OnEditResearch()
    {
        CurrentResearchSelected.Name = ResearchName.text;
        EventChangeResearch();
    }
    void Start()
    {
        CurrentResearchSelected = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
