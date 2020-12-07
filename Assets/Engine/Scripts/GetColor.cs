using UnityEngine;
using System.Collections;

public class GetColor : MonoBehaviour
{

    private Camera cam;
    [SerializeField] private CanvasGroup canvasGroup;
    private GameObject sphere;
    [SerializeField] private GameObject Earth;
    void Start()
    {
        cam = Camera.main;
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position=new Vector3(100,100,100);
    }
    void Update()
    {
        if (!Input.GetMouseButton(0)||canvasGroup.alpha!=1)
            return;

        RaycastHit hit;
        if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
            return;

        Renderer rend = hit.transform.GetComponent<Renderer>();
        MeshCollider meshCollider = hit.collider as MeshCollider;
        if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
            return;

        sphere.transform.position = hit.point;
        sphere.transform.parent = Earth.transform;

        Texture2D tex = rend.material.mainTexture as Texture2D;
        Vector2 pixelUV = hit.textureCoord;
        //Debug.Log("XYY:::" + pixelUV);
        pixelUV.x *= tex.width;
        pixelUV.y *= tex.height;

      //  Debug.Log("X:::" + pixelUV.x + " Y::" + pixelUV.y);
        Color32 c;

        c = tex.GetPixel((int)pixelUV.x, (int)pixelUV.y);

        Debug.Log("color::" + c);
     //   Debug.Log(c.ToString());
    }



}

