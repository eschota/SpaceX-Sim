using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Module : Unit // главное это префаб модуля, по нему мы ищем и создаем необходимые ГО в игре 
{
    public enum ModuleType { None=-1, Engine=0, FirstStage=1, SecondStage=2, IssModule=3,EarthModules =4}
    public ModuleType type;
    
     
    public int Cost = 1;
    private Sprite _icon;
    public Sprite Icon
    {
        get 
        {
            if (_icon == null) _icon = Resources.Load<Sprite>("Modules/Icons/" + Prefab.name  );
            return _icon;
        }
    }
    public string IconFilePath
    {
        get => Application.dataPath + "/Resources/Modules/Icons/" + Prefab.name + ".png";
    }

    public int[] ProductionTime = new int[] { 10,10,10};
    public string Description="Description";
    [SerializeField] public Camera _cam;
    
  

    public override void Awake()
    {
        base.Awake();
        
    }
    public override void Ini()
    {
        name = Name = "Module_ " + Name;
        ScenarioManager.instance.Modules.Add(this); 
    }
    public override void OnDestroy()
    {
        ScenarioManager.instance.Modules.Remove(this);
    }
 
}
