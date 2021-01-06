using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[ExecuteInEditMode]
public class Module : Unit // главное это префаб модуля, по нему мы ищем и создаем необходимые ГО в игре 
{
    public enum ModuleType { None=-1, Engine=0, FirstStage=1, SecondStage=2, IssModule=3 }
    public ModuleType type;
    
     
    public int Cost = 1;
    private Sprite _icon;
    public Sprite Icon
    {
        get
        {
            if (_icon == null) _icon=GetIcon();
            return _icon;
        }
        set
        {
            _icon = value;
        }
    }

    public int[] ProductionTime = new int[] { 10,10,10};
    public string Description="Description";
    [SerializeField] public Camera _cam;
    
   public string IconFilePath
    {
     get =>   Application.dataPath + "/Resources/Modules/Icons/" + Prefab.name+".png" ;
    }

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
    public override void Update()
    {
        if (!Application.isPlaying)
            if (Selection.activeGameObject == gameObject)
            {
                foreach (var item in FindObjectsOfType<Module>())
                {
                    item._cam.gameObject.SetActive(false);
                }
                _cam.gameObject.SetActive(true);
                EditorWindow.GetWindow<SceneView>().Repaint();
            }
    }

    [ContextMenu("ChangeToSprite")]
    public void ChangeToSprite()
    {
        TextureImporter importer = AssetImporter.GetAtPath(IconFilePath) as TextureImporter;
        importer.textureType = TextureImporterType.Sprite;
        AssetDatabase.WriteImportSettingsIfDirty(IconFilePath);
    }
    [ContextMenu ("GetIcon")]
    public Sprite GetIcon()
    {
        return Resources.Load<Sprite>("Modules/Icons/"+Prefab.name);
       
    }


#if UNITY_EDITOR
    [ExecuteInEditMode]
    [CustomEditor(typeof(Module))]
    public class RenderCam : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Render Icon"))
            {
                Selection.activeGameObject.GetComponent<Module>().RenderIcon();
            }
        }
    }
#endif
    private void Reset()
    {
        gameObject.layer = 9;// Modules
        Name = gameObject.name;
        if (_cam == null) _cam = GetComponentInChildren<Camera>();
        Icon = Resources.Load<Sprite>("Modules/Icons/" + gameObject.name);
        _cam.targetTexture = Resources.Load<RenderTexture>("Modules/RT");

    }
    public void RenderIcon()
    {



        ScreenCapture.CaptureScreenshot(IconFilePath);
        AssetDatabase.Refresh();
        AssetDatabase.ImportAsset(IconFilePath);
        AssetDatabase.Refresh();
    }

}
