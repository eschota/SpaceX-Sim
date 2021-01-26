using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedManager : MonoBehaviour
{
    [SerializeField] Vector3 ScaleDown= Vector3.one*0.75f;
    [SerializeField] Image play;
    [SerializeField] Image pause;
    int lastid;
    public enum Speed {Stop=0, Normal=1, Fast=2,UltraFast=3  }
    public Speed LastSpeed;
    private Speed _CurrenSpeed;
    public Speed CurrenSpeed
    {
        get => _CurrenSpeed;
        set
        {
            if (LastSpeed != value && value != Speed.Stop) LastSpeed= value;
            for (int i = 0; i < transform.childCount; i++)
            {
                if ((int)value == i) transform.GetChild(i).localScale = Vector3.one;
                else transform.GetChild(i).localScale = ScaleDown;

                if ((int)value == 0)
                {
                    Time.timeScale = 0;
                }
                if ((int)value == 1)
                {
                    Time.timeScale = 1;
                }
                if ((int)value == 2)
                {
                    Time.timeScale = 5;
                }
                if ((int)value == 3)
                {
                    Time.timeScale = 30;
                }
            }
            Debug.Log("Speed Changed: " + value);
            _CurrenSpeed = value;
        }
    }
    public static SpeedManager instance;
    private void Start()
    {
        instance = this;
        for (int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).localScale = ScaleDown;
        }
     CurrenSpeed=Speed.Normal;


        GameManager.EventChangeState += OnChangeState;
    }
 
    void OnChangeState()
    {
        switch (GameManager.CurrentState)
        {
            case GameManager.State.MenuStartGame:
                CurrenSpeed = Speed.Stop;
                break;
            case GameManager.State.Pause:
                CurrenSpeed = Speed.Stop;
                break;
            case GameManager.State.MenuLoadGame:
                CurrenSpeed = Speed.Stop;
                break;
            case GameManager.State.PlaySpace:
                break;
            case GameManager.State.CreateLaunchPlace:
                CurrenSpeed = Speed.Stop;
                break;
            case GameManager.State.CreateResearchLab:
                CurrenSpeed = Speed.Stop;
                break;
            case GameManager.State.CreateProductionFactory:
                CurrenSpeed = Speed.Stop;
                break;
            case GameManager.State.PlayStation:
                break;
            case GameManager.State.PlayBase:
                break;
            case GameManager.State.ResearchGlobal:
                CurrenSpeed = Speed.Stop;
                break;
            case GameManager.State.EarthResearchLab:

                break;
            case GameManager.State.EarthProductionFactory:
                break;
            case GameManager.State.EarthLauchPlace:
                break;
            case GameManager.State.ScenarioEditorSelection:
                CurrenSpeed = Speed.Stop;
                break;
            case GameManager.State.Settings:
                CurrenSpeed = Speed.Stop;
                break;
            case GameManager.State.Save:
                CurrenSpeed = Speed.Stop;
                break;
            case GameManager.State.Load:
                CurrenSpeed = Speed.Stop;
                break;
            case GameManager.State.PlayEarth:
                break;
            case GameManager.State.ScenarioEditorGlobal:
                CurrenSpeed = Speed.Stop;
                break;
            case GameManager.State.StartGameSelectScenario:
                CurrenSpeed = Speed.Stop;
                break;
            case GameManager.State.CreateSeaLaunch:
                CurrenSpeed = Speed.Stop;
                break;
            case GameManager.State.Back:
                break;
            default:
                break;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
            if (Time.timeScale <1) CurrenSpeed = LastSpeed;
            else
            {

                CurrenSpeed = Speed.Stop;
            }

        if (Input.GetKeyDown(KeyCode.Alpha1)) CurrenSpeed = Speed.Normal;
        if (Input.GetKeyDown(KeyCode.Alpha2)) CurrenSpeed = Speed.Fast;
        if (Input.GetKeyDown(KeyCode.Alpha3)) CurrenSpeed = Speed.UltraFast;
    }

    public void Click(int id)
    {
        CurrenSpeed = (Speed)id;
    }
}
