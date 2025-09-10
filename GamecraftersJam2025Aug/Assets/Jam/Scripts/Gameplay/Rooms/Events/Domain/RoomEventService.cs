using System;
using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Gameplay.Inventory;
using Jam.Scripts.Gameplay.Inventory.Models;
using Jam.Scripts.Gameplay.Rooms.Battle.Queue;
using Jam.Scripts.Gameplay.Rooms.Events.DamageRisk;
using Jam.Scripts.Gameplay.Rooms.Events.GoldRisk;
using Jam.Scripts.Gameplay.Rooms.Events.MaxHpIncreaseReward;
using Jam.Scripts.Gameplay.Rooms.Events.Presentation;
using Jam.Scripts.MapFeature.Map.Data;
using Jam.Scripts.MapFeature.Map.Domain;
using ModestTree;
using Zenject;
using Random = UnityEngine.Random;

namespace Jam.Scripts.Gameplay.Rooms.Events.Domain
{
    public class RoomEventService : IInitializable, IDisposable
    {
        [Inject] private RoomEventRepository _roomEventRepository;
        [Inject] private RoomEventConfig _config;
        [Inject] private RoomEventBus _roomEventBus;
        [Inject] private BattleStarter _battleStarter;
        [Inject] private LocalizationTool _localizationTool;
        [Inject] private MapEventBus _mapEventBus;
        [Inject] private BallsGenerator _ballsGenerator;
        [Inject] private PlayerInventoryService _playerInventoryService;
        [Inject] private BallDescriptionGenerator _ballDescriptionGenerator;
        
        private readonly RoomEventsModel _roomEventsModel = new();

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

        private void OnBallSelected(BallType type)
        {
            var model = _ballsGenerator.CreateBallFrom(type);
            _playerInventoryService.AddBall(model);
        }

        private RewardUiData GetRewardsForReward(RoomRewardEvent roomRewardEvent)
        {
            var rewards = roomRewardEvent.RewardsList
                .Select(GetRewardByType)
                .ToList();
            return new RewardUiData(roomRewardEvent.Sprite, rewards);
        }

        private DealUiData GetRewardsAndRiskForDeal(RoomDealEvent roomDealEvent)
        {
            DealUiData dealUiData = new DealUiData(roomDealEvent.Sprite);
            foreach (var roomDealData in roomDealEvent.DealData)
            {
                var btnText = _localizationTool.GetText(roomDealData.ActionDescKey);
                IRewardCardUiData rewardCard = GetRewardByType(roomDealData.RewardType);
                IRiskCardUiData risk = GetRiskByType(roomDealData.RiskType);
                var data = new DealButtonData(btnText, rewardCard, risk);
                dealUiData.Buttons.Add(data);
            }

            return dealUiData;
        }

        private IRiskCardUiData GetRiskByType(RoomRiskEventData risk)
        {
            var icon = risk.Sprite;
            var desc = "";
            switch (risk)
            {
                case BallRiskData p:
                {
                    var ball = _ballsGenerator.CreateBallRewardDtoFrom(p.Ball.BallType);
                    desc = ball.Description;
                    return new BallLoseRiskCardUiData(new BallRewardCardUiData(icon, desc, ball.Type));
                }
                case DamageRiskData p:
                    desc = GetDamageDesc(p);
                    return new DamageRiskCardUiData(icon, desc, p.Value);
                case GoldRiskData p:
                    desc = GetGoldDesc(p);
                    return new GoldRiskCardUiData(icon, desc, p.Value);
                case MaxHpDecreaseRiskData p:
                    desc = GetMaxHpDecreaseDesc(p);
                    return new MaxHpDecreaseRiskCardUiData(icon, desc, p.Value);
                default:
                    return null;
            }
        }

        private IRewardCardUiData GetRewardByType(RoomRewardEventData reward)
        {
            var icon = reward.Sprite;
            var desc = "";
            switch (reward)
            {
                case RandomBallRewardData:
                {
                    List<BallRewardCardUiData> rewards = new();
                    for (int i = 0; i < _config.BallsCountForRandomBall; i++)
                    {
                        BallRewardDto ballRewardDto = GetRandomBall();
                        var data = new BallRewardCardUiData(icon, ballRewardDto.Description, ballRewardDto.Type);
                        rewards.Add(data);
                    }

                    return new RandomBallRewardCardUiData(rewards);
                }
                case ConcreteBallRewardData p:
                {
                    var ball = _ballsGenerator.CreateBallRewardDtoFrom(p.ConcreteBall.BallType);
                    return new ConcreteBallRewardCardUiData(new BallRewardCardUiData(icon, ball.Description,
                        ball.Type));
                }
                case GoldRewardData p:
                    desc = GetGoldDesc(p);
                    return new GoldRewardCardUiData(icon, desc, p.Amount);
                case MaxHpIncreaseRewardData p:
                    desc = GetMaxHpIncreaseDesc(p);
                    return new GoldRewardCardUiData(icon, desc, p.Value);
                case HealRewardData p:
                    desc = GetHealDesc(p);
                    return new HealRewardCardUiData(icon, desc, p.HealPercent);
                default: return null;
            }
        }

        private string GetHealDesc(HealRewardData p) => $"{GetSign(p.HealPercent)} {p.HealPercent} %";

        private string GetMaxHpIncreaseDesc(MaxHpIncreaseRewardData p) => $"{GetSign(p.Value)} {p.Value} %";

        private string GetMaxHpDecreaseDesc(MaxHpDecreaseRiskData p) => $"{GetSign(p.Value)} {p.Value} %";

        private string GetGoldDesc(GoldRewardData p) => $"{GetSign(p.Amount)} {p.Amount}";

        private string GetGoldDesc(GoldRiskData p) => $"{GetSign(p.Value)} {p.Value}";

        private string GetDamageDesc(DamageRiskData p) => $"{GetSign(p.Value)} {p.Value}";

        private string GetSign(float value) => value < 0 ? "-" : "+";

        private BallRewardDto GetRandomBall() => _ballsGenerator.CreateRandomBallRewardDto();

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

        private void OnEventFinished() => _mapEventBus.RoomCompleted();

        public void Initialize()
        {
            _roomEventBus.OnBallSelected += OnBallSelected;
            _roomEventBus.OnEventFinished += OnEventFinished;
        }

        public void Dispose()
        {
            _roomEventBus.OnBallSelected -= OnBallSelected;
            _roomEventBus.OnEventFinished -= OnEventFinished;
        }
    }
}