﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.EventSystems;
using Utils;

public class WorldMapManager : MonoBehaviour
{
    #region Variables
    [SerializeField] GameObject Trajectory;
    [SerializeField] MeshRenderer EarthRenderer;
    [SerializeField] GameObject Clouds;
    [SerializeField] GameObject Glow;
    [SerializeField]
    private List<Texture2D> WorldLayersTextures;
    [SerializeField]
    private PlaceInfoType[] textureTypes;
    public List<Country> countries;
    public Material Earth;
    public Material Population;
    public Material Science;
    public Material Transport;
    public Material Disaster;
    public Material Climat;
    public List<Color> ClimatZonesColors;
    public List<string> ClimatZonesNames;
    public TextAsset CountryNamesJSONFile;
    public TextAsset CountryPopulationJSonFile;
    public Transform[] RocketDangerZone;
    

    public Country CurrentHovered;
    public Country CurrentPointCountry;
    public float currentPointValue;
    LayerMask EarthMask;
    public GameObject CurrenUnitPoint;
    public static event Action EventChangeState;
    public static event Action<Unit> EventCreatedNewUnit;

    private Dictionary<PlaceInfoType, Texture2D> typeToInfoTexture = new Dictionary<PlaceInfoType, Texture2D>();

    public Action<bool> OnAllowedBuildChanged;
    private bool _isAllowedToBuild;
    public bool isAllowedToBuild {
        set
        {
            bool prevValue = _isAllowedToBuild;
            _isAllowedToBuild = value;
            if (_isAllowedToBuild != prevValue) OnAllowedBuildChanged?.Invoke(_isAllowedToBuild);
        }
        get
        {
            return _isAllowedToBuild;
        }
    }

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
    public static WorldMapManager _instance;
    public static WorldMapManager instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<WorldMapManager>();
            }
            return _instance;
        }
        set { _instance = value; }
    }
    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this){ Destroy(gameObject); return; };

        for (int i = 0; i < textureTypes.Length; i++)
        {
            if (textureTypes[i] == PlaceInfoType.None) continue;
            typeToInfoTexture[textureTypes[i]] = WorldLayersTextures[i];
        }
       
        GameManager.EventChangeState += OnChangeState;
        GameManager.EventWithUnit += OnUnitCreated;
         
        
        EarthMask = LayerMask.GetMask("Earth");
       
    }
    private void Start()
    {
        HideMap(); 
    }
    public Texture2D GetTexture(PlaceInfoType type)
    {
        return typeToInfoTexture[type];
    }
    void OnUnitCreated(Unit unit)
    {
       
    }
    void OnChangeState()
    {
        if (GameManager.CurrentState == GameManager.State.CreateLaunchPlace) { Trajectory.SetActive(true); } else Trajectory.SetActive(false);
        if (GameManager.CurrentState == GameManager.State.CreateLaunchPlace || GameManager.CurrentState == GameManager.State.CreateProductionFactory || GameManager.CurrentState == GameManager.State.CreateResearchLab)
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
        CameraControllerInSpace.instance.thisCamera.cullingMask = ~0;
    }
    void HideMap()
    {
       
        CameraControllerInSpace.instance.thisCamera.cullingMask = ~LayerMask.GetMask("Country");
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
        
       if(GameManager.AboveEarth)SelectCountry();
    }
    public float MaxDamage = 0;
    void RocketDangerZoneCompute()
    {
        MaxDamage = 0;
        foreach (var item in RocketDangerZone)
        {
            RaycastHit hit;

          //  Debug.DrawRay(item.position, (-item.position + transform.position).normalized,Color.yellow);
            if (Physics.Raycast(item.position, (-item.position+transform.position).normalized, out hit, Mathf.Infinity, EarthMask))
            {
                MaxDamage+= GetPercentByTexture(GetTexture(PlaceInfoType.Population), hit.textureCoord);
            }
        }
    }
    void SelectCountry()
    {
        PlaceUnitPoint();

        RaycastHit hit;
        Ray ray = CameraControllerInSpace.instance.thisCamera.ScreenPointToRay(Input.mousePosition);

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

    public Vector2 HoveredEarthUVCoord;
    public Vector2 SelectedEarthUVCoord;


    void PlaceUnitPoint()
    {
        RaycastHit hit;
        if (Physics.Raycast(CameraControllerInSpace.instance.thisCamera.ScreenPointToRay(Input.mousePosition), out hit, 1000, EarthMask))
        {
            if (CurrenUnitPoint == null) CurrenUnitPoint = Instantiate(Resources.Load("UnitPoint/UnitPoint")) as GameObject;
            if (GameManager.Creation && Input.GetMouseButton(0))
            {
                RocketDangerZoneCompute();

                CurrentPointCountry = CurrentHovered;
                SelectedEarthUVCoord = HoveredEarthUVCoord;
                Trajectory.transform.rotation = Quaternion.LookRotation(-hit.point);
                Trajectory.transform.position = hit.point;
                
                CurrenUnitPoint.transform.position = hit.point;
                CurrenUnitPoint.transform.SetParent(GameManager.UnitsAll.Find(u => u.GetType() == typeof(UnitEarth)).transform);

                isAllowedToBuild = CurrentPointCountry && (GameManager.CurrentState == GameManager.State.CreateLaunchPlace || !CurrentPointCountry.isOcean);
            }
            HoveredEarthUVCoord = hit.textureCoord;
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
    [ContextMenu("SetNames")]
    void SetNames()
    {
        string[] nms = CountryNamesJSONFile.text.Split('}');
        foreach (var str in nms)
            foreach (var item in countries)
            {
                if (str.Substring(11, 2) == item.name) item.Name = str.Substring(25, str.Length - 26);
            }
       
    }
    [ContextMenu("SetPopulation")]
    void SetPopulationAndWealth()
    {
        ;
        string []nms= CountryPopulationJSonFile.text.Split('\n');
        foreach (var str in nms)
        {

            string[] cntr = str.Split('\t');
        
            foreach (var item in countries)
            {
                if(cntr[0].Trim().ToLower()==item.Name.ToLower())
                {
                    float.TryParse(cntr[3], out item.Population);
                    float.TryParse(cntr[2], out item.Wealth);
                }
            }

        }

    }

    public int GetZone(Texture2D tex, Vector2 uv)
    {
        Color col = tex.GetPixel(Mathf.RoundToInt(uv.x * tex.width), Mathf.RoundToInt(uv.y * tex.height));
        float max = 1000000;

        int result = -1;
        for (int i = 0; i < ClimatZonesColors.Count; i++)
        {
            float temp = Vector3.Distance(new Vector3(col.r, col.g, col.b), new Vector3(ClimatZonesColors[i].r, ClimatZonesColors[i].g, ClimatZonesColors[i].b));
            if (max > temp)
            {
                max = temp;
                result = i;
            }
        }
        return result;
    }
    public int GetPercentByTexture(Texture2D tex, Vector2 uv)
    {
        Color col = tex.GetPixel(Mathf.RoundToInt(uv.x * tex.width), Mathf.RoundToInt(uv.y * tex.height));
        return Mathf.RoundToInt(col.r * 100);
    }
}
