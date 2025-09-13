using System;
using Jam.Scripts.Gameplay.Rooms.Battle.Queue;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Jam.Scripts.Gameplay.Rooms.Battle.ShellGame
{
    public class BoardBallView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public int BallId { get; private set; }

        [field: SerializeField]
        public SpriteRenderer Sprite { get; private set; }

        [field: SerializeField]
        public BallUnitType UnitType { get; private set; }

        [field: SerializeField]
        public TextMeshProUGUI IdText { get; private set; }

        public Action<string> OnEnter { get; set; } = delegate { };
        public Action OnExit { get; set; } = delegate { };
        
        private bool _hovering;
        private bool _isHoveringActive;
        
        public BallDto Dto { get; private set; }

        public void Init(BallDto ball)
        {
            Dto = ball;
            
            BallId = ball.Id;
            Sprite.sprite = ball.Sprite;

            if (IdText != null)
                IdText.text = ball.Id.ToString();
        }
        
        private bool IsPointerOverUI()
        {
            return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        }

        public void CleanUp()
        {
            BallId = 0;
            if (IdText != null)
                IdText.text = "None";
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnEnter.Invoke(Dto.Description);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnExit?.Invoke();
        }
        
        private void OnMouseEnter()
        {
            if (IsPointerOverUI() || !_isHoveringActive) return;        // если сверху UI, игнорируем
            _hovering = true;
            OnEnter.Invoke(Dto.Description);
        }

        private void OnMouseOver()
        {
            // если во время ховера всплыл UI поверх — считаем, что “вышли”
            if (_hovering && IsPointerOverUI())
            {
                _hovering = false;
                OnExit.Invoke();
            }
        }

        private void OnMouseExit() 
        {
            if (!_hovering) return;
            _hovering = false;
            OnExit.Invoke();
        }
        
        public void SetHoveringActive(bool isActive)
        {
            _isHoveringActive = isActive;
        }
    }
}