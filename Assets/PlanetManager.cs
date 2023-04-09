using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class PlanetManager : MonoBehaviour
{
    [SerializeField] Planet GenerativePlanetPrefab;

    private int _currentPlanet;
    [SerializeField] public int CurrentPlanet
    {
        get 
        {
            return _currentPlanet;
        }
        set
        {
            if (value > PlanetList.Count)
            {
                _currentPlanet = 0;
            }
            if (value< 0) _currentPlanet = PlanetList.Count;


            _currentPlanet =value;
            foreach (var p in PlanetList)
            {
                if (p == null) 
                { 
                    RefreshPlanetList();
                    _currentPlanet = 0;
                }
                if (p.ID != value)
                {
                    p.Show(false);
                }
                else                     
                    p.Show(true);
            }
        }
    }
    [HideInInspector]
    [SerializeField]
    public List<Planet> PlanetList=new List<Planet>();

    [SerializeField] public int Seed = 0;
    
    void Awake()
    {
        RefreshPlanetList();
    }

    public void RefreshPlanetList()
    {
        PlanetList.Clear();
        PlanetList.AddRange(FindObjectsOfType<Planet>());
        foreach (var p in PlanetList)
        {
            p.ID = PlanetList.IndexOf(p);
        }
        if(CurrentPlanet!=0) while (PlanetList[CurrentPlanet]==null) CurrentPlanet--;
    }
    public void AddPlanet()
    {
        if(PlanetList.Count == 0) { PlanetList.AddRange(FindObjectsOfType<Planet>()); }
        foreach (Planet go in Resources.FindObjectsOfTypeAll(typeof(Planet)) as Planet[])
        {
            if(go.planetType==Planet.PlanetType.Generative) 
            {
            GameObject GO=Instantiate(go.gameObject);
              PlanetList.Add(GO.GetComponent<Planet>());
              PlanetList[PlanetList.Count-1].ID=PlanetList.Count-1;
              PlanetList[PlanetList.Count - 1].Regenerate(Seed);
              CurrentPlanet = PlanetList.Count-1;
                PlanetList[PlanetList.Count - 1].gameObject.name="Planet "+ (PlanetList[PlanetList.Count - 1].ID+1).ToString();
                return; 
               
            }
        }
    }
    public void RemovePlanet()
    {
        DestroyImmediate( PlanetList[CurrentPlanet].gameObject);
    }
    public void EditPlanet()
    {
        Debug.Log("Edit Planet");
    }
     
    void Update()
    {

        
        
    }
}
