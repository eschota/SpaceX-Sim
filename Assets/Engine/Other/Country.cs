using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Country : MonoBehaviour
{
    [SerializeField] public Color ColorCountry;
    [SerializeField] public string Name;
    [SerializeField] public float Population;
    [SerializeField] public float Wealth;
    private bool _Hovered;
    public bool Hovered
    {
        get => _Hovered;
        set
        {
            if(value==true)
            gameObject.layer = 0;
            else
                gameObject.layer = 11;
            _Hovered = value;
            
        }
    }
    void Start()
    {
        ChangeColor();
        
    }
  

    void ChangeColor()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        // create new colors array where the colors will be created.
        Color[] colors = new Color[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
            colors[i] = ColorCountry;

        // assign the array of colors to the Mesh.
        mesh.colors = colors;
    }
    // Update is called once per frame
    void Update()
    {
        if (Hovered)
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * 1.00f,3* Time.unscaledDeltaTime);
        else
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * 1.0f,3* Time.unscaledDeltaTime);

    }


}
