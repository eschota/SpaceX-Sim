using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;

public class UIUnitManager : MonoBehaviour 
{
    [SerializeField] ButtonBuilding SelectUnitExampleButton;
    [SerializeField] public UIWindows BuildingsPanel;
    [SerializeField] public UIWindows WindowSelectUnit;
    List<EarthCollider> colliders = new List<EarthCollider>();
    private Selectable CurrentBuildingGameObject;
    private Unit _CurrentBuilding;
    public Unit CurrentBuilding
    {

        get => _CurrentBuilding;
        set
        {
            if (value == null)
            {
                _CurrentBuilding = value;
                return;
            }
            if (CurrentBuildingGameObject != null)
            {
                DestroyImmediate(CurrentBuildingGameObject);
                
            }
            
            CurrentBuildingGameObject = Instantiate(value.Prefab).GetComponent<Selectable>();
            _CurrentBuilding = value;
        }
    }
    List<ButtonBuilding> buttons = new List<ButtonBuilding>();

    public static UIUnitManager instance;
    void Start()
    {
        colliders.AddRange(FindObjectsOfType<EarthCollider>());
        instance = this; 
        SelectUnitExampleButton.gameObject.SetActive(false);
        WindowSelectUnit.CurrentMode = UIWindows.Mode.hide;
        BuildingsPanel.Hide();
    }

    private void Update()
    {
        ConstructPositioning();
        Deselection();
        
    }
    
    void Deselection()
    {
        if(Input.GetMouseButtonDown(1))
        {if(CurrentBuildingGameObject!=null)
            Destroy(CurrentBuildingGameObject.gameObject);
            CurrentBuilding = null;
            BuildingsPanel.CurrentMode = UIWindows.Mode.hide;
        }
    }
    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;

        return results.Count >0;
    }
    void ConstructPositioning()
    {

        if(!IsPointerOverUIObject())
        if (CurrentBuilding != null)
        {
            //bool noUIcontrolsInUse = EventSystem.current.currentSelectedGameObject == null;

            Vector3 target = CurrentBuildingGameObject.transform.position;
            RaycastHit hit;
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit))
            {
                target = new Vector3(hit.point.x, 0, hit.point.z);
                target = new Vector3(100 * (Mathf.RoundToInt(target.x / 100f)), 0, 100 * (Mathf.RoundToInt(target.z / 100f)));

                    if (!isObjectOverOther(target))
                    {
                        CurrentBuildingGameObject.ChangeMatsCancel();
                    }
                    else
                    {
                        CurrentBuildingGameObject.ChangeMats(false);
                        if (Input.GetMouseButtonDown(0))
                        {

                            UnitManager.instance.PlaceBuilding(CurrentBuilding as BuildingUnit, CurrentBuildingGameObject.gameObject, target, Vector3.zero);
                            CurrentBuilding.transform.SetParent(GameManager.instance.BuildingsTransform);
                            (CurrentBuilding as BuildingUnit). ConsctructionProcess = 1;
                            GameManager.Buildings.Add(CurrentBuilding as BuildingUnit);
                            CurrentBuilding = null;

                            CurrentBuildingGameObject = null;
                            return;
                        }
                    }
            }
              
         //   CurrentBuildingGameObject.transform.position = Vector3.Lerp(CurrentBuildingGameObject.transform.position, target, Time.unscaledDeltaTime * 23);
            CurrentBuildingGameObject.transform.position =  target ;
        }
    }
    bool isObjectOverOther(Vector3 pos)
    {
        foreach (var item in UnitManager.instance.Selectables)
        {
            if(item!=CurrentBuildingGameObject)
            if (Vector3.Distance(pos, item.transform.position) < 50) return false;
        } foreach (var item in colliders)
        {
            if (Vector3.Distance(pos, item.transform.position) < 50) return false;
        }
        return true;
    }
    public bool inside(Vector2 bounds)
    {
        if (Input.mousePosition.x > Screen.width * bounds.x)
            if (Input.mousePosition.x < Screen.width * (1 - bounds.x))
                if (Input.mousePosition.y > Screen.height * bounds.y)
                    if (Input.mousePosition.y < Screen.height * (1 - bounds.y))
                        return true;
        return false;
    }
    public void ShowSelectBuildingPanel()
    {

        if (UnitManager.instance.CurrentState==UnitManager.State.PlaceBuilding)
        {
            BuildingsPanel.CurrentMode = UIWindows.Mode.hide;
            UnitManager.instance.CurrentState = UnitManager.State.None;
            SpeedManager.instance.CurrenSpeed = SpeedManager.instance.LastSpeed;
            return;
        }
        for (int i = 0; i < buttons.Count; i++)
        {
            DestroyImmediate( buttons[i].gameObject);
        }
        buttons.Clear();
        
        foreach (var item in UnitManager.instance.buildingUnitPrefabs)
        {
            buttons.Add(Instantiate(SelectUnitExampleButton, SelectUnitExampleButton.transform.parent));
            buttons[buttons.Count - 1].building = item;
            buttons[buttons.Count - 1].icon.sprite= item.Icon;
            buttons[buttons.Count - 1].Name.text= item.Name;
            buttons[buttons.Count - 1].gameObject.SetActive(true);
        }
        BuildingsPanel.CurrentMode = UIWindows.Mode.show;
        UnitManager.instance.CurrentState = UnitManager.State.PlaceBuilding;

    }
    public void HideSelectBuildingPanel()
    {

    }
}
