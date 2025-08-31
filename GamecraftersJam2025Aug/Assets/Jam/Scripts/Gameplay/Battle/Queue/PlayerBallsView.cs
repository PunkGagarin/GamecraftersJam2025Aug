using Jam.Scripts.Gameplay.Battle.Queue.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Jam.Scripts.Gameplay.Battle.Queue
{
    public class PlayerBallsView : MonoBehaviour
    {
        public int BallId { get; private set; }

        [field: SerializeField] 
        public Image Image { get; private set; }
        
        [field: SerializeField] 
        public TextMeshProUGUI DescriptionText { get; private set; }

        public void Init(BallDto dto)
        {
            BallId = dto.Id;
            Image.sprite = dto.Sprite;
            DescriptionText.text = dto.Description;
        }
        
        public void ShowDescription() => DescriptionText.gameObject.SetActive(true);

    }
}