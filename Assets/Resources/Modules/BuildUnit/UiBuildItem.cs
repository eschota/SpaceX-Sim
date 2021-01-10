using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiBuildItem : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Button button;

    public Image IconImage => iconImage;
    public TextMeshProUGUI TitleText => titleText;
    public Button Button => button;
}