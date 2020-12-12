using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimation : MonoBehaviour
{
    [SerializeField] Image img;
    [SerializeField] Vector3 Axis= new Vector3(0,0,1);
    [SerializeField] float Speed=45;
    // Update is called once per frame
    private void Reset()
    {
        img = GetComponent<Image>();
    }
    void Update()
    {
        img.rectTransform.Rotate(Axis * Speed * Time.unscaledDeltaTime);
    }
}
