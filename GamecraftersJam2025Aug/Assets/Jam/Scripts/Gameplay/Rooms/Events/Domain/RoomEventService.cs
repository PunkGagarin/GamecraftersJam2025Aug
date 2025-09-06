using System;
using System.Linq;
using Jam.Scripts.Gameplay.Rooms.Events.Data;
using Jam.Scripts.MapFeature.Map.Data;
using ModestTree;
using Zenject;
using Random = UnityEngine.Random;

namespace Jam.Scripts.Gameplay.Rooms.Events.Domain
{
    public class RoomEventService
    {
        [Inject] private RoomEventRepository _roomEventRepository;
        [Inject] private RoomEventBus _roomEventBus;
        [Inject] private RoomRewardBus _roomRewardBus;
        [Inject] private BattleStarter _battleStarter;

        public void StartEvent(Room room)
        {
            var roomEvent = GetRandomEventFromPool();
            switch (roomEvent.Type)
            {
                case RoomEventType.Reward:
                    _roomRewardBus.InvokeEventReward(roomEvent);
                    break;
                case RoomEventType.Fight:
                    _battleStarter.StartBattle(room);
                    break;
                case RoomEventType.Deal:
                    _roomEventBus.StartEvent(roomEvent);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private RoomEvent GetRandomEventFromPool()
        {
            var unusedEvents = _roomEventRepository.Definitions
                .Where(e => !_roomEventRepository.UsedDefinitionsIds.Contains(e.Id))
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

            _roomEventRepository.UsedDefinitionsIds.Add(chosenEvent.Id);

            return chosenEvent;
        }
    }
}