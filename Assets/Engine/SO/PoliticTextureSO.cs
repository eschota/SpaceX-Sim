using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PoliticTexture", menuName = "ScriptableObjects/PoliticTexture", order = 3)]

public class PoliticTextureSO : ScriptableObject
{
    public Texture2D PoliticTexture;
    public List<Color> CountrieColors;
    public List<CountrySO> CountrieSOs;
}
