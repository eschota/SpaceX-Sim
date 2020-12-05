using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBalance : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI balance;
    void Start()
    {
        Eco.EventChangeBalance += OnChange;
    }
    private void OnChange()
    {
        balance.text = Eco.Balance.ToString()+" M$";
    }
}
