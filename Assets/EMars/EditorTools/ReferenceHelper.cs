using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ReferenceHelper : MonoBehaviour
{
    [SerializeField] Color TransparencyColor ;
    private void Update()
    {
        if (true) ;
    }
    void OnGUI()
        {
            Event e = new Event();
            while (Event.PopEvent(e))
            {
            Vector3 lastPos = Camera.main.transform.position;
            Image Ref1 = GameObject.Find("Ref1").GetComponent<Image>();

            if (e.rawType == EventType.MouseDown && e.button == 0)
                {
                
                
                if (!Ref1) return;

                if (Ref1.color.a < 0.1f)
                { 
                    Ref1.color = TransparencyColor;
                }
                else Ref1.color = new Color(255, 255, 255, 0);
               
                //Ray ray = Camera.main.ScreenPointToRay(new Vector2(e.mousePosition.x, Screen.height - e.mousePosition.y));
                //RaycastHit hit;
                //if (Physics.Raycast(ray, out hit))
                //{

                //    Selection.activeGameObject = hit.collider.gameObject;
                //    Color C = Color.red;
                //    C.a = 0.33f;
                //    Gizmos.color = C;

                //    // Gizmos.DrawCube(Selection.activeGameObject.transform.position, Selection.activeGameObject.GetComponent<MeshRenderer>().bounds.size);

                //}
            }
            if (e.rawType == EventType.MouseDrag && e.button == 1)
            {
                if (!Ref1) return;

                Ref1.fillAmount = 1-(e.mousePosition.x / Screen.width);
                Ref1.color = new Color(255,255,255,2* (1-(e.mousePosition.y / Screen.height)));

            }

            if (e.rawType == EventType.MouseDown && e.button == 3)
            {
                Ref1.fillAmount = 0.1f;
            }
            if (e.rawType == EventType.Used && e.button ==2)
            {
                Ref1.fillAmount = 0.1f;
            } 
            if (e.type == EventType.ScrollWheel )
            {
                Ref1.fillAmount = 0.1f;
            }

                Camera.main.transform.Translate(Vector3.up, Space.World);
            Camera.main.transform.position = lastPos;
        }
        }
    }
