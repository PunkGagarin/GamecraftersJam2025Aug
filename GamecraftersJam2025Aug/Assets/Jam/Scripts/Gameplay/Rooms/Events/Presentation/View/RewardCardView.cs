using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation
{
    public class RewardCardView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<RewardCardView, IRewardCardUiData> OnClick;
        public event Action<RewardCardView> OnMouseEnter;
        public event Action<RewardCardView> OnMouseExit;

        [SerializeField] private Image _rewardIcon;
        [SerializeField] private TextMeshProUGUI _rewardDesc;
        [SerializeField] private RectTransform _rectTransform;
        
        private IRewardCardUiData _data;
        
        private Vector3 originalScale;
        public float scaleMultiplier = 1.2f;

        private void Start() => originalScale = transform.localScale;
        
        public void SetData(IRewardCardUiData data)
        {
            gameObject.SetActive(true);
            _data = data;
            SetIcon(_data.Icon);
            SetDescription(_data.Desc);
        }

        private void SetDescription(string dataDesc)
        {
            if (_data.Desc == null) _rewardDesc.gameObject.SetActive(false);
            _rewardDesc.text = dataDesc;
        }

        private void SetIcon(Sprite dataIcon)
        {
            if (_data.Icon == null) _rewardIcon.gameObject.SetActive(false);
            _rewardIcon.sprite = dataIcon;
        }

        public void ScaleUp() => transform.localScale = originalScale * scaleMultiplier;
        public void RestoreScale() => transform.localScale = originalScale;

        public void OnPointerClick(PointerEventData eventData) => OnClick?.Invoke(this, _data);
        public void OnPointerEnter(PointerEventData eventData) => OnMouseEnter?.Invoke(this);
        public void OnPointerExit(PointerEventData eventData) => OnMouseExit?.Invoke(this);

    }
}