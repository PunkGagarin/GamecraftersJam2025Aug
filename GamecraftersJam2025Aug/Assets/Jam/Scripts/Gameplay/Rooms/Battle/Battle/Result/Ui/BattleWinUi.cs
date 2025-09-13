using System.Collections.Generic;
using Jam.Scripts.Gameplay.Rooms.Battle.Systems;
using Jam.Scripts.Gameplay.Rooms.Events.Presentation;
using Jam.Scripts.Gameplay.Rooms.Events.Presentation.WithGold;
using Jam.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Jam.Scripts.Gameplay.Rooms.Battle
{
    public class BattleWinUi : ContentUi
    {

        [field: SerializeField]
        public Button ToMapButton { get; private set; }

        [field: SerializeField]
        public Button UpgradeButton { get; private set; }

        [field: SerializeField]
        public HealWinButton HealButton { get; private set; }

        [field: SerializeField]
        public List<BallRewardWithGoldView> BallBuyViews { get; private set; }


        private bool _isCardSelected;

        private void Awake()
        {
            foreach (var ballView in BallBuyViews)
            {
                ballView.OnClick += OnBallSelected;
                ballView.OnMouseEnter += OnCardMouseEnter;
                ballView.OnMouseExit += OnCardMouseExit;
            }
        }

        public void InitWinData(WinDto winDto)
        {
            for (int i = 0; i < BallBuyViews.Count; i++)
            {
                BallBuyViews[i].gameObject.SetActive(true);
                BallBuyViews[i].SetData(winDto.Balls[i]);
                BallBuyViews[i].SetGoldAmount(winDto.Balls[i].GoldPrice);
            }

            HealButton.HealButton.interactable = true;
        }


        private void OnCardMouseExit(RewardCardView view)
        {
            if (!_isCardSelected && ((BallRewardWithGoldView)view).BackgroundButton.interactable)
                view.RestoreScale();
        }

        private void OnCardMouseEnter(RewardCardView view)
        {
            if (!_isCardSelected && ((BallRewardWithGoldView)view).BackgroundButton.interactable)
                view.ScaleUp();
        }

        private void OnBallSelected(RewardCardView view, ICardUiData data)
        {
            if (!_isCardSelected && ((BallRewardWithGoldView)view).BackgroundButton.interactable)
                view.RestoreScale();

            _isCardSelected = true;
        }

        private void HideNonSelectedCards(RewardCardView view)
        {
            foreach (var rewardCardView in BallBuyViews)
                rewardCardView.gameObject.SetActive(rewardCardView == view);
        }

        public void SetEnoughGoldForHeal(bool hasGoldForHeal)
        {
            HealButton.HealButton.interactable = hasGoldForHeal;
        }

        public void SetEnoughGoldForUpgrade(bool hasGoldForUpgrade)
        {
            UpgradeButton.interactable = hasGoldForUpgrade;
        }

        public void SetEnoughGoldToBuyBalls(bool hasGoldToBuyBall, bool hasGoldToBuySecondGrade)
        {
            foreach (BallRewardWithGoldView ball in BallBuyViews)
            {
                if (ball.IsFirstGrade())
                    ball.SetInteractable(hasGoldToBuyBall);
                else
                    ball.SetInteractable(hasGoldToBuySecondGrade);
            }
        }
    }
}