using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowBuildingConstruction : MonoBehaviour
{
    [SerializeField] ButtonBuilding baseButton;
    [SerializeField] UIWindows BuildingsPanel;

    List<ButtonBuilding> buttons = new List<ButtonBuilding>();
    void Start()
    {
        baseButton.gameObject.SetActive(false);
        BuildingsPanel.Hide();
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
