using Jam.Scripts.Gameplay.Battle;
using Jam.Scripts.MapFeature.Map.Data;
using Zenject;

namespace Jam.Scripts.Gameplay
{
    public class BattleStarter
    {
        [Inject] private BattleSystem _battleSystem;

        public void StartBattle(Room room)
        {
            //todo: get Level from room
            var roomBattleConfig = new RoomBattleConfig(room.Type, 1, room.Floor);
            _battleSystem.StartBattle(roomBattleConfig);
        }
    }
}