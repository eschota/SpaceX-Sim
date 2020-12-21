using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelectModule : MonoBehaviour
{
    [SerializeField] Dropdown DropdownByTypes;
    [SerializeField] CanvasGroup CG;
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
            }
        }
    }

   
   
}
