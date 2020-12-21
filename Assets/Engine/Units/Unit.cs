using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class Unit : MonoBehaviour
{
    
    public string Name;
    public Vector3 localPosition, localRotation, localScale;
    public string ParentName = "";
    public string Class;
    public int[] CreationDate;
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
    public virtual void OnDestroy()
    {
        GameManager.UnitsAll.Remove(this);
    }

    public virtual void Update()
    {
        
    }
    public virtual void Save()
    {
        string jsonData = JsonUtility.ToJson(this, true);
        string ScenarioPath = Application.streamingAssetsPath + "/" + ScenarioManager.instance.CurrentScenario.Name + "/";
        string ClassTypePath = GetType().ToString() + "/";
        if (!Directory.Exists(ScenarioPath)) Directory.CreateDirectory(ScenarioPath);
        if (!Directory.Exists(ScenarioPath + ClassTypePath)) Directory.CreateDirectory(ScenarioPath + ClassTypePath);
        File.WriteAllText(ScenarioPath + ClassTypePath +Name+ ".unit", jsonData);
        Debug.Log("File Saved at: " + ScenarioPath + ClassTypePath + ".unit");
    }
}
