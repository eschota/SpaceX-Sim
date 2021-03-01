using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click : MonoBehaviour
{
    [SerializeField]
    private LayerMask Layer;

    private List<GameObject> selectedObjects;

    [HideInInspector]
    public List<GameObject> selectebleObjects;
    private Vector3 mousePos1;
    private Vector3 mousePos2;

    void Awake()
    {
        selectedObjects = new List<GameObject>();
        selectebleObjects = new List<GameObject>();
    }

    void Start()
    {
        selectedObjects = new List<GameObject>();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ClearSelection();
        }

        if (Input.GetMouseButtonDown(0))
        {
            mousePos1 = Camera.main.ScreenToViewportPoint(Input.mousePosition);

            RaycastHit rayHit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayHit, Mathf.Infinity, Layer))
            {
                ClickOn clickOnScript = rayHit.collider.GetComponent<ClickOn>();

                if (Input.GetKey("left ctrl"))
                {
                    if (clickOnScript.currentSelected == false)
                    {
                        selectedObjects.Add(rayHit.collider.gameObject);
                        clickOnScript.currentSelected = true;
                        clickOnScript.ClickMe();
                    }
                    else
                    {
                        selectedObjects.Remove(rayHit.collider.gameObject);
                        clickOnScript.currentSelected = false;
                        clickOnScript.ClickMe();
                    }
                }
                else
                {
                    ClearSelection();
                    
                    selectedObjects.Add(rayHit.collider.gameObject);
                    clickOnScript.currentSelected = true;
                    clickOnScript.ClickMe();
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            mousePos2 = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            if (mousePos1 != mousePos2)
            {
                SelectObjects();
            }
        }
    }

    void SelectObjects()
    {
        List<GameObject> remObjects = new List<GameObject>();

        if (Input.GetKey("left ctrl") == false)
        {
            ClearSelection();
        }

        Rect selectRect = new Rect(mousePos1.x, mousePos1.y, mousePos2.x - mousePos1.x, mousePos2.y - mousePos1.y);

        foreach (GameObject selectObject in selectebleObjects)
        {
            if (selectObject != null)
            {
                if (selectRect.Contains(Camera.main.WorldToViewportPoint(selectObject.transform.position), true))
                {
                    selectedObjects.Add(selectObject);
                    selectObject.GetComponent<ClickOn>().currentSelected = true;
                    selectObject.GetComponent<ClickOn>().ClickMe();

                }
            }
            else
            {
                remObjects.Add(selectObject);
            }            
        }

        if (remObjects.Count > 0)
        {
            foreach(GameObject rem in remObjects)
            {
                selectebleObjects.Remove(rem);
            }

            remObjects.Clear();
        }
    }

    void ClearSelection()
    {
        if (selectedObjects.Count > 0)
        {
            foreach (GameObject obj in selectedObjects)
            {
                obj.GetComponent<ClickOn>().currentSelected = false;
                obj.GetComponent<ClickOn>().ClickMe();
            }

            selectedObjects.Clear();
        }
    }

}