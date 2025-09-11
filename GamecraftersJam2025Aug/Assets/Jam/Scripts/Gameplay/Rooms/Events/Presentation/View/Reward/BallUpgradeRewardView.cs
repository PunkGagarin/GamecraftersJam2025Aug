using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation
{
    public class BallUpgradeRewardView : RewardView
    {
        [SerializeField] private RewardCardView _prevBallView;
        [SerializeField] private RewardCardView _newBallView;

        public override void SetData(IRewardCardUiData cardData)
        {
            if (cardData is BallUpgradeRewardCardUiData data)
            {
                //todo
            }
        }
    }
}