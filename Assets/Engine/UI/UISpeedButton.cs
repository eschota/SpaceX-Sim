using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpeedButton : MonoBehaviour
{
    [SerializeField] Vector3 ScaleDown= Vector3.one*0.75f;
    [SerializeField] Image play;
    [SerializeField] Image pause;
    int lastid;
    private void Start()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).localScale = ScaleDown;
        }
        click(1);
    }
    public void click (int id)
    {

        if (lastid != id && id!=0) lastid = id;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (id == i) transform.GetChild(i).localScale = Vector3.one;
            else transform.GetChild(i).localScale = ScaleDown;

            if (id == 0)
            {
                Time.timeScale = 0;
            }if (id == 1)
            {
                Time.timeScale = 1;
            }if (id == 2)
            {
                Time.timeScale = 30;
            }if (id == 3)
            {
                Time.timeScale = 100;
            }
        }


        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
            if (Time.timeScale <1) click(lastid);
            else
            {
                
                click(0);
            }

        if (Input.GetKeyDown(KeyCode.Alpha1)) click(1);
        if (Input.GetKeyDown(KeyCode.Alpha2)) click(2);
        if (Input.GetKeyDown(KeyCode.Alpha3)) click(3);
    }
}
