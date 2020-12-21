using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

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
    [SerializeField] public Material Climat;
    public Country CurrentHovered;
    LayerMask mask;
    public GameObject CurrenUnitPoint;
    public static event Action EventChangeState;
    public static event Action<Unit> EventCreatedNewUnit;
    public enum State { Earth=0, Politic=1, Population=2, Science=3, Transport=4,Disaster=5,Climat=6 }
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
                    EarthRenderer.sharedMaterial = Earth;
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
                case State.Climat:
                    EarthRenderer.sharedMaterial = Climat;
                    Glow.SetActive(false);
                    Clouds.SetActive(false);
                    HideMap();
                    break;
                default:
                    break;
            }

            
            _currentState = value;
            EventChangeState();
        }
    
    }
    #endregion
    public static WorldMapManager instance;
    void Awake()
    {
        if (instance == null) instance = this;
        else { DestroyImmediate(this.gameObject); return; };
       
        GameManager.EventChangeState += OnChangeState;
        GameManager.EventWithUnit += OnUnitCreated;
         
        HideMap();
        mask = LayerMask.GetMask("Earth");
    }
    void OnUnitCreated(Unit unit)
    {
       
    }
    void OnChangeState()
    {
        if (GameManager.CurrentState == GameManager.State.CreateLauchPlace || GameManager.CurrentState == GameManager.State.CreateProductionFactory || GameManager.CurrentState == GameManager.State.CreateResearchLab)
        {
            ShowMap();
            CurrentState = State.Politic;
        }
        else
        {
            CurrentState = State.Earth;
            HideMap();
        }
        if (GameManager.CurrentState == GameManager.State.PlaySpace) Destroy(CurrenUnitPoint.gameObject);
    }
    private void OnDestroy()
    {
        GameManager.EventChangeState -= OnChangeState;
        GameManager.EventWithUnit -= OnUnitCreated;
    }

    void ShowMap()
    {
        foreach (var item in countries)
        {
            item.gameObject.SetActive(true);
        }
       
    }
    void HideMap()
    {
        foreach (var item in countries)
        {
            item.gameObject.SetActive(false);
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
        if (Input.GetKeyDown(KeyCode.F7)) CurrentState = State.Climat;
        if (GameManager.CurrentState == GameManager.State.CreateLauchPlace || GameManager.CurrentState == GameManager.State.CreateProductionFactory || GameManager.CurrentState == GameManager.State.CreateResearchLab) SelectCountry();
    }

    void SelectCountry()
    {
        if (Input.GetMouseButton(0)) PlaceUnitPoint();

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000))
        {

            if (hit.collider.gameObject == null) return;
            Country tempCountry = countries.Find(X => X.gameObject == hit.collider.gameObject);
            if (tempCountry != null)
            {
                if (tempCountry != CurrentHovered)
                {
                    if (CurrentHovered != null) CurrentHovered.Hovered = false;
                    CurrentHovered = tempCountry;
                    Debug.Log("Hovered:" + tempCountry.name);
                }

                CurrentHovered.Hovered = true;
                return;
            }
            else
            {
                if (CurrentHovered != null) CurrentHovered.Hovered = false;
                CurrentHovered = null;
            }
        }
        else
        {
            if (CurrentHovered != null) CurrentHovered.Hovered = false;
            CurrentHovered = null;
        }
       

    }
    void PlaceUnitPoint()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000, mask))
        {
            if (CurrenUnitPoint == null) CurrenUnitPoint = Instantiate(Resources.Load("UnitPoint/UnitPoint")) as GameObject;
            CurrenUnitPoint.transform.position = hit.point;
            CurrenUnitPoint.transform.SetParent(GameManager.UnitsAll.Find(u => u.GetType() == typeof(UnitEarth)).transform);
        }
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
