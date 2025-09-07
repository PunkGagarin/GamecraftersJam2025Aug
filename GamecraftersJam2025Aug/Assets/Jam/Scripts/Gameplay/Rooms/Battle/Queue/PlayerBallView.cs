using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Jam.Scripts.Gameplay.Rooms.Battle.Queue
{
    public class PlayerBallView : MonoBehaviour
    {
        public int BallId { get; private set; }

        [field: SerializeField] 
        public Image Image { get; private set; }
        
        [field: SerializeField] 
        public TextMeshProUGUI DescriptionText { get; private set; }  
        
        [field: SerializeField] 
        public TextMeshProUGUI BallIdText { get; private set; }

        public void Init(BallDto dto)
        {
            BallId = dto.Id;
            Image.sprite = dto.Sprite;
            DescriptionText.text = dto.Description;
            BallIdText.text = dto.Id.ToString();
        }
        
        public void ShowDescription() => DescriptionText.gameObject.SetActive(true);
    }
}