using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMap : MonoBehaviour
{
    private EMapWires Wires;
    GameObject WiresGO;
    void Start()
    {
        EMapIni();
    }
    private void Awake()
    {
        Core.EventChangeState += OnChangeState;
    }
    public void EMapIni()
    {

        WiresGO = new GameObject("Wires").gameObject; 
        WiresGO.transform.position= Vector3.zero; 
       

        for (int j = 0; j < 24; j++)
        {
            GameObject child = Instantiate(Resources.Load("Emap\\Wires")) as GameObject;
            child.transform.SetParent(WiresGO.transform);
            child.transform.localRotation = Quaternion.Euler(new Vector3(0, (j + 1) * 15, 0));
        }
        WiresGO.SetActive(false);
    }
    public void EmapClear()
    {
        Destroy(WiresGO);
    }

    void OnChangeState()
    {
        if (Core.CurrentState == Core.State.Building)
        {
            WiresGO.SetActive(true);
        }
        else
        {
            WiresGO.SetActive(false);
        }
    }
    private void OnDestroy()
    {
        Core.EventChangeState -= OnChangeState;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
