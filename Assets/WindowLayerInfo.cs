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

        layersText[0].text = WorldMapManager.instance.CurrentHovered ? WorldMapManager.instance.CurrentHovered.Name : "No Country";
        layersText[1].text = WorldMapManager.instance.GetPercentByTexture(WorldMapManager.instance.WorldLayers[1], WorldMapManager.instance.HoveredEarthUVCoord).ToString()+"%"; 
        layersText[2].text = WorldMapManager.instance.GetPercentByTexture(WorldMapManager.instance.WorldLayers[2], WorldMapManager.instance.HoveredEarthUVCoord).ToString() + "%"; 
        layersText[3].text = WorldMapManager.instance.GetPercentByTexture(WorldMapManager.instance.WorldLayers[3], WorldMapManager.instance.HoveredEarthUVCoord).ToString() + "%"; 
        layersText[4].text = WorldMapManager.instance.GetPercentByTexture(WorldMapManager.instance.WorldLayers[4], WorldMapManager.instance.HoveredEarthUVCoord).ToString() + "%"; 
        layersText[5].text = WorldMapManager.instance.GetPercentByTexture(WorldMapManager.instance.WorldLayers[5], WorldMapManager.instance.HoveredEarthUVCoord).ToString() + "%";
        layersText[6].text = WorldMapManager.instance.ClimatZonesNames[WorldMapManager.instance.GetZone(WorldMapManager.instance.WorldLayers[6], WorldMapManager.instance.HoveredEarthUVCoord)];



    }

    
}
