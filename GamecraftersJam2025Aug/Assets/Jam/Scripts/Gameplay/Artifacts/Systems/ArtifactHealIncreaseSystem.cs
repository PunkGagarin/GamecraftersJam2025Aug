using Jam.Scripts.Gameplay.Battle.Player;
using Jam.Scripts.Gameplay.Rooms.Battle;
using Zenject;

namespace Jam.Scripts.Gameplay.Artifacts
{
    public class ArtifactHealIncreaseSystem : IArtifactSystem
    {
        [Inject] private BattleEventBus _battleEventBus;
        [Inject] private PlayerService _playerService;

        private int _healIncreaseAmount;

        public void Initialize()
        {
            _battleEventBus.OnBeforeHeal += HandleEvent;
        }

        public void Dispose()
        {
            _battleEventBus.OnBeforeHeal -= HandleEvent;
        }

        public void Init(ArtifactSo data)
        {
            ArtifactHealIncreaseSo so = data as ArtifactHealIncreaseSo;

            if (so != null)
                _healIncreaseAmount = so.IncreaseAmount;
        }

        private void HandleEvent(OnHealDto dto)
        {
            dto.HealAmount += _healIncreaseAmount;
        }


        public void Execute()
        {
            _playerService.Heal(_healIncreaseAmount);
        }
    }
}