using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[ExecuteInEditMode]
public class UIResearchManager : MonoBehaviour
{
    
    [SerializeField] UIResearchButton button;
    [SerializeField] Arrow arrow;
    [SerializeField] List<UIResearchButton> buttons;

    [SerializeField] float DistanceArrows=100;
    List<Arrow> Arrows;
    void Update()
    {
    // if(Selection.activeGameObject==gameObject)    
            Rebuild();
         
    }
    void Rebuild()
    {
        List<ResearchSO> Researches = new List<ResearchSO>();
        Researches.AddRange(Resources.LoadAll<ResearchSO>("Researches"));
        List<UIResearchButton> tempButtons = new List<UIResearchButton>();
        tempButtons.AddRange(FindObjectsOfType<UIResearchButton>());
        for (int i = 0; i < tempButtons.Count; i++)
        {
            if (tempButtons[i] != button) DestroyImmediate(tempButtons[i].gameObject);
        }
         Arrows = new List<Arrow>();
        Arrows.AddRange(FindObjectsOfType<Arrow>());
        for (int i = 0; i < Arrows.Count; i++)
        {
            if (Arrows[i] != arrow) DestroyImmediate(Arrows[i].gameObject);
        }
        buttons = new List<UIResearchButton>();
        buttons.Clear();
        for (int i = 0; i < Researches.Count; i++)
        {
            buttons.Add(Instantiate(button, transform));
            buttons[buttons.Count - 1].Rect.position = Researches[i].position;
            buttons[buttons.Count - 1].text.text = Researches[i].Name;
            buttons[buttons.Count - 1].research = Researches[i];

        }
        for (int i = 0; i < Researches.Count; i++)
        {
            if (Researches[i].Dependances != null)
                if (Researches[i].Dependances.Length > 0)
                {
                    for (int j = 0; j < Researches[i].Dependances.Length; j++)
                    {
                        CreateLink(buttons[i].pivotStart.position, buttons.Find(X => X.research == Researches[i].Dependances[j]).pivotEnd.position);
                    }
                }
        }
    }
    void CreateLink(Vector2 start, Vector2 end)
    {
        float dis = Vector2.Distance(start, end);
        for (int i = 0; i < dis/DistanceArrows; i++)
        {
            Arrows.Add(Instantiate(arrow, transform));
            Arrows[Arrows.Count - 1].Rect.position =Vector2.Lerp(start,end, (float)i/(dis / DistanceArrows));
        }
    }
}
