using System;
using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Gameplay.Inventory;
using Jam.Scripts.Gameplay.Inventory.Models;
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
        [Inject] private MapEventBus _mapEventBus;
        [Inject] private PlayerInventoryService _playerInventoryService;
        [Inject] private BallDescriptionGenerator _ballDescriptionGenerator;
        [Inject] private BallsGenerator _ballsGenerator;
        [Inject] private RewardRiskService _rewardRiskService;

        public void Initialize()
        {
            _roomEventBus.OnBallSelected += OnBallSelected;
            _roomEventBus.OnEventFinished += OnEventFinished;
        }

        private readonly RoomEventsModel _roomEventsModel = new();

        public void StartEvent(Room room)
        {
            RoomEvent roomEvent = GetRandomEventFromPool();
            if (roomEvent == null)
            {
                OnEventFinished();
                return;
            }

            switch (roomEvent)
            {
                case RoomRewardEvent e:
                    var rewardData = _rewardRiskService.GetRewardsForReward(e);
                    _roomEventBus.StartRewardEvent(rewardData);
                    break;
                case RoomFightEvent:
                    _battleStarter.StartBattle(room);
                    break;
                case RoomDealEvent e:
                    var dealData = _rewardRiskService.GetRewardsAndRiskForDeal(e);
                    _roomEventBus.StartDealEvent(dealData);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void StartChestEvent(Room room)
        {
            var rewardData = _rewardRiskService.GetChestReward();
            _roomEventBus.StartRewardEvent(rewardData);
        }

        private void OnBallSelected(BallType type, int grade)
        {
            var model = _ballsGenerator.CreateBallFor(type, grade);
            _playerInventoryService.AddBall(model);
        }


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

        public void ProcessReward(IRewardCardUiData data) =>
            _rewardRiskService.ProcessReward(data);


        public void ProcessRisk(IRiskCardUiData dataRisk) =>
            _rewardRiskService.ProcessRisk(dataRisk);

        public void Dispose()
        {
            _roomEventBus.OnBallSelected -= OnBallSelected;
            _roomEventBus.OnEventFinished -= OnEventFinished;
        }
    }
}