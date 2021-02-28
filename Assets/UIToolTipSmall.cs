using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIToolTipSmall : MonoBehaviour
{
    [SerializeField] CanvasGroup CG;
    [SerializeField] ContentSizeFitter CSF;
    [SerializeField] RectTransform _rect;
    [SerializeField] TMPro.TextMeshProUGUI Text;
    [SerializeField] TMPro.TextMeshProUGUI CopyText;
    [SerializeField] AnimationCurve FadeCurve;
    [SerializeField] float TimeToFade;
    public static UIToolTipSmall instance;
    public List<ToolTip> ToolTips = new List<ToolTip>();
    public ToolTip CurrentToolTip
    {
        get => _CurrentToolTip;

        set
        {
            if (value == _CurrentToolTip) return;
            _CurrentToolTip = value;

            if (value == null) return;
            Canvas.ForceUpdateCanvases();
            CSF.enabled = false;
            Text.text = value.CustomText;
            CopyText.text = value.CustomText;
            CSF.enabled = true;
            Canvas.ForceUpdateCanvases();
        }
    }
    private ToolTip _CurrentToolTip;


    private void Awake()
    {
        instance = this;
       // CG.alpha = 0;
    }

    float timer;
    private void Update()
    {
        CurrentToolTip = FindToolTip();
        _rect.position = Input.mousePosition +new Vector3(Screen.width*0.02f,0,0);

        if (CurrentToolTip != null)
            timer+=Time.unscaledDeltaTime*TimeToFade;
        else
            timer -= Time.unscaledDeltaTime*TimeToFade;
        timer = Mathf.Clamp(timer, 0, 1);
        CG.alpha = FadeCurve.Evaluate(timer);


    }
    public ToolTip FindToolTip()// здесь обрабатывается клик и ищется зависимость рисерча
    {

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            pointerId = -1,
        };

        pointerData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);   
        if(results.Count>0)
            foreach (var item in results)
            {
                    if(ToolTips.Exists(X=>X.gameObject==item.gameObject))
                    return ToolTips.Find(X => X.gameObject == results[0].gameObject);
            }
        

        return null;
        
    }
}
