using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WorldMapManager : MonoBehaviour
{
    #region Variables
    [SerializeField] MeshRenderer EarthRenderer;
    [SerializeField] GameObject Clouds;
    [SerializeField] GameObject Glow;
    [SerializeField] public List<Country> countries;
    [SerializeField] public Material Earth;
    [SerializeField] public Material Population;
    [SerializeField] public Material Science;
    [SerializeField] public Material Transport;
    [SerializeField] public Material Disaster;
    public static event Action EventChangeState;
    public static event Action<Unit> EventCreatedNewUnit;
    public enum State { Earth, Politic, Population, Science, Transport,Disaster }
    private  State _currentState;
    public  State CurrentState
    {
        get => _currentState;
        set
        {
            switch (value)
            {
                case State.Earth:
                    EarthRenderer.sharedMaterial = Earth;
                    Clouds.SetActive(true);
                    Glow.SetActive(true);
                    HideMap();
                    break;
                case State.Politic:
                    ShowMap();
                    Clouds.SetActive(false);
                    Glow.SetActive(false);

                    break;
                case State.Population:
                    EarthRenderer.sharedMaterial = Population;
                    
                    HideMap();
                    Clouds.SetActive(false);
                    Glow.SetActive(false);
                    break;
                case State.Science:
                    EarthRenderer.sharedMaterial = Science;
                    Glow.SetActive(false);
                    HideMap();
                    Clouds.SetActive(false);
                    break;
                case State.Transport:
                    EarthRenderer.sharedMaterial = Transport;
                    Glow.SetActive(false);
                    HideMap();
                    Clouds.SetActive(false);
                    break;
                case State.Disaster:
                    EarthRenderer.sharedMaterial = Disaster;
                    Glow.SetActive(false);
                    Clouds.SetActive(false);
                    HideMap();
                    break;
                default:
                    break;
            }


            _currentState = value;
        }
    
    }
    #endregion
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
       
    }
    void HideMap()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1)) CurrentState = State.Earth;
        if (Input.GetKeyDown(KeyCode.F2)) CurrentState = State.Politic;
        if (Input.GetKeyDown(KeyCode.F3)) CurrentState = State.Population;
        if (Input.GetKeyDown(KeyCode.F4)) CurrentState = State.Science;
        if (Input.GetKeyDown(KeyCode.F5)) CurrentState = State.Transport;
        if (Input.GetKeyDown(KeyCode.F6)) CurrentState = State.Disaster;
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
