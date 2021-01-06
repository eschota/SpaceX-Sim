using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonLaunchPlaceOk : MonoBehaviour
{
    public static CountrySO CurrentLauchPlace;
    [SerializeField] GameObject CancelButton;
    private GameObject UnitLaunchPrefab;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);


    }
    
    void OnClick()
    {

        if (WorldMapManager.instance.CurrenUnitPoint == null) { Alert.instance.AlertMessage = "Select Place First!!!";  return; }

            GameObject obj = Instantiate(Resources.Load<GameObject>("UnitPoint/UnitPoint"));

             
           
           

           
        
        obj.transform.parent = GameManager.UnitsAll.Find(X => X.GetType() == typeof(UnitEarth)).transform;
        obj.transform.position = WorldMapManager.instance.CurrenUnitPoint.transform.position;

        CameraManager.FlyToUnit = obj.transform;
        switch (GameManager.CurrentState)
        {

            case GameManager.State.CreateLaunchPlace:
                var unit = obj.AddComponent<UnitLaunchPlace>();
                GameManager.instance.OpenUnitScene(unit);
                break;
            case GameManager.State.CreateResearchLab:
                var unit2 = obj.AddComponent<UnitResearchLab>();
                GameManager.instance.OpenUnitScene(unit2);
                break;
            case GameManager.State.CreateProductionFactory:

                var unit3 = obj.AddComponent<UnitProductionFactory>();
                GameManager.instance.OpenUnitScene(unit3);
                break;
        }
            


    }
    
}
