using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[ExecuteInEditMode]
public class UIResearchManager : MonoBehaviour
{
    
    [SerializeField] UIResearchButton button;
    [SerializeField] Arrow arrow;
    [SerializeField] Transform PivotButtons;
    [SerializeField] Transform PivotArrows;
    [SerializeField] List<UIResearchButton> buttons;
    [SerializeField] private RectTransform CameraPivot;
    [SerializeField] float DistanceArrows=100;
    
    List<Arrow> Arrows;
    private float zoom = 0;
    private Vector3 startPos, target;
    Vector3 maxpos;
    List<Research> Researches = new List<Research>();
    void Update()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying) if (Selection.activeGameObject == null) Rebuild();
        else
        MouseControl();
#endif
    }
    public static UIResearchManager instance;
    void Awake()
    {
        instance = this;
        maxpos = -Vector3.one * 10000;
        foreach (var item in FindObjectsOfType<UIResearchButton>())
        {
            if (item.Rect.position.x > maxpos.x) maxpos = new Vector3(item.Rect.position.x, maxpos.y, 0);
            if (item.Rect.position.y > maxpos.y) maxpos = new Vector3(maxpos.x, item.Rect.position.y, 0);
        }
    }
    void MouseControl()
    {
        if(GameManager.CurrentState!=GameManager.State.ResearchGlobal) return;
        if (Input.GetMouseButtonDown(1)|| Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            target = CameraPivot.position;
        }
        if (Input.GetMouseButton(1)|| Input.GetMouseButtonDown(0)) CameraPivot.position = target+(Input.mousePosition-startPos);
        if (Input.GetMouseButtonUp(1)) ;

        CameraPivot.position = new Vector3(Mathf.Clamp(CameraPivot.position.x, 0, maxpos.x), Mathf.Clamp(CameraPivot.position.y, 0, maxpos.y), 0);
        zoom += 0.1f*Input.mouseScrollDelta.y;
        zoom= Mathf.Clamp(zoom, -0.5f, 0.5f);
        CameraPivot.localScale = Vector3.one *( 1 + zoom) ;
    }
   public void Rebuild()
    {
        Debug.Log("rebuild");
        Researches = new List<Research>();
        Researches.AddRange(Resources.LoadAll<Research>("Researches"));
        List<UIResearchButton> tempButtons = new List<UIResearchButton>();
        tempButtons.AddRange(FindObjectsOfType<UIResearchButton>());
        for (int i = 0; i < tempButtons.Count; i++)
        {
            if (tempButtons[i] != button) DestroyImmediate(tempButtons[i].gameObject);
        }
      
        buttons = new List<UIResearchButton>();
        buttons.Clear();
        for (int i = 0; i < Researches.Count; i++)
        {
            buttons.Add(Instantiate(button, PivotButtons));
            buttons[buttons.Count - 1].Rect.position = Researches[i].position;
            buttons[buttons.Count - 1].text.text = Researches[i].name;
            buttons[buttons.Count - 1].research = Researches[i];
            buttons[buttons.Count - 1].pivotStart.localPosition = new Vector3(Researches[i].pivotStart.x, Researches[i].pivotStart.y,0);
            buttons[buttons.Count - 1].pivotEnd.localPosition = new Vector3(Researches[i].pivotEnd.x, Researches[i].pivotStart.y,0);
            buttons[buttons.Count - 1].CostText.text = Researches[i].TimeCost.ToString();
            buttons[buttons.Count - 1].name = Researches[i].Name;
        }
        RebuildLinks();


    }
    public void RebuildLinks()
    {
        Arrows = new List<Arrow>();
        Arrows.AddRange(FindObjectsOfType<Arrow>());
        for (int i = 0; i < Arrows.Count; i++)
        {
            if (Arrows[i] != arrow) DestroyImmediate(Arrows[i].gameObject);
        }
        for (int i = 0; i < Researches.Count; i++)
        {
            if (Researches[i].Dependances != null)
                if (Researches[i].Dependances.Count > 0)
                {
                    for (int j = 0; j < Researches[i].Dependances.Count; j++)
                    {
                        CreateLink(buttons[i].pivotEnd.position, buttons.Find(X => X.research == Researches[i].Dependances[j]).pivotStart.position);
                    }
                }
        }
    }
    void CreateLink(Vector2 start, Vector2 end)
    {
        float dis = Vector2.Distance(start, end);
        for (int i = 1; i < dis/DistanceArrows-2; i++)
        {
            Arrows.Add(Instantiate(arrow, PivotArrows));
            Arrows[Arrows.Count - 1].Rect.position =Vector2.Lerp(start,end, (float)i/(dis / DistanceArrows));
        }
    }

#if UNITY_EDITOR
    [MenuItem("My Commands/Special Command %z")]
    static void SpecialCommand()
    {
        Research RSO = Selection.activeGameObject.GetComponent<UIResearchButton>().research;
        if (RSO.Dependances.Count > 0)
        {
            RSO.Dependances.RemoveAt(RSO.Dependances.Count - 1);
        
        Debug.Log("Remove Dependencies");
        }
        else
            Debug.Log("Все связи уже удалены, узбагойзя");

        FindObjectOfType<UIResearchManager>().RebuildLinks();
    }
    [MenuItem("My Commands/Special Command %x")]
    static void SpecialCommandx()
    {
         
            Debug.Log(Camera.current.ScreenToWorldPoint(Vector3.zero)+"    "+"mouse pos" +Input.mousePosition);
        
        FindObjectOfType<UIResearchManager>().RebuildLinks();
    } 
#endif


#if UNITY_EDITOR
   
#endif
}
