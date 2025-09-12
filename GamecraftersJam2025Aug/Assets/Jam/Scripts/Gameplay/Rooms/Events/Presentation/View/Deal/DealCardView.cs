using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation
{
    public class DealCardView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<DealCardView, DealButtonData> OnClick;
        public event Action<DealCardView> OnMouseEnter;
        public event Action<DealCardView> OnMouseExit;

        [SerializeField] private TextMeshProUGUI _desc;
        [SerializeField] private RectTransform _rectTransform;

        private DealButtonData _data;
        private Vector3 originalScale;
        public float scaleMultiplier = 1.2f;

        private void Start() => originalScale = transform.localScale;

        public void SetData(DealButtonData data)
        {
            gameObject.SetActive(true);
            _data = data;
            SetDescription(_data.Description);
        }

        private void SetDescription(string dataDesc)
        {
            if (_data.Description == null) _desc.gameObject.SetActive(false);
            _desc.text = dataDesc;
        }

        public void ScaleUp() => transform.localScale = originalScale * scaleMultiplier;
        public void RestoreScale() => transform.localScale = originalScale;

        public void OnPointerClick(PointerEventData eventData) => OnClick?.Invoke(this, _data);
        public void OnPointerEnter(PointerEventData eventData) => OnMouseEnter?.Invoke(this);
        public void OnPointerExit(PointerEventData eventData) => OnMouseExit?.Invoke(this);
    }
}