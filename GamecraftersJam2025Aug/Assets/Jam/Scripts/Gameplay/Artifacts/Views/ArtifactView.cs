using Jam.Scripts.Gameplay.Rooms.Battle.Queue;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Jam.Scripts.Gameplay.Artifacts
{
    public class ArtifactView : MonoBehaviour
    {

        [field: SerializeField] 
        public Image Image { get; private set; }
        
        [field: SerializeField] 
        public TextMeshProUGUI DescriptionText { get; private set; }  
        
        [field: SerializeField] 
        public TextMeshProUGUI ArtifactTypeText { get; private set; }

        public void Init(ArtifactDto dto)
        {
            Image.sprite = dto.Sprite;
            // DescriptionText.text = dto.Description;
            ArtifactTypeText.text = dto.Type.ToString();
        }
    }
}