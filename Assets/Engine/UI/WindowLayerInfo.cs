using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowLayerInfo : UIWindows
{
    public static WindowLayerInfo instance;

    [SerializeField] public List<TMPro.TextMeshProUGUI> layersText;
    [SerializeField]
    private UIWindRose windRose;

    private float maxWealth = float.MaxValue;
    void Awake() 
    {
        instance = this;

        List<Country> countries = WorldMapManager.instance.countries;
        float maxWealth = 0;
        for (int i = 0; i < countries.Count; i++)
        {
            Country country = countries[i];
            if (country.Wealth > maxWealth) maxWealth = country.Wealth;
        }
        this.maxWealth = maxWealth;
    }

    private void Update()
    {
        if (!GameManager.AboveEarth) return;

        Country country;
        Vector2 uvCoords;
        if (GameManager.CurrentState == GameManager.State.PlaySpace)
        {
            country = WorldMapManager.instance.CurrentHovered;
            uvCoords = WorldMapManager.instance.HoveredEarthUVCoord;
        }
        else
        {
            country = WorldMapManager.instance.CurrentPointCountry;
            uvCoords = WorldMapManager.instance.SelectedEarthUVCoord;
        }
        layersText[0].text = country ? country.Name : "No Country";
        layersText[1].text = country ? country.Wealth +"$" : "N/A";
        layersText[2].text = country ? country.Population.ToString() + "K" : "N/A";
        layersText[3].text = WorldMapManager.instance.GetPercentByTexture(WorldMapManager.instance.WorldLayersTextures[3], uvCoords).ToString() + "%";
        layersText[4].text = WorldMapManager.instance.GetPercentByTexture(WorldMapManager.instance.WorldLayersTextures[4], uvCoords).ToString() + "%";
        layersText[5].text = WorldMapManager.instance.GetPercentByTexture(WorldMapManager.instance.WorldLayersTextures[5], uvCoords).ToString() + "%";
        layersText[6].text = WorldMapManager.instance.ClimatZonesNames[WorldMapManager.instance.GetZone(WorldMapManager.instance.WorldLayersTextures[6], uvCoords)];

        List<float> values = new List<float>();
        values.Add(1f - (country ? country.Wealth / maxWealth : 0));
        values.Add(WorldMapManager.instance.GetPercentByTexture(WorldMapManager.instance.WorldLayersTextures[3], uvCoords) / 100f);
        if (GameManager.CurrentState == GameManager.State.PlaySpace || GameManager.CurrentState == GameManager.State.CreateResearchLab)
        {
            values.Add(WorldMapManager.instance.GetPercentByTexture(WorldMapManager.instance.WorldLayersTextures[1], uvCoords) / 100f);
        }
        if (GameManager.CurrentState == GameManager.State.PlaySpace || GameManager.CurrentState != GameManager.State.CreateResearchLab)
        {
            values.Add(WorldMapManager.instance.GetPercentByTexture(WorldMapManager.instance.WorldLayersTextures[4], uvCoords) / 100f);
        }
        values.Add((100f - WorldMapManager.instance.GetPercentByTexture(WorldMapManager.instance.WorldLayersTextures[5], uvCoords)) / 100f);
        float maxClimatZone = (WorldMapManager.instance.ClimatZonesNames.Count - 1f) / 2f;
        float normalizedClimat = 1f - Mathf.Abs(WorldMapManager.instance.GetZone(WorldMapManager.instance.WorldLayersTextures[6], uvCoords) - maxClimatZone) / maxClimatZone;
        values.Add(normalizedClimat);
        float averageValue = 0;
        for (int i = 0; i < values.Count; i++)
        {
            float value = values[i];
            if (i == 0) value = 1f - value;
            averageValue += value;
        }
        averageValue /= values.Count;
        WorldMapManager.instance.currentPointValue = averageValue;
        windRose.UpdateValues(values.ToArray());
    }
}
