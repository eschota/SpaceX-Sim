using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
[ExecuteInEditMode]

public class Planet : MonoBehaviour
{
   [HideInInspector]
   [SerializeField] public PlanetType planetType; [HideInInspector]
    
    [SerializeField] public MeshRenderer AtmosphereMR;
    [SerializeField] public MeshRenderer PlanetMR;
    [SerializeField] public MeshRenderer StarsMR;
    [SerializeField] public Volume volume;
    [HideInInspector]
    public int ID=-1;


    public enum PlanetType
    {
        Exist,
        Generative
    }
    void Awake()
    {
        Debug.Log("Planet Awaked: " + gameObject.name);
    }

    [ContextMenu("Randomize")]
    public void Regenerate(int seed)
    {
        UnityEngine.Random.seed = seed;
        SetAtmosphereColor(Random.ColorHSV());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void Show(bool state)
    {
        AtmosphereMR.gameObject.SetActive(state);
        PlanetMR.gameObject.SetActive(state);
        StarsMR.gameObject.SetActive(state);
        volume.gameObject.SetActive(state);
    }
    private void OnDestroy()
    {
        FindObjectOfType<PlanetManager>().RefreshPlanetList();
    }
    public void SetAtmosphereColor(Color color)
    {
        AtmosphereMR.sharedMaterial.SetColor("_AtmosphereColor", color);
    }
}
