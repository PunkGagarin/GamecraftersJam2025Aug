using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation
{
    public class DealCardView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<DealCardView, DealButtonData> OnClick;
        public event Action<DealCardView> OnMouseEnter;
        public event Action<DealCardView> OnMouseExit;

        [SerializeField] private TextMeshProUGUI _desc;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private RewardCardView _rewardCardView;
        [SerializeField] private RewardCardView _riskCardView;

        private DealButtonData _data;
        private Vector3 originalScale;
        public float scaleMultiplier = 1.2f;

        private void Start() => originalScale = transform.localScale;

        public void SetData(DealButtonData data)
        {
            gameObject.SetActive(true);
            _data = data;
            SetReward(data.Reward);
            SetRisk(data.Risk);
        }

        private void SetRisk(IRiskCardUiData dataRisk)
        {
            if (dataRisk == null) 
                _riskCardView.gameObject.SetActive(false);
            _riskCardView.SetData(dataRisk);
        }

        private void SetReward(IRewardCardUiData dataReward)
        {
            if (dataReward == null) 
                _rewardCardView.gameObject.SetActive(false);
            _rewardCardView.SetData(dataReward);
        }

        public void ScaleUp() => transform.localScale = originalScale * scaleMultiplier;
        public void RestoreScale() => transform.localScale = originalScale;

        public void OnPointerClick(PointerEventData eventData) => OnClick?.Invoke(this, _data);
        public void OnPointerEnter(PointerEventData eventData) => OnMouseEnter?.Invoke(this);
        public void OnPointerExit(PointerEventData eventData) => OnMouseExit?.Invoke(this);
    }
}