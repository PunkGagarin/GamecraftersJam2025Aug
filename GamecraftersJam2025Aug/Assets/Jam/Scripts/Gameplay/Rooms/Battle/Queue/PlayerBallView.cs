using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Jam.Scripts.Gameplay.Rooms.Battle.Queue
{
    public class PlayerBallView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public int BallId { get; private set; }

        [field: SerializeField]
        public Image Image { get; private set; }

        [field: SerializeField]
        public TextMeshProUGUI DescriptionText { get; private set; }

        [field: SerializeField]
        public TextMeshProUGUI BallTypeText { get; private set; }

        [field: SerializeField]
        public TextMeshProUGUI BallGradeText { get; private set; }

        public BallDto Dto { get; private set; }
        
        public Action<string> OnEnter { get; set; } = delegate { };
        public Action OnExit { get; set; } = delegate { };

        public void Init(BallDto dto)
        {
            Dto = dto;
            BallId = dto.Id;
            Image.sprite = dto.Sprite;
            DescriptionText.text = dto.Description;
            BallTypeText.text = dto.Type.ToString();
            BallGradeText.text = dto.Grade.ToString();
        }
        
        public void ShowDescription() => DescriptionText.gameObject.SetActive(true);

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnEnter.Invoke(Dto.Description);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnExit?.Invoke();
        }
    }
}