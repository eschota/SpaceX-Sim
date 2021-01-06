using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiCountryChoiceText : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI Name;
    [SerializeField] TMPro.TextMeshProUGUI CostBuild;
    [SerializeField] TMPro.TextMeshProUGUI CostPerMonth;
    [SerializeField] TMPro.TextMeshProUGUI CostPerLaunch;

    public void SetCountryToGUI(CountrySO country)
    {
        Name.text = country.Name;
        CostBuild.text = country.CostBuild.ToString();
        CostPerMonth.text = country.CostPerMonth.ToString();
        CostPerLaunch.text = country.CostLaunch.ToString();
        WindowCreateNewEarthUnit.CurrentLauchPlace = country;
    }
}
