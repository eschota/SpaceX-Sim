using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.UI;
using UnityEditor;


[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(RectTransform))]
public class UIState : MonoBehaviour
{

    [SerializeField] Core.State thisState;
    [SerializeField] RectTransform rect;
    [SerializeField] Image img;
    [SerializeField] Vector3 Pos1 = Vector3.one * 0.5f;
    [SerializeField] Vector3 Pos2 = Vector3.one * 0.5f;
    [SerializeField] Vector3 Rot1 = Vector3.zero;
    [SerializeField] Vector3 Rot2 = Vector3.zero;
    [SerializeField] Color32 Color1 = Color.white;
    [SerializeField] Color32 Color2 = new Color32(0, 0, 0, 0);

    [Range(0, 1)]
    public float CurrentLerp = 0;

    public virtual void OnValidate()
    {
        OnChange();
        if (rect == null) rect = GetComponent<RectTransform>();
        if (img == null) img = GetComponent<Image>();
        rect.anchoredPosition = Vector3.Lerp(Pos1, Pos2, CurrentLerp);
        rect.rotation = Quaternion.Euler(Vector3.Lerp(Rot1, Rot2, CurrentLerp));
        img.color = Color.Lerp(Color1, Color2, CurrentLerp);
    }

    public virtual void Reset()
    {
        if (rect == null) rect = GetComponent<RectTransform>();
        if (img == null) img = GetComponent<Image>();
        Pos1 = rect.anchoredPosition;
        Pos2 = rect.anchoredPosition;
        Color1 = img.color;
    }

    public virtual void OnChange()
    {

    }
    public virtual void Update()
    {
        if (!Application.isPlaying) return;
        if (thisState != Core.CurrentState)
        {
            rect.anchoredPosition = Vector3.Lerp(Pos1, Pos2, Core.EventTimer);
            rect.rotation = Quaternion.Euler(Vector3.Lerp(Rot1, Rot2, Core.EventTimer));
            img.color = Color.Lerp(Color1, Color2, CurrentLerp);
        }
        else
        {
            rect.anchoredPosition = Vector3.Lerp(Pos2, Pos1, 5 * Time.deltaTime);
            rect.rotation = Quaternion.Euler(Vector3.Lerp(Rot2, Rot1, Core.EventTimer));
            img.color = Color.Lerp(Color2, Color1, Core.EventTimer);
        }
    }
}