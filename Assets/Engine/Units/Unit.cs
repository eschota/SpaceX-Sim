using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
public class Unit : MonoBehaviour
{
    public int ID;
    public string Name;
    [HideInInspector]
    public Vector3 localPosition, localRotation, localScale;
    [HideInInspector]
    public string ParentName = "";
    [HideInInspector]
    public string Class;
    [HideInInspector]
    public int[] CreationDate;

    private string _filePath;
    public string FilePath
    {
        get => _filePath;
        set
        {
            if (value != "")
            {
                _filePath = value;
                LoadJSON();
            }
        }
    }
   
    public virtual void Ini()
    {
        //transform.position = unitSO.Position;
        //Name = unitSO.Name;
    }
    public virtual void Start()
    {
        Class = this.ToString();
        CreationDate = new int [3];
        CreationDate[0] = TimeManager.Days; CreationDate[1] = TimeManager.Months; CreationDate[2]= TimeManager.Years;
        localPosition = transform.localPosition;
        localRotation = transform.rotation.eulerAngles;
        localScale = transform.localScale;
       if(transform.parent!=null) ParentName = transform.parent.name;
        Debug.Log(string.Format("<color=green> Created new Unit:" + name + "</color>"));
        GameManager.UnitsAll.Add(this);
    }


    public virtual void Awake()
    {
        Name = name;
      
    }
    public virtual void OnDestroy()
    {
        GameManager.UnitsAll.Remove(this);
    }

    public virtual void Update()
    {
        
    }
    public virtual void SaveJSON()
    {
        ID = GetInstanceID();
        string jsonData = JsonUtility.ToJson(this, true); 
        File.WriteAllText(Path.Combine( ScenarioManager.instance.CurrentScenario.CurrentFolder, GetInstanceID() + ".unit"), jsonData);
        Debug.Log("File Saved at: "+ Path.Combine(ScenarioManager.instance.CurrentScenario.CurrentFolder, GetInstanceID() + ".unit"));
    }
    public virtual void LoadJSON()
    {
     
    }
    public class SaveData
    {
        public int ID;
        public string Name;
    }
}
