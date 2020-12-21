using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIEditResearch : MonoBehaviour
{
    [SerializeField] public TMPro.TMP_InputField ResearchName;
    [SerializeField] public TMPro.TMP_InputField Light;
    [SerializeField] public TMPro.TMP_InputField Medium;
    [SerializeField] public TMPro.TMP_InputField Heavy;
    [SerializeField] public CanvasGroup CG;
    [SerializeField] public Toggle Completed;
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
                UISelectModule.instance.CurrentResearchSelected = null;
            }
            else
            {
                CG.alpha = 1;
                ResearchName.text = value.Name;
                Light.text = value.TimeCost[0].ToString();
                Medium.text = value.TimeCost[1].ToString();
                Heavy.text = value.TimeCost[2].ToString();
                Completed.isOn = value.Completed;
                EventChangeResearch();
            }
        }
    }
    
     

    public void OnEditResearch()
    {
        CurrentResearchSelected.Name = ResearchName.text;

        int light = 0;
        int.TryParse(Light.text,out light);
        int medium = 0;
        int.TryParse(Medium.text,out medium);
        int heavy = 0;
        int.TryParse(Heavy.text,out heavy);

        CurrentResearchSelected.TimeCost = new[] {light,medium,heavy};
        CurrentResearchSelected.Completed = Completed.isOn;        
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
