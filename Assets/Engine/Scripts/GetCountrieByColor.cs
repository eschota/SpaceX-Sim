﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GetCountrieByColor : MonoBehaviour
{

    private Camera cam;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject LaunchPlacePrefab;
    [SerializeField] private GameObject Earth;
    private PoliticTextureSO politicSO;
    public static GameObject launchPlace;
    private LayerMask mask;

    void Start()
    {
        cam = Camera.main;
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        
        politicSO = Resources.Load<PoliticTextureSO>("PoliticTexture/PoliticTexture");

        mask = LayerMask.GetMask("Earth");

        Earth = FindObjectOfType<UnitEarth>().gameObject;

       

    }

    void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        if (GameManager.CurrentState == GameManager.State.CreateLauchPlace ||
            GameManager.CurrentState == GameManager.State.CreateProductionFactory ||
            GameManager.CurrentState == GameManager.State.CreateResearchLab)
        {
            RaycastHit hit;
            if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, 100, mask))
                return;


            if (politicSO == null)
                return;

            if (launchPlace == null)
            {
                launchPlace = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            }

            launchPlace.transform.position = hit.point;
            launchPlace.transform.parent = Earth.transform;

            Texture2D tex = politicSO.PoliticTexture as Texture2D;
            Vector2 pixelUV = hit.textureCoord;
            //Debug.Log("XYY:::" + pixelUV);
            pixelUV.x *= tex.width;
            pixelUV.y *= tex.height;

            //  Debug.Log("X:::" + pixelUV.x + " Y::" + pixelUV.y);
            Color32 c;

            c = tex.GetPixel((int) pixelUV.x, (int) pixelUV.y);

            Debug.Log("color::" + c);
            // Debug.Log(ComparableColors(c).name);
            //   Debug.Log(c.ToString());
        }
    }

    private CountrySO ComparableColors(Color colorUnderMouse)
    {
        int indexColorAndCountry = -1;
        for (int i = 0; i < politicSO.CountrieColors.Count; i++)
        {
            if (colorUnderMouse == politicSO.CountrieColors[i])
            {
                indexColorAndCountry = i;
            }
        }

        if (indexColorAndCountry > politicSO.CountrieSOs.Count || indexColorAndCountry == -1 ||
            politicSO.CountrieSOs[indexColorAndCountry] == null)

        {
            Debug.LogError("Country not found,Choose default");
            indexColorAndCountry = 0;
        }
        if(politicSO.CountrieSOs.Count>0)
        return politicSO.CountrieSOs[indexColorAndCountry];
        else
        {
            Debug.Log("NoCountry");

        }

        return null;
    }


    public void BuildLaunchPlace()
    {
      
    }
}

