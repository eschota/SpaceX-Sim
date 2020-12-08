using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string Name;
    public int Days = 0;
    public int[] CreationDate;
    public virtual void Start()
    {
        CreationDate = new int [3];
        CreationDate[0] = TimeManager.Days; CreationDate[1] = TimeManager.Months; CreationDate[2]= TimeManager.Years;
    }


    public virtual void Update()
    {
        
    }
}
