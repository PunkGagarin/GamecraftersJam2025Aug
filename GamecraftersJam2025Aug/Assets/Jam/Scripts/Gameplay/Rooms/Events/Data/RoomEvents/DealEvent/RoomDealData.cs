using System;
using Jam.Scripts.Gameplay.Rooms.Events.DamageRisk;
using Jam.Scripts.Gameplay.Rooms.Events.GoldRisk;
using Jam.Scripts.Gameplay.Rooms.Events.MaxHpIncreaseReward;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events
{
    [Serializable]
    [CreateAssetMenu(fileName = "RoomDealData", menuName = "Game Resources/RoomEvents/RoomDealData")]
    public class RoomDealData : ScriptableObject
    {
        [SerializeField] public string ActionDescKey;
        [SerializeReference] public RoomRewardEventData RewardType;
        [SerializeReference] public RoomRiskEventData RiskType;

        #region ContexMenu

        [ContextMenu("Risk/Add/BallRisk")]
        private void AddBallRiskToFirstItem() => RiskType = new BallRiskData();

        [ContextMenu("Risk/Add/GoldRisk")]
        private void AddGoldRiskToFirstItem() => RiskType = new GoldRiskData();

        [ContextMenu("Risk/Add/DamageRisk")]
        private void AddDamageRiskToFirstItem() => RiskType = new DamageRiskData();

        [ContextMenu("Risk/Add/MaxHpDecreaseRisk")]
        private void AddMaxHpDecreaseRiskToFirstItem() => RiskType = new MaxHpDecreaseRiskData();

        [ContextMenu("Rewards/Add/RandomBall")]
        private void AddRandomBallToFirstItem() => RewardType = new RandomBallRewardData();

        [ContextMenu("Rewards/Add/ConcreteBall")]
        private void AddConcreteBallToFirstItem() => RewardType = new ConcreteBallRewardData();

        [ContextMenu("Rewards/Add/GoldReward")]
        private void AddGoldAmountToFirstItem() => RewardType = new GoldRewardData();

        [ContextMenu("Rewards/Add/HealReward")]
        private void AddHealPercentToFirstItem() => RewardType = new HealRewardData();

        [ContextMenu("Rewards/Add/MaxHpIncreaseReward")]
        private void AddMaxHpIncreaseValueToFirstItem() => RewardType = new MaxHpIncreaseRewardData();

        #endregion
    }
}