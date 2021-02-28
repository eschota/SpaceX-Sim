using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click : MonoBehaviour
{
    [SerializeField]
    private LayerMask Layer;

    private List<GameObject> selectedObjects;

    void Start()
    {
        selectedObjects = new List<GameObject>();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(1))
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

        if (Input.GetMouseButtonDown(0))
        {
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
                    if (selectedObjects.Count > 0)
                    {
                        foreach (GameObject obj in selectedObjects)
                        {
                            obj.GetComponent<ClickOn>().currentSelected = false;
                            obj.GetComponent<ClickOn>().ClickMe();
                        }

                        selectedObjects.Clear();
                    }
                    
                    selectedObjects.Add(rayHit.collider.gameObject);
                    clickOnScript.currentSelected = true;
                    clickOnScript.ClickMe();
                }
            }
        }
    }
}