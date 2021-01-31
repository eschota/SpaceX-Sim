using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class WindowLayerInfo : UIWindows
{
    public static WindowLayerInfo instance;

    [SerializeField] 
    private List<TMPro.TextMeshProUGUI> layersText;
    [SerializeField] 
    private List<TMPro.TextMeshProUGUI> layersLabels;
    [SerializeField]
    private UIWindRose windRose;
    [SerializeField]
    private string[] literals;

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

    private string GetLiteralValue(float normalizedValue)
    {
        for (int i = 0; i < literals.Length; i++)
        {
            float emin = (i - .5f) / (literals.Length - 1f);
            float max = (i + .5f) / (literals.Length - 1f);
            if (normalizedValue >= emin && normalizedValue < max)
            {
                return literals[i];
            }
        }
        return "!Non!";
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
        if (!country)
        {
            layersText[0].text = "No Country";
        }
        else
        {
            if (country.isOcean)
            {
                layersLabels[0].text = "Ocean:";
            }
            else
            {
                layersLabels[0].text = "Country:";
            }
            layersText[0].text = country.Name;
        }
        layersText[1].text = GetLiteralValue((country ? country.Wealth : 0f) / maxWealth);
        layersText[2].text = GetLiteralValue(WorldMapManager.instance.GetPercentByTexture(WorldMapManager.instance.GetTexture(PlaceInfoType.Population), uvCoords) / 100f);
        layersText[3].text = GetLiteralValue(WorldMapManager.instance.GetPercentByTexture(WorldMapManager.instance.GetTexture(PlaceInfoType.Science), uvCoords) / 100f);
        layersText[4].text = GetLiteralValue(WorldMapManager.instance.GetPercentByTexture(WorldMapManager.instance.GetTexture(PlaceInfoType.Transportation), uvCoords) / 100f);
        layersText[5].text = GetLiteralValue(WorldMapManager.instance.GetPercentByTexture(WorldMapManager.instance.GetTexture(PlaceInfoType.Disaster), uvCoords) / 100f);
        layersText[6].text = WorldMapManager.instance.ClimatZonesNames[WorldMapManager.instance.GetZone(WorldMapManager.instance.GetTexture(PlaceInfoType.Climat), uvCoords)];

        List<float> values = new List<float>();
        List<PlaceInfoType> types = new List<PlaceInfoType>();
        values.Add(1f - (country ? country.Wealth / maxWealth : 0));
        types.Add(PlaceInfoType.Wealth);
        values.Add(WorldMapManager.instance.GetPercentByTexture(WorldMapManager.instance.GetTexture(PlaceInfoType.Population), uvCoords) / 100f);
        types.Add(PlaceInfoType.Population);
        if (GameManager.CurrentState == GameManager.State.PlaySpace || GameManager.CurrentState == GameManager.State.CreateResearchLab)
        {
            values.Add(WorldMapManager.instance.GetPercentByTexture(WorldMapManager.instance.GetTexture(PlaceInfoType.Science), uvCoords) / 100f);
            types.Add(PlaceInfoType.Science);
        }
        if (GameManager.CurrentState == GameManager.State.PlaySpace || GameManager.CurrentState != GameManager.State.CreateResearchLab)
        {
            values.Add(WorldMapManager.instance.GetPercentByTexture(WorldMapManager.instance.GetTexture(PlaceInfoType.Transportation), uvCoords) / 100f);
            types.Add(PlaceInfoType.Transportation);
        }
        values.Add((100f - WorldMapManager.instance.GetPercentByTexture(WorldMapManager.instance.GetTexture(PlaceInfoType.Disaster), uvCoords)) / 100f);
        types.Add(PlaceInfoType.Disaster);
        float maxClimatZone = (WorldMapManager.instance.ClimatZonesNames.Count - 1f) / 2f;
        float normalizedClimat = 1f - Mathf.Abs(WorldMapManager.instance.GetZone(WorldMapManager.instance.GetTexture(PlaceInfoType.Climat), uvCoords) - maxClimatZone) / maxClimatZone;
        values.Add(normalizedClimat);
        types.Add(PlaceInfoType.Climat);
        float averageValue = 0;
        for (int i = 0; i < values.Count; i++)
        {
            float value = values[i];
            if (i == 0) value = 1f - value;
            averageValue += value;
        }
        averageValue /= values.Count;
        WorldMapManager.instance.currentPointValue = averageValue;
        windRose.UpdateValues(values.ToArray(), types.ToArray());
    }
}
