using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public Unit RootUnit;
    MeshRenderer MR;
    Collider Col;
    void Start()
    {
        UnitManager.instance.Selectables.Add(this);
    }
    private void OnDestroy()
    {
        UnitManager.instance.Selectables.Remove(this);
    }

}
