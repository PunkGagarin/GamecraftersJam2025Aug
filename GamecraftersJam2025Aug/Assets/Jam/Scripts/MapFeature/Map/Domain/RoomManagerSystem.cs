using System;
using Jam.Scripts.Gameplay;
using Jam.Scripts.Gameplay.ChestReward;
using Jam.Scripts.MapFeature.Map.Data;
using Zenject;

namespace Jam.Scripts.MapFeature.Map.Domain
{
    public class RoomManagerSystem
    {
        [Inject] private BattleStarter _battleStarter;
        [Inject] private RoomEventSystem _eventSystem;
        [Inject] private ChestRewardSystem _chestRewardSystem;

        public void ChooseRoomToOpen(Room room){
            switch (room.Type) {
                case RoomType.DefaultFight:
                case RoomType.BossFight:
                case RoomType.EliteFight:
                    _battleStarter.StartBattle(room);
                    break;
                case RoomType.Chest:
                    _chestRewardSystem.Handle(room);
                    break;
                case RoomType.Event:
                    _eventSystem.StartEvent(room);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

    }
}