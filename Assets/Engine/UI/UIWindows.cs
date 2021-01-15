using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIWindows : MonoBehaviour
{

    private CanvasGroup _cg;
    public CanvasGroup CG
    {
        get {
                if (_cg == null) _cg = GetComponent<CanvasGroup>();
                return _cg;
            }
    }

    private Mode _mode;
    public Mode CurrentMode
    {
        get => _mode;
        set
        {
            _mode = value;
            switch (value)
            {
                case Mode.hide:
                    Hide();
                    break;
                case Mode.show:
                    Show();
                    break;
                case Mode.transparent:
                    Transparent();
                    break;
                default:
                    break;
            }

        }
    }
    public enum Mode {hide, show, transparent }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public virtual void Hide()
    {
        CG.alpha = 0;
        CG.interactable = false;
        CG.blocksRaycasts = false;
    }
    public virtual void Show()
    {
        CG.alpha = 1;
        CG.interactable = true;
        CG.blocksRaycasts = true;
    }
    public virtual void Transparent()
    {
        CG.alpha = 0.5f;
        CG.interactable = false;
        CG.blocksRaycasts = false;
    }
}
