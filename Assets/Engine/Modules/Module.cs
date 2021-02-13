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
            if (_icon == null)
            {
                Debug.LogError("Нет иконки! " + PrefabName);
                _icon = Resources.Load<Sprite>("UI/DefaultModuleIcon.png");
            }
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

    
    public int[] ProductionTime = new int[] { 30,30,30};
    public string Description="Description";
    [SerializeField] public Camera _cam;
    
  

    public override void Awake()
    {
        base.Awake();
      
    }
    public override void IniAfterJSONRead()
    {
        base.IniAfterJSONRead();
         
        ScenarioManager.instance.Modules.Add(this);
        transform.localPosition = localPosition * 0.1f;
    }
    public override void OnDestroy()
    {
        ScenarioManager.instance?.Modules.Remove(this);
    }



    public virtual void OnValidate()
    { 
        if (Application.isPlaying) return;

        if (Prefab.gameObject == this.gameObject) 
        {
            Prefab = null;
            Debug.LogError("ВЫДЕЛЕН ТОТ ЖЕ ПРЕФАБ ЧТО И ИСХОДНЫЙ!! ОЛОЛОЛО");
        }
        string path = AssetDatabase.GetAssetPath(Prefab);
        PrefabPath = path.Substring(17,path.Length-24);
        PrefabName = Prefab.name;

        if (Prefab && !Icon) 
        {
            Texture2D tex = AssetPreview.GetAssetPreview(Prefab);
            int tries = 0;
            while (!tex && tries < 1000)            
            {
                    if (!AssetPreview.IsLoadingAssetPreview(Prefab.GetInstanceID()))
                    {
                        tex = AssetPreview.GetAssetPreview(Prefab);
                        Debug.Log("PNG Generated: " + tex);
                        break;
                    }
                tries++;
            }

            if (tex != null)
             {
                if (System.IO.File.Exists(IconFilePath)) System.IO.File.Delete(IconFilePath);
                System.IO.File.WriteAllBytes(IconFilePath, tex.EncodeToPNG());
            }


        AssetDatabase.Refresh();
        }
    }
}
