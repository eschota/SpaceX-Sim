using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiProductionButton : MonoBehaviour
{
    public BuildingProductionFactory ProductionFactory;
   [SerializeField] public Button ButtonAddThisLabToResearch;


    private void Awake()
    {
      //  ButtonAddThisLabToResearch.onClick.AddListener(OnClick);
    }


    void OnClick()
    {
        //UIResearchManager.instance.AddLabToResearch(ProductionFactory);
        Debug.Log("Clicked: Add  Lab To Research :" + name);
    }

    private void OnDestroy()
    {
      
     //   ButtonAddThisLabToResearch.onClick.RemoveListener(OnClick);

    }
}
