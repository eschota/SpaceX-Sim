using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowOnEarthSelectBuilding : UIWindows
{

    [SerializeField] TMPro.TextMeshProUGUI Name;
    [SerializeField] Transform ModuleButtonsTransform;
    [SerializeField] UIBUttonSelectModuleForBuild ButtonPrefab;
    [SerializeField] Button ButtonOrderModule;
    [SerializeField] Button ButtonCancelModuleProduction;
    public Module CurrentSelectedModule;
    public static WindowOnEarthSelectBuilding instance;
    void Start()
    {
        instance = this;
    }

     
    private Unit _unit;
    public Unit unit
    {
        get => _unit;
        set
        {
            _unit = value;
            Name.text = value.Name;
        }
    }


    public override void Show()
    {
        base.Show();
        unit = UnitManager.instance.CurrentSelected;
        RefreshAvailableModules();

        
    }
    void RefreshAvailableModules()
    {
        ClearBUttons();
        if (unit.GetType() != typeof(BuildingProductionFactory)) return;

        foreach (var item in ResearchAndProductionManager.instance.ModulesAvailableForProduction)
            if(item.ProductionTime[(int)(CurrentSelectedModule as BuildingUnit).CurrentBuildingClass]>0)
        {
            ModuleButtons.Add(Instantiate(ButtonPrefab,ModuleButtonsTransform));
            ModuleButtons[ModuleButtons.Count - 1].Ini(item);
        }
        

    }
   List< UIBUttonSelectModuleForBuild> ModuleButtons = new List<UIBUttonSelectModuleForBuild>();
    void ClearBUttons()
    {
        foreach (var item in ModuleButtons)
        {
            Destroy(item.gameObject);
        }
        ModuleButtons.Clear();
    }

}
