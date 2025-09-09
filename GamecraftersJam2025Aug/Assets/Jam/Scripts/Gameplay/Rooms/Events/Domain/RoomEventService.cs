using System;
using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Gameplay.Inventory.Models;
using Jam.Scripts.Gameplay.Inventory.Models.Definitions;
using Jam.Scripts.Gameplay.Rooms.Events.DamageRisk;
using Jam.Scripts.Gameplay.Rooms.Events.Data;
using Jam.Scripts.Gameplay.Rooms.Events.GoldRisk;
using Jam.Scripts.Gameplay.Rooms.Events.MaxHpIncreaseReward;
using Jam.Scripts.Gameplay.Rooms.Events.Presentation;
using Jam.Scripts.MapFeature.Map.Data;
using ModestTree;
using Zenject;
using Random = UnityEngine.Random;

namespace Jam.Scripts.Gameplay.Rooms.Events.Domain
{
    public class RoomEventService
    {
        [Inject] private RoomEventRepository _roomEventRepository;
        [Inject] private RoomEventConfig _config;
        [Inject] private RoomEventBus _roomEventBus;
        [Inject] private RoomRewardBus _roomRewardBus;
        [Inject] private BattleStarter _battleStarter;
        [Inject] private LocalizationTool _localizationTool;
        private readonly RoomEventsModel _roomEventsModel = new();

        private const string FIRST_TARGET_KEY = "BALL_REWARD_DESC_TARGET_FIRST";
        private const string SECOND_TARGET_KEY = "BALL_REWARD_DESC_TARGET_SECOND";
        private const string LAST_TARGET_KEY = "BALL_REWARD_DESC_TARGET_LAST";
        private const string RANDOM_TARGET_KEY = "BALL_REWARD_DESC_TARGET_RANDOM";
        private const string PLAYER_TARGET_KEY = "BALL_REWARD_DESC_TARGET_PLAYER";
        private const string ALL_TARGET_KEY = "BALL_REWARD_DESC_TARGET_ALL";
        private const string DAMAGE_BALL_KEY = "BALL_REWARD_DESC_DAMAGE";
        private const string CRITICAL_BALL_KEY = "BALL_REWARD_DESC_CRITICAL";
        private const string HEAL_BALL_KEY = "BALL_REWARD_DESC_HEAL";
        private const string HEAL_ENEMY_BALL_KEY = "BALL_REWARD_DESC_HEAL_ENEMY";
        private const string POISON_BALL_KEY = "BALL_REWARD_DESC_POISON";
        private const string SHIELD_BALL_KEY = "BALL_REWARD_DESC_SHIELD";

        public void StartEvent(Room room)
        {
            RoomEvent roomEvent = GetRandomEventFromPool();
            switch (roomEvent)
            {
                case RoomRewardEvent e:
                    var rewardData = GetRewardsForReward(e);
                    _roomEventBus.StartRewardEvent(rewardData);
                    break;
                case RoomFightEvent:
                    _battleStarter.StartBattle(room);
                    break;
                case RoomDealEvent e:
                    var dealData = GetRewardsAndRiskForDeal(e);
                    _roomEventBus.StartDealEvent(dealData);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private RewardUiData GetRewardsForReward(RoomRewardEvent roomRewardEvent)
        {
            var rewards = roomRewardEvent.RewardsList
                .Select(GetRewardByType)
                .ToList();
            return new RewardUiData(rewards);
        }

        private DealUiData GetRewardsAndRiskForDeal(RoomDealEvent roomDealEvent)
        {
            DealUiData dealUiData = new DealUiData();
            foreach (var roomDealData in roomDealEvent.DealData)
            {
                var btnText = _localizationTool.GetText(roomDealData.ActionDescKey);
                RewardCardUiData rewardCard = GetRewardByType(roomDealData.RewardType);
                RewardCardUiData risk = GetRiskByType(roomDealData.RiskType);
                var data = new DealButtonData(btnText, rewardCard, risk);
                dealUiData.Buttons.Add(data);
            }

            return dealUiData;
        }

        private RewardCardUiData GetRiskByType(RoomRiskEventData risk)
        {
            var icon = risk.Sprite;
            var desc = "";
            switch (risk)
            {
                case BallRiskData p:
                {
                    var ball = p.Ball;
                    desc = GetBallDescription(ball);
                }
                    break;
                case DamageRiskData p:
                    desc = GetDamageDesc(p);
                    break;
                case GoldRiskData p:
                    desc = GetGoldDesc(p);
                    break;
                case MaxHpDecreaseRiskData p:
                    desc = GetMaxHpDecreaseDesc(p);
                    break;
            }

            return new RewardCardUiData(icon, desc);
        }

        private RewardCardUiData GetRewardByType(RoomRewardEventData reward)
        {
            var icon = reward.Sprite;
            var desc = "";
            switch (reward)
            {
                case RandomBallRewardData:
                {
                    BallSo ball = GetRandomBall();
                    desc = GetBallDescription(ball);
                }
                    break;
                case ConcreteBallRewardData p:
                {
                    var ball = p.ConcreteBall;
                    desc = GetBallDescription(ball);
                }
                    break;
                case GoldRewardData p:
                    desc = GetGoldDesc(p);
                    break;
                case MaxHpIncreaseRewardData p:
                    desc = GetMaxHpIncreaseDesc(p);
                    break;
                case HealRewardData p:
                    desc = GetHealDesc(p);
                    break;
            }

            return new RewardCardUiData(icon, desc);
        }

        private string GetHealDesc(HealRewardData p) => $"{GetSign(p.HealPercent)} {p.HealPercent} %";

        private string GetMaxHpIncreaseDesc(MaxHpIncreaseRewardData p) => $"{GetSign(p.Value)} {p.Value} %";

        private string GetMaxHpDecreaseDesc(MaxHpDecreaseRiskData p) => $"{GetSign(p.Value)} {p.Value} %";

        private string GetGoldDesc(GoldRewardData p) => $"{GetSign(p.Amount)} {p.Amount}";

        private string GetGoldDesc(GoldRiskData p) => $"{GetSign(p.Value)} {p.Value}";

        private string GetDamageDesc(DamageRiskData p) => $"{GetSign(p.Value)} {p.Value}";

        private string GetSign(float value) => value < 0 ? "-" : "+";

        private BallSo GetRandomBall()
        {
            //todo where is pool
            return null;
        }

        private string GetBallDescription(BallSo ball)
        {
            string desc = "";
            foreach (var ballEffect in ball.Effects)
            {
                var targetDescKey = GetTargetDescKey(ballEffect.Targeting);
                var targetDesc = _localizationTool.GetText(targetDescKey);
                var effectTextKey = GetEffectTextKey(ballEffect);
                var effectText = _localizationTool.GetText(effectTextKey);
                var value = GetEffectValue(ballEffect);
                desc += string.Format(effectText, targetDesc, value);
            }

            return desc;
        }

        private string GetEffectValue(EffectDef ballEffect)
        {
            var value = "";
            switch (ballEffect)
            {
                case DamageEffectDef e: value = e.Amount.ToString(); break;
                case CriticalEffectDef e: value = e.Damage.ToString(); break;
                case HealEffectDef e: value = e.Amount.ToString(); break;
                case PoisonEffectDef e: value = e.Damage.ToString(); break;
                case ShieldEffectDef e: value = e.Amount.ToString(); break;
            }

            return value;
        }

        private string GetEffectTextKey(EffectDef ballEffect)
        {
            var key = "";
            switch (ballEffect)
            {
                case DamageEffectDef: key = DAMAGE_BALL_KEY; break;
                case CriticalEffectDef: key = CRITICAL_BALL_KEY; break;
                case HealEffectDef: key = HEAL_BALL_KEY; break;
                case PoisonEffectDef: key = POISON_BALL_KEY; break;
                case ShieldEffectDef: key = SHIELD_BALL_KEY; break;
            }

            return key;
        }

        private string GetTargetDescKey(TargetType ballEffectTargeting)
        {
            var key = "";
            switch (ballEffectTargeting)
            {
                case TargetType.None:
                    break;
                case TargetType.First:
                    key = FIRST_TARGET_KEY;
                    break;
                case TargetType.All:
                    key = ALL_TARGET_KEY;
                    break;
                case TargetType.Last:
                    key = LAST_TARGET_KEY;
                    break;
                case TargetType.Random:
                    key = RANDOM_TARGET_KEY;
                    break;
                case TargetType.Player:
                    key = PLAYER_TARGET_KEY;
                    break;
            }

            return key;
        }

        private RoomEvent GetRandomEventFromPool()
        {
            var events = GetEventsList();

            var unusedEvents = events
                .Where(e => !_roomEventsModel.UsedDefinitionsIds.Contains(e.Id))
                .ToList();

            float totalWeight = unusedEvents.Sum(e => e.Weight);

            float randomValue = Random.Range(0f, totalWeight);

            RoomEvent chosenEvent = null;
            foreach (var e in unusedEvents)
            {
                randomValue -= e.Weight;
                if (randomValue <= 0f)
                {
                    chosenEvent = e;
                    break;
                }
            }

            if (chosenEvent == null)
            {
                Log.Error("No chosen event was found.");
                return null;
            }

            _roomEventsModel.UsedDefinitionsIds.Add(chosenEvent.Id);

            return chosenEvent;
        }

        private List<RoomEvent> GetEventsList()
        {
            List<RoomEvent> list = new();
            switch (Random.Range(0, 3))
            {
                case 0:
                    list.AddRange(_roomEventRepository.RoomFightEvents);
                    break;
                case 1:
                    list.AddRange(_roomEventRepository.RoomRewardEvents);
                    break;
                case 2:
                    list.AddRange(_roomEventRepository.RoomDealEvents);
                    break;
            }

            return list;
        }
    }
}