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

    public void EMapIni()
    {
        WiresGO = new GameObject("Wires").gameObject; 
       

        for (int j = 0; j < 12; j++)
        {
            GameObject child = GameObject.Instantiate(Resources.Load<GameObject>("Emap\\Wires"));
            child.transform.SetParent(WiresGO.transform);
            child.transform.localRotation = Quaternion.Euler(new Vector3(0, (j + 1) * 30, 0));
        }

    }
    public void EmapClear()
    {
        Destroy(Wires.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
