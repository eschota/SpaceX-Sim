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
            _renderer.material.SetColor("_Color", Color.green);
        else
            _renderer.material.SetColor("_Color", Color.white);
    }
    
    public GameObject Building
    {
        get => building;
        set => building = value;
    }

    public bool IsSelected => _isSelected;
}