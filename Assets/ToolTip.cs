using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTip : MonoBehaviour
{
    public ToolTipType CurrentType;
    public string CustomText;
    [SerializeField] public RectTransform thisRect;
    public enum ToolTipType { Small, Big, Module }

    private void Reset()
    {
        thisRect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        UIToolTipSmall.instance.ToolTips.Add(this);
    }
    private void OnDestroy()
    {
        if(UIToolTipSmall.instance.ToolTips.Contains(this))
        UIToolTipSmall.instance.ToolTips.Remove(this);
    }
}
