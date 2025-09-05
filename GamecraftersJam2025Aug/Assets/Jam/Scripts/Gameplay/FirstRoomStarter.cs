using Jam.Scripts.Gameplay.Rooms.Battle.Systems;
using Jam.Scripts.MapFeature.Map.Data;
using Zenject;

namespace Jam.Scripts.Gameplay
{
    public class FirstRoomStarter : IInitializable
    {
        [Inject] private BattleSystem _battleSystem;

        //
        public void Initialize()
        {
            RoomBattleConfig room = GetFirstRoomConfig();
            _battleSystem.StartBattle(room);
        }

        private RoomBattleConfig GetFirstRoomConfig()
        {
            return new RoomBattleConfig(RoomType.DefaultFight, 1, 1);
        }
    }

}