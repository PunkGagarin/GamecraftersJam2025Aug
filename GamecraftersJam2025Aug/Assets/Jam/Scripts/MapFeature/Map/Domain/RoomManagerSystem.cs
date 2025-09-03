using System;
using Jam.Scripts.Gameplay;
using Jam.Scripts.MapFeature.Map.Data;
using Zenject;

namespace Jam.Scripts.MapFeature.Map.Domain
{
    public class RoomManagerSystem
    {
        [Inject] private BattleStarter _battleStarter;
        [Inject] private RoomEventSystem _eventSystem;

        public void ChooseRoomToOpen(Room room){
            switch (room.Type) {
                case RoomType.DefaultFight:
                case RoomType.BossFight:
                case RoomType.EliteFight:
                    _battleStarter.StartBattle(room);
                    break;
                case RoomType.Merchant:
                    //tbd 
                    break;
                case RoomType.Chest:
                    //todo idk
                    break;
                case RoomType.Campfire:
                    //tbd 
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