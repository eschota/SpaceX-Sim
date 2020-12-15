using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayersController : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Dropdown drop;
    
    void Awake()
    {
        drop.onValueChanged.AddListener(OnChange);
    }
    private void OnChange(int id)
    {
        WorldMapManager WM = FindObjectOfType<WorldMapManager>();
        if (id == 0) WM.CurrentState = WM.CurrentState = WorldMapManager.State.Earth;
        if (id == 1) WM.CurrentState = WM.CurrentState = WorldMapManager.State.Politic;
        if (id == 2) WM.CurrentState = WM.CurrentState = WorldMapManager.State.Population;
        if (id == 3) WM.CurrentState = WM.CurrentState = WorldMapManager.State.Science;
        if (id == 4) WM.CurrentState = WM.CurrentState = WorldMapManager.State.Transport;
        if (id == 5) WM.CurrentState = WM.CurrentState = WorldMapManager.State.Disaster;
    }
    private void OnDestroy()
    {
        drop.onValueChanged.RemoveListener(OnChange);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
