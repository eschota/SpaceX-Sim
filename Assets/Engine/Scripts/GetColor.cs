using UnityEngine;
using System.Collections;

public class GetColor : MonoBehaviour
{

    private Camera cam;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField]
    private GameObject LaunchPlacePrefab;
    [SerializeField] private GameObject Earth;
    private Texture politicTexture;
    private UnitLaunchPlace launchPlace;
    private LayerMask mask;
    void Start()
    {
        cam = Camera.main;
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        launchPlace = Instantiate(LaunchPlacePrefab, new Vector3(100, 100, 100), Quaternion.identity).GetComponent<UnitLaunchPlace>();
        
        politicTexture=Resources.Load<PoliticTextureSO>("PoliticTexture/PoliticTexture").PoliticTexture;

        mask = LayerMask.GetMask("Earth");
    }
    void Update()
    {
        if (!Input.GetMouseButtonDown(0)||GameManager.CurrentState == GameManager.State.CreateLauchPlace)
            return;

        RaycastHit hit;
        if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition),out hit, 100, mask))
            return;

        
        if (politicTexture == null)
            return;

        launchPlace.transform.position = hit.point;
        launchPlace.transform.parent = Earth.transform;

        Texture2D tex = politicTexture as Texture2D;
        Vector2 pixelUV = hit.textureCoord;
        //Debug.Log("XYY:::" + pixelUV);
        pixelUV.x *= tex.width;
        pixelUV.y *= tex.height;

      //  Debug.Log("X:::" + pixelUV.x + " Y::" + pixelUV.y);
        Color32 c;

        c = tex.GetPixel((int)pixelUV.x, (int)pixelUV.y);

        Debug.Log("color::" + c);
        Debug.Log(tex.name);
     //   Debug.Log(c.ToString());
    }



}

