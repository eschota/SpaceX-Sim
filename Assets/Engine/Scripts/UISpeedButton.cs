using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpeedButton : MonoBehaviour
{
    [SerializeField] Vector3 ScaleDown= Vector3.one*0.75f;
    [SerializeField] Image play;
    [SerializeField] Image pause;

    private void Start()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).localScale = ScaleDown;
        }
        pause.gameObject.SetActive(false);
    }
    public void click (int id)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (id == i) transform.GetChild(i).localScale = Vector3.one;
            else transform.GetChild(i).localScale = ScaleDown;

            if (id == 0)
            {
                Time.timeScale = 1;
            }if (id == 1)
            {
                Time.timeScale = 10;
            }if (id == 2)
            {
                Time.timeScale = 60;
            }
        }
    }
     
}
