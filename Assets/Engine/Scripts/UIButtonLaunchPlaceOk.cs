using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonLaunchPlaceOk : MonoBehaviour
{
    public static CountrySO CurrentLauchPlace;
    [SerializeField] GameObject CancelButton;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
        GameManager.EventChangeState += OnChange;
    }
    void OnChange()
    {
        if (GameManager.CurrentState == GameManager.State.CreateLauchPlace)
        {
            //if (FindObjectOfType<UnitLaunchPlace>()==null) CancelButton.gameObject.SetActive(false);
            //else CancelButton.gameObject.SetActive(true);
        }
    }
    void OnClick()
    {
        if (CurrentLauchPlace != null)
        {
            if (GetCountrieByColor.launchPlace != null)
            {
                switch (GameManager.CurrentState)
                {
                    case GameManager.State.CreateLauchPlace:
                        UnitLaunchPlace launchPlace=GameObject.CreatePrimitive(PrimitiveType.Sphere).AddComponent<UnitLaunchPlace>();
                        launchPlace.name = "LaunchPlace";
                        launchPlace.Country = CurrentLauchPlace;
                        GameManager.CreateLaunchPlace(CurrentLauchPlace, GetCountrieByColor.launchPlace,launchPlace);
                        break;
                    case GameManager.State.CreateProductionFactory:
                        UnitProductionFactory productionFactory = GameObject.CreatePrimitive(PrimitiveType.Sphere).AddComponent<UnitProductionFactory>();
                        productionFactory.name = "ProductionFactory";
                        productionFactory.Country = CurrentLauchPlace;
                        GameManager.CreateLaunchPlace(CurrentLauchPlace, GetCountrieByColor.launchPlace, productionFactory);
                        break;
                    case GameManager.State.CreateResearchLab:
                        UnitResearchLab researchLab = GameObject.CreatePrimitive(PrimitiveType.Sphere).AddComponent<UnitResearchLab>();
                        researchLab.name = "ResearchLab";
                        researchLab.Country = CurrentLauchPlace;
                        GameManager.CreateLaunchPlace(CurrentLauchPlace, GetCountrieByColor.launchPlace,researchLab);
                        break;
                }
                
            }
            else
            {
                Debug.Log("Chose point");

            }
        }
        else Alert.instance.AlertMessage = "Select Launch Place First!!!";
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
