using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
[ExecuteInEditMode]
public class ModuleISS : Module
{
    public int Power = 100;
    public int FuelTaking = 1;







    //public SaveDataEngine SD;
    

    //public override void LoadJSON()
    //{
    //    SD = JsonUtility.FromJson<SaveDataEngine>(File.ReadAllText(JsonFilePath));
    //    ID = SD.ID;
    //    Name = SD.Name;
    //    ProductionTime = SD.ProductionTime;
    //    Power = SD.Power;
    //    FuelTaking = SD.FuelTaking;
    //    Cost = SD.Cost;
    //    Prefab = Resources.Load<GameObject>("Modules/" + SD.PrefabName);
    //    foreach (var item in ScenarioManager.instance.CurrentScenario.Researches)
    //    {
    //        if (item.SD.ModulesID.Exists(X => X == SD.ID))
    //        {
    //            item.ModulesOpen.Add(this);
    //            item.researchButton.Refresh();
    //            transform.SetParent(item.transform);
    //        }
    //    }
    //}


    //[System.Serializable]
    //public class SaveDataEngine: SaveData
    //{
        
    //    public int Cost;
    //    public int[] ProductionTime;
    //    public int Power;
    //    public int FuelTaking;
    //    public SaveDataEngine(string name, int id, string prefabName, int cost, int []productionTime, int fuelTaking, int power)
    //    {
    //        ID = id; Name = name; PrefabName = prefabName; Cost = cost; ProductionTime = productionTime; Power = power; FuelTaking = fuelTaking;
    //    }
    //} 
}
