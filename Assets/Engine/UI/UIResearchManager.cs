using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIResearchManager : MonoBehaviour
{
    public static UIResearchManager instance;
    [SerializeField] public Transform Grid;

    public List<UIResearchButton> Researches;
    void Start()
    {
        instance = this;
        GameManager.EventChangeState += OnChangeState;
    }


    void OnChangeState ()
    {
        ClearResearches();
        AddButtons();
    }

    private void AddButtons()
    {
        foreach (var item in GameManager.Buildings)
        {
            if (!item.isResearch)
                if (item.GetType() == typeof(BuildingResearchLab))
                    (item as BuildingResearchLab).ButtonLab.name += Researches.Count.ToString();
        }
    }
    private void ClearResearches()
    {
        if (GameManager.CurrentState != GameManager.State.ResearchGlobal) return;
        if (Researches != null) for (int i = 0; i < Researches.Count; i++)
            {
                Destroy(Researches[i].gameObject);
            }
        Researches = new List<UIResearchButton>();

    }
    private void OnDestroy()
    {
        GameManager.EventChangeState -= OnChangeState;

    }
}
