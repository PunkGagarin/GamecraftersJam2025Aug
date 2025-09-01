using System;
using DG.Tweening;
using Jam.Scripts.MapFeature.Map.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Jam.Scripts.MapFeature.Map.Presentation
{
    public class RoomNodePrefab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public Action<Room> OnRoomNodeClicked = delegate { };

        [SerializeField] private Image _image;
        [SerializeField] private RectTransform _rectTransform;
        public Room Room { private set; get; }
        public bool IsActive { get; set; }
        private Tween _hoverTween;
        private Vector3 _origScale;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!IsActive)
                return;
            _hoverTween?.Kill();
            var duration = 0.2f;
            _hoverTween = _rectTransform
                .DOScale(_origScale.x + 0.2f, duration)
                .SetEase(Ease.OutBack);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!IsActive)
                return;
            _hoverTween?.Kill();
            var duration = 0.2f;
            _hoverTween = _rectTransform
                .DOScale(_origScale, duration)
                .SetEase(Ease.OutBack);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!IsActive)
                return;
            OnRoomNodeClicked.Invoke(Room);
            var duration = 0.05f;
            _rectTransform
                .DOScale(_origScale.x - .1f, duration)
                .SetEase(Ease.InQuad)
                .OnComplete(() =>
                {
                    _rectTransform
                        .DOScale(_origScale.x + 0.2f, 0.15f)
                        .SetEase(Ease.OutBack)
                        .OnComplete(() => { _rectTransform.DOScale(_origScale, duration); });
                });
        }

        public void Setup(Room room, Vector2 pos, string nodeName, bool isLastFloor)
        {
            Room = room;
            SetIcon(room.MapIcon);
            SetPosition(pos);
            SetScale(isLastFloor);
            gameObject.SetActive(false);
            gameObject.name = nodeName;
        }

        private void SetScale(bool isLastFloor)
        {
            if (isLastFloor) 
                _rectTransform.localScale *= 2f;
            _origScale = _rectTransform.localScale;
        }

        private void SetPosition(Vector2 pos) =>
            _rectTransform.anchoredPosition = pos;

        private void SetIcon(Sprite sprite) =>
            _image.sprite = sprite;
    }
}