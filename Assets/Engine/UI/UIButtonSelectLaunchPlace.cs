using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonSelectLaunchPlace : MonoBehaviour
{
    [SerializeField] CountrySO launchPlace;
    [SerializeField] TMPro.TextMeshProUGUI Name;
    [SerializeField] TMPro.TextMeshProUGUI CostBuild;
    [SerializeField] TMPro.TextMeshProUGUI CostPerMonth;
    [SerializeField] TMPro.TextMeshProUGUI CostPerLaunch;
       void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }
    void OnClick()
    {
        Name.text = launchPlace.Name;
        CostBuild.text = launchPlace.CostBuild.ToString();
        CostPerMonth.text = launchPlace.CostPerMonth.ToString();
        CostPerLaunch.text = launchPlace.CostLaunch.ToString();
        WindowCreateNewEarthUnit.CurrentLauchPlace = launchPlace;
    }

    public void SetCountryToGUI(CountrySO country)
    {
        Name.text = country.Name;
        CostBuild.text = country.CostBuild.ToString();
        CostPerMonth.text = country.CostPerMonth.ToString();
        CostPerLaunch.text = country.CostLaunch.ToString();
        WindowCreateNewEarthUnit.CurrentLauchPlace = country;
    }
}
