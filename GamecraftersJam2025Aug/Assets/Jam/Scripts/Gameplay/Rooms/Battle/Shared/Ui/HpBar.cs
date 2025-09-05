using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{

    [field: SerializeField]
    public Image HpBarImage { get; private set; }

    public void SetHpBarFill(float fill)
    {
        HpBarImage.fillAmount = fill;
    }
}
