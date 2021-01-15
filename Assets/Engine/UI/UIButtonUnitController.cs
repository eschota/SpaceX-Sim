using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
public class UIButtonUnitController : MonoBehaviour
{
    UIButtonUnit[] units;

    private Unit SelectedUnit;

    [SerializeField] Button EnterCurrentUnit;
    [SerializeField] public List<UIButtonUnit> buttons;
    
    [SerializeField] public Transform UnitsGrid;
    public static UIButtonUnitController instance;
    void Start()
    {
        instance = this;
        EnterCurrentUnit.onClick.AddListener(OnClickEnter);
     
        EnterCurrentUnit.gameObject.SetActive(false);
    }
    public void OnClickEnter()
    {
        GameManager.instance.OpenUnitScene(SelectedUnit);
        CameraManager.FlyToUnit = SelectedUnit.transform;
    }

    public void ShowEnterButton(Unit unit, Vector3 pos)
    {
        SelectedUnit = unit;
        EnterCurrentUnit.transform.position = pos + Vector3.up * 80;
        EnterCurrentUnit.gameObject.SetActive(true);
    }

    public void HideEnterButton()
    {
        SelectedUnit = null;
        EnterCurrentUnit.gameObject.SetActive(false);
    }
}