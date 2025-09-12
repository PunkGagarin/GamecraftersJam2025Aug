using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation
{
    public class DefaultRewardView : RewardView
    {
        [SerializeField]
        private RewardCardView _rewardCardView;

        public override void SetData(IRewardCardUiData cardData) => _rewardCardView.SetData(cardData);
    }
}