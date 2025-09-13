using Jam.Scripts.Gameplay.Rooms;
using Jam.Scripts.Gameplay.Rooms.Battle.Player;
using Zenject;

namespace Jam.Scripts.Gameplay.Artifacts
{
    public class ArtifactMaxHpEndBattleIncreaseSystem : IArtifactSystem
    {
        public class ArtifactFactory : BaseArtifactFactory<ArtifactMaxHpEndBattleIncreaseSystem>
        {

        }

        [Inject] private RoomRewardBus _rewardBus;
        [Inject] private PlayerBattleService _playerBattleService;

        public ArtifactMaxHpEndBattleIncreaseSystem(ArtifactSo data)
        {
            ArtifactMaxHpEndBattleIncreaseSo so = data as ArtifactMaxHpEndBattleIncreaseSo;

            if (so != null)
                _hpIncrease = so.HpIncrease;
        }

        private int _hpIncrease;

        public void Initialize()
        {
            _rewardBus.OnRoomCompleted += HandleEvent;
        }

        public void Dispose()
        {
            _rewardBus.OnRoomCompleted -= HandleEvent;
        }

        private void HandleEvent()
        {
            Execute();
        }


        public void Execute()
        {
            _playerBattleService.IncreaseMaxHp(_hpIncrease);
        }
    }
}