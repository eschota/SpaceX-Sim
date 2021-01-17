using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;

public class WindowBuildingConstruction : MonoBehaviour 
{
    [SerializeField] ButtonBuilding baseButton;
    [SerializeField] UIWindows BuildingsPanel;

    private GameObject CurrentBuildingGameObject;
    private Unit _CurrentBuilding;
    public Unit CurrentBuilding
    {

        get => _CurrentBuilding;
        set
        {
            if (value == null)
            {
                _CurrentBuilding = null;
                return;
            }
            if (CurrentBuildingGameObject != null)
            {
                DestroyImmediate(CurrentBuildingGameObject);
                
            }
            
            CurrentBuildingGameObject = Instantiate(value.Prefab);
            _CurrentBuilding = value;
        }
    }
    List<ButtonBuilding> buttons = new List<ButtonBuilding>();

    public static WindowBuildingConstruction instance;
    void Start()
    {
        instance = this; 
        baseButton.gameObject.SetActive(false);
        BuildingsPanel.Hide();
    }

    private void Update()
    {
        ConstructPositioning();
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


                    if (Input.GetMouseButtonDown(0))
                    {
                        
                       Instantiate(CurrentBuilding).transform.position=target;
                        CurrentBuilding = null;                        
                        CurrentBuildingGameObject = null;
                        return;
                    }
            }
              
            CurrentBuildingGameObject.transform.position = Vector3.Lerp(CurrentBuildingGameObject.transform.position, target, Time.unscaledDeltaTime * 23);
        }
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
    public void GetBuildings()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            DestroyImmediate( buttons[i].gameObject);
        }
        buttons.Clear();
        
        foreach (var item in UnitManager.instance.buildingUnitPrefabs)
        {
            buttons.Add(Instantiate(baseButton, baseButton.transform.parent));
            buttons[buttons.Count - 1].building = item;
            buttons[buttons.Count - 1].icon.sprite= item.Icon;
            buttons[buttons.Count - 1].Name.text= item.Name;
            buttons[buttons.Count - 1].gameObject.SetActive(true);
        }
        BuildingsPanel.Show();
    }
}
