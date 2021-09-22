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
        EState.EventChangeState += OnChangeState;
    }
    public void EMapIni()
    {

        WiresGO = new GameObject("Wires").gameObject; 
        WiresGO.transform.position= Vector3.zero; 
       

        for (int j = 0; j < 12; j++)
        {
            GameObject child = GameObject.Instantiate(Resources.Load<GameObject>("Emap\\Wires"));
            child.transform.SetParent(WiresGO.transform);
            child.transform.localRotation = Quaternion.Euler(new Vector3(0, (j + 1) * 30, 0));
        }
        WiresGO.SetActive(false);
    }
    public void EmapClear()
    {
        Destroy(WiresGO);
    }

    void OnChangeState()
    {
        if (EState.CurrentState == EState.UIState.Building)
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
        EState.EventChangeState -= OnChangeState;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
