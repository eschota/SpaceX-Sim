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

        layersText[0].text = WorldMapManager.instance.CurrentHovered ? WorldMapManager.instance.CurrentHovered.name : "No Country";
        layersText[1].text = GetPercentByTexture(WorldMapManager.instance.WorldLayers[1], WorldMapManager.instance.HoveredEarthUVCoord).ToString()+"%"; 
        layersText[2].text = GetPercentByTexture(WorldMapManager.instance.WorldLayers[2], WorldMapManager.instance.HoveredEarthUVCoord).ToString() + "%"; 
        layersText[3].text = GetPercentByTexture(WorldMapManager.instance.WorldLayers[3], WorldMapManager.instance.HoveredEarthUVCoord).ToString() + "%"; 
        layersText[4].text = GetPercentByTexture(WorldMapManager.instance.WorldLayers[4], WorldMapManager.instance.HoveredEarthUVCoord).ToString() + "%"; 
        layersText[5].text = GetPercentByTexture(WorldMapManager.instance.WorldLayers[5], WorldMapManager.instance.HoveredEarthUVCoord).ToString() + "%"; 
        layersText[6].text = GetPercentByTexture(WorldMapManager.instance.WorldLayers[6], WorldMapManager.instance.HoveredEarthUVCoord).ToString() + "%"; 
        
       
    }

    int  GetPercentByTexture(Texture2D tex, Vector2 uv)
    {
        Color col=tex.GetPixel(Mathf.RoundToInt(uv.x * tex.width), Mathf.RoundToInt(uv.x * tex.height));
        return Mathf.RoundToInt( col.r*100);
    }
}
