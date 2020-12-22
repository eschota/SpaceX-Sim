using System;
using UnityEngine;

public class BuildCell : MonoBehaviour
{
    public int x, y;
    
    [SerializeField] private GameObject building;

    private Renderer _renderer;
    private bool _isSelected;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void SetSelection(bool selected)
    {
        _isSelected = selected;
        if (selected)
        {
            var color = Color.green;
            color.a = .5f;
            _renderer.material.SetColor("_Color", color);
        }
        else
        {
            var color = Color.white;
            color.a = 0f;
            _renderer.material.SetColor("_Color", color);
        }
    }
    
    public GameObject Building
    {
        get => building;
        set => building = value;
    }

    public bool IsSelected => _isSelected;
}