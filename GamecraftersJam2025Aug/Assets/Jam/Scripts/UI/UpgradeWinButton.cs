using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeWinButton : MonoBehaviour
{
    [field: SerializeField] public Button UpgradeButton { get; private set; }

    [field: SerializeField] public TextMeshProUGUI PriceText { get; private set; }

    [field: SerializeField] public Image DisabledBg { get; private set; }

    public void SetInteractable(bool isInteractable)
    {
        DisabledBg.gameObject.SetActive(!isInteractable);
        UpgradeButton.interactable = isInteractable;
    }
}