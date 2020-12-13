using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GetCountrieByColor : MonoBehaviour
{

    private Camera cam;
    [SerializeField] private CanvasGroup canvasGroup;
    
    [SerializeField] private GameObject Earth;
    private PoliticTextureSO politicSO;
    public static GameObject launchPlace;
    private LayerMask mask;
    private List<GuiCountryChoiceText> changeTextCountry;
    private GameObject launchPlacePrefab;
    private int fingerID = -1;
    void Start()
    {
        cam = Camera.main;
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        
        politicSO = Resources.Load<PoliticTextureSO>("PoliticTexture/BasePoliticMap");

        mask = LayerMask.GetMask("Earth");

        Earth = FindObjectOfType<UnitEarth>().gameObject;

        changeTextCountry=new List<GuiCountryChoiceText>();
        changeTextCountry.AddRange(FindObjectsOfType<GuiCountryChoiceText>());

     launchPlacePrefab= Resources.Load<GameObject>("UnitPoint/UnitPoint");

#if !UNITY_EDITOR
     fingerID = 0; 
#endif
    }

    void Update()
    {
        if (!Input.GetMouseButton(0))
            return;

        if (EventSystem.current.IsPointerOverGameObject(fingerID)) // is the touch on the GUI
        {
            return;
        }


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
                launchPlace = Instantiate(launchPlacePrefab);
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
            Debug.Log(ComparableColors(c)?.name);
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

        if (politicSO.CountrieSOs.Count > 0)
        {
            foreach (GuiCountryChoiceText choiceText in changeTextCountry)
            {
                choiceText.SetCountryToGUI(politicSO.CountrieSOs[indexColorAndCountry]);
            }
            
            return politicSO.CountrieSOs[indexColorAndCountry];
        }
        else
        {
            Debug.Log("NoCountrys");

        }

        return null;
    }


    public void BuildLaunchPlace()
    {
      
    }
}

