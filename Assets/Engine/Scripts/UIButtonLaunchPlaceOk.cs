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
            if (GameManager.LaunchPlaces.Count == 0) CancelButton.gameObject.SetActive(false);
            else CancelButton.gameObject.SetActive(true);
        }
    }
    void OnClick()
    {
        if (CurrentLauchPlace != null)
            GameManager.CreateLaunchPlace(CurrentLauchPlace);
        else Alert.instance.AlertMessage = "Select Launch Place First!!!";
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
