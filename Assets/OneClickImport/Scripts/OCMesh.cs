
using UnityEngine;
[ExecuteInEditMode]
public class OCMesh  
{
    public string ocname;
    
    public bool castShadows;
    public bool receiveShadows;
    public bool renderable;
    public bool visibility;
    public int idx;
    public int paridx;
    public int meshidx;
    public string octype;
    public Vector3 nodepos;
    public Quaternion noderot;
    public Vector3 nodescale;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 dir;
    public int last;
    public MeshRenderer meshRenderer;
    public bool skiplm;
    
    public OCMesh(string ocname, bool castShadows, bool receiveShadows, bool renderable, bool visibility, int idx, int paridx, int meshidx, string octype, float[] nodepos, float[] noderot, float[] nodescale, float[] position, float[] rotation, float[] dir, int last, bool skiplm)
    {
        this.ocname = ocname;
        this.castShadows = castShadows;
        this.receiveShadows = receiveShadows;
        this.renderable = renderable;
        this.visibility = visibility;
        this.idx = idx;
        this.paridx = paridx;
        this.meshidx = meshidx;
        this.octype = octype;
        this.nodepos = new Vector3(-nodepos[0], nodepos[2], -nodepos[1]);
        
        this.noderot = new Quaternion(noderot[0], noderot[2], noderot[1], noderot[3]);
        //this.noderot = new Quaternion(noderot[0], noderot[2], noderot[1], noderot[3]);
        
        this.nodescale = new Vector3(nodescale[0], nodescale[1], nodescale[2]);
        this.position = new Vector3(position[0], position[2], position[1]);
      if(rotation!=null) this.rotation = new Quaternion(rotation[0], rotation[2],rotation[1],rotation[3]);
      if(dir!=null)  this.dir = new Vector3(dir[0], dir[1], dir[2]);
        this.last = last;
        this.skiplm= skiplm;

    } 
    //[SerializeField]bool enabl = true;
    //[ContextMenu("ChangeShader")]
    //void ChangeShader()
    //{
    //    meshRenderer = GetComponent<MeshRenderer>();
    //    if (meshRenderer != null)
    //    {
    //        if(enabl)
    //        foreach (var item in meshRenderer.sharedMaterials)
    //        {


    //                item.EnableKeyword("_EMISSION");
    //                item.EnableKeyword("_UseEmissionIntensity");
    //                item.SetColor("_EmissiveColor", Color.red * 300);
    //            item.SetFloat("_EmissiveExposureWeight", 0.1f);
    //                item.SetFloat("_EmissiveIntensity", 9.99f);
    //                item.SetColor("_EmissiveColorLDR", Color.red * 3f);
    //                item.globalIlluminationFlags = MaterialGlobalIlluminationFlags.AnyEmissive;

    //                item.SetColor("_Color", Color.green);
    //                Debug.Log("EnableKeyword");
    //                Canvas.ForceUpdateCanvases();
    //                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    //                EditorUtility.SetDirty(item);
    //                enabl = false;
    //                DynamicGI.UpdateEnvironment();
    //            }
    //        else
    //        {
    //            foreach (var item in meshRenderer.sharedMaterials)
    //            {
    //                item.DisableKeyword("_EMISSION");
    //                item.DisableKeyword("_UseEmissionIntensity");
    //                item.SetColor("_EmissiveColor", Color.blue * 100);
    //                item.SetFloat("_EmissiveExposureWeight", 0.2f);
    //                item.SetColor("_Color", Color.blue);

    //                //m.SetFloat("_EmissionIntensity", 700f); 
    //                //m.SetColor("_EmissiveColorLDR", item.selfIllumination); 
    //                item.DisableKeyword("_EMISSION");
    //                Debug.Log("Disable");
    //                enabl = true;
    //                DynamicGI.UpdateEnvironment();
    //            }
    //        }
    //    }
    //}
}
