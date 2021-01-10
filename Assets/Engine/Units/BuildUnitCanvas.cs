using UnityEngine;
using UnityEngine.UI;

public class BuildUnitCanvas : MonoBehaviour
{
    [SerializeField] private Button buildButton;
    [SerializeField] private Button deleteUnitYesButton;
    [SerializeField] private Button deleteUnitNoButton;
    
    [SerializeField] private GameObject deleteUnitPopup;
    [SerializeField] private Transform buildItemsContainer;
    [SerializeField] private UiBuildItem buildItemPrefab;

    public Button BuildButton => buildButton;
    public Button DeleteUnitYesButton => deleteUnitYesButton;
    public Button DeleteUnitNoButton => deleteUnitNoButton;
    public GameObject DeleteUnitPopup => deleteUnitPopup;
    public Transform BuildItemsContainer => buildItemsContainer;
    public UiBuildItem BuildItemPrefab => buildItemPrefab;
}