using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelectScenario : MonoBehaviour
{
    [SerializeField] public TMPro.TextMeshProUGUI ScenarioName;
    public ScenarioManager.Scenario scenario;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);

    }

    private void OnClick()
    {
        ScenarioManager.instance.CurrentScenario = scenario;
        ScenarioManager.instance.SaveNameInputField.SetTextWithoutNotify(scenario.Name);
    }
    private void OnDestroy()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();

    }
}
