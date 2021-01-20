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
        ChangeMats(true);
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
    { if (Phases.Length <= 1) return;
        int num = Mathf.RoundToInt(Phases.Length * RootUnit.ConsctructionProcess / 100f);
        for (int i = 0; i < Phases.Length; i++)
        {
            if(i!=num)
            Phases[i].SetActive(false);
            else
        
            Phases[i].SetActive(true);
        }


    }


    void ChangeMats(bool _is)
    {
        foreach (var item in MRS)
        {
            item.sharedMaterial = selectable;
        }
    }


    private void OnValidate()
    {
        MRS.Clear();
        MRS.AddRange(GetComponentsInChildren<MeshRenderer>());
        foreach (var item in MRS)
        {
            MTS.Add(item.sharedMaterial);
        }
        
    }
}
