using System;
using System.Collections.Generic;
using System.Linq;
using Jam.Prefabs.Gameplay.Gold;
using Jam.Scripts.Gameplay.Artifacts;
using Jam.Scripts.Gameplay.Inventory;
using Jam.Scripts.Gameplay.Inventory.Models;
using Jam.Scripts.Gameplay.Rooms.Battle.Player;
using Jam.Scripts.Gameplay.Rooms.Battle.Queue;
using Jam.Scripts.Gameplay.Rooms.Events.DamageRisk;
using Jam.Scripts.Gameplay.Rooms.Events.GoldRisk;
using Jam.Scripts.Gameplay.Rooms.Events.MaxHpIncreaseReward;
using Jam.Scripts.Gameplay.Rooms.Events.Presentation;
using Jam.Scripts.MapFeature.Map.Data;
using Jam.Scripts.MapFeature.Map.Domain;
using ModestTree;
using UnityEngine;
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
        [Inject] private ArtifactService _artifactService;
        [Inject] private PlayerService _playerService;
        [Inject] private GoldService _goldService;

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

        private void OnBallSelected(BallType type, int grade)
        {
            var model = _ballsGenerator.CreateBallFor(type, grade);
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
                    var grade = Random.Range(1, 3);
                    var ball = _ballsGenerator.CreateBallRewardDtoFrom(p.Ball.BallType, grade);
                    desc = ball.Desc;
                    return new BallLoseRiskCardUiData(new BallRewardCardUiData(icon, desc, ball.Type, ball.Grade));
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
                        var data = GetRandomBall();
                        rewards.Add(data);
                    }

                    return new RandomBallRewardCardUiData(rewards);
                }
                case ConcreteBallRewardData p:
                {
                    var grade = Random.Range(1, 3);
                    var ball = _ballsGenerator.CreateBallRewardDtoFrom(p.ConcreteBall.BallType, grade);
                    return new ConcreteBallRewardCardUiData(ball);
                }
                case GoldRewardData p:
                    desc = GetGoldDesc(p);
                    return new GoldRewardCardUiData(icon, desc, p.Amount);
                case MaxHpIncreaseRewardData p:
                    desc = GetMaxHpIncreaseDesc(p);
                    return new GoldRewardCardUiData(icon, desc, p.Value);
                case HealRewardData p:
                    desc = GetHealDesc(p);
                    return new HealRewardCardUiData(icon, desc, p.Value);
                case ArtifactRewardData p:
                {
                    desc = GetArtifactDesc(p);
                    return new ArtifactRewardCardUiData(icon, desc, p.ArtifactType);
                }
                case BallUpgradeRewardData p:
                    var prevBall = GetRandomBall();
                    var newBall = GetRandomBall(); // todo prevBall upgraded version 
                    var prevValue = new BallRewardCardUiData(prevBall.Icon, prevBall.Desc, prevBall.Type, prevBall.Grade);
                    var newValue = new BallRewardCardUiData(newBall.Icon, newBall.Desc, newBall.Type, newBall.Grade);
                    return new BallUpgradeRewardCardUiData(prevValue, newValue, icon, desc);
                default: return null;
            }
        }
        
        private string GetArtifactDesc(ArtifactRewardData artifactRewardData) =>
            _artifactService.GetArtifactDtoByType(artifactRewardData.ArtifactType).Description;
        private string GetHealDesc(HealRewardData p) => $"{GetSign(p.Value)} {p.Value}";
        private string GetMaxHpIncreaseDesc(MaxHpIncreaseRewardData p) => $"{GetSign(p.Value)} {p.Value} %";
        private string GetGoldDesc(GoldRewardData p) => $"{GetSign(p.Amount)} {p.Amount}";
        private string GetMaxHpDecreaseDesc(MaxHpDecreaseRiskData p) => $"{GetSign(p.Value)} {p.Value} % от общего здоровья";
        private string GetGoldDesc(GoldRiskData p) => $"{GetSign(p.Value)} {p.Value} золота";
        private string GetDamageDesc(DamageRiskData p) => $"{GetSign(p.Value)} {p.Value} урона";
        private string GetSign(float value) => value < 0 ? "-" : "+";

        private BallRewardCardUiData GetRandomBall() => _ballsGenerator.CreateRandomBallRewardDto();

        private RoomEvent GetRandomEventFromPool()
        {
            var events = GetRandomEventsList();

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

        private List<RoomEvent> GetRandomEventsList()
        {
            var candidates = new List<List<RoomEvent>>
            { 
                _roomEventRepository.RoomFightEvents.Cast<RoomEvent>().ToList(),
                _roomEventRepository.RoomRewardEvents.Cast<RoomEvent>().ToList(),
                _roomEventRepository.RoomDealEvents.Cast<RoomEvent>().ToList()
            };

            int startIndex = Random.Range(0, candidates.Count);

            for (int i = 0; i < candidates.Count; i++)
            {
                int index = (startIndex + i) % candidates.Count;
                if (candidates[index].Count > 0)
                {
                    return candidates[index];
                }
            }

            return new List<RoomEvent>();
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

        public void ProcessReward(IRewardCardUiData rewardCardUiData)
        {
            switch (rewardCardUiData)
            {
                case RandomBallRewardCardUiData data: AddBallToPlayer(data.SelectedBall.BallType, data.SelectedBall.Grade); break;
                case ConcreteBallRewardCardUiData data: AddBallToPlayer(data.BallReward.Type, data.BallReward.Grade); break;
                case GoldRewardCardUiData data: AddGoldToPlayer(data.Value); break;
                case HealRewardCardUiData data: HealPlayer(data.Value); break;
                case MaxHpIncreaseRewardCardUiData data: IncreasePlayerMaxHp(data.Value); break;
                case ArtifactRewardCardUiData data: AddArtifactToPlayer(data.Type); break;
            }
        }
        
        private void AddArtifactToPlayer(ArtifactType dataType) => 
            _artifactService.AddArtifact(dataType);

        private void AddGoldToPlayer(float value) => 
            _goldService.AddGold(Mathf.RoundToInt(value));

        private void IncreasePlayerMaxHp(float value) => 
            _playerService.IncreaseMaxHp(Mathf.RoundToInt(value));

        private void HealPlayer(float value) => 
            _playerService.Heal(Mathf.RoundToInt(value));

        private void AddBallToPlayer(BallType ballType, int ballRewardGrade) =>
            _roomEventBus.BallSelected(ballType, ballRewardGrade);

        public void ProcessRisk(IRiskCardUiData dataRisk)
        {
            switch (dataRisk)
            {
               case BallLoseRiskCardUiData data: RemoveRandomBallFromPlayer(); break;
               case DamageRiskCardUiData data: TakeDamageFromPlayer(data.Value); break;
               case GoldRiskCardUiData data: TakeGoldFromPlayer(data.Value); break;
               case MaxHpDecreaseRiskCardUiData data: DecreasePlayerMaxHp(data.Value); break;
            }
        }

        private void RemoveRandomBallFromPlayer()
        {
            var ball = GetRandomBall();
            var ballModel = _ballsGenerator.CreateBallFor(ball.Type, ball.Grade);
            _playerInventoryService.RemoveBall(ballModel);
        }

        private void TakeDamageFromPlayer(float value) => 
            _playerService.TakeDamage(Mathf.RoundToInt(value));

        private void TakeGoldFromPlayer(float value) => 
            _goldService.RemoveGold(Mathf.RoundToInt(value));

        private void DecreasePlayerMaxHp(float value) => 
            _playerService.DecreaseMaxHp(Mathf.RoundToInt(value));
    }
}