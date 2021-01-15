using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowLayerInfo : UIWindows
{
    public static WindowLayerInfo instance;

    [SerializeField] public List<TMPro.TextMeshProUGUI> layersText;
    void Awake() 
    {
        instance = this;
    }

    private void Update()
    {
        if (GameManager.CurrentState != GameManager.State.PlaySpace) return;
        layersText[0].text = WorldMapManager.instance.CurrentHovered ? WorldMapManager.instance.CurrentHovered.Name : "No Country";
        layersText[1].text = WorldMapManager.instance.CurrentHovered ?WorldMapManager.instance.CurrentHovered.Wealth +"$" : "N/A"; 
        layersText[2].text = WorldMapManager.instance.CurrentHovered? WorldMapManager.instance.CurrentHovered.Population.ToString() + "K" : "N/A"; 
        layersText[3].text = WorldMapManager.instance.GetPercentByTexture(WorldMapManager.instance.WorldLayersTextures[3], WorldMapManager.instance.HoveredEarthUVCoord).ToString() + "%"; 
        layersText[4].text = WorldMapManager.instance.GetPercentByTexture(WorldMapManager.instance.WorldLayersTextures[4], WorldMapManager.instance.HoveredEarthUVCoord).ToString() + "%"; 
        layersText[5].text = WorldMapManager.instance.GetPercentByTexture(WorldMapManager.instance.WorldLayersTextures[5], WorldMapManager.instance.HoveredEarthUVCoord).ToString() + "%";
        layersText[6].text = WorldMapManager.instance.ClimatZonesNames[WorldMapManager.instance.GetZone(WorldMapManager.instance.WorldLayersTextures[6], WorldMapManager.instance.HoveredEarthUVCoord)];



    }

    
}
