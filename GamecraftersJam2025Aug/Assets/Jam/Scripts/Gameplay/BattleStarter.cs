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
            var roomBattleConfig = new RoomBattleConfig(room.Type, room.Level, room.Floor);
            _battleSystem.StartBattle(roomBattleConfig);
        }
    }
}