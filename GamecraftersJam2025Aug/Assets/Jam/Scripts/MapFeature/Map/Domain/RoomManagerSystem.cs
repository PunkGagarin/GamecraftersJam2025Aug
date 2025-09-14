using System;
using Jam.Scripts.Gameplay;
using Jam.Scripts.Gameplay.Rooms.ChestReward;
using Jam.Scripts.Gameplay.Rooms.Events.Domain;
using Jam.Scripts.MapFeature.Map.Data;
using Zenject;

namespace Jam.Scripts.MapFeature.Map.Domain
{
    public class RoomManagerSystem
    {
        [Inject] private BattleStarter _battleStarter;
        [Inject] private RoomEventService _eventService;
        [Inject] private ChestRewardSystem _chestRewardSystem;

        public void ChooseRoomToOpen(Room room){
            switch (room.Type) {
                case RoomType.DefaultFight:
                case RoomType.BossFight:
                case RoomType.EliteFight:
                    _battleStarter.StartBattle(room);
                    break;
                case RoomType.Chest:
                    _eventService.StartChestEvent(room);
                    break;
                case RoomType.Event:
                    _eventService.StartEvent(room);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

    }
}