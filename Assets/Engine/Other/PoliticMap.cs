using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliticMap : MonoBehaviour
{
    [SerializeField] Texture2D PoliticMapTex;
    List<Color> UniqueColors = new List<Color>();
    void Start()
    {
        GetColors();
    }

    void GetColors()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
