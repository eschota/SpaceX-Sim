using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIProductionManager : MonoBehaviour
{
    public static UIProductionManager instance;
    [SerializeField] public Transform Grid;
    [SerializeField] public Image ProductionGlobalButtonProgressFill;
    [SerializeField] public UiProductionButton ButtonPrefab;
    public List<UiProductionButton> ButtonsProductionFactories = new List<UiProductionButton>();
    void Start()
    {
        instance = this;
        GameManager.EventChangeState += OnChangeState;
    }
    

    void OnChangeState()
    {
        if (GameManager.CurrentState != GameManager.State.ProductionGlobal)
        {
            ClearProductionFactoryButtons();
            return;
        }
        else
        {
            ClearProductionFactoryButtons();
            AddProductionFactoryButtons();
        }
    }

    void AddProductionFactoryButtons()
    {
        foreach (var item in GameManager.Buildings)
        {
            if (!item.isResearch)
                if (item.GetType() == typeof(BuildingProductionFactory))
                    if (item.ConstructionCompletedPercentage >= 100)
                    {
                        
                    ButtonsProductionFactories.Add(Instantiate(ButtonPrefab, Grid));
                    ButtonsProductionFactories[ButtonsProductionFactories.Count - 1].ProductionFactory = item as BuildingProductionFactory;
                }
        }
    }
    void ClearProductionFactoryButtons()
    {
        foreach (var item in ButtonsProductionFactories)
        {
            Destroy(item.gameObject);
        }
        ButtonsProductionFactories.Clear();
    }
}
