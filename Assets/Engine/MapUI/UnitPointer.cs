using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPointer : MonoBehaviour
{
    [Header("Init")]
    [SerializeField]
    private MeshRenderer meshRenderer;

    [Header("Options")]
    [SerializeField]
    private Color allowedColor = Color.green;
    [SerializeField]
    private Color notAllowedColor = Color.red;

    private void Awake()
    {
        OnAllowedBuildChanged(WorldMapManager.instance.isAllowedToBuild);
        WorldMapManager.instance.OnAllowedBuildChanged += OnAllowedBuildChanged;
    }

    private void OnDestroy()
    {
        if (WorldMapManager.instance) WorldMapManager.instance.OnAllowedBuildChanged -= OnAllowedBuildChanged;
    }

    private void OnAllowedBuildChanged(bool allowed)
    {
        meshRenderer.material.SetColor("_EmissiveColor", allowed ? allowedColor : notAllowedColor);
    }
}
