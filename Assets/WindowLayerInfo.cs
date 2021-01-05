using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowLayerInfo : UIWindows
{
    public static WindowLayerInfo instance;

    [SerializeField] public List<TMPro.TextMeshProUGUI> layersText;
    void Awake() 
    {
        instance = this;
    }

    
}
