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
    void Start()
    {
        UnitManager.instance.Selectables.Add(this);
       
        TimeManager.EventChangeDay += OnChangeDay;
     
    }
    public void IniSelectable()
    {
        progress = Instantiate(progress,GameManager.Canvas.transform);
        progress.Root = this.transform;
        progress.Progress.fillAmount = RootUnit.ConsctructionProcess / 100f;
        ShowPhase();
    }
    private void OnDestroy()
    {
        UnitManager.instance.Selectables.Remove(this);
        TimeManager.EventChangeDay -= OnChangeDay;
        Destroy(progress?.gameObject);
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
}
