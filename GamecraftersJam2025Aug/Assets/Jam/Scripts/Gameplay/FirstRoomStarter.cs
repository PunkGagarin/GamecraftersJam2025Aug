using Jam.Scripts.Gameplay.Rooms.Battle;
using Jam.Scripts.Gameplay.Rooms.Battle.Systems;
using Jam.Scripts.MapFeature.Map.Data;
using Zenject;

namespace Jam.Scripts.Gameplay
{
    public class FirstRoomStarter : IInitializable
    {
        [Inject] private BattleSystem _battleSystem;
        [Inject] private TutorialSystem _tutorialSystem;

        //
        public void Initialize()
        {
            if (_tutorialSystem.IsTutorial) return;
            RoomBattleConfig room = GetFirstRoomConfig();
            _battleSystem.StartBattle(room);
        }

        private RoomBattleConfig GetFirstRoomConfig()
        {
            return new RoomBattleConfig(RoomType.DefaultFight, 1, 1);
        }
    }

}