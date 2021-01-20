using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public BuildingUnit RootUnit;
    MeshRenderer MR;
    Collider Col;
    [SerializeField] UICircleProgress progress;
    [SerializeField] GameObject[] Phases;
    [SerializeField] List<Material> MTS;
    [SerializeField] List<MeshRenderer> MRS;
    Material selectable;
    void Start()
    {
        UnitManager.instance.Selectables.Add(this);
       selectable= Resources.Load<Material>("SelectableMesh");
        TimeManager.EventChangeDay += OnChangeDay;
       
    }
    public void IniSelectable()
    {
        progress = Instantiate(progress,GameManager.Canvas?.transform);
        progress.Root = this.transform;
        progress.Progress.fillAmount = RootUnit.ConsctructionProcess / 100f;
        ShowPhase();
    }
    private void OnDestroy()
    {
        UnitManager.instance.Selectables.Remove(this);
        TimeManager.EventChangeDay -= OnChangeDay;
        DestroyImmediate(progress?.gameObject);
    }

   
    void OnChangeDay()
    {
        progress.Progress.fillAmount = RootUnit.ConsctructionProcess / 100f;
        ShowPhase();
    }

    void ShowPhase()
    {
        if (Phases.Length <= 1) return;
        int num = Mathf.RoundToInt(Phases.Length * RootUnit.ConsctructionProcess / 100f);
        for (int i = 0; i < Phases.Length; i++)
        {
            if(i!=num)
            Phases[i].SetActive(false);
            else
        
            Phases[i].SetActive(true);
        }


    }
    void OnMouseEnter()
    {
     //   if (UnitManager.instance.CurrentState != UnitManager.State.None) return;
        ChangeMats(true);
        Debug.Log("Mouse is over GameObject.");
    }
    private void OnMouseDown()
    {
        if (UnitManager.instance.CurrentState != UnitManager.State.None) return;
        UnitManager.instance.CurrentSelected = RootUnit;
    }
    void OnMouseExit()
    {
    //    if (UnitManager.instance.CurrentState != UnitManager.State.None) return;
        ChangeMats(false);
        //The mouse is no longer hovering over the GameObject so output this message each frame
        Debug.Log("Mouse is no longer on GameObject.");
    }

    void ChangeMats(bool _is)
    {
        if (_is)
            foreach (var item in MRS)
            {
                item.sharedMaterial = selectable;
            }
        else
        {
            if (MRS[0].sharedMaterial == MTS[0]) return;
            for (int i = 0; i < MRS.Count; i++)
                MRS[i].sharedMaterial = MTS[i];
        }
    }


    private void OnValidate()
    {
        MRS.Clear();
        MTS.Clear();
        MRS.AddRange(GetComponentsInChildren<MeshRenderer>());
        foreach (var item in MRS)
        {
            MTS.Add(item.sharedMaterial);
        }
        
    }
}
