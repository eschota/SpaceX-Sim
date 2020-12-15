using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMap : MonoBehaviour
{
    [SerializeField] GameObject Clouds;
    [SerializeField] public List<Country> countries;
    void Awake()
    {
        GameManager.EventChangeState += OnChangeState;
        HideMap();
    }

    void OnChangeState()
    {
        if (GameManager.CurrentState == GameManager.State.CreateLauchPlace || GameManager.CurrentState == GameManager.State.CreateProductionFactory || GameManager.CurrentState == GameManager.State.CreateResearchLab) ShowMap();
        else HideMap();
    }
    private void OnDestroy()
    {
        GameManager.EventChangeState -= OnChangeState;
    }

    void ShowMap()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
        Clouds.SetActive(false);
    }
    void HideMap()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        Clouds.SetActive(true);
    }
    void Update()
    {
        
    }

    void SelectCountry()
    {

    }

    [ContextMenu ("Select AllCountryes")]
    void SelectAllCountriesInEditor()
    {
        countries.Clear();
        countries.AddRange(FindObjectsOfType<Country>());
    }
    [ContextMenu("SetRandomColors")]
    void SetRandomColorsAllCountriesInEditor()
    {
        foreach (var item in countries)
        {
            item.ColorCountry = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        }
    }
}
