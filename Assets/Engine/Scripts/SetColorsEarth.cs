using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SetColorsEarth : MonoBehaviour
{
    public static void SetColors()
    {
        PoliticTextureSO politicSO = Resources.Load<PoliticTextureSO>("PoliticTexture/PoliticTexture");
        Texture2D texture=politicSO.PoliticTexture as Texture2D;
        List<Color> allColors=new List<Color>();
        if (texture != null)
        {
            for (int i = 0; i < texture.width; i++)
            {
                for (int j = 0; j < texture.height; j++)
                {
                    allColors.Add(texture.GetPixel(i, j));
                }
            }
        }
        List<Color> uniqueColors=new List<Color>();
        uniqueColors.AddRange(allColors.Distinct().ToList());
        //for (int i = 0; i < allColors.Count; i++)
        //{
        //    if (!uniqueColors.Contains(allColors[i]))
        //    {
        //        uniqueColors.Add(allColors[i]);
        //    }
        //}

        politicSO.CountrieColors.Clear();
            //politicSO.CountrieColors.AddRange(allColors);
            for (int i = 0; i < 30; i++)
            {
                politicSO.CountrieColors.Add(uniqueColors[i]);
            }
    }
}
