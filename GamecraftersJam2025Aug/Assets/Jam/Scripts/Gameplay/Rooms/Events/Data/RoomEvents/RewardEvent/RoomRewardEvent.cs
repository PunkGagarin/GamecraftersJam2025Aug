using System.Collections.Generic;
using Jam.Scripts.Gameplay.Rooms.Events.MaxHpIncreaseReward;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events
{
    [CreateAssetMenu(fileName = "RoomRewardEvent", menuName = "Game Resources/RoomEvents/RoomRewardEvent")]
    public class RoomRewardEvent : RoomEvent
    {
        [field: SerializeField] public RoomEventType Type { get; private set; } = RoomEventType.Reward;
        [field: SerializeField] public Sprite Sprite { get; private set; }

        [field: SerializeReference] public List<RoomRewardEventData> RewardsList { get; private set; }

        #region ContexMenu

        [ContextMenu("Rewards/Add/RandomBall")]
        private void AddRandomBall() => RewardsList.Add(new RandomBallRewardData());

        [ContextMenu("Rewards/Add/ConcreteBall")]
        private void AddConcreteBall() => RewardsList.Add(new ConcreteBallRewardData());

        [ContextMenu("Rewards/Add/GoldReward")]
        private void AddGoldAmount() => RewardsList.Add(new GoldRewardData());

        [ContextMenu("Rewards/Add/HealReward")]
        private void AddHealPercent() => RewardsList.Add(new HealRewardData());

        [ContextMenu("Rewards/Add/MaxHpIncreaseReward")]
        private void AddMaxHpIncreaseValue() => RewardsList.Add(new MaxHpIncreaseRewardData());

        #endregion
    }
}