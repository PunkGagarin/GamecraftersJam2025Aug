using System;
using Jam.Scripts.Gameplay.Battle;
using Jam.Scripts.MapFeature.Map.Data;
using Zenject;

namespace Jam.Scripts.MapFeature.Map.Domain
{
    public class RoomManagerSystem
    {
        [Inject] private BattleSystem _battleSystem;
        [Inject] private RoomEventSystem _eventSystem;

        public void ChooseRoomToOpen(Room room){
            switch (room.Type) {
                case RoomType.DefaultFight:
                    _battleSystem.StartBattle(room);
                    break;
                case RoomType.BossFight:
                    _battleSystem.StartBattle(room);
                    break;
                case RoomType.EliteFight:
                    _battleSystem.StartBattle(room);
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