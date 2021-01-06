using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonLaunchPlaceOk : MonoBehaviour
{
    public static CountrySO CurrentLauchPlace;
    [SerializeField] GameObject CancelButton;
    private GameObject UnitLaunchPrefab;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
        GameManager.EventChangeState += OnChange;

        UnitLaunchPrefab = Resources.Load<GameObject>("UnitPoint/UnitPoint");
    }
    void OnChange()
    {
        if (GameManager.CurrentState == GameManager.State.CreateLaunchPlace)
        {
            //if (FindObjectOfType<UnitLaunchPlace>()==null) CancelButton.gameObject.SetActive(false);
            //else CancelButton.gameObject.SetActive(true);
        }
    }
    void OnClick()
    {
       
            if (WorldMapManager.instance.CurrenUnitPoint!=null)
            {
                switch (GameManager.CurrentState)
                {
                    case GameManager.State.CreateLaunchPlace:
                        UnitLaunchPlace launchPlace=Instantiate(UnitLaunchPrefab).AddComponent<UnitLaunchPlace>();
                        launchPlace.name = "LaunchPlace";
                        launchPlace.Country = WorldMapManager.instance.CurrentHovered;

                        launchPlace.transform.parent = GameManager.UnitsAll.Find(X => X.GetType() == typeof(UnitEarth)).transform;
                    launchPlace.transform.position = WorldMapManager.instance.CurrenUnitPoint.transform.position;
                        GameManager.UnitsAll.Add(launchPlace);

                        GameManager.instance.OpenUnitScene(launchPlace);
                        CameraManager.FlyToUnit = launchPlace;
                        break;
                    case GameManager.State.CreateProductionFactory:
                        UnitProductionFactory unit = Instantiate(UnitLaunchPrefab).AddComponent<UnitProductionFactory>();
                        unit.name = "ProductionFactory";
                        unit.Country = WorldMapManager.instance.CurrentHovered;

                        unit.transform.parent = GameManager.UnitsAll.Find(X => X.GetType() == typeof(UnitEarth)).transform;
                    unit.transform.position = WorldMapManager.instance.CurrenUnitPoint.transform.position;
                    GameManager.UnitsAll.Add(unit);

                        GameManager.instance.OpenUnitScene(unit);
                        CameraManager.FlyToUnit = unit;
                        break;
                    case GameManager.State.CreateResearchLab:
                        UnitResearchLab unitR = Instantiate(UnitLaunchPrefab).AddComponent<UnitResearchLab>();
                        unitR.name = "ResearchLab";
                        unitR.Country = WorldMapManager.instance.CurrentHovered;

                        unitR.transform.parent = GameManager.UnitsAll.Find(X => X.GetType() == typeof(UnitEarth)).transform;
                    unitR.transform.position = WorldMapManager.instance.CurrenUnitPoint.transform.position;
                    GameManager.UnitsAll.Add(unitR);

                        GameManager.instance.OpenUnitScene(unitR);
                        CameraManager.FlyToUnit = unitR;
                        break;
                }
            }
           
         
        else Alert.instance.AlertMessage = "Select Launch Place First!!!";
    }
    
    void Update()
    {
        
    }
}
