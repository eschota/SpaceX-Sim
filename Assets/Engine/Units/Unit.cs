using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
[System.Serializable]
public class Unit : MonoBehaviour
{
    [HideInInspector]
    public int ID;
     
    public string Name;
    [SerializeField] public GameObject Prefab;
    [HideInInspector]
    public Vector3 localPosition, localRotation, localScale;
    [HideInInspector]
    public string ParentName = "";
    [HideInInspector]
    public string Class;
    [HideInInspector]
    public int CreationDate;// дата создания юнита в игре 

    private string _JSONFilePath;
    public string JsonFilePath
    {
        get => _JSONFilePath;
        set
        {
            if (value != "")
            {
                _JSONFilePath = value;
               // LoadJSON(value);
            }
        }
    }
   
    public virtual void IniAfterJSONRead()
    {
        transform.SetParent(GameManager.instance.transform);
        name = Name;
        transform.localPosition = localPosition;
        transform.localRotation = Quaternion.Euler(localRotation);
    }
    public virtual void Start()
    {
        Class = this.ToString();
         
        CreationDate = TimeManager.TotalDays;
        localPosition = transform.localPosition;
        localRotation = transform.rotation.eulerAngles;
        localScale = transform.localScale;
       if(transform.parent!=null) ParentName = transform.parent.name;
        Debug.Log(string.Format("<color=green> Created new Unit:" + name + "</color>"));
        GameManager.UnitsAll.Add(this);
        
       
    }


    public virtual void Awake()
    {
        
    }

    
    public virtual void OnDestroy()
    {
       if( GameManager.UnitsAll.Contains(this) ) GameManager.UnitsAll.Remove(this);
    }

    public virtual void Update()
    {
        
    }
    public virtual void SaveJSON()
    {
        ID = GetInstanceID();
        string jsonData = JsonUtility.ToJson(this, true);
        Name = name;
        localPosition = transform.position;
        localRotation = transform.localRotation.eulerAngles;
        File.WriteAllText(Path.Combine( ScenarioManager.instance.CurrentScenario.CurrentFolder, ID+"."+ GetType().ToString() ), jsonData);
        Debug.Log("File Saved at: "+ Path.Combine(ScenarioManager.instance.CurrentScenario.CurrentFolder, ID + "." + GetType().ToString()));
    }
    public List<int> GetIDs(List<Unit> units)
    {
        List<int> IDs = new List<int>();
        foreach (var item in units)
        {
            IDs.Add(item.GetInstanceID());
        }

        return IDs;
    }
}
