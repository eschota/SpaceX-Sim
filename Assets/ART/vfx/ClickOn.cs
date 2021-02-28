using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOn : MonoBehaviour
{
    [SerializeField]
    private Material red;
    [SerializeField]
    private Material green;

    private MeshRenderer myRend;

    [HideInInspector]
    public bool currentSelected = false;

    void Start()
        
    {
        myRend = GetComponent<MeshRenderer>();
        ClickMe();
    }

    public void ClickMe()
    {
        if (currentSelected == false)
        {
            myRend.material = red;
        }
        else
        {
            myRend.material = green;
        }
    }
}
