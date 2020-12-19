using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEditResearch : MonoBehaviour
{
    [SerializeField] public TMPro.TMP_InputField ResearchName;
    [SerializeField] public TMPro.TMP_InputField Light;
    [SerializeField] public TMPro.TMP_InputField Medium;
    [SerializeField] public TMPro.TMP_InputField Heavy;
    [SerializeField] public CanvasGroup CG;
    private Research _research;
    public Research ResearchSelected
    {
        get => _research;
        set
        {
            _research = value;
            if (value == null)
            {
                CG.alpha = 0;
            }
            else CG.alpha = 1;
        }
    }
    
    void Start()
    {
        ResearchSelected = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
