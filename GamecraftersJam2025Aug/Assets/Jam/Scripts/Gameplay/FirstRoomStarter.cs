using System.Threading.Tasks;
using Jam.Scripts.Gameplay.Battle;
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