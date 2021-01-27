using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public BuildingUnit RootUnit;
    MeshRenderer MR;
    Collider Col;
    UICircleProgress progress;
    [SerializeField] GameObject[] Phases;
    [SerializeField] List<Material> MTS;
    [SerializeField] List<MeshRenderer> MRS;
    Material selectable;
    Material selectableCancel;
    void Start()
    {
        UnitManager.instance.Selectables.Add(this);
       selectable= Resources.Load<Material>("UI/SelectableMesh");
       selectableCancel= Resources.Load<Material>("UI/SelectableMeshCantPlace");
        TimeManager.EventChangeDay += OnChangeDay;
       
    }
    public void IniSelectable()
    {
        progress = Instantiate(Resources.Load<UICircleProgress>("UI/ButtonUnits/UICircleProgress" ),UIUnitManager.instance.transform);
        progress.Root = this.transform;
        ShowPhase();
        OnChangeDay();
    }
    private void OnDestroy()
    {
        UnitManager.instance.Selectables.Remove(this);
        TimeManager.EventChangeDay -= OnChangeDay;
      //  Destroy(progress);
    }

   
   public void OnChangeDay()
    {
        progress.Progress.fillAmount = RootUnit.ConstructionCompletedPercentage/100f;
        progress.percentage.text = (Mathf.RoundToInt( RootUnit.ConstructionCompletedPercentage).ToString()) + "%";
        ShowPhase();
    }

    void ShowPhase()
    {
        if (Phases.Length <= 1) return;
        int num = Mathf.RoundToInt(Phases.Length * (RootUnit.ConstructionCompletedPercentage) / 100f);
        if (num == 0) num = 1;
        for (int i = 0; i < Phases.Length; i++)
        {
            if(i!=num-1)
            Phases[i].SetActive(false);
            else
        
            Phases[i].SetActive(true);
        }


    }
    void OnMouseEnter()
    {
        if (UnitManager.instance.CurrentState == UnitManager.State.PlaceBuilding) return;
        ChangeMats(true);
        Debug.Log("Mouse is over GameObject.");
    }
    private void OnMouseDown()
    {
       if (UnitManager.instance.CurrentState == UnitManager.State.PlaceBuilding) return;
        UnitManager.instance.CurrentSelected = RootUnit;
    }
    void OnMouseExit()
    {
    //    if (UnitManager.instance.CurrentState != UnitManager.State.None) return;
        ChangeMats(false);
        //The mouse is no longer hovering over the GameObject so output this message each frame
        Debug.Log("Mouse is no longer on GameObject.");
    }

 public    void ChangeMats(bool _is)
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
    public void ChangeMatsCancel()
    {        
            foreach (var item in MRS)
            {
                item.sharedMaterial = selectableCancel;
            }
    }

    public void OnValidate()
    {
        GetAllDependance();
    }

    public void GetAllDependance()
    {
        MRS = new List<MeshRenderer>();
        MRS.Clear();
        MTS = new List<Material>();
        MTS.Clear();
        MRS.AddRange(GetComponentsInChildren<MeshRenderer>());
        foreach (var item in MRS)
        {
            MTS.Add(item.sharedMaterial);
        }
    }
    [SerializeField] public BoxCollider col;
    private void Reset()
    {
        if (col == null)
        {
            col = gameObject.AddComponent<BoxCollider>();
            col.size = Vector3.one * 100;
        }
        col.isTrigger = true;
    }
}
