using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
[System.Serializable]
public class Unit : MonoBehaviour
{
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
    public int[] CreationDate;

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
   
    public virtual void Ini()
    { 
        
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
