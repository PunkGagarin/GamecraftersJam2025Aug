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

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!IsActive)
                return;
            _hoverTween?.Kill();
            _hoverTween = _rectTransform
                .DOScale(1.2f, 0.2f)
                .SetEase(Ease.OutBack);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!IsActive)
                return;
            _hoverTween?.Kill();
            _hoverTween = _rectTransform
                .DOScale(1f, 0.2f)
                .SetEase(Ease.OutBack);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!IsActive)
                return;
            OnRoomNodeClicked.Invoke(Room);
            _rectTransform
                .DOScale(0.9f, 0.05f)
                .SetEase(Ease.InQuad)
                .OnComplete(() =>
                {
                    _rectTransform
                        .DOScale(1.2f, 0.15f)
                        .SetEase(Ease.OutBack)
                        .OnComplete(() => { _rectTransform.DOScale(1f, 0.05f); });
                });
        }

        public void Setup(Room room, Vector2 pos, string nodeName)
        {
            Room = room;
            SetIcon(room.MapIcon);
            SetPosition(pos);
            gameObject.SetActive(false);
            gameObject.name = nodeName;
        }

        private void SetPosition(Vector2 pos) =>
            _rectTransform.anchoredPosition = pos;

        private void SetIcon(Sprite sprite) =>
            _image.sprite = sprite;
    }
}