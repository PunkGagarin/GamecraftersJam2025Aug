using Jam.Scripts.UI;
using TMPro;
using UnityEngine;

namespace Jam.Scripts.Gameplay
{
    public class BallDescriptionUi : ContentUi
    {
    
        [field: SerializeField]
        public TextMeshProUGUI DescText { get; set; }
    
        public void SetDesc(string desc)
        {
            DescText.text = desc;
        }
    }
}
