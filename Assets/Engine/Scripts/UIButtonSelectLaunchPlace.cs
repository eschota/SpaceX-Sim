﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonSelectLaunchPlace : MonoBehaviour
{
    [SerializeField] GameParametersLaunchPlace launchPlace;
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
        UIButtonLaunchPlaceOk.CurrentLauchPlace = launchPlace;
    }
}
