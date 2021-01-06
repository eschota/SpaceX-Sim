using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowCreateNewEarthUnit : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Dropdown SelectSize;
    [SerializeField] TMPro.TextMeshProUGUI TotalCost;
    public static CountrySO CurrentLauchPlace;
    [SerializeField] GameObject CancelButton;
    private GameObject UnitLaunchPrefab;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);

        SelectSize.onValueChanged.AddListener(OnChangeValue);
        OnChangeValue(0);
    }
    
    void OnChangeValue(int id)
    {
        TotalCost.text = ((id + 1) * 100).ToString()+"K$";
    }
    private void OnDestroy()
    {
        SelectSize.onValueChanged.RemoveAllListeners();
    }
    void OnClick()
    {

        if (WorldMapManager.instance.CurrenUnitPoint == null) { Alert.instance.AlertMessage = "Select Place First!!!";  return; }

            GameObject obj = Instantiate(Resources.Load<GameObject>("UnitPoint/UnitPoint"));

             
           
           

           
        
        obj.transform.parent = GameManager.UnitsAll.Find(X => X.GetType() == typeof(UnitEarth)).transform;
        obj.transform.position = WorldMapManager.instance.CurrenUnitPoint.transform.position;

        CameraManager.FlyToUnit = obj.transform;
        switch (GameManager.CurrentState)
        {

            case GameManager.State.CreateLaunchPlace:
                var unit = obj.AddComponent<UnitLaunchPlace>();
                unit.EcoRentCost = (SelectSize.value + 1) * 100;
                GameManager.instance.OpenUnitScene(unit);
                break;
            case GameManager.State.CreateResearchLab:
                var unit2 = obj.AddComponent<UnitResearchLab>();
                unit2.EcoRentCost = (SelectSize.value + 1) * 100;
                GameManager.instance.OpenUnitScene(unit2);
                break;
            case GameManager.State.CreateProductionFactory:

                var unit3 = obj.AddComponent<UnitProductionFactory>();
                unit3.EcoRentCost = (SelectSize.value + 1) * 100;
                GameManager.instance.OpenUnitScene(unit3);
                break;
        }
            


    }
    
}
