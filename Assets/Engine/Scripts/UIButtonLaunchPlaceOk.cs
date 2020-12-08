using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonLaunchPlaceOk : MonoBehaviour
{
    public static CountrySO CurrentLauchPlace;
    [SerializeField] GameObject CancelButton;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
        GameManager.EventChangeState += OnChange;
    }
    void OnChange()
    {
        if (GameManager.CurrentState == GameManager.State.CreateLauchPlace)
        {
            //if (FindObjectOfType<UnitLaunchPlace>()==null) CancelButton.gameObject.SetActive(false);
            //else CancelButton.gameObject.SetActive(true);
        }
    }
    void OnClick()
    {
        if (CurrentLauchPlace != null)
        {
            if (GetCountrieByColor.launchPlaces.transform.position != new Vector3(100, 100, 100))
            {
                GameManager.CreateLaunchPlace(CurrentLauchPlace, GetCountrieByColor.launchPlaces);
            }
            else
            {
                Debug.Log("Chose point");

            }
        }
        else Alert.instance.AlertMessage = "Select Launch Place First!!!";
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
