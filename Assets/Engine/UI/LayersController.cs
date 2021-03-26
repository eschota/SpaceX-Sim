using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayersController : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Dropdown drop;
    List<string> options;
     
    void Awake()
    {
        drop=GetComponent<TMPro.TMP_Dropdown>();
        drop.onValueChanged.AddListener(OnChange); 
        WorldMapManager.EventChangeState += OnChangeState;
    }
    private void OnChange(int id)
    {
       
        if (id == 0) WorldMapManager.instance.CurrentState = WorldMapManager.instance.CurrentState = WorldMapManager.State.Earth;
        if (id == 1) WorldMapManager.instance.CurrentState = WorldMapManager.instance.CurrentState = WorldMapManager.State.Politic;
        if (id == 2) WorldMapManager.instance.CurrentState = WorldMapManager.instance.CurrentState = WorldMapManager.State.Population;
        if (id == 3) WorldMapManager.instance.CurrentState = WorldMapManager.instance.CurrentState = WorldMapManager.State.Science;
        if (id == 4) WorldMapManager.instance.CurrentState = WorldMapManager.instance.CurrentState = WorldMapManager.State.Transport;
        if (id == 5) WorldMapManager.instance.CurrentState = WorldMapManager.instance.CurrentState = WorldMapManager.State.Disaster;
        if (id == 6) WorldMapManager.instance.CurrentState = WorldMapManager.instance.CurrentState = WorldMapManager.State.Climat;
    }
    private void OnDestroy()
    {
        drop.onValueChanged.RemoveListener(OnChange);
        WorldMapManager.EventChangeState -= OnChangeState;
    }
    void OnChangeState()
    {
        drop.SetValueWithoutNotify((int)WorldMapManager.instance.CurrentState);
        drop.SetValueWithoutNotify((int)WorldMapManager.instance.CurrentState);
    }
    void Update()
    {
        
    }
}
