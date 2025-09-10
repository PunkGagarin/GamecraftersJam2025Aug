using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation
{
    public class RandomRewardView : RewardView
    {
        public event Action<RandomBallRewardCardUiData, BallRewardCardUiData> OnRandomBallSelected;

        [SerializeField] private List<RewardCardView> _rewardCards;

        private RandomBallRewardCardUiData _data;
        private bool _isCardSelected;

        public override void SetData(IRewardCardUiData data)
        {
            if (data is RandomBallRewardCardUiData cardData)
            {
                _data = cardData;
                for (var i = 0; i < cardData.RewardCards.Count; i++)
                {
                    _rewardCards[i].SetData(cardData.RewardCards[i]);
                    _rewardCards[i].OnClick += OnBallSelected;
                    _rewardCards[i].OnMouseEnter += OnCardMouseEnter;
                    _rewardCards[i].OnMouseExit += OnCardMouseExit;
                }
            }
        }

        private void OnCardMouseExit(RewardCardView view)
        {
            if (!_isCardSelected)
                view.RestoreScale();
        }

        private void OnCardMouseEnter(RewardCardView view)
        {
            if (!_isCardSelected)
                view.ScaleUp();
        }

        private void OnBallSelected(RewardCardView view, IRewardCardUiData data)
        {
            if (!_isCardSelected)
            {
                view.RestoreScale();
                foreach (var rewardCardView in _rewardCards)
                    rewardCardView.gameObject.SetActive(rewardCardView == view);
                OnRandomBallSelected?.Invoke(_data, data as BallRewardCardUiData);
            }
            _isCardSelected = true;
        }

        private void OnDestroy()
        {
            foreach (var rewardCardView in _rewardCards)
            {
                rewardCardView.OnClick -= OnBallSelected;
                rewardCardView.OnMouseEnter -= OnCardMouseEnter;
                rewardCardView.OnMouseExit -= OnCardMouseExit;
            }
        }
    }
}