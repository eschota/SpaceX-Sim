using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Module : Unit // главное это префаб модуля, по нему мы ищем и создаем необходимые ГО в игре 
{
    [HideInInspector]
    public string PrefabName="";
    public string PrefabPath="";
    
    public enum ModuleType { None=-1, Engine=0, FirstStage=1, SecondStage=2, IssModule=3,EarthModules =4}
    public ModuleType type;
    public bool isResearch = true;
    
    public int Cost = 1;
    private Sprite _icon;
    public Sprite Icon
    {
        get
        {
            _icon= Resources.Load<Sprite>("Modules/Icons/" + PrefabName);           
            if (_icon == null) Debug.LogError("Нет иконки! " + PrefabName);            
            return _icon;
        }
    }
    public string IconFilePath
    {
        get
        {
            
            if(PrefabName == "") Debug.LogError("не задано имя префаба, это критикал дружок"+ gameObject.name);
            
            return Application.dataPath + "/Resources/Modules/Icons/" + PrefabName + ".png";
        }
    }

    
    public int[] ProductionTime = new int[] { 10,10,10};
    public string Description="Description";
    [SerializeField] public Camera _cam;
    
  

    public override void Awake()
    {
        base.Awake();
      
    }
    public override void IniAfterJSONRead()
    {
        
        name =  "Module_ "+ Name ;
        ScenarioManager.instance.Modules.Add(this); 
    }
    public override void OnDestroy()
    {
        ScenarioManager.instance.Modules.Remove(this);
    }



    public virtual void OnValidate()
    { if (Application.isPlaying) return;
        string path = AssetDatabase.GetAssetPath(Prefab);
        PrefabPath = path.Substring(17,path.Length-24);
        PrefabName = Prefab.name;
    }
}
