using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UISelectModule : MonoBehaviour
{
    [SerializeField] UISelectModuleButton ButtonSelectModule;
    [SerializeField] TMPro.TMP_Dropdown DropdownByTypes;
    [SerializeField] CanvasGroup CG;
    private Research _research;
    public Research CurrentResearchSelected
    {
        get => _research;
        set
        {
            _research = value;
            if (value == null)
            {
                CG.alpha = 0;
            }
            else
            { 
                CG.alpha = 1;               
            }
        }
    }

    public static UISelectModule instance;
    public List<Module> DefaultModules = new List<Module>();
    public List<Module> CurrentShow = new List<Module>();
    void Awake()
    {
        DropdownByTypes.onValueChanged.AddListener(OnChange);
        instance = this;
        DefaultModules.AddRange(Resources.LoadAll("Modules/", typeof(Module)).Cast<Module>());
        Debug.Log("Modules Loaded: " + DefaultModules.Count);
    }
   public void AddModule(int id)
    {
        //CurrentResearchSelected.ModulesOpen.Add()
    }

    void OnChange(int id)
    {

        CurrentShow= DefaultModules.FindAll(X => (int)X.type == id);



    }

}
